using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrundelFramework
{
    internal enum GlobalShaderType
    {
        Standard,
        Textured
    }
 internal sealed class Shader:IDisposable
    {
        private int _handle;
        private int _vertexShader;
        private int _fragmentShader;
        private bool disposedValue = false;
        public readonly GlobalShaderType Type;
        public Shader( GlobalShaderType globalShaderType)
        {
            Type = globalShaderType;
            switch (Type)
            {
                case GlobalShaderType.Standard:
                    GenerateShader(ShaderType.VertexShader, "shader.vert", out _vertexShader);
                    GenerateShader(ShaderType.FragmentShader, "shader.frag", out _fragmentShader);
                    break;
                case GlobalShaderType.Textured:
                    GenerateShader(ShaderType.VertexShader, "shaderTex.vert", out _vertexShader);
                    GenerateShader(ShaderType.FragmentShader, "shaderTex.frag", out _fragmentShader);
                    break;
                default:
                    break;
            }
         
            CreateProgram();
        }  
        public void CreateProgram()
        {
            _handle = GL.CreateProgram();
            GL.AttachShader(_handle, _vertexShader);
            GL.AttachShader(_handle, _fragmentShader);
            GL.LinkProgram(_handle);
        }
        public void DeleteProgram()
        {
                GL.DeleteProgram(_handle);
        }
        public void Dispose()
        {
          
            GC.SuppressFinalize(this);
        }
       
        public int GetUniform(string name)
        {
            return GL.GetUniformLocation(_handle, name);
        }
        public void SetUniform4(Matrix4 matrix,string name)
        {
            int location = GL.GetUniformLocation(_handle, name);

            GL.UniformMatrix4(location, true, ref matrix);
        }
        
        public void Use()
        {
            GL.UseProgram(_handle);
        }
        private void GenerateShader(ShaderType shaderType,string path,out int _vertexId)
        {
            string ShaderSource;
            using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
            {
                ShaderSource = reader.ReadToEnd();
            }
            _vertexId = GL.CreateShader(shaderType);
            GL.ShaderSource(_vertexId, ShaderSource);
            GL.CompileShader(_vertexId);
            string infoLogVert = GL.GetShaderInfoLog(_vertexId);
            if (infoLogVert != string.Empty)
                Console.WriteLine(infoLogVert);
        }
    }
}
