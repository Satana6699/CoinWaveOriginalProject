using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Coin_Wave_Lib;
using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Numerics;

namespace Coin_Wave_Lib
{
    public class BufferManager
    {
        int vertexArrayObject = 0;
        int vertexBufferObject = 0;
        double[] vertices;
        static Shader shader;
        Texture texture;

        static BufferManager()
        {
            shader = new Shader(@"data\shaders\shader.vert", @"data\shaders\shader.frag");
        }
        public BufferManager(double[] vertices, string texturePath)
        {
            this.vertices = vertices;
            GenerateBuffers(texturePath);
        }

        private void GenerateBuffers(string texturePath)
        {
            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(double), vertices, BufferUsageHint.DynamicDraw);

            shader = new Shader(@"data\shaders\shader.vert", @"data\shaders\shader.frag");
            shader.Use();
            var vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Double, false, 5 * sizeof(double), 0);

            var texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Double, false, 5 * sizeof(double), 3 * sizeof(double));

            texture = Texture.LoadFromFile(texturePath);
            texture.Use(TextureUnit.Texture0);
        }

        public void UpdateDate(double[] vertices)
        {
            this.vertices = vertices;
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, this.vertices.Length * sizeof(double), this.vertices, BufferUsageHint.DynamicDraw);
        }

        public void Render()
        {
            GL.BindVertexArray(vertexArrayObject);
            texture.Use(TextureUnit.Texture0);
            shader.Use();
            GL.DrawArrays(PrimitiveType.Quads, 0, vertices.Length);
        }
    }
}
