using Commander;
using PropertyChanged;
using Reconstruction3D.Models;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Reconstruction3D.ViewModels
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel
    {
        public static int i = -1;
        public static ObservableCollection<Mesh> Meshes { get; set; }
        public static Mesh SelectedMesh { get; set; }
        public static string TexturePath { get; set; }

        public MainWindowViewModel()
        {
            Meshes = new ObservableCollection<Mesh>();
        }

        [OnCommand("LoadTexture")]
        public void LoadTexture()
        {
            var openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                TexturePath = openFileDialog.FileName;
            }
        }

        [OnCommand("RedrawOnImage")]
        public void RedrawOnImage(Canvas canvas)
        {
            try
            {
                _3DViewModel.TranslateX = SelectedMesh.Transformation.TranslateX;
                _3DViewModel.TranslateY = SelectedMesh.Transformation.TranslateY;
                _3DViewModel.TranslateZ = SelectedMesh.Transformation.TranslateZ;
                _3DViewModel.RotateX = SelectedMesh.Transformation.RotateX;
                _3DViewModel.RotateY = SelectedMesh.Transformation.RotateY;
                _3DViewModel.RotateZ = SelectedMesh.Transformation.RotateZ;
                _3DViewModel.Depth = SelectedMesh.Transformation.Depth;
                canvas.Children.RemoveRange(1, 9);
                SelectedMesh.RedrawOnImage(canvas);
                i = -1;
            }
            catch (System.Exception)
            {

            }
        }

        [OnCommand("AddSelectedMesh")]
        public void AddSelectedMesh()
        {
            Meshes.Add(SelectedMesh);
        }

        [OnCommand("DeleteSelectedMesh")]
        public void DeleteSelectedMesh()
        {
            Meshes.Remove(SelectedMesh);
        }
    }
}