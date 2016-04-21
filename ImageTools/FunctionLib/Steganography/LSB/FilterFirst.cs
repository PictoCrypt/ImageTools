using System.Collections.Generic;
using System.Linq;
using FunctionLib.Filter;
using FunctionLib.Model;
using FunctionLib.Model.Message;

namespace FunctionLib.Steganography.LSB
{
    public class FilterFirst : LsbAlgorithmBase
    {
        private IOrderedEnumerable<KeyValuePair<Pixel, int>> mLaplaceValues;

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

        protected override void InitializeEncoding(string src, ISecretMessage message, int passHash, int lsbIndicator)
        {
            base.InitializeEncoding(src, message, passHash, lsbIndicator);
            var filter = new Laplace(Bitmap, LsbIndicator, 8);
            IDictionary<Pixel, int> filtered = new Dictionary<Pixel, int>();
            for (var x = 0; x < Bitmap.Width; x++)
            {
                for (var y = 0; y < Bitmap.Height; y++)
                {
                    filtered.Add(new Pixel(x, y), filter.GetValue(x, y));
                }
            }
            var ordered = filtered.OrderByDescending(key => key.Value);
            mLaplaceValues = ordered;
        }

        protected override bool EncodingIteration()
        {
            //var random = new Random(password);

            foreach (var key in mLaplaceValues)
            {
                var x = key.Key.X;
                var y = key.Key.Y;
                //var x = GetNextRandom("x", orderedLaplace.Count(), random);
                EncodeBytes(x, y, LsbIndicator);
                if (EncodeCheckForEnd())
                {
                    return true;
                }
            }
            return false;
        }

        protected override void InitializeDecoding(string src, int passHash, int lsbIndicator)
        {
            base.InitializeDecoding(src, passHash, lsbIndicator);
            var filter = new Laplace(Bitmap, LsbIndicator, 8);
            IDictionary<Pixel, int> filtered = new Dictionary<Pixel, int>();
            for (var x = 0; x < Bitmap.Width; x++)
            {
                for (var y = 0; y < Bitmap.Height; y++)
                {
                    filtered.Add(new Pixel(x, y), filter.GetValue(x, y));
                }
            }
            mLaplaceValues = filtered.OrderByDescending(key => key.Value);
        }

        protected override bool DecodingIteration()
        {
            //var random = new Random(password);

            foreach (var key in mLaplaceValues)
            {
                var x = key.Key.X;
                var y = key.Key.Y;
                //var x = GetNextRandom("x", orderedLaplace.Count(), random);
                DecodeBytes(x, y, LsbIndicator);
                //TODO: Fix this? Why is this so fucking cumbersome? Cant REF BitHolder
                var bitHolder = BitHolder;
                Bytes = BitToByte(Bytes.ToList(), ref bitHolder);
                BitHolder = bitHolder;
                if (DecodeCheckForEnd())
                {
                    return true;
                }
            }
            return false;
        }
    }
}