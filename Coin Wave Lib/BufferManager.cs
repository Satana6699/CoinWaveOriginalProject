using Coin_Wave_Lib.ObjCS;
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
using static Coin_Wave_Lib.PlayerDouble;
using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;
using static Coin_Wave_Lib.PlayerFloat;
using Coin_Wave_Lib.MapGenerator;
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
        public BufferManager(int vertexArrayObject, int vertexBufferObject,
                             double[] vertices, Texture texture,
                             string texturePath)
        {
            this.vertexArrayObject = vertexArrayObject;
            this.vertexBufferObject = vertexBufferObject;
            this.vertices = vertices;
            this.texture = texture;
            GenerateBuffers(texturePath);
        }
        public BufferManager(string texturePath)
        {
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

            texture = Texture.LoadFromFile(@"D:\Семестр 4 (полигон)\Курсовая работа\coin wave\Coin Wave (sln)\Coin Wave Lib\data\image\sqrt.png");
            texture.Use(TextureUnit.Texture0);
        }
        }
    }
