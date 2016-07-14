using MahApps.Metro.Controls;
using Reconstruction3D.ViewModels;
using SharpGL;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Assets;
using System.Collections.Generic;
using System.Windows;

namespace Reconstruction3D
{
    public partial class MainWindow : MetroWindow
    {
        Texture texture = new Texture();
        float rtri = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Init(object sender, OpenGLEventArgs args)
        {
            Commands.openGL = args.OpenGL;
            Commands.openGL.Enable(OpenGL.GL_TEXTURE_2D);
            texture.Create(Commands.openGL, Commands.TexturePath);
        }

        private void Draw(object sender, OpenGLEventArgs args)
        {

            Commands.openGL.ClearColor(0f, 0f, 0f, 1f);
            Commands.openGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            Commands.openGL.LoadIdentity();
            Commands.openGL.Translate(0.0f, 0.0f, -6.0f);

            Commands.openGL.Rotate(rtri, 0.0f, 1.0f, 0.0f);

            texture.Bind(Commands.openGL);
            Commands.ChangeRenderMode(Commands.openGL);
            Commands.LoadTexture(texture, Commands.openGL);
            rtri += 1.0f;
        }
        
        //private void Resize(object sender, OpenGLEventArgs args)
        //{
        //    scene.CreateProjectionMatrix(args.OpenGL, (float)ActualWidth, (float)ActualHeight);
        //}
    }
}
