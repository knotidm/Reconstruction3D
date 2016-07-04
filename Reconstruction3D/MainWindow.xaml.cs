using MahApps.Metro.Controls;
using Reconstruction3D.ViewModels;
using SharpGL;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Assets;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Shaders;

namespace Reconstruction3D
{
    public partial class MainWindow : MetroWindow
    {
        private readonly Axies axies = new Axies();
        private float theta = 0;
        private Scene scene;
        //Texture texture = new Texture();
        //float rtri = 0;
        //ShaderProgram matCap;
       
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Init(object sender, OpenGLEventArgs args)
        {
            var openGL = args.OpenGL;
            //openGL.Enable(OpenGL.GL_DEPTH_TEST);
            
            //float[] global_ambient = new float[] { 0.5f, 0.5f, 0.5f, 1.0f };
            //float[] light0pos = new float[] { 0.0f, 5.0f, 10.0f, 1.0f };
            //float[] light0ambient = new float[] { 0.2f, 0.2f, 0.2f, 1.0f };
            //float[] light0diffuse = new float[] { 0.3f, 0.3f, 0.3f, 1.0f };
            //float[] light0specular = new float[] { 0.8f, 0.8f, 0.8f, 1.0f };

            //float[] lmodel_ambient = new float[] { 0.2f, 0.2f, 0.2f, 1.0f };
            //openGL.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, lmodel_ambient);

            //openGL.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, global_ambient);
            //openGL.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, light0pos);
            //openGL.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, light0ambient);
            //openGL.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, light0diffuse);
            //openGL.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPECULAR, light0specular);
            //openGL.Enable(OpenGL.GL_LIGHTING);
            //openGL.Enable(OpenGL.GL_LIGHT0);

            //openGL.ShadeModel(OpenGL.GL_SMOOTH);

            //openGL.Enable(OpenGL.GL_TEXTURE_2D);
            scene = new Scene(openGL);
            //texture.Create(openGL, "D:/Visual Studio/Reconstruction3D/Reconstruction3D/matCap/generator7.jpg");

            //matCap = new ShaderProgram();

            ////  Create a vertex shader.
            //VertexShader vertexShader = new VertexShader();
            //vertexShader.CreateInContext(openGL);
            //vertexShader.LoadSource("D:/Visual Studio/Reconstruction3D/Reconstruction3D/Shaders/matCap.vert");

            ////  Create a fragment shader.
            //FragmentShader fragmentShader = new FragmentShader();
            //fragmentShader.CreateInContext(openGL);
            //fragmentShader.LoadSource("D:/Visual Studio/Reconstruction3D/Reconstruction3D/Shaders/matCap.frag");

            ////  Compile them both.
            //vertexShader.Compile();
            //fragmentShader.Compile();

            ////  Build a program.
            //matCap.CreateInContext(openGL);

            ////  Attach the shaders.
            //matCap.AttachShader(vertexShader);
            //matCap.AttachShader(fragmentShader);
            //matCap.Link();
            
        }
        private void Draw(object sender, OpenGLEventArgs args)
        {
            var openGL = args.OpenGL;
            theta += 0.01f;
            scene.CreateModelviewAndNormalMatrix(theta);

            openGL.ClearColor(0f, 0f, 0f, 1f);
            openGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);

            //openGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            //openGL.LoadIdentity();
            //openGL.Translate(0.0f, 0.0f, -6.0f);

            //openGL.Rotate(rtri, 0.0f, 1.0f, 0.0f);

            ////  Bind the texture.
            //texture.Bind(openGL);

            ////matCap.Push(openGL, null);
            //openGL.Begin(OpenGL.GL_QUADS);

            //// Front Face
            //openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(-1.0f, -1.0f, 1.0f); // Bottom Left Of The Texture and Quad
            //openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(1.0f, -1.0f, 1.0f);  // Bottom Right Of The Texture and Quad
            //openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(1.0f, 1.0f, 1.0f);   // Top Right Of The Texture and Quad
            //openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(-1.0f, 1.0f, 1.0f);  // Top Left Of The Texture and Quad

            //// Back Face
            //openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(-1.0f, -1.0f, -1.0f);    // Bottom Right Of The Texture and Quad
            //openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(-1.0f, 1.0f, -1.0f); // Top Right Of The Texture and Quad
            //openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(1.0f, 1.0f, -1.0f);  // Top Left Of The Texture and Quad
            //openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(1.0f, -1.0f, -1.0f); // Bottom Left Of The Texture and Quad

            //// Top Face
            //openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(-1.0f, 1.0f, -1.0f); // Top Left Of The Texture and Quad
            //openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(-1.0f, 1.0f, 1.0f);  // Bottom Left Of The Texture and Quad
            //openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(1.0f, 1.0f, 1.0f);   // Bottom Right Of The Texture and Quad
            //openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(1.0f, 1.0f, -1.0f);  // Top Right Of The Texture and Quad

            //// Bottom Face
            //openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(-1.0f, -1.0f, -1.0f);    // Top Right Of The Texture and Quad
            //openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(1.0f, -1.0f, -1.0f); // Top Left Of The Texture and Quad
            //openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(1.0f, -1.0f, 1.0f);  // Bottom Left Of The Texture and Quad
            //openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(-1.0f, -1.0f, 1.0f); // Bottom Right Of The Texture and Quad

            //// Right face
            //openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(1.0f, -1.0f, -1.0f); // Bottom Right Of The Texture and Quad
            //openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(1.0f, 1.0f, -1.0f);  // Top Right Of The Texture and Quad
            //openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(1.0f, 1.0f, 1.0f);   // Top Left Of The Texture and Quad
            //openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(1.0f, -1.0f, 1.0f);  // Bottom Left Of The Texture and Quad

            //// Left Face
            //openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(-1.0f, -1.0f, -1.0f);    // Bottom Left Of The Texture and Quad
            //openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(-1.0f, -1.0f, 1.0f); // Bottom Right Of The Texture and Quad
            //openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(-1.0f, 1.0f, 1.0f);  // Top Right Of The Texture and Quad
            //openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(-1.0f, 1.0f, -1.0f);	// Top Left Of The Texture and Quad
            //openGL.End();

            //openGL.Flush();
            ////matCap.Pop(openGL, null);

            //rtri += 1.0f;// 0.2f;						// Increase The Rotation Variable For The Triangle 

            Commands.ChangeRenderMode(scene, openGL, axies);
            //Commands.LoadTexture(texture, openGL);
        }

        private void Resize(object sender, OpenGLEventArgs args)
        {
            scene.CreateProjectionMatrix(args.OpenGL, (float)ActualWidth, (float)ActualHeight);
        }
    }
}
