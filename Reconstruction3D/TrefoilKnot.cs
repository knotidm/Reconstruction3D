using GlmNet;
using SharpGL;
using SharpGL.VertexBuffers;
using System;
using System.Linq;

namespace Reconstruction3D
{
    //  Original Source: http://prideout.net/blog/?p=22
    public class TrefoilKnot
    {
        private const uint slices = 128;
        private const uint stacks = 32;
        public VertexBufferArray VertexBufferArray { get; set; }
        public vec3[] Vertices { get; set; }
        public vec3[] Normals { get; set; }
        public ushort[] Indices { get; set; }

        public TrefoilKnot(OpenGL openGL, uint vertexAttributeLocation, uint normalAttributeLocation)
        {
            VertexBufferArray = new VertexBufferArray();
            VertexBufferArray.Create(openGL);
            VertexBufferArray.Bind(openGL);
            CreateVertexNormalBuffer(openGL, vertexAttributeLocation, normalAttributeLocation);
            CreateIndexBuffer(openGL);
            VertexBufferArray.Unbind(openGL);
        }
        private void CreateVertexNormalBuffer(OpenGL openGL, uint vertexAttributeLocation, uint normalAttributeLocation)
        {
            var vertexCount = slices * stacks;

            Vertices = new vec3[vertexCount];
            Normals = new vec3[vertexCount];

            int count = 0;

            float ds = 1.0f / slices;
            float dt = 1.0f / stacks;

            for (float s = 0; s < 1 - ds / 2; s += ds)
            {
                for (float t = 0; t < 1 - dt / 2; t += dt)
                {
                    const float E = 0.01f;
                    vec3 p = EvaluateTrefoil(s, t);
                    vec3 u = EvaluateTrefoil(s + E, t) - p;
                    vec3 v = EvaluateTrefoil(s, t + E) - p;
                    vec3 n = glm.normalize(glm.cross(u, v));
                    Vertices[count] = p;
                    Normals[count] = n;
                    count++;
                }
            }

            var vertexBuffer = new VertexBuffer();
            vertexBuffer.Create(openGL);
            vertexBuffer.Bind(openGL);
            vertexBuffer.SetData(openGL, vertexAttributeLocation, Vertices.SelectMany(v => v.to_array()).ToArray(), false, 3);

            var normalBuffer = new VertexBuffer();
            normalBuffer.Create(openGL);
            normalBuffer.Bind(openGL);
            normalBuffer.SetData(openGL, normalAttributeLocation, Normals.SelectMany(v => v.to_array()).ToArray(), false, 3);
        }
        private void CreateIndexBuffer(OpenGL openGL)
        {
            const uint vertexCount = slices * stacks;
            const uint indexCount = vertexCount * 6;
            Indices = new ushort[indexCount];
            int count = 0;

            ushort n = 0;
            for (ushort i = 0; i < slices; i++)
            {
                for (ushort j = 0; j < stacks; j++)
                {
                    Indices[count++] = (ushort)(n + j);
                    Indices[count++] = (ushort)(n + (j + 1) % stacks);
                    Indices[count++] = (ushort)((n + j + stacks) % vertexCount);

                    Indices[count++] = (ushort)((n + j + stacks) % vertexCount);
                    Indices[count++] = (ushort)((n + (j + 1) % stacks) % vertexCount);
                    Indices[count++] = (ushort)((n + (j + 1) % stacks + stacks) % vertexCount);
                }

                n += (ushort)stacks;
            }

            var indexBuffer = new IndexBuffer();
            indexBuffer.Create(openGL);
            indexBuffer.Bind(openGL);
            indexBuffer.SetData(openGL, Indices);
        }
        private static vec3 EvaluateTrefoil(float s, float t)
        {
            const float TwoPi = (float)Math.PI * 2;

            float a = 0.5f;
            float b = 0.3f;
            float c = 0.5f;
            float d = 0.1f;
            float u = (1 - s) * 2 * TwoPi;
            float v = t * TwoPi;
            float r = (float)(a + b * Math.Cos(1.5f * u));
            float x = (float)(r * Math.Cos(u));
            float y = (float)(r * Math.Sin(u));
            float z = (float)(c * Math.Sin(1.5f * u));

            vec3 dv = new vec3();
            dv.x = (float)(-1.5f * b * Math.Sin(1.5f * u) * Math.Cos(u) - (a + b * Math.Cos(1.5f * u)) * Math.Sin(u));
            dv.y = (float)(-1.5f * b * Math.Sin(1.5f * u) * Math.Sin(u) + (a + b * Math.Cos(1.5f * u)) * Math.Cos(u));
            dv.z = (float)(1.5f * c * Math.Cos(1.5f * u));

            vec3 q = glm.normalize(dv);
            vec3 qvn = glm.normalize(new vec3(q.y, -q.x, 0.0f));
            vec3 ww = glm.cross(q, qvn);

            vec3 range = new vec3();
            range.x = (float)(x + d * (qvn.x * Math.Cos(v) + ww.x * Math.Sin(v)));
            range.y = (float)(y + d * (qvn.y * Math.Cos(v) + ww.y * Math.Sin(v)));
            range.z = (float)(z + d * ww.z * Math.Sin(v));

            return range;
        }
    }
}