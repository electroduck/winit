using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

/// <summary>
/// Code used internally by Electroduck.UI.Panels.
/// </summary>
namespace Electroduck.UI.Panels.Internal
{
    /// <summary>
    /// Image Effects
    /// </summary>
    public static class ImageFX
    {
        /// <summary>
        /// Blur an image.
        /// </summary>
        /// <param name="image">The image to blur.</param>
        /// <param name="blurSize">The strength of the blur.</param>
        /// <returns>Blurred image.</returns>
        public static Bitmap BlurMethodA(Bitmap image, Int32 blurSize) {
            return BlurMethodA(image, new Rectangle(0, 0, image.Width, image.Height), blurSize);
        }

        /// <summary>
        /// Blur part of an image.
        /// </summary>
        /// <param name="image">The image to apply the effect to.</param>
        /// <param name="rectangle">The rectangle to blur.</param>
        /// <param name="blurSize">The strength of the blur.</param>
        /// <returns>Image with blurred section.</returns>
        /// <remarks>Source: https://stackoverflow.com/questions/44827093/how-to-apply-blur-effect-on-an-image-in-c </remarks>
        public unsafe static Bitmap BlurMethodA(Bitmap image, Rectangle rectangle, Int32 blurSize) {
            Bitmap blurred = new Bitmap(image.Width, image.Height);

            // make an exact copy of the bitmap provided
            using (Graphics graphics = Graphics.FromImage(blurred))
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

            // Lock the bitmap's bits
            BitmapData blurredData = blurred.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, blurred.PixelFormat);

            // Get bits per pixel for current PixelFormat
            int bitsPerPixel = Image.GetPixelFormatSize(blurred.PixelFormat);
            if (bitsPerPixel != 32)
                throw new FormatException("Only 32bpp bitmaps are supported");

            // Get pointer to first line
            byte* scan0 = (byte*)blurredData.Scan0.ToPointer();

            // look at every pixel in the blur rectangle
            for (int xx = rectangle.X; xx < rectangle.X + rectangle.Width; xx++) {
                for (int yy = rectangle.Y; yy < rectangle.Y + rectangle.Height; yy++) {
                    int avgR = 0, avgG = 0, avgB = 0, avgA = 0;
                    int blurPixelCount = 0;

                    // average the color of the red, green and blue for each pixel in the
                    // blur size while making sure you don't go outside the image bounds
                    for (int x = xx; (x < xx + blurSize && x < image.Width); x++) {
                        for (int y = yy; (y < yy + blurSize && y < image.Height); y++) {
                            // Get pointer to RGB
                            byte* data = scan0 + y * blurredData.Stride + x * 4;

                            avgB += data[0]; // Blue
                            avgG += data[1]; // Green
                            avgR += data[2]; // Red
                            avgA += data[3]; // Alpha

                            blurPixelCount++;
                        }
                    }

                    avgR = avgR / blurPixelCount;
                    avgG = avgG / blurPixelCount;
                    avgB = avgB / blurPixelCount;
                    avgA = avgA / blurPixelCount;

                    // now that we know the average for the blur size, set each pixel to that color
                    for (int x = xx; x < xx + blurSize && x < image.Width && x < rectangle.Width; x++) {
                        for (int y = yy; y < yy + blurSize && y < image.Height && y < rectangle.Height; y++) {
                            // Get pointer to RGB
                            byte* data = scan0 + y * blurredData.Stride + x * 4;

                            // Change values
                            data[0] = (byte)avgB;
                            data[1] = (byte)avgG;
                            data[2] = (byte)avgR;
                            data[3] = (byte)avgA;
                        }
                    }
                }
            }

            // Unlock the bits
            blurred.UnlockBits(blurredData);

            return blurred;
        }

        /// <summary>
        /// Blur using an alternate method.
        /// </summary>
        /// <param name="bmToBlur">The bitmap to blur.</param>
        /// <param name="nStrength">Strength of the blur effect.</param>
        /// <returns>Blurred bitmap.</returns>
        public static Bitmap BlurMethodB(Bitmap bmToBlur, int nStrength) {
            Bitmap bmBlurred = bmToBlur;
            for (int n = 0; n < nStrength; n++)
                bmBlurred = BlurMethodB(bmBlurred);
            return bmBlurred;
        }

        /// <summary>
        /// Blur using an alternate method.
        /// </summary>
        /// <param name="bmToBlur">The bitmap to blur.</param>
        /// <returns>Blurred bitmap.</returns>
        public unsafe static Bitmap BlurMethodB(Bitmap bmToBlur) {
            // Check input bits per pixel
            int bitsPerPixel = Image.GetPixelFormatSize(bmToBlur.PixelFormat);
            if (bitsPerPixel != 32)
                throw new FormatException("Only 32bpp bitmaps are supported");

            // Create new bitmap
            Bitmap bmBlurred = new Bitmap(bmToBlur.Width, bmToBlur.Height);

            // Lock the bitmap's bits
            BitmapData bdatIn = bmToBlur.LockBits(new Rectangle(0, 0, bmToBlur.Width, bmToBlur.Height),
                ImageLockMode.ReadWrite, bmToBlur.PixelFormat);
            BitmapData bdatOut = bmBlurred.LockBits(new Rectangle(0, 0, bmBlurred.Width, bmBlurred.Height),
                ImageLockMode.ReadWrite, bmToBlur.PixelFormat);

            // Get pointer to first line
            byte* pInScan0 = (byte*)bdatIn.Scan0.ToPointer();
            byte* pOutScan0 = (byte*)bdatOut.Scan0.ToPointer();

            // Iterate through pixels
            for(int y = 1; y < bmToBlur.Height - 1; y++) {
                byte* pInScanCur = pInScan0 + y * bdatIn.Stride;
                byte* pInScanPrev = pInScanCur - bdatIn.Stride;
                byte* pInScanNext = pInScanCur + bdatIn.Stride;
                byte* pOutScanCur = pOutScan0 + y * bdatOut.Stride;

                for (int x = 1; x < bmToBlur.Width - 1; x++) {
                    pOutScanCur[x * 4] = (byte)((pInScanCur[x * 4] + pInScanPrev[x * 4] + pInScanNext[x * 4]
                        + pInScanCur[(x + 1) * 4] + pInScanCur[(x - 1) * 4]) / 5);
                    pOutScanCur[x * 4 + 1] = (byte)((pInScanCur[x * 4 + 1] + pInScanPrev[x * 4 + 1] + pInScanNext[x * 4 + 1]
                        + pInScanCur[(x + 1) * 4 + 1] + pInScanCur[(x - 1) * 4 + 1]) / 5);
                    pOutScanCur[x * 4 + 2] = (byte)((pInScanCur[x * 4 + 2] + pInScanPrev[x * 4 + 2] + pInScanNext[x * 4 + 2]
                        + pInScanCur[(x + 1) * 4 + 2] + pInScanCur[(x - 1) * 4 + 2]) / 5);
                    pOutScanCur[x * 4 + 3] = (byte)((pInScanCur[x * 4 + 3] + pInScanPrev[x * 4 + 3] + pInScanNext[x * 4 + 3]
                        + pInScanCur[(x + 1) * 4 + 3] + pInScanCur[(x - 1) * 4 + 3]) / 5);
                }
            }

            // Unlock the bits
            bmToBlur.UnlockBits(bdatIn);
            bmBlurred.UnlockBits(bdatOut);

            return bmBlurred;
        }
    }
}
