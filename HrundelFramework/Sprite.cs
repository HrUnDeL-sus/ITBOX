using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using ResourseLibrary;
namespace HrundelFramework
{
   
    internal class Sprite
    {
        internal readonly byte[] ArraySprite;
        internal readonly int Height;
        internal readonly int Width;
        private int _vertexBufferObjectTex;
        public Sprite(DescriptionResourseSprite descriptionResourseSprite)
        {
            ArraySprite=new byte[descriptionResourseSprite.ArraySprite.Length];
            descriptionResourseSprite.ArraySprite.CopyTo(ArraySprite,0);
            Height = descriptionResourseSprite.Height;
            Width = descriptionResourseSprite.Width;
           
        }
        float[] GetVerticesTexture()
        {
            return new float[] {
                   1,0,
                   1,1,
                   0,1,
                   0,0
            };
        }
        internal void Rendering()
        {
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
            _vertexBufferObjectTex = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectTex);
            GL.BufferData(BufferTarget.ArrayBuffer, GetVerticesTexture().Length * sizeof(float), GetVerticesTexture(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width,Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ArraySprite);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
          
        }
    }
}
