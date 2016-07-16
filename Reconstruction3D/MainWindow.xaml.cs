using MahApps.Metro.Controls;
using Reconstruction3D.ViewModels;
using SharpGL;
using SharpGL.SceneGraph;

namespace Reconstruction3D
{
    public partial class MainWindow : MetroWindow
    {
        float rtri = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Init(object sender, OpenGLEventArgs args)
        {
            Commands.openGL = args.OpenGL;
            Commands.openGL.Enable(OpenGL.GL_TEXTURE_2D);
        }

        private void Draw(object sender, OpenGLEventArgs args)
        {
            Commands.openGL.ClearColor(0f, 0f, 0f, 1f);
            Commands.openGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            Commands.openGL.LoadIdentity();
            Commands.openGL.Translate(-3.0f, 2.0f, -5.0f);

            Commands.openGL.Rotate(180, 1.0f, 0.0f, 0.0f);

            Commands.ChangeRenderMode(Commands.openGL);
            rtri += 1.0f;
        }
    }
}
