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
using SharpGL.WPF;
using System.Windows.Controls.Primitives;

namespace Reconstruction3D.ViewModels
{
    //TODO: Undo / Redo Frmaweork
    [ImplementPropertyChanged]
    public class Commands
    {
        int i = -1;
        public Thumb thumb;

        #region Image Properties

        public string ImagePath { get; set; }
        public Visibility ImageInfo { get; set; }
        public Point CurrentPoint { get; set; }
        public List<Point> PointsToAdd { get; set; }
        public ObservableCollection<Mesh> Meshes { get; set; }
        public Mesh SelectedMesh { get; set; }
        public string MeshName { get; set; }
        public ObservableCollection<string> MeshTypes { get; set; }
        public string SelectedMeshType { get; set; }

        #endregion

        #region Mesh Properties

        SharpGL.OpenGL openGL { get; set; }
        public ObservableCollection<string> RenderModes { get; set; }
        public string SelectedRenderMode { get; set; }
        public bool DrawAll { get; set; }
        public bool EditMode { get; set; }
        public float TranslateX { get; set; }
        public float TranslateY { get; set; }
        public float TranslateZ { get; set; }
        public float RotateX { get; set; }
        public float RotateY { get; set; }
        public float RotateZ { get; set; }
        public float Depth { get; set; }

        #endregion

        public Commands()
        {
            openGL = new SharpGL.OpenGL();
            RenderModes = new ObservableCollection<string> { "Retained Mode", "Immediate Mode" };
            MeshTypes = new ObservableCollection<string> { "Tylne Oparcie", "Boczne Oparcie", "Siedzenie" };
            Meshes = new ObservableCollection<Mesh>();
            ImageInfo = Visibility.Hidden;
            PointsToAdd = new List<Point>();
        }

        #region Image Commands

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
        public void LoadTexture(OpenGLControl openGLControl)
        {
            if (SelectedMesh != null)
            {
                SelectedMesh.LoadTexture(openGLControl);
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

                thumb = new Thumb()
                {
                    
                };
                canvas.Children.Add(thumb);

                Canvas.SetLeft(thumb, CurrentPoint.X);
                Canvas.SetTop(thumb, CurrentPoint.Y);

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
                    //var bitmap = CreateTexture.CropImage(CurrentPoint, ImagePath);
                    //bitmap.Save("C:/VISUAL STUDIO PROJECTS/Reconstruction3D/Reconstruction3D/Textures/Crate2.bmp");
                    i = -1;
                }

                thumb.DragDelta += Thumb_DragDelta;
            }
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Canvas.SetLeft(thumb, Canvas.GetLeft(thumb) + e.HorizontalChange);
            Canvas.SetTop(thumb, Canvas.GetTop(thumb) + e.VerticalChange);            
        }

        [OnCommand("CreateMesh")]
        public void CreateMesh(Canvas canvas)
        {
            if (PointsToAdd.Count == 4)
            {
                Meshes.Add(new Mesh(openGL, MeshName, SelectedMeshType, new List<Point>(PointsToAdd), new Transformation(), new SharpGL.SceneGraph.Assets.Texture()));
                i = -1;
            }
        }

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

        #endregion

        #region Mesh Commands

        [OnCommand("Init")]
        public void Init(OpenGLControl openGLControl)
        {
            openGL = openGLControl.OpenGL;
        }

        [OnCommand("Draw")]
        public void Draw(OpenGLControl openGLControl)
        {
            openGL.ClearColor(0f, 0f, 0f, 1f);
            openGL.Clear(SharpGL.OpenGL.GL_COLOR_BUFFER_BIT | SharpGL.OpenGL.GL_DEPTH_BUFFER_BIT);

            openGL.LoadIdentity();
            openGL.Translate(-3.0f, 2.0f, -5.0f);

            openGL.Rotate(180, 1.0f, 0.0f, 0.0f);

            GlobalRotate(openGLControl);
            ChangeRenderMode(openGL);
        }

        [OnCommand("GlobalRotate")]
        public void GlobalRotate(OpenGLControl openGLControl)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && EditMode == false && openGLControl.IsMouseOver)
            {
                
                openGL.Rotate((float)Mouse.GetPosition(openGLControl).Y, (float)Mouse.GetPosition(openGLControl).X, 0);
            }
        }

        // TODO: FacesToMesh
        [OnCommand("FacesToMesh")]
        public void FacesToMesh()
        {

        }

        #region Methods
        public void ChangeRenderMode(SharpGL.OpenGL openGL)
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
                        RenderImmediateMode(openGL);
                        break;
                    }
            }
        }
        public void RenderRetainedMode(SharpGL.OpenGL openGL)
        {
            var axies = new Axies();
            axies.Render(openGL, RenderMode.Design);

            if (DrawAll == true)
            {
                foreach (var mesh in Meshes)
                {
                    mesh.DrawMesh(openGL);
                    //mesh.Transformation.TranslateX = TranslateX;
                    //mesh.Transformation.TranslateY = TranslateY;
                    //mesh.Transformation.TranslateZ = TranslateZ;
                    //mesh.Transformation.RotateX = RotateX;
                    //mesh.Transformation.RotateY = RotateY;
                    //mesh.Transformation.RotateZ = RotateZ;
                    //mesh.Transformation.Depth = Depth;
                }
            }
            else
            {
                if (SelectedMesh != null)
                {
                    SelectedMesh.DrawMesh(openGL);
                    SelectedMesh.Transformation.TranslateX = TranslateX;
                    SelectedMesh.Transformation.TranslateY = TranslateY;
                    SelectedMesh.Transformation.TranslateZ = TranslateZ;
                    SelectedMesh.Transformation.RotateX = RotateX;
                    SelectedMesh.Transformation.RotateY = RotateY;
                    SelectedMesh.Transformation.RotateZ = RotateZ;
                    SelectedMesh.Transformation.Depth = Depth;
                }
            }
        }
        public void RenderImmediateMode(SharpGL.OpenGL openGL)
        {
            openGL.PushAttrib(SharpGL.OpenGL.GL_POLYGON_BIT);
            openGL.PolygonMode(FaceMode.FrontAndBack, PolygonMode.Lines);
            RenderRetainedMode(openGL);
            openGL.PopAttrib();
        }

        #endregion

        #endregion
    }
}