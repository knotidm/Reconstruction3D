using Commander;
using PropertyChanged;
using SharpGL;
using SharpGL.SceneGraph.Assets;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Primitives;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using SharpGL.Enumerations;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Controls;
using Reconstruction3D.Models;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Reconstruction3D.ViewModels
{

    //TODO: Undo / Rendo Frmaweork
    [ImplementPropertyChanged]
    public class Commands
    {
        public static OpenGL openGL { get; set; }

        int i = -1;
        public ObservableCollection<string> RenderModes { get; set; }
        public ObservableCollection<string> MeshTypes { get; set; }
        public string SelectedType { get; set; }
        public ObservableCollection<Mesh> Meshes { get; set; }
        public static Mesh SelectedMesh { get; set; }
        public string ImagePath { get; set; }
        public string NewFaceName { get; set; }
        public static string TexturePath { get; set; } = "D:/Visual Studio/Reconstruction3D/Reconstruction3D/Textures/Crate.bmp";
        public static bool ToonShader { get; set; }
        public bool EditMode { get; set; }
        public static string SelectedRenderMode { get; set; }
        public Visibility ImageInfo { get; set; }
        public Point CurrentPoint { get; set; }
        public List<Point> PointsToAdd { get; set; }
        public Commands()
        {
            RenderModes = new ObservableCollection<string> { "Retained Mode", "Immediate Mode" };
            MeshTypes = new ObservableCollection<string> { "Tylne Oparcie", "Boczne Oparcie", "Siedzenie" };
            Meshes = new ObservableCollection<Mesh>();
            ImageInfo = Visibility.Hidden;
            PointsToAdd = new List<Point>();
        }
        [OnCommand("LoadImage")]
        public void LoadImage(Canvas canvas)
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
        public void VerticesToFace(Canvas canvas)
        {
            if (PointsToAdd.Count == 4)
            {
                Meshes.Add(new Mesh(openGL, NewFaceName, SelectedType, new List<Point>(PointsToAdd), TexturePath));
                i = -1;
            }
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
        [OnCommand("RedrawOnImage")]
        public void RedrawOnImage(Canvas canvas)
        {
            try
            {
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
        // UNDONE : 4 wierzchołek za każdym razem na nowo jest ustawiany
        // UNDONE : Wycinanie tekstury z zaznaczonego obszaru ze zdjęcia
        [OnCommand("ImageLeftClick")]
        public void ImageLeftClick(Canvas canvas)
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
                    i++;
                    var line = new Line()
                    {
                        Stroke = Brushes.Red,
                        X1 = PointsToAdd[i].X,
                        Y1 = PointsToAdd[i].Y,
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

                    var bitmap = CreateTexture.CropImage(CurrentPoint, ImagePath, PointsToAdd[1].X - PointsToAdd[0].X, PointsToAdd[1].Y - PointsToAdd[2].Y);
                    bitmap.Save("D:/Visual Studio/Reconstruction3D/Reconstruction3D/Textures/Crate.bmp");
                }
            }
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

        public static void RenderRetainedMode(OpenGL openGL)
        {
            if (SelectedMesh != null)
            {
                SelectedMesh.DrawMesh(openGL);
            }
        }
        public static void RenderImmediateMode(OpenGL openGL)
        {
            if (SelectedMesh != null)
            {
                openGL.PushAttrib(OpenGL.GL_POLYGON_BIT);
                openGL.PolygonMode(FaceMode.FrontAndBack, PolygonMode.Lines);
                SelectedMesh.DrawMesh(openGL);
                openGL.PopAttrib();
            }
        }
        public static void LoadTexture(Texture texture, OpenGL openGL)
        {
            texture.Destroy(openGL);
            texture.Create(openGL, TexturePath);
        }
    }
}