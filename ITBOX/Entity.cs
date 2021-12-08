using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITBOX
{
    
  public sealed class Entity
    {
        public readonly string Name;
        public  Entity(string name)
        {
          
            Name = name;

        }
       
        public int[] GetIndices()
        {
          return  new int[] {
              
                0, 1, 3, 1, 2, 3
            };
        }
        public  Vector3[] GetVertex()
        {
            return new Vector3[] {
            
                new Vector3( 1f,  1f, 0.0f),
                new Vector3( 1f, -1f, 0.0f),
                new Vector3(-1f, -1f, 0.0f),
                new Vector3(-1f,  1f, 0.0f)
            };
        }
        public Vector2[] GetTexVertex()
        {
            return new Vector2[] {
                 new Vector2(0f, 0f),
                  new Vector2(0, 1),
                    new Vector2(1, 1f),
                 new Vector2(1f, 0)
              
               
               
               
            };
        }
    }
}
