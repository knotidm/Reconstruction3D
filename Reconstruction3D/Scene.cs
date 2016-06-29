using System;
using System.Collections.Generic;
using GlmNet;
using SharpGL;
using SharpGL.Enumerations;
using SharpGL.Shaders;

namespace Reconstruction3D
{
    public class Scene
    {
        private ShaderProgram shaderPerPixel;
        private ShaderProgram shaderToon;

        private mat4 modelviewMatrix = mat4.identity();
        private mat4 projectionMatrix = mat4.identity();
        private mat3 normalMatrix = mat3.identity();

        private TrefoilKnot trefoilKnot;

        const uint positionAttribute = 0;
        const uint normalAttribute = 1;
        public Scene(OpenGL gl)
        {
            var attributeLocations = new Dictionary<uint, string>
            {
                {positionAttribute, "Position"},
                {normalAttribute, "Normal"},
            };

            shaderPerPixel = new ShaderProgram();
            shaderPerPixel.Create(gl, ManifestResourceLoader.LoadTextFile(@"Shaders\PerPixel.vert"), ManifestResourceLoader.LoadTextFile(@"Shaders\PerPixel.frag"), attributeLocations);

            shaderToon = new ShaderProgram();
            shaderToon.Create(gl, ManifestResourceLoader.LoadTextFile(@"Shaders\Toon.vert"), ManifestResourceLoader.LoadTextFile(@"Shaders\Toon.frag"), attributeLocations);

            trefoilKnot = new TrefoilKnot(gl, positionAttribute, normalAttribute);
        }
        public void CreateProjectionMatrix(OpenGL gl, float screenWidth, float screenHeight)
        {
            const float S = 0.46f;
            float H = S * screenHeight / screenWidth;
            projectionMatrix = glm.frustum(-S, S, -H, H, 1, 100);

            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.MultMatrix(projectionMatrix.to_array());
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }
        public void CreateModelviewAndNormalMatrix(float rotationAngle)
        {
            mat4 rotation = glm.rotate(mat4.identity(), rotationAngle, new vec3(0, 1, 0));
            mat4 translation = glm.translate(mat4.identity(), new vec3(0, 0, -4));
            modelviewMatrix = rotation * translation;
            normalMatrix = modelviewMatrix.to_mat3();
        }
        public void RenderImmediateMode(OpenGL gl)
        {
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();
            gl.MultMatrix(modelviewMatrix.to_array());

            gl.PushAttrib(OpenGL.GL_POLYGON_BIT);
            gl.PolygonMode(FaceMode.FrontAndBack, PolygonMode.Lines);

            var vertices = trefoilKnot.Vertices;
            gl.Begin(BeginMode.Triangles);
            foreach (var index in trefoilKnot.Indices)
                gl.Vertex(vertices[index].x, vertices[index].y, vertices[index].z);
            gl.End();

            gl.PopAttrib();
        }
        public void RenderRetainedMode(OpenGL gl, bool useToonShader)
        {
            var shader = useToonShader ? shaderToon : shaderPerPixel;

            shader.Bind(gl);

            shader.SetUniform3(gl, "DiffuseMaterial", 0f, 0.75f, 0.75f);
            shader.SetUniform3(gl, "AmbientMaterial", 0.04f, 0.04f, 0.04f);
            shader.SetUniform3(gl, "SpecularMaterial", 0.5f, 0.5f, 0.5f);
            shader.SetUniform1(gl, "Shininess", 50f);

            shader.SetUniform3(gl, "LightPosition", 0.25f, 0.25f, 1f);

            shader.SetUniformMatrix4(gl, "Projection", projectionMatrix.to_array());
            shader.SetUniformMatrix4(gl, "Modelview", modelviewMatrix.to_array());
            shader.SetUniformMatrix3(gl, "NormalMatrix", normalMatrix.to_array());

            trefoilKnot.VertexBufferArray.Bind(gl);

            gl.DrawElements(OpenGL.GL_TRIANGLES, trefoilKnot.Indices.Length, OpenGL.GL_UNSIGNED_SHORT, IntPtr.Zero);

            shader.Unbind(gl);
        }
    }
}