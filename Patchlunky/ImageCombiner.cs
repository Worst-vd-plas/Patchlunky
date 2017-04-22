using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace Patchlunky
{
    internal static class ImageCombiner
    {
        /// <summary>
        /// Helper class for pixels. Used for storing information about neighboring pixels.
        /// </summary>
        private class BitmapPixel
        {
            public BitmapPixel(int x, int y)
            {
                X = x;
                Y = y;
                InRegion = false;
                Neighbors = new List<BitmapPixel>();
            }

            public int X, Y;
            public bool InRegion;
            public List<BitmapPixel> Neighbors;
        }

        /// <summary>
        /// Determines which regions have been modified.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="modded"></param>
        /// <returns>List of Rectangles that have been modified.</returns>
        private static List<Rectangle> ModdedRegions(Bitmap original, Bitmap modded)
        {
            // Exception checking
            if (original == null || modded == null)
            {
                throw new ArgumentNullException();
            }
            if (original.Width != modded.Width || original.Height != modded.Height)
            {
                throw new ArgumentException("Images to be merged need to have the same dimensions!");
            }

            // Find all non-matching pixels
            List<BitmapPixel> nonMatchingPixels = new List<BitmapPixel>();
            for (int y = 0; y < original.Height; ++y)
            {
                for (int x = 0; x < original.Width; ++x)
                {
                    if (original.GetPixel(x, y) != modded.GetPixel(x, y))
                        nonMatchingPixels.Add(new BitmapPixel(x, y));
                }
            }

            // Determine the neighbors of all pixels
            for (int i = 0; i < nonMatchingPixels.Count; ++i)
            {
                if (i % (nonMatchingPixels.Count / 10) == 0)
                    Msg.Log($"Non matching pixels processed: {(int)((i / (float)nonMatchingPixels.Count) * 100) + 1}% {i}/{nonMatchingPixels.Count}");

                for (int j = 0; j < nonMatchingPixels.Count; ++j)
                {
                    BitmapPixel pixel1 = nonMatchingPixels[i];
                    BitmapPixel pixel2 = nonMatchingPixels[j];

                    if (pixel1.X == pixel2.X && pixel1.Y == pixel2.Y) // same pixel if both coordinates match
                        continue;

                    // TODO: implement better/smoother neighbor detection, now they have to be exactly next to each other. e.g. With recoloring, the black lines are usually kept, so more modded regions are used than needed
                    if (Math.Abs(pixel1.X - pixel2.X) <= 1 && Math.Abs(pixel1.Y - pixel2.Y) <= 1)
                        pixel1.Neighbors.Add(pixel2);
                }
            }

            // Group them in rectangles
            List<Rectangle> moddedRegions = new List<Rectangle>();
            Queue<BitmapPixel> toVisit = new Queue<BitmapPixel>();

            foreach (BitmapPixel bitmapPixel in nonMatchingPixels)
            {
                if (bitmapPixel.InRegion) // skip if pixel has already been added to a region
                    continue;

                List<BitmapPixel> currentRegionPixels = new List<BitmapPixel>();

                toVisit.Enqueue(bitmapPixel);
                BitmapPixel refPixel = bitmapPixel;
                refPixel.InRegion = true;
                currentRegionPixels.Add(bitmapPixel);

                // As long as pixels have to be visited
                while (toVisit.Count > 0)
                {
                    BitmapPixel currentPixel = toVisit.Dequeue();

                    for (int i = 0; i < currentPixel.Neighbors.Count; ++i)
                    {
                        BitmapPixel neighbor = currentPixel.Neighbors[i];
                        if (neighbor.InRegion) // skip if the neighbor has already been added to a region
                            continue;

                        neighbor.InRegion = true;
                        currentRegionPixels.Add(neighbor);
                        toVisit.Enqueue(neighbor);
                    }
                }

                // Once all neighbors have been visited, add the rectangle to the modded regions
                Rectangle moddedRegion = new Rectangle();
                moddedRegion.X = currentRegionPixels.Min(pixel => pixel.X);
                moddedRegion.Width = currentRegionPixels.Max(pixel => pixel.X) - moddedRegion.X;
                moddedRegion.Y = currentRegionPixels.Min(pixel => pixel.Y);
                moddedRegion.Height = currentRegionPixels.Max(pixel => pixel.Y) - moddedRegion.Y;

                moddedRegions.Add(moddedRegion);
            }

            return moddedRegions;
        }

        /// <summary>
        /// Merges two images with each other. Compares the changes of the secondImage with the originalImage and applies those on the firstImage.
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="firstImage"></param>
        /// <param name="secondImage"></param>
        /// <returns>A bitmap that represents the changes of the second image with the original image applied on the first image.</returns>
        public static Bitmap MergeImages(Bitmap originalImage, Bitmap firstImage, Bitmap secondImage)
        {
            // Check the difference between the ORIGINAL and the modded image
            List<Rectangle> moddedRegions = ModdedRegions(originalImage, secondImage);

            Bitmap outputImage = new Bitmap(firstImage.Width, firstImage.Height, PixelFormat.Format32bppArgb); // 32 for transparency
            using (Graphics graphics = Graphics.FromImage(outputImage))
            {
                // Exclude all the modded regions so the original image is not drawn for those parts
                foreach (Rectangle rect in moddedRegions)
                    graphics.ExcludeClip(rect);

                // Draw the original image
                graphics.DrawImage(firstImage, new Rectangle(new Point(), firstImage.Size), new Rectangle(new Point(), firstImage.Size), GraphicsUnit.Pixel);

                // Reset the clip so the modded regions can be drawn, without resetting you can't draw on those regions
                Region modClipped = graphics.Clip.Clone(); // clone so this state can be inverted later on
                graphics.ResetClip();

                // Draw the modded part by inverting the previously clipped region
                graphics.SetClip(modClipped, CombineMode.Exclude); // invert the region
                graphics.DrawImage(secondImage, new Rectangle(new Point(), secondImage.Size), new Rectangle(new Point(), secondImage.Size), GraphicsUnit.Pixel);
            }

            return outputImage;
        }
    }
}