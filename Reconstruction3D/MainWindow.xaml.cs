using MahApps.Metro.Controls;
using Reconstruction3D.ViewModels;
using SharpGL;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Primitives;

namespace Reconstruction3D
{
    public partial class MainWindow : MetroWindow
    {
        private readonly Axies axies = new Axies();
        private float theta = 0;
        private Scene scene;

        float _rotatePyramid;
        float _rquad;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Init(object sender, OpenGLEventArgs args)
        {
            //args.OpenGL.Enable(OpenGL.GL_DEPTH_TEST);
            scene = new Scene(args.OpenGL);
        }
        private void Draw(object sender, OpenGLEventArgs args)
        {
            var openGL = args.OpenGL;
            theta += 0.01f;
            scene.CreateModelviewAndNormalMatrix(theta);

            openGL.ClearColor(0f, 0f, 0f, 1f);
            openGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);

            Commands.ChangeRenderMode(scene, openGL, axies);


            //openGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            //openGL.LoadIdentity();

            //openGL.Translate(-1.5f, 0.0f, -6.0f);
            //openGL.Rotate(_rotatePyramid, 0.0f, 1.0f, 0.0f);

            //openGL.Begin(OpenGL.GL_TRIANGLES);

            //openGL.Color(1.0f, 0.0f, 0.0f);
            //openGL.Vertex(0.0f, 1.0f, 0.0f);
            //openGL.Color(0.0f, 1.0f, 0.0f);
            //openGL.Vertex(-1.0f, -1.0f, 1.0f);
            //openGL.Color(0.0f, 0.0f, 1.0f);
            //openGL.Vertex(1.0f, -1.0f, 1.0f);

            //openGL.Color(1.0f, 0.0f, 0.0f);
            //openGL.Vertex(0.0f, 1.0f, 0.0f);
            //openGL.Color(0.0f, 0.0f, 1.0f);
            //openGL.Vertex(1.0f, -1.0f, 1.0f);
            //openGL.Color(0.0f, 1.0f, 0.0f);
            //openGL.Vertex(1.0f, -1.0f, -1.0f);

            //openGL.Color(1.0f, 0.0f, 0.0f);
            //openGL.Vertex(0.0f, 1.0f, 0.0f);
            //openGL.Color(0.0f, 1.0f, 0.0f);
            //openGL.Vertex(1.0f, -1.0f, -1.0f);
            //openGL.Color(0.0f, 0.0f, 1.0f);
            //openGL.Vertex(-1.0f, -1.0f, -1.0f);

            //openGL.Color(1.0f, 0.0f, 0.0f);
            //openGL.Vertex(0.0f, 1.0f, 0.0f);
            //openGL.Color(0.0f, 0.0f, 1.0f);
            //openGL.Vertex(-1.0f, -1.0f, -1.0f);
            //openGL.Color(0.0f, 1.0f, 0.0f);
            //openGL.Vertex(-1.0f, -1.0f, 1.0f);

            //openGL.End();

            //openGL.LoadIdentity();

            //openGL.Translate(1.5f, 0.0f, -7.0f);
            //openGL.Rotate(_rquad, 1.0f, 1.0f, 1.0f);

            //openGL.Begin(OpenGL.GL_QUADS);

            //openGL.Color(0.0f, 1.0f, 0.0f);
            //openGL.Vertex(1.0f, 1.0f, -1.0f);
            //openGL.Vertex(-1.0f, 1.0f, -1.0f);
            //openGL.Vertex(-1.0f, 1.0f, 1.0f);
            //openGL.Vertex(1.0f, 1.0f, 1.0f);

            //openGL.Color(1.0f, 0.5f, 0.0f);
            //openGL.Vertex(1.0f, -1.0f, 1.0f);
            //openGL.Vertex(-1.0f, -1.0f, 1.0f);
            //openGL.Vertex(-1.0f, -1.0f, -1.0f);
            //openGL.Vertex(1.0f, -1.0f, -1.0f);

            //openGL.Color(1.0f, 0.0f, 0.0f);
            //openGL.Vertex(1.0f, 1.0f, 1.0f);
            //openGL.Vertex(-1.0f, 1.0f, 1.0f);
            //openGL.Vertex(-1.0f, -1.0f, 1.0f);
            //openGL.Vertex(1.0f, -1.0f, 1.0f);

            //openGL.Color(1.0f, 1.0f, 0.0f);
            //openGL.Vertex(1.0f, -1.0f, -1.0f);
            //openGL.Vertex(-1.0f, -1.0f, -1.0f);
            //openGL.Vertex(-1.0f, 1.0f, -1.0f);
            //openGL.Vertex(1.0f, 1.0f, -1.0f);

            //openGL.Color(0.0f, 0.0f, 1.0f);
            //openGL.Vertex(-1.0f, 1.0f, 1.0f);
            //openGL.Vertex(-1.0f, 1.0f, -1.0f);
            //openGL.Vertex(-1.0f, -1.0f, -1.0f);
            //openGL.Vertex(-1.0f, -1.0f, 1.0f);

            //openGL.Color(1.0f, 0.0f, 1.0f);
            //openGL.Vertex(1.0f, 1.0f, -1.0f);
            //openGL.Vertex(1.0f, 1.0f, 1.0f);
            //openGL.Vertex(1.0f, -1.0f, 1.0f);
            //openGL.Vertex(1.0f, -1.0f, -1.0f);

            //openGL.End();

            //openGL.Flush();

            //_rotatePyramid += 3.0f;
            //_rquad -= 3.0f;
        }

        private void Resize(object sender, OpenGLEventArgs args)
        {
            scene.CreateProjectionMatrix(args.OpenGL, (float)ActualWidth, (float)ActualHeight);
        }
    }
}
