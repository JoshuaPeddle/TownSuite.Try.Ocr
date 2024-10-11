using Tesseract;

namespace TownSuite.Web.Ocr
{
    public static class ImagePreprocessor
    {
        public static Pix Preprocess(Pix image)
        {
            image = Deskew(image);
            image = ConvertToGrayscale(image);
            image = Denoise(image);

            return image;
        }

        private static Pix ConvertToGrayscale(Pix image)
        {
            if (image.Depth == 32)
            {
                image = image.ConvertRGBToGray();
            }
            return image;
        }

        private static Pix Denoise(Pix image)
        {
            if (image.Depth == 32 || image.Depth == 8 && (image.Width > 1000 || image.Height > 1000))
            {
                try
                {
                    image = image.Despeckle(Pix.SEL_STR2, 2);
                }
                catch
                {
                    // If the image has very few or no speckles, or if it is highly complex, the despeckle algorithm might not be able to process it effectively and will throw an exception.
                }
            }
            return image;
        }

        private static Pix Deskew(Pix image)
        {
            return image.Deskew();
        }
    }
}