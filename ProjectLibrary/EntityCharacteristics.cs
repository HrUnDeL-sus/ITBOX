using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OpenTK.Graphics;
namespace ProjectLibrary
{
    [Serializable]
    public  class EntityCharacteristics
    {
        public TypeEntity MyType { get; set; }

        public Color4 MyColor { get; set; }
        public Animator MyAnimator { get; set; }
        public readonly string Name;
        private Shader _myShaderNotTex=null;
        private Shader _myShaderTex = null;
        private float POSITION_VERTICES = 0.5f;
        private uint[] _indices = {
    0, 1, 3,
    1, 2, 3
                };
        private int _vertexBufferObject;
        private int _vertexBufferObjectTex;
        private int _vertexArrayObject;
        private int _elementBufferObject;
        private bool _isShaders;
        public EntityCharacteristics(string name)
        {
            Name = name;
            _myShaderNotTex = new Shader("shader.vert", "shader.frag");
            _myShaderTex = new Shader("shaderTex.vert", "shaderTex.frag");
        }
        public void SetShader()
        {
            _myShaderNotTex = new Shader("shader.vert", "shader.frag");
            _myShaderTex = new Shader("shaderTex.vert", "shaderTex.frag");
        }
        private void Clear()
        {
            //     GL.DeleteBuffer(_vertexArrayObject);
            //     GL.DeleteBuffer(_elementBufferObject);
        //    _myShaderTex.DeleteProgram();
       //     _myShaderNotTex.DeleteProgram();
        }
        private float[] GetVertices()
        {
            return new float[] {
                POSITION_VERTICES, POSITION_VERTICES, 0.0f,
                POSITION_VERTICES, -POSITION_VERTICES, 0.0f,
                -POSITION_VERTICES, -POSITION_VERTICES, 0.0f,
                -POSITION_VERTICES, POSITION_VERTICES, 0.0f
            };
        }
       
        public void Rendering(Matrix4 orthoMatrix,Vector2 Position,Vector2 Scale)
        {
            _vertexBufferObject = GL.GenBuffer();
            _elementBufferObject = GL.GenBuffer();
            _vertexArrayObject = GL.GenVertexArray();
            GL.BlendFunc(BlendingFactor.SrcAlpha,BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
            if (MyAnimator != null && MyAnimator.Animations.ContainsKey(MyAnimator.SelectedAnimation) && MyAnimator.Animations[MyAnimator.SelectedAnimation].Sprites.ContainsKey(MyAnimator.Animations[MyAnimator.SelectedAnimation].SelectedSprite))
            {
                _myShaderTex.CreateProgram();
            }else
                _myShaderNotTex.CreateProgram();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BindVertexArray(_vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, GetVertices().Length * sizeof(float), GetVertices(), BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            if (MyAnimator != null && MyAnimator.Animations.ContainsKey(MyAnimator.SelectedAnimation)&& MyAnimator.Animations[MyAnimator.SelectedAnimation].Sprites.ContainsKey(MyAnimator.Animations[MyAnimator.SelectedAnimation].SelectedSprite))
            {
                Animation animation = MyAnimator.Animations[MyAnimator.SelectedAnimation];
                Sprite sprite = animation.Sprites[animation.SelectedSprite];
                int texCoordLocation = _myShaderTex.GetAttribLocation("aTexCoord");
                _vertexBufferObjectTex = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectTex);
                GL.BufferData(BufferTarget.ArrayBuffer, MyAnimator.GetVerticesTexture().Length * sizeof(float), MyAnimator.GetVerticesTexture(), BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
                GL.EnableVertexAttribArray(1);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);              
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, sprite.MainSprite.Width, sprite.MainSprite.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, sprite.MainSprite.ArraySprite);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
              
                _myShaderTex.Use();
                Matrix4 transform = Matrix4.CreateScale(Scale.X, Scale.Y, 0) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(0)) * Matrix4.CreateTranslation(new Vector3(Position.X, Position.Y, 0)) * orthoMatrix;
                _myShaderTex.SetUniform4(transform, "transform");
                _myShaderTex.SetUniform4(orthoMatrix, "ortho");
             
            }
            else
            {
                _myShaderNotTex.Use();
                int id = _myShaderNotTex.GetUniform("ourColor");
                GL.Uniform4(id, MyColor.R, MyColor.G, MyColor.B, MyColor.A);
                Matrix4 transform = Matrix4.CreateScale(Scale.X, Scale.Y, 0) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(0)) * Matrix4.CreateTranslation(new Vector3(Position.X, Position.Y, 0)) * orthoMatrix;
                _myShaderNotTex.SetUniform4(transform, "transform");
                _myShaderNotTex.SetUniform4(orthoMatrix, "ortho");
            }
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0); 
            Clear();
        }
        
    }
}
