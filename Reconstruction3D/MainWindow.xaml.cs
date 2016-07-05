using MahApps.Metro.Controls;
using Reconstruction3D.ViewModels;
using SharpGL;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Assets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Reconstruction3D
{
    public partial class MainWindow : MetroWindow
    {
        Texture texture = new Texture();
        float rtri = 0;
        Point currentPoint = new Point();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Init(object sender, OpenGLEventArgs args)
        {
            var openGL = args.OpenGL;
            openGL.Enable(OpenGL.GL_TEXTURE_2D);
            texture.Create(openGL, Commands.TexturePath);
        }
        private void Draw(object sender, OpenGLEventArgs args)
        {
            var openGL = args.OpenGL;
            
            openGL.ClearColor(0f, 0f, 0f, 1f);
            openGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            openGL.LoadIdentity();
            openGL.Translate(0.0f, 0.0f, -6.0f);

            openGL.Rotate(rtri, 0.0f, 1.0f, 0.0f);

            texture.Bind(openGL);
            Commands.ChangeRenderMode(openGL);
            Commands.LoadTexture(texture, openGL);
            rtri += 1.0f;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if (e.ButtonState == MouseButtonState.Pressed)
            //    currentPoint = e.;

            Ellipse ellipse = new Ellipse();
            ellipse.Fill = Brushes.Sienna;
            ellipse.Width = 10;
            ellipse.Height = 10;
            ellipse.StrokeThickness = 2;

            canvas.Children.Add(ellipse);

            Canvas.SetLeft(ellipse, e.GetPosition(this).X);
            Canvas.SetTop(ellipse, e.GetPosition(this).Y);

        }

        //private void Resize(object sender, OpenGLEventArgs args)
        //{
        //    scene.CreateProjectionMatrix(args.OpenGL, (float)ActualWidth, (float)ActualHeight);
        //}
    }
}
