using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITBOX
{
  public sealed class Transform:Component
    {
        public Vector3 Scale;
        public Vector3 Rotate;
        private Vector3 _position;
        public Vector3 Position
        {
            get
            {
                return _position;
            }
            set
            {
                BoxCollider BoxCollider = MainPrefabEntity?.GetComponent<BoxCollider>();

                if (BoxCollider != null)
                {
                   
                    float deltaX = _position.X - value.X;
                    float deltaY = _position.Y - value.Y;
                    if(deltaX!=0)
                    BoxCollider.UpdateBoxCollider(new Vector2(value.X, _position.Y));
                    if ((deltaX > 0 && BoxCollider.HasCollision(Collision.Left))
                    || (deltaX < 0 && BoxCollider.HasCollision(Collision.Right)))
                    {
                        value.X = _position.X;
                    }
                    if (deltaY != 0)
                        BoxCollider.UpdateBoxCollider(new Vector2(_position.X, value.Y));
                    if ((deltaY < 0 && BoxCollider.HasCollision(Collision.Up))
                      || (deltaY > 0 && BoxCollider.HasCollision(Collision.Down)))
                    {
                        value.Y = _position.Y;
                    }
                    
                    
                }     
                _position = value;
                MainPrefabEntity?.UpdateBoxCollider();

            }
        }
            
                
        public Transform() 
        {

        }
    }
}
