using Commander;
using PropertyChanged;
using SharpGL.Enumerations;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Primitives;
using SharpGL.WPF;
using System.Collections.ObjectModel;

namespace Reconstruction3D.ViewModels
{
    [ImplementPropertyChanged]
    class _3DViewModel
    {
        public static SharpGL.OpenGL openGL { get; set; }
        public ObservableCollection<string> RenderModes { get; set; }
        public string SelectedRenderMode { get; set; }
        public bool DrawAll { get; set; }
        public bool EditMode { get; set; }
        public static float TranslateX { get; set; }
        public static float TranslateY { get; set; }
        public static float TranslateZ { get; set; }
        public static float RotateX { get; set; }
        public static float RotateY { get; set; }
        public static float RotateZ { get; set; }
        public static float Depth { get; set; }

        public _3DViewModel()
        {
            openGL = new SharpGL.OpenGL();
            RenderModes = new ObservableCollection<string> { "Retained Mode", "Immediate Mode" };
        }

        [OnCommand("Init")]
        public void Init(OpenGLControl openGLControl)
        {
            openGL = openGLControl.OpenGL;
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
        // TODO: FacesToMesh
        [OnCommand("FacesToMesh")]
        public void FacesToMesh()
        {

        }

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
                foreach (var mesh in MainWindowViewModel.Meshes)
                {
                    mesh.Draw(openGL);
                }
            }
            else
            {
                if (MainWindowViewModel.SelectedMesh != null)
                {
                    MainWindowViewModel.SelectedMesh.Draw(openGL);
                    MainWindowViewModel.SelectedMesh.Transformation.TranslateX = TranslateX;
                    MainWindowViewModel.SelectedMesh.Transformation.TranslateY = TranslateY;
                    MainWindowViewModel.SelectedMesh.Transformation.TranslateZ = TranslateZ;
                    MainWindowViewModel.SelectedMesh.Transformation.RotateX = RotateX;
                    MainWindowViewModel.SelectedMesh.Transformation.RotateY = RotateY;
                    MainWindowViewModel.SelectedMesh.Transformation.RotateZ = RotateZ;
                    MainWindowViewModel.SelectedMesh.Transformation.Depth = Depth;
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
    }
}
