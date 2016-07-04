using Commander;
using PropertyChanged;
using SharpGL;
using SharpGL.SceneGraph.Assets;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Primitives;
using SharpGL.WPF;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Forms;

namespace Reconstruction3D.ViewModels
{
    [ImplementPropertyChanged]
    public class Commands
    {
        public ObservableCollection<string> RenderModes { get; set; }
        public string ImagePath { get; set; }
        public static string TexturePath { get; set; } = "D:/Visual Studio/Reconstruction3D/Reconstruction3D/matCap/generator7.jpg";
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
        public static void ChangeRenderMode(Scene scene, OpenGL openGL, Axies axies)
        {
            switch (SelectedRenderMode)
            {
                case "Retained Mode":
                    {
                        scene.RenderRetainedMode(openGL, ToonShader);
                        break;
                    }
                case "Immediate Mode":
                    {
                        axies.Render(openGL, RenderMode.Design);
                        scene.RenderImmediateMode(openGL);
                        break;
                    }
            }
        }

        public static void LoadTexture(Texture texture, OpenGL openGL)
        {
            texture.Destroy(openGL);
            texture.Create(openGL, TexturePath);
        }
    }
}
