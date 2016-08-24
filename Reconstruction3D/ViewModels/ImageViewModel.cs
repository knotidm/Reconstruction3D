using Commander;
using PropertyChanged;
using Reconstruction3D.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Reconstruction3D.ViewModels
{
    [ImplementPropertyChanged]
    public class ImageViewModel
    {
        public string ImagePath { get; set; }
        public Visibility ImageInfo { get; set; }
        public Point CurrentPoint { get; set; }
        public List<Point> PointsToAdd { get; set; }
        public string MeshName { get; set; }
        public ObservableCollection<string> MeshTypes { get; set; }
        public string SelectedMeshType { get; set; }

        public ImageViewModel()
        {
            ImageInfo = Visibility.Hidden;
            PointsToAdd = new List<Point>();
            MeshTypes = new ObservableCollection<string> { "Tylne Oparcie", "Boczne Oparcie", "Siedzenie", "Noga" };
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

        // UNDONE : Wycinanie tekstury z zaznaczonego obszaru ze zdjęcia
        [OnCommand("LeftClickOnImage")]
        public void LeftClickOnImage(Canvas canvas)
        {
            if (canvas.Children.Count == 9)
            {
                canvas.Children.RemoveRange(1, 9);
                PointsToAdd.Clear();
            }

            if (Mouse.LeftButton == MouseButtonState.Pressed && PointsToAdd.Count < 4)
            {
                CurrentPoint = Mouse.GetPosition(canvas);
                PointsToAdd.Add(CurrentPoint);

                var ellipse = new Ellipse()
                {
                    Fill = Brushes.Black,
                    Width = 4,
                    Height = 4,
                    StrokeThickness = 1
                };

                canvas.Children.Add(ellipse);

                Canvas.SetLeft(ellipse, CurrentPoint.X);
                Canvas.SetTop(ellipse, CurrentPoint.Y);

                if (PointsToAdd.Count > 1)
                {
                    MainWindowViewModel.i++;
                    var line = new Line()
                    {
                        Stroke = Brushes.Red,
                        X1 = PointsToAdd[MainWindowViewModel.i].X,
                        Y1 = PointsToAdd[MainWindowViewModel.i].Y,
                        X2 = PointsToAdd[PointsToAdd.Count - 1].X,
                        Y2 = PointsToAdd[PointsToAdd.Count - 1].Y
                    };
                    canvas.Children.Add(line);
                }

                if (PointsToAdd.Count == 4)
                {
                    var line = new Line()
                    {
                        Stroke = Brushes.Red,
                        X1 = PointsToAdd[PointsToAdd.Count - 1].X,
                        Y1 = PointsToAdd[PointsToAdd.Count - 1].Y,
                        X2 = PointsToAdd[0].X,
                        Y2 = PointsToAdd[0].Y
                    };

                    canvas.Children.Add(line);

                    // TODO: Zrobić żeby szerokość i wysokość tekstury same dopasowywały się do wycinanego obszaru
                    //var bitmap = CreateTexture.CropImage(CurrentPoint, ImagePath);
                    //bitmap.Save("C:/VISUAL STUDIO PROJECTS/Reconstruction3D/Reconstruction3D/Textures/Crate2.bmp");
                    MainWindowViewModel.i = -1;
                }
            }
        }

        [OnCommand("CreateMesh")]
        public void CreateMesh(Canvas canvas)
        {
            if (PointsToAdd.Count == 4)
            {
                MainWindowViewModel.Meshes.Add(new Mesh(_3DViewModel.openGL, MeshName, SelectedMeshType, new List<Point>(PointsToAdd), new Transformation(), MainWindowViewModel.TexturePath));
                MainWindowViewModel.i = -1;
            }
        }
    }
}