using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ITBOX
{
  public  class ShaderProgram
    {
        public readonly int Handle;
        public readonly int location;
        public ShaderProgram(string vertexPath, string fragmentPath)
        {
            string VertexShaderSource;
            int VertexShader;
            int FragmentShader;
            using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8))
            {
                VertexShaderSource = reader.ReadToEnd();
            }

            string FragmentShaderSource;

            using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8))
            {
                FragmentShaderSource = reader.ReadToEnd();
            }
           VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);
           FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);
            GL.CompileShader(VertexShader);
            string infoLogVert = GL.GetShaderInfoLog(VertexShader);
            if (infoLogVert != String.Empty)
               Console.WriteLine(infoLogVert);

            GL.CompileShader(FragmentShader);

            string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

            if (infoLogFrag != String.Empty)
                
               Console.WriteLine(infoLogFrag);
            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            GL.LinkProgram(Handle);
            location = GL.GetUniformLocation(Handle, "modelview");
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
            
        }
       
        public void Use()
        {
            GL.UseProgram(Handle);
        }
    }
}
