using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace ITBOX
{
  public  class Camera
    {
        private float _size;
        public Matrix4 GetOrthoMatrix()
        {
            return Matrix4.CreateOrthographicOffCenter(-_size, _size, -_size, _size, 1, -1);
        }
        public Camera(float size)
        {
            _size = size;
        }
    }
}
