using Commander;
using PropertyChanged;
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
using UndoRedoFramework.Core;
using SharpGL.SceneGraph;
using SharpGL.WPF;

namespace Reconstruction3D.ViewModels
{
    //TODO: Undo / Redo Frmaweork
    [ImplementPropertyChanged]
    public class Commands
    {
        int i = -1;

        private UndoRedoContext undoRedoContext;

        #region Image Properties

        public string ImagePath { get; set; }
        public Visibility ImageInfo { get; set; }
        public Point CurrentPoint { get; set; }
        public List<Point> PointsToAdd { get; set; }
        public  ObservableCollection<Mesh> Meshes { get; set; }
        public  Mesh SelectedMesh { get; set; }
        public string MeshName { get; set; }
        public ObservableCollection<string> MeshTypes { get; set; }
        public string SelectedMeshType { get; set; }
        public string TexturePath { get; set; } = "D:\\Visual Studio\\Reconstruction3D\\Reconstruction3D\\Textures\\Crate.bmp";

        #endregion

        #region Mesh Properties

        SharpGL.OpenGL openGL { get; set; } = new SharpGL.OpenGL();
        public ObservableCollection<string> RenderModes { get; set; }
        public  string SelectedRenderMode { get; set; }
        public  bool DrawAll { get; set; }
        public bool EditMode { get; set; }
        public  float TranslateX { get; set; }
        public  float TranslateY { get; set; }
        public  float TranslateZ { get; set; }
        public  float ScaleX { get; set; }
        public  float ScaleY { get; set; }
        public  float ScaleZ { get; set; }
        public  float RotateX { get; set; }
        public  float RotateY { get; set; }
        public  float RotateZ { get; set; }
        #endregion

        public Commands()
        {
            undoRedoContext = new UndoRedoContext();
            RenderModes = new ObservableCollection<string> { "Retained Mode", "Immediate Mode" };
            MeshTypes = new ObservableCollection<string> { "Tylne Oparcie", "Boczne Oparcie", "Siedzenie" };
            Meshes = new ObservableCollection<Mesh>();
            ImageInfo = Visibility.Hidden;
            PointsToAdd = new List<Point>();
        }

        #region Undo / Redo Commands

        [OnCommand("Undo")]
        public ICommand Undo()
        {
            return undoRedoContext.GetUndoCommand();
        }
        [OnCommand("Redo")]
        public ICommand Redo()
        {
            return undoRedoContext.GetRedoCommand();
        }
        #endregion

        #region Image Commands

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

                    // TODO: Zrobić żeby szerokość i wysokość tekstury same dopasowywały się do wycinanego obszaru
                    //var bitmap = CreateTexture.CropImage(CurrentPoint, ImagePath, PointsToAdd[1].X - PointsToAdd[0].X, PointsToAdd[1].Y - PointsToAdd[2].Y);
                    //bitmap.Save("D:/Visual Studio/Reconstruction3D/Reconstruction3D/Textures/Crate.bmp");
                }
            }
        }

        [OnCommand("CreateMesh")]
        public void CreateMesh(Canvas canvas)
        {
            if (PointsToAdd.Count == 4)
            {
                Meshes.Add(new Mesh(openGL, MeshName, SelectedMeshType, new List<Point>(PointsToAdd), TexturePath));
                i = -1;
            }
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

        #endregion

        #region Mesh Commands

        [OnCommand("FacesToMesh")]
        public void FacesToMesh()
        {

        }

        #endregion

        #region Methods
        public  void ChangeRenderMode(SharpGL.OpenGL openGL)
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
        public  void RenderRetainedMode(SharpGL.OpenGL openGL)
        {
            if (DrawAll == true)
            {
                foreach (var mesh in Meshes)
                {
                    mesh.DrawMesh(openGL);
                    mesh.TranslateX = TranslateX;
                    mesh.TranslateY = TranslateY;
                    mesh.TranslateZ = TranslateZ;
                    mesh.ScaleX = ScaleX;
                    mesh.ScaleY = ScaleY;
                    mesh.ScaleZ = ScaleZ;
                    mesh.RotateX = RotateX;
                    mesh.RotateY = RotateY;
                    mesh.RotateZ = RotateZ;
                }
            }
            else
            {
                if (SelectedMesh != null)
                {
                    SelectedMesh.DrawMesh(openGL);
                    SelectedMesh.TranslateX = TranslateX;
                    SelectedMesh.TranslateY = TranslateY;
                    SelectedMesh.TranslateZ = TranslateZ;
                    SelectedMesh.ScaleX = ScaleX;
                    SelectedMesh.ScaleY = ScaleY;
                    SelectedMesh.ScaleZ = ScaleZ;
                    SelectedMesh.RotateX = RotateX;
                    SelectedMesh.RotateY = RotateY;
                    SelectedMesh.RotateZ = RotateZ;
                }
            }
        }
        public  void RenderImmediateMode(SharpGL.OpenGL openGL)
        {
            openGL.PushAttrib(SharpGL.OpenGL.GL_POLYGON_BIT);
            openGL.PolygonMode(FaceMode.FrontAndBack, PolygonMode.Lines);
            RenderRetainedMode(openGL);
            openGL.PopAttrib();
        }
        #endregion


        [OnCommand("Init")]
        public void Init(OpenGLControl openGLControl)
        {
            openGL = openGLControl.OpenGL;
            openGL.Enable(SharpGL.OpenGL.GL_TEXTURE_2D);
        }

        [OnCommand("Draw")]
        public void Draw()
        {
            openGL.ClearColor(0f, 0f, 0f, 1f);
            openGL.Clear(SharpGL.OpenGL.GL_COLOR_BUFFER_BIT | SharpGL.OpenGL.GL_DEPTH_BUFFER_BIT);

            openGL.LoadIdentity();
            openGL.Translate(-3.0f, 2.0f, -5.0f);

            openGL.Rotate(180, 1.0f, 0.0f, 0.0f);

            ChangeRenderMode(openGL);
        }
    }
}