using System;
using System.Collections.Generic;
using FunctionLib.Model;
using FunctionLib.Model.Message;

namespace FunctionLib.Steganography.LSB
{
    public abstract class LsbWithRandomness : LsbAlgorithmBase
    {
        private HashSet<Pixel> mPixels;
        protected Random Random { get; set; }

        protected override void InitializeEncoding(string src, ISecretMessage message, int passHash, int lsbIndicator)
        {
            base.InitializeEncoding(src, message, passHash, lsbIndicator);
            mPixels = new HashSet<Pixel>();
            Random = new Random(PassHash);
        }

        protected override void InitializeDecoding(string src, int passHash, int lsbIndicator)
        {
            base.InitializeDecoding(src, passHash, lsbIndicator);
            mPixels = new HashSet<Pixel>();
            Random = new Random(PassHash);
        }

        protected Pixel GetNextRandom(int xMax, int yMax, Random random)
        {
            //TODO verbessern: Vergleichen und nur die Koordinate ersetzen, welche schon vorhanden ist?
            var pixel = GenerateRandomPixel(xMax, yMax, random);
            while (mPixels.Contains(pixel))
            {
                pixel = GenerateRandomPixel(xMax, yMax, random);
            }
            mPixels.Add(pixel);
            return pixel;
        }

        private static Pixel GenerateRandomPixel(int xMax, int yMax, Random random)
        {
            var randomX = random.Next(xMax);
            var randomY = random.Next(yMax);
            var pixel = new Pixel(randomX, randomY);
            return pixel;
        }
    }
}