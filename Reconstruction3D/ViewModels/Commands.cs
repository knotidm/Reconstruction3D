using Commander;
using PropertyChanged;
using SharpGL;
using SharpGL.SceneGraph.Assets;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Primitives;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Forms;
using SharpGL.Enumerations;

namespace Reconstruction3D.ViewModels
{
    [ImplementPropertyChanged]
    public class Commands
    {
        public ObservableCollection<string> RenderModes { get; set; }
        public string ImagePath { get; set; }
        public static string TexturePath { get; set; } = "D:/Visual Studio/Reconstruction3D/Reconstruction3D/matCap/Crate.bmp";
        public static bool ToonShader { get; set; }
        public bool EditMode { get; set; }
        public static string SelectedRenderMode { get; set; }
        public Visibility ImageInfo { get; set; }
        public Commands()
        {
            RenderModes = new ObservableCollection<string> { "Retained Mode", "Immediate Mode" };
            ImageInfo = Visibility.Hidden;
        }
        [OnCommand("LoadImage")]
        public void LoadImage()
        {
            var openFileDialog = new OpenFileDialog() { Filter = @"JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif" };
            var result = openFileDialog.ShowDialog();
            switch (result)
            {
                case DialogResult.OK:
                    ImagePath = openFileDialog.FileName;
                    ImageInfo = Visibility.Visible;
                    break;
                case DialogResult.Cancel:
                    break;
            }
        }
        [OnCommand("LoadTexture")]
        public void LoadTexture()
        {
            var openFileDialog = new OpenFileDialog() { Filter = @"JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif" };
            var result = openFileDialog.ShowDialog();
            switch (result)
            {
                case DialogResult.OK:
                    TexturePath = openFileDialog.FileName;
                    break;
                case DialogResult.Cancel:
                    break;
            }
        }
        [OnCommand("VerticesToFace")]
        public void VerticesToFace()
        {

        }
        [OnCommand("Undo")]
        public void Undo()
        {

        }
        [OnCommand("Redo")]
        public void Redo()
        {

        }
        [OnCommand("FacesToMesh")]
        public void FacesToMesh()
        {

        }
        public static void ChangeRenderMode(OpenGL openGL)
        {
            switch (SelectedRenderMode)
            {
                case "Retained Mode":
                    {
                        RenderRetainedMode(openGL);
                        break;
                    }
                case "Immediate Mode":
                    {
                        var axies = new Axies();
                        axies.Render(openGL, RenderMode.Design);
                        RenderImmediateMode(openGL);
                        break;
                    }
            }
        }
        private static void RenderRetainedMode(OpenGL openGL)
        {
            Cube(openGL);
        }
        private static void RenderImmediateMode(OpenGL openGL)
        {
            openGL.PushAttrib(OpenGL.GL_POLYGON_BIT);
            openGL.PolygonMode(FaceMode.FrontAndBack, PolygonMode.Lines);

            Cube(openGL);
            openGL.PopAttrib();
        }
        public static void LoadTexture(Texture texture, OpenGL openGL)
        {
            texture.Destroy(openGL);
            texture.Create(openGL, TexturePath);
        }
        private static void Cube(OpenGL openGL)
        {
            openGL.Begin(OpenGL.GL_QUADS);

            // Front Face
            openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(-1.0f, -1.0f, 1.0f); // Bottom Left Of The Texture and Quad
            openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(1.0f, -1.0f, 1.0f); // Bottom Right Of The Texture and Quad
            openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(1.0f, 1.0f, 1.0f); // Top Right Of The Texture and Quad
            openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(-1.0f, 1.0f, 1.0f); // Top Left Of The Texture and Quad

            // Back Face
            openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(-1.0f, -1.0f, -1.0f); // Bottom Right Of The Texture and Quad
            openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(-1.0f, 1.0f, -1.0f); // Top Right Of The Texture and Quad
            openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(1.0f, 1.0f, -1.0f);  // Top Left Of The Texture and Quad
            openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(1.0f, -1.0f, -1.0f); // Bottom Left Of The Texture and Quad

            // Top Face
            openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(-1.0f, 1.0f, -1.0f); // Top Left Of The Texture and Quad
            openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(-1.0f, 1.0f, 1.0f);  // Bottom Left Of The Texture and Quad
            openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(1.0f, 1.0f, 1.0f);   // Bottom Right Of The Texture and Quad
            openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(1.0f, 1.0f, -1.0f);  // Top Right Of The Texture and Quad

            // Bottom Face
            openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(-1.0f, -1.0f, -1.0f); // Top Right Of The Texture and Quad
            openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(1.0f, -1.0f, -1.0f); // Top Left Of The Texture and Quad
            openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(1.0f, -1.0f, 1.0f);  // Bottom Left Of The Texture and Quad
            openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(-1.0f, -1.0f, 1.0f); // Bottom Right Of The Texture and Quad

            // Right face
            openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(1.0f, -1.0f, -1.0f); // Bottom Right Of The Texture and Quad
            openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(1.0f, 1.0f, -1.0f);  // Top Right Of The Texture and Quad
            openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(1.0f, 1.0f, 1.0f);   // Top Left Of The Texture and Quad
            openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(1.0f, -1.0f, 1.0f);  // Bottom Left Of The Texture and Quad

            // Left Face
            openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(-1.0f, -1.0f, -1.0f); // Bottom Left Of The Texture and Quad
            openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(-1.0f, -1.0f, 1.0f); // Bottom Right Of The Texture and Quad
            openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(-1.0f, 1.0f, 1.0f);  // Top Right Of The Texture and Quad
            openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(-1.0f, 1.0f, -1.0f);	// Top Left Of The Texture and Quad
            openGL.End();

            openGL.Flush();
        }
    }
}
