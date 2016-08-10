using System.Drawing;

namespace Reconstruction3D.ViewModels
{
    public class CreateTexture
    {
        // TODO: Tekstury muszą się tworzyć jako nowe kolejene pliki każdorazwowo
        public static Bitmap CropImage(System.Windows.Point location, string sourcePath)
        {
            Rectangle rectangle = new Rectangle((int)location.X, (int)location.Y, 256, 256);
            Bitmap bmp = new Bitmap(rectangle.Width, rectangle.Height);
            Graphics graphics = Graphics.FromImage(bmp);

            graphics.DrawImage(new Bitmap(sourcePath), (float)location.X, (float)location.Y, rectangle, GraphicsUnit.Pixel);
            return bmp;
        }
    }
}