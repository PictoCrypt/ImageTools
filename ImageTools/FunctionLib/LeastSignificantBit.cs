﻿using System.Drawing;

namespace FunctionLib
{
    public static class LeastSignificantBit
    {
        public static Bitmap Encrypt(Bitmap src, string text)
        {
            var result = new Bitmap(src);
            var lockBitmap = new LockBitmap(result);
            lockBitmap.LockBits();

            // initially, we'll be hiding characters in the image
            var state = State.Hiding;

            // holds the index of the character that is being hidden
            var charIndex = 0;

            // holds the value of the character converted to integer
            var charValue = 0;

            // holds the index of the color element (R or G or B) that is currently being processed
            long pixelElementIndex = 0;

            // holds the number of trailing zeros that have been added when finishing the process
            var zeros = 0;

            // hold pixel elements

            // pass through the rows
            for (var i = 0; i < lockBitmap.Height; i++)
            {
                // pass through each row
                for (var j = 0; j < lockBitmap.Width; j++)
                {
                    // holds the pixel that is currently being processed
                    var pixel = lockBitmap.GetPixel(j, i);

                    // now, clear the least significant bit (LSB) from each pixel element
                    var r = pixel.R - pixel.R%2;
                    var g = pixel.G - pixel.G%2;
                    var b = pixel.B - pixel.B%2;

                    // for each pixel, pass through its elements (RGB)
                    for (var n = 0; n < 3; n++)
                    {
                        // check if new 8 bits has been processed
                        if (pixelElementIndex%8 == 0)
                        {
                            // check if the whole process has finished
                            // we can say that it's finished when 8 zeros are added
                            if (state == State.FillingWithZeros && zeros == 8)
                            {
                                // apply the last pixel on the image
                                // even if only a part of its elements have been affected
                                if ((pixelElementIndex - 1)%3 < 2)
                                {
                                    lockBitmap.SetPixel(j, i, Color.FromArgb(r, g, b));
                                }

                                // return the bitmap with the text hidden in
                                lockBitmap.UnlockBits();
                                return result;
                            }

                            // check if all characters has been hidden
                            if (charIndex >= text.Length)
                            {
                                // start adding zeros to mark the end of the text
                                state = State.FillingWithZeros;
                            }
                            else
                            {
                                // move to the next character and process again
                                charValue = text[charIndex++];
                            }
                        }

                        // check which pixel element has the turn to hide a bit in its LSB
                        switch (pixelElementIndex%3)
                        {
                            case 0:
                            {
                                if (state == State.Hiding)
                                {
                                    // the rightmost bit in the character will be (charValue % 2)
                                    // to put this value instead of the LSB of the pixel element
                                    // just add it to it
                                    // recall that the LSB of the pixel element had been cleared
                                    // before this operation
                                    r += charValue%2;

                                    // removes the added rightmost bit of the character
                                    // such that next time we can reach the next one
                                    charValue /= 2;
                                }
                            }
                                break;
                            case 1:
                            {
                                if (state == State.Hiding)
                                {
                                    g += charValue%2;

                                    charValue /= 2;
                                }
                            }
                                break;
                            case 2:
                            {
                                if (state == State.Hiding)
                                {
                                    b += charValue%2;

                                    charValue /= 2;
                                }

                                lockBitmap.SetPixel(j, i, Color.FromArgb(r, g, b));
                            }
                                break;
                        }

                        pixelElementIndex++;

                        if (state == State.FillingWithZeros)
                        {
                            // increment the value of zeros until it is 8
                            zeros++;
                        }
                    }
                }
            }
            lockBitmap.UnlockBits();
            return result;
        }

        public static string DecryptText(Bitmap src)
        {
            var lockBitmap = new LockBitmap(new Bitmap(src));
            lockBitmap.LockBits();

            var colorUnitIndex = 0;
            var charValue = 0;

            // holds the text that will be extracted from the image
            var result = string.Empty;

            // pass through the rows
            for (var i = 0; i < lockBitmap.Height; i++)
            {
                // pass through each row
                for (var j = 0; j < lockBitmap.Width; j++)
                {
                    var pixel = lockBitmap.GetPixel(j, i);

                    // for each pixel, pass through its elements (RGB)
                    for (var n = 0; n < 3; n++)
                    {
                        switch (colorUnitIndex%3)
                        {
                            case 0:
                            {
                                // get the LSB from the pixel element (will be pixel.R % 2)
                                // then add one bit to the right of the current character
                                // this can be done by (charValue = charValue * 2)
                                // replace the added bit (which value is by default 0) with
                                // the LSB of the pixel element, simply by addition
                                charValue = charValue*2 + pixel.R%2;
                            }
                                break;
                            case 1:
                            {
                                charValue = charValue*2 + pixel.G%2;
                            }
                                break;
                            case 2:
                            {
                                charValue = charValue*2 + pixel.B%2;
                            }
                                break;
                        }

                        colorUnitIndex++;

                        // if 8 bits has been added,
                        // then add the current character to the result text
                        if (colorUnitIndex%8 == 0)
                        {
                            // reverse? of course, since each time the process occurs
                            // on the right (for simplicity)
                            charValue = ReverseBits(charValue);

                            // can only be 0 if it is the stop character (the 8 zeros)
                            if (charValue == 0)
                            {
                                return result;
                            }

                            // convert the character value from int to char
                            var c = (char) charValue;

                            // add the current character to the result text
                            result += c.ToString();
                        }
                    }
                }
            }

            lockBitmap.UnlockBits();
            return result;
        }

        private static int ReverseBits(int n)
        {
            var result = 0;

            for (var i = 0; i < 8; i++)
            {
                result = result*2 + n%2;

                n /= 2;
            }

            return result;
        }

        private enum State
        {
            Hiding,
            FillingWithZeros
        };
    }
}