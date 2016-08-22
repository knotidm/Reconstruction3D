using Commander;
using PropertyChanged;
using Reconstruction3D.Models;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Reconstruction3D.ViewModels
{
    [ImplementPropertyChanged]

    public class MainWindowViewModel
    {
        public static int i = -1;
        public static SharpGL.OpenGL openGL { get; set; }
        public static ObservableCollection<Mesh> Meshes { get; set; }
        public static Mesh SelectedMesh { get; set; }

        [OnCommand("RedrawOnImage")]
        public void RedrawOnImage(Canvas canvas)
        {
            try
            {
                TranslateX = SelectedMesh.Transformation.TranslateX;
                TranslateY = SelectedMesh.Transformation.TranslateY;
                TranslateZ = SelectedMesh.Transformation.TranslateZ;
                RotateX = SelectedMesh.Transformation.RotateX;
                RotateY = SelectedMesh.Transformation.RotateY;
                RotateZ = SelectedMesh.Transformation.RotateZ;
                Depth = SelectedMesh.Transformation.Depth;
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
