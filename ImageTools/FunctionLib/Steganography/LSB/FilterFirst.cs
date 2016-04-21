using System.Collections.Generic;
using System.Linq;
using FunctionLib.Filter;
using FunctionLib.Model;

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

        protected override bool EncodingIteration()
        {
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
            //var random = new Random(password);

            foreach (var key in ordered)
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

        protected override bool DecodingIteration()
        {
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
            //var random = new Random(password);

            foreach (var key in ordered)
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