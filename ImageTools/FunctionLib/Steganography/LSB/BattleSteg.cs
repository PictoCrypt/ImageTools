using System;
using System.Collections.Generic;
using System.Linq;
using FunctionLib.Filter;
using FunctionLib.Model;
using FunctionLib.Model.Message;

namespace FunctionLib.Steganography.LSB
{
    public class BattleSteg : LsbWithRandomness
    {
        private HashSet<Pixel> mShips;

        public override string Name
        {
            get { return "BattleSteg"; }
        }

        public override string Description
        {
            get
            {
                return "The algorithm filters the image using an edge-detection filter " +
                       "like \"Laplace-Filter\". Hiding the secret information into random pixels with high filter-values.";
            }
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
            var ordered = filtered.OrderByDescending(key => key.Value);
            //TODO: dynamic maybe? Top 100
            mShips = new HashSet<Pixel>(ordered.Select((x, y) => x.Key).Take(100));
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
            mShips = new HashSet<Pixel>(ordered.Select((x, y) => x.Key).Take(100));
        }

        protected override bool EncodingIteration()
        {
            while (!EncodeCheckForEnd())
            {
                var pixel = GetNextRandom(Bitmap.Width, Bitmap.Height, Random);
                if (CheckShipShot(pixel))
                {
                    FillNeighbors(pixel);
                }
                EncodeBytes(pixel.X, pixel.Y, LsbIndicator);
            }
            return true;
        }

        private void FillNeighbors(Pixel pixel)
        {
            EncodeBytes(pixel.X+1, pixel.Y, LsbIndicator);
            EncodeBytes(pixel.X-1, pixel.Y, LsbIndicator);
            EncodeBytes(pixel.X, pixel.Y+1, LsbIndicator);
            EncodeBytes(pixel.X, pixel.Y-1, LsbIndicator);
        }

        private bool CheckShipShot(Pixel pixel)
        {
            return mShips.Contains(pixel);
        }

        protected override bool DecodingIteration()
        {
            while (!DecodeCheckForEnd())
            {
                var pixel = GetNextRandom(Bitmap.Width, Bitmap.Height, Random);
                if (CheckShipShot(pixel))
                {
                    ReadNeighbors(pixel);
                }

                DecodeBytes(pixel.X, pixel.Y, LsbIndicator);
                //TODO: Fix this? Why is this so fucking cumbersome? Cant REF BitHolder
                var bitHolder = BitHolder;
                Bytes = BitToByte(Bytes, ref bitHolder);
                BitHolder = bitHolder;
            }
            return Bytes.Length == EndCount;
        }

        private void ReadNeighbors(Pixel pixel)
        {
            DecodeBytes(pixel.X + 1, pixel.Y, LsbIndicator);
            DecodeBytes(pixel.X - 1, pixel.Y, LsbIndicator);
            DecodeBytes(pixel.X, pixel.Y + 1, LsbIndicator);
            DecodeBytes(pixel.X, pixel.Y - 1, LsbIndicator);
        }
    }
}