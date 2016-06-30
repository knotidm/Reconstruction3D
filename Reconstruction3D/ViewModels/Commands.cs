using System.Windows.Forms;
using Commander;
using PropertyChanged;
using SharpGL;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Core;
using System.Collections.ObjectModel;
using System.Windows;

namespace Reconstruction3D.ViewModels
{
    [ImplementPropertyChanged]
    public class Commands
    {
        public ObservableCollection<string> RenderModes { get; set; }
        public string ImagePath { get; set; }
        public static bool ToonShader { get; set; }
        public bool EditMode { get; set; }
        public static string SelectedRenderMode { get; set; }
        public Visibility ImageInfo { get; set; }

        public Commands()
        {
            RenderModes = new ObservableCollection<string> { "Retained Mode", "Immediate Mode" };
            ImageInfo = Visibility.Hidden;
        }

        [OnCommand("OpenFile")]
        public void OpenFile()
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
    }
}
