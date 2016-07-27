using System.Drawing;

namespace Reconstruction3D.ViewModels
{
    public class CreateTexture
    {
        // TODO: Tekstury muszą się tworzyć jako nowe kolejene pliki każdorazwowo
        public  Bitmap CropImage(System.Windows.Point location, string sourcePath, double width, double height)
        {
            Rectangle section = new Rectangle((int)location.X, (int)location.Y, (int)width, (int)height);
            // An empty bitmap which will hold the cropped image
            Bitmap bmp = new Bitmap(section.Width, section.Height);

            Graphics g = Graphics.FromImage(bmp);

            // Draw the given area (section) of the source image
            // at location 0,0 on the empty bitmap (bmp)
            g.DrawImage(new Bitmap(sourcePath), (float)location.X, (float)location.Y, section, GraphicsUnit.Pixel);
            return bmp;
        }

        // Example use:
        // Point location = new Point(0, 0);
        // Bitmap source = new Bitmap(@"C:\tulips.jpg");
        // Rectangle section = new Rectangle(new Point(12, 50), new Size(150, 150));

        //Bitmap CroppedImage = CropImage(location, source, section);
    }
}
