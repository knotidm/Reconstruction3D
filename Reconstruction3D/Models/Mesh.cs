using Reconstruction3D.ViewModels;
using SharpGL;
using SharpGL.SceneGraph.Assets;
using System.Collections.Generic;
using System.Windows;

namespace Reconstruction3D.Models
{
    public class Mesh
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<Point> Points { get; set; }

        Texture texture = new Texture();
        public Mesh(OpenGL openGL)
        {
            texture.Create(Commands.openGL, Commands.TexturePath);
            texture.Bind(Commands.openGL);
        }

        public void Draw(OpenGL openGL)
        {
            openGL.Begin(OpenGL.GL_QUADS);

            // Front Face
            openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(Points[0].X * 0.01, Points[0].Y * 0.01, 1.0f); // Bottom Left Of The Texture and Quad
            openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(Points[1].X * 0.01, Points[1].Y * 0.01, 1.0f); // Bottom Right Of The Texture and Quad
            openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(Points[2].X * 0.01, Points[2].Y * 0.01, 1.0f); // Top Right Of The Texture and Quad
            openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(Points[3].X * 0.01, Points[3].Y * 0.01, 1.0f); // Top Left Of The Texture and Quad

            // Back Face
            openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(Points[0].X * 0.01, Points[0].Y * 0.01, -1.0f); // Bottom Right Of The Texture and Quad
            openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(Points[3].X * 0.01, Points[3].Y * 0.01, -1.0f); // Top Right Of The Texture and Quad
            openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(Points[2].X * 0.01, Points[2].Y * 0.01, -1.0f);  // Top Left Of The Texture and Quad
            openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(Points[1].X * 0.01, Points[1].Y * 0.01, -1.0f); // Bottom Left Of The Texture and Quad

            // Top Face
            openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(Points[3].X * 0.01, Points[3].Y * 0.01, -1.0f); // Top Left Of The Texture and Quad
            openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(Points[3].X * 0.01, Points[3].Y * 0.01, 1.0f);  // Bottom Left Of The Texture and Quad
            openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(Points[2].X * 0.01, Points[2].Y * 0.01, 1.0f);   // Bottom Right Of The Texture and Quad
            openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(Points[2].X * 0.01, Points[2].Y * 0.01, -1.0f);  // Top Right Of The Texture and Quad

            // Bottom Face
            openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(Points[0].X * 0.01, Points[0].Y * 0.01, -1.0f); // Top Right Of The Texture and Quad
            openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(Points[1].X * 0.01, Points[1].Y * 0.01, -1.0f); // Top Left Of The Texture and Quad
            openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(Points[1].X * 0.01, Points[1].Y * 0.01, 1.0f);  // Bottom Left Of The Texture and Quad
            openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(Points[0].X * 0.01, Points[0].Y * 0.01, 1.0f); // Bottom Right Of The Texture and Quad

            // Right face
            openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(Points[1].X * 0.01, Points[1].Y * 0.01, -1.0f); // Bottom Right Of The Texture and Quad
            openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(Points[2].X * 0.01, Points[2].Y * 0.01, -1.0f);  // Top Right Of The Texture and Quad
            openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(Points[2].X * 0.01, Points[2].Y * 0.01, 1.0f);   // Top Left Of The Texture and Quad
            openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(Points[1].X * 0.01, Points[1].Y * 0.01, 1.0f);  // Bottom Left Of The Texture and Quad

            // Left Face
            openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(Points[0].X * 0.01, Points[0].Y * 0.01, -1.0f); // Bottom Left Of The Texture and Quad
            openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(Points[0].X * 0.01, Points[0].Y * 0.01, 1.0f); // Bottom Right Of The Texture and Quad
            openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(Points[3].X * 0.01, Points[3].Y * 0.01, 1.0f);  // Top Right Of The Texture and Quad
            openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(Points[3].X * 0.01, Points[3].Y * 0.01, -1.0f);	// Top Left Of The Texture and Quad
            openGL.End();

            openGL.Flush();
        }
    }
}