using Commander;
using PropertyChanged;
using SharpGL;
using SharpGL.SceneGraph;

namespace Reconstruction3D.ViewModels
{
    [ImplementPropertyChanged]
    public class OpenGLWindow
    {

        float _rotatePyramid;
        float _rotateQuad;

        [OnCommand("Init")]
        public void Init(object sender, OpenGLEventArgs args)
        {
            //  Enable the OpenGL depth testing functionality.
            args.OpenGL.Enable(OpenGL.GL_DEPTH_TEST);
        }

        [OnCommand("Draw")]
        public void Draw(object sender, OpenGLEventArgs args)
        {
            //  Get the OpenGL instance that's been passed to us.
            var openGL = args.OpenGL;

            //  Clear the color and depth buffers.
            openGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            //  Reset the modelview matrix.
            openGL.LoadIdentity();

            //  Move the geometry into a fairly central position.
            openGL.Translate(-1.5f, 0.0f, -6.0f);

            //  Draw a pyramid. First, rotate the modelview matrix.
            openGL.Rotate(_rotatePyramid, 0.0f, 1.0f, 0.0f);

            //  Start drawing triangles.
            openGL.Begin(OpenGL.GL_TRIANGLES);

            openGL.Color(1.0f, 0.0f, 0.0f);
            openGL.Vertex(0.0f, 1.0f, 0.0f);
            openGL.Color(0.0f, 1.0f, 0.0f);
            openGL.Vertex(-1.0f, -1.0f, 1.0f);
            openGL.Color(0.0f, 0.0f, 1.0f);
            openGL.Vertex(1.0f, -1.0f, 1.0f);

            openGL.Color(1.0f, 0.0f, 0.0f);
            openGL.Vertex(0.0f, 1.0f, 0.0f);
            openGL.Color(0.0f, 0.0f, 1.0f);
            openGL.Vertex(1.0f, -1.0f, 1.0f);
            openGL.Color(0.0f, 1.0f, 0.0f);
            openGL.Vertex(1.0f, -1.0f, -1.0f);

            openGL.Color(1.0f, 0.0f, 0.0f);
            openGL.Vertex(0.0f, 1.0f, 0.0f);
            openGL.Color(0.0f, 1.0f, 0.0f);
            openGL.Vertex(1.0f, -1.0f, -1.0f);
            openGL.Color(0.0f, 0.0f, 1.0f);
            openGL.Vertex(-1.0f, -1.0f, -1.0f);

            openGL.Color(1.0f, 0.0f, 0.0f);
            openGL.Vertex(0.0f, 1.0f, 0.0f);
            openGL.Color(0.0f, 0.0f, 1.0f);
            openGL.Vertex(-1.0f, -1.0f, -1.0f);
            openGL.Color(0.0f, 1.0f, 0.0f);
            openGL.Vertex(-1.0f, -1.0f, 1.0f);

            openGL.End();

            //  Reset the modelview.
            openGL.LoadIdentity();

            //  Move into a more central position.
            openGL.Translate(1.5f, 0.0f, -7.0f);

            //  Rotate the cube.
            openGL.Rotate(_rotateQuad, 1.0f, 1.0f, 1.0f);

            //  Provide the cube colors and geometry.
            openGL.Begin(OpenGL.GL_QUADS);

            openGL.Color(0.0f, 1.0f, 0.0f);
            openGL.Vertex(1.0f, 1.0f, -1.0f);
            openGL.Vertex(-1.0f, 1.0f, -1.0f);
            openGL.Vertex(-1.0f, 1.0f, 1.0f);
            openGL.Vertex(1.0f, 1.0f, 1.0f);

            openGL.Color(1.0f, 0.5f, 0.0f);
            openGL.Vertex(1.0f, -1.0f, 1.0f);
            openGL.Vertex(-1.0f, -1.0f, 1.0f);
            openGL.Vertex(-1.0f, -1.0f, -1.0f);
            openGL.Vertex(1.0f, -1.0f, -1.0f);

            openGL.Color(1.0f, 0.0f, 0.0f);
            openGL.Vertex(1.0f, 1.0f, 1.0f);
            openGL.Vertex(-1.0f, 1.0f, 1.0f);
            openGL.Vertex(-1.0f, -1.0f, 1.0f);
            openGL.Vertex(1.0f, -1.0f, 1.0f);

            openGL.Color(1.0f, 1.0f, 0.0f);
            openGL.Vertex(1.0f, -1.0f, -1.0f);
            openGL.Vertex(-1.0f, -1.0f, -1.0f);
            openGL.Vertex(-1.0f, 1.0f, -1.0f);
            openGL.Vertex(1.0f, 1.0f, -1.0f);

            openGL.Color(0.0f, 0.0f, 1.0f);
            openGL.Vertex(-1.0f, 1.0f, 1.0f);
            openGL.Vertex(-1.0f, 1.0f, -1.0f);
            openGL.Vertex(-1.0f, -1.0f, -1.0f);
            openGL.Vertex(-1.0f, -1.0f, 1.0f);

            openGL.Color(1.0f, 0.0f, 1.0f);
            openGL.Vertex(1.0f, 1.0f, -1.0f);
            openGL.Vertex(1.0f, 1.0f, 1.0f);
            openGL.Vertex(1.0f, -1.0f, 1.0f);
            openGL.Vertex(1.0f, -1.0f, -1.0f);

            openGL.End();

            //  Flush OpenGL.
            openGL.Flush();

            //  Rotate the geometry a bit.
            _rotatePyramid += 3.0f;
            _rotateQuad -= 3.0f;

        }
        
        [OnCommand("Resize")]
        public void Resize(object sender, OpenGLEventArgs args)
        {
            // Get the OpenGL instance.
            OpenGL openGL = args.OpenGL;

            // Load and clear the projection matrix.
            openGL.MatrixMode(OpenGL.GL_PROJECTION);
            openGL.LoadIdentity();

            // Perform a perspective transformation
            openGL.Perspective(45.0f, openGL.RenderContextProvider.Width / (float)openGL.RenderContextProvider.Height, 0.1f, 100.0f);

            // Load the modelview.
            openGL.MatrixMode(OpenGL.GL_MODELVIEW);
        }
    }
}
