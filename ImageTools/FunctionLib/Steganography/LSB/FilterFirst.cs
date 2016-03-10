using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FunctionLib.Filter;
using FunctionLib.Helper;
using FunctionLib.Model;
using FunctionLib.Model.Message;

namespace FunctionLib.Steganography.LSB
{
    public class FilterFirst : LsbAlgorithmBase
    {
        public override string Name
        {
            get { return "FilterFirst"; }
        }

        public override string Description
        {
            get
            {
                return "The algorithm filters the image using an edge-detection filter " +
                       "like \"Laplace-Filter\". Hiding the secret information into the " +
                       "pixels with the highest filter-values.";
            }
        }

        protected override bool Iteration(int lsbIndicator)
        {
            var filter = new Laplace(Bitmap, 1, 8);
            IDictionary<Pixel, int> laplace = new Dictionary<Pixel, int>();
            for (var x = 0; x < Bitmap.Width; x++)
            {
                for (var y = 0; y < Bitmap.Height; y++)
                {
                    laplace.Add(new Pixel(x, y), filter.GetValue(x, y));
                }
            }
            var orderedLaplace = laplace.OrderByDescending(key => key.Value);
            //var random = new Random(password);

            foreach (var key in orderedLaplace)
            {
                var x = key.Key.X;
                var y = key.Key.Y;
                //var x = GetNextRandom("x", orderedLaplace.Count(), random);
                EncodeBytes(x, y, lsbIndicator);
                if (CheckForEnd())
                {
                    return true;
                }
            }
            return false;
        }

        public override ISecretMessage Decode(Bitmap src, int passHash, MessageType type, int lsbIndicator = 3)
        {
            //TODO geht das mit lock?
            var filter = new Laplace(src, 1, 8);
            IDictionary<Pixel, int> laplace = new Dictionary<Pixel, int>();
            for (var x = 0; x < src.Width; x++)
            {
                for (var y = 0; y < src.Height; y++)
                {
                    laplace.Add(new Pixel(x, y), filter.GetValue(x, y));
                }
            }

            var result = LockBitmap(src);
            var orderedLaplace = laplace.OrderByDescending(key => key.Value);

            var byteList = new List<byte>();
            var end = int.MaxValue;
            ICollection<int> bitHolder = new List<int>();
            foreach (var key in orderedLaplace)
            {
                var x = key.Key.X;
                var y = key.Key.Y;

                var pixel = result.GetPixel(x, y);
                for (var i = 0; i < lsbIndicator; i++)
                {
                    var bit = ByteHelper.GetBit(pixel.R, 8 - lsbIndicator + i);
                    bitHolder.Add(bit);
                }

                for (var i = 0; i < lsbIndicator; i++)
                {
                    var bit = ByteHelper.GetBit(pixel.G, 8 - lsbIndicator + i);
                    bitHolder.Add(bit);
                }

                for (var i = 0; i < lsbIndicator; i++)
                {
                    var bit = ByteHelper.GetBit(pixel.B, 8 - lsbIndicator + i);
                    bitHolder.Add(bit);
                }
                byteList = DecryptHelper(byteList, ref bitHolder);
                // Check for EndTag (END)
                if (end == int.MaxValue)
                {
                    var index = ListHelper.IndexOf(byteList, Constants.TagSeperator);
                    if (index > 0)
                    {
                        var seq = byteList.Take(index);
                        int.TryParse(ConvertHelper.Convert(seq.ToArray()), out end);
                        if (end == 0)
                        {
                            throw new ArithmeticException();
                        }

                        end = end + seq.Count() + Constants.TagSeperator.Length;
                    }
                }

                if (byteList.Count >= end)
                {
                    return MethodHelper.GetSpecificMessage(type, byteList.ToArray());
                }
            }
            throw new SystemException("Error, anything happened (or maybe not).");
        }
    }
}