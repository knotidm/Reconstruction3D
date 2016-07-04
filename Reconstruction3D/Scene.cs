using GlmNet;
using SharpGL;
using SharpGL.Enumerations;
using SharpGL.SceneGraph.Assets;
using SharpGL.Shaders;
using System;
using System.Collections.Generic;

namespace Reconstruction3D
{
    public class Scene
    {
        private ShaderProgram matCap;
        private ShaderProgram shaderToon;

        private mat4 modelviewMatrix = mat4.identity();
        private mat4 projectionMatrix = mat4.identity();
        private mat3 normalMatrix = mat3.identity();

        private TrefoilKnot trefoilKnot;

        const uint positionAttribute = 0;
        const uint normalAttribute = 1;

        Texture texture = new Texture();

        public Scene(OpenGL openGL)
        {
            var attributeLocations = new Dictionary<uint, string>
            {
                {positionAttribute, "Position"},
                {normalAttribute, "Normal"},
            };

            matCap = new ShaderProgram();
            matCap.Create(openGL, ManifestResourceLoader.LoadTextFile(@"Shaders\matCap.vert"), ManifestResourceLoader.LoadTextFile(@"Shaders\matCap.frag"), attributeLocations);

            shaderToon = new ShaderProgram();
            shaderToon.Create(openGL, ManifestResourceLoader.LoadTextFile(@"Shaders\Toon.vert"), ManifestResourceLoader.LoadTextFile(@"Shaders\Toon.frag"), attributeLocations);
            texture.Create(openGL, "D:/Visual Studio/Reconstruction3D/Reconstruction3D/matCap/generator7.jpg");

            trefoilKnot = new TrefoilKnot(openGL, positionAttribute, normalAttribute);
        }
        public void CreateProjectionMatrix(OpenGL openGL, float screenWidth, float screenHeight)
        {
            const float S = 0.46f;
            float H = S * screenHeight / screenWidth;
            projectionMatrix = glm.frustum(-S, S, -H, H, 1, 100);

            openGL.MatrixMode(OpenGL.GL_PROJECTION);
            openGL.LoadIdentity();
            openGL.MultMatrix(projectionMatrix.to_array());
            openGL.MatrixMode(OpenGL.GL_MODELVIEW);
        }
        public void CreateModelviewAndNormalMatrix(float rotationAngle)
        {
            mat4 rotation = glm.rotate(mat4.identity(), rotationAngle, new vec3(0, 1, 0));
            mat4 translation = glm.translate(mat4.identity(), new vec3(0, 0, -4));
            modelviewMatrix = rotation * translation;
            normalMatrix = modelviewMatrix.to_mat3();
        }
        public void RenderImmediateMode(OpenGL openGL)
        {
            openGL.MatrixMode(OpenGL.GL_MODELVIEW);
            openGL.LoadIdentity();
            openGL.MultMatrix(modelviewMatrix.to_array());

            openGL.PushAttrib(OpenGL.GL_POLYGON_BIT);
            openGL.PolygonMode(FaceMode.FrontAndBack, PolygonMode.Lines);

            var vertices = trefoilKnot.Vertices;

            openGL.Begin(BeginMode.Triangles);
            foreach (var index in trefoilKnot.Indices)
            {
                openGL.Vertex(vertices[index].x, vertices[index].y, vertices[index].z);
            }
            Cube(openGL);
            openGL.End();

            openGL.PopAttrib();
        }
        public void RenderRetainedMode(OpenGL openGL, bool useToonShader)
        {
            var shader = useToonShader ? shaderToon : matCap;
            texture.Bind(openGL);

            shader.Bind(openGL);

            shader.SetUniform3(openGL, "DiffuseMaterial", 0f, 0.75f, 0.75f);
            shader.SetUniform3(openGL, "AmbientMaterial", 0.04f, 0.04f, 0.04f);
            shader.SetUniform3(openGL, "SpecularMaterial", 0.5f, 0.5f, 0.5f);
            shader.SetUniform1(openGL, "Shininess", 50f);

            shader.SetUniform3(openGL, "LightPosition", 0.25f, 0.25f, 1f);

            shader.SetUniformMatrix4(openGL, "Projection", projectionMatrix.to_array());
            shader.SetUniformMatrix4(openGL, "Modelview", modelviewMatrix.to_array());
            shader.SetUniformMatrix3(openGL, "NormalMatrix", normalMatrix.to_array());

            trefoilKnot.VertexBufferArray.Bind(openGL);
            //Cube(openGL);
            openGL.DrawElements(OpenGL.GL_TRIANGLES, trefoilKnot.Indices.Length, OpenGL.GL_UNSIGNED_SHORT, IntPtr.Zero);

            shader.Unbind(openGL);
        }

        public void Cube(OpenGL openGL)
        {
            openGL.Begin(OpenGL.GL_QUADS);

            // Front Face
            openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(-1.0f, -1.0f, 1.0f); // Bottom Left Of The Texture and Quad
            openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(1.0f, -1.0f, 1.0f);  // Bottom Right Of The Texture and Quad
            openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(1.0f, 1.0f, 1.0f);   // Top Right Of The Texture and Quad
            openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(-1.0f, 1.0f, 1.0f);  // Top Left Of The Texture and Quad

            // Back Face
            openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(-1.0f, -1.0f, -1.0f);    // Bottom Right Of The Texture and Quad
            openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(-1.0f, 1.0f, -1.0f); // Top Right Of The Texture and Quad
            openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(1.0f, 1.0f, -1.0f);  // Top Left Of The Texture and Quad
            openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(1.0f, -1.0f, -1.0f); // Bottom Left Of The Texture and Quad

            // Top Face
            openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(-1.0f, 1.0f, -1.0f); // Top Left Of The Texture and Quad
            openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(-1.0f, 1.0f, 1.0f);  // Bottom Left Of The Texture and Quad
            openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(1.0f, 1.0f, 1.0f);   // Bottom Right Of The Texture and Quad
            openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(1.0f, 1.0f, -1.0f);  // Top Right Of The Texture and Quad

            // Bottom Face
            openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(-1.0f, -1.0f, -1.0f);    // Top Right Of The Texture and Quad
            openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(1.0f, -1.0f, -1.0f); // Top Left Of The Texture and Quad
            openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(1.0f, -1.0f, 1.0f);  // Bottom Left Of The Texture and Quad
            openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(-1.0f, -1.0f, 1.0f); // Bottom Right Of The Texture and Quad

            // Right face
            openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(1.0f, -1.0f, -1.0f); // Bottom Right Of The Texture and Quad
            openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(1.0f, 1.0f, -1.0f);  // Top Right Of The Texture and Quad
            openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(1.0f, 1.0f, 1.0f);   // Top Left Of The Texture and Quad
            openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(1.0f, -1.0f, 1.0f);  // Bottom Left Of The Texture and Quad

            // Left Face
            openGL.TexCoord(0.0f, 0.0f); openGL.Vertex(-1.0f, -1.0f, -1.0f);    // Bottom Left Of The Texture and Quad
            openGL.TexCoord(1.0f, 0.0f); openGL.Vertex(-1.0f, -1.0f, 1.0f); // Bottom Right Of The Texture and Quad
            openGL.TexCoord(1.0f, 1.0f); openGL.Vertex(-1.0f, 1.0f, 1.0f);  // Top Right Of The Texture and Quad
            openGL.TexCoord(0.0f, 1.0f); openGL.Vertex(-1.0f, 1.0f, -1.0f);	// Top Left Of The Texture and Quad
            openGL.End();

            openGL.Flush();
        }
    }
}