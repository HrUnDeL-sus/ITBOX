using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ITBOX
{
 public  abstract class Entity
    {
        private Shader _myShader;
        private float[] _vertices = {
    0.5f,  0.5f, 0.0f, 
     0.5f, -0.5f, 0.0f, 
    -0.5f, -0.5f, 0.0f,  
    -0.5f,  0.5f, 0.0f   
                };
        uint[] _indices = {
    0, 1, 3,
    1, 2, 3
                };
        private int _vertexBufferObject;
       private int _vertexArrayObject;
        private int _elementBufferObject;
        private Vector2 _position;
        private Vector2 _rotate;
        private Vector2 _scale=new Vector2(1,1);
        public virtual Vector2 Position { get => _position; protected set => _position = value; }
        public virtual Vector2 Rotate { get => _rotate; protected set => _rotate = value; }
        public virtual Vector2 Scale { get => _scale; protected set => _scale = value; }
        
        public Entity(Vector2 _startPosition,Vector2 _startScale)
        {
            _position = _startPosition;
            _scale = _startScale;
            GenBuffersAndGetShader();
        }
        public Entity()
        {
            GenBuffersAndGetShader();
        }
        private void GenBuffersAndGetShader()
        {
            _myShader = MapManager.GetShader();
            _vertexBufferObject = GL.GenBuffer();
            _elementBufferObject = GL.GenBuffer();
            _vertexArrayObject = GL.GenVertexArray();
        }
        public void Rendering(Matrix4 orthoMatrix)
        {
         
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BindVertexArray(_vertexArrayObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            _myShader.Use();
            _myShader.SetUniform4(orthoMatrix,"ortho");
            _myShader.SetUniform4(Matrix4.CreateTranslation(new Vector3(_position.X,_position.Y,0)),"position");
            _myShader.SetUniform4(Matrix4.CreateRotationX(0), "rotate");
            _myShader.SetUniform4(Matrix4.CreateScale(new Vector3(_scale.X, _scale.Y, 0)), "scale");
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}
