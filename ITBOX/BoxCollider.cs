using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITBOX
{
    public enum Collision
    {
        Left,
        Right,
        Up,
        Down
    }
    public enum StateChangeSizeCollider
    {
        NotChange,
        Change
    }
  public  class BoxCollider:Component
    {
        public static List<BoxCollider> AllBoxColliders = new List<BoxCollider>();
        public StateChangeSizeCollider StateChangeSize { get; private set; }
      
        struct Point
        {
           public float X { get; private set; }
           public float Y { get; private set; }
            public void MathPosition(Vector2 Position,Vector2 Size,bool IsDown,bool IsLeft)
            {
                X =IsLeft ? Position.X + Size.X /2 : Position.X - Size.X /2;
                Y = IsDown ? Position.Y + Size.Y /2 : Position.Y - Size.Y /2;
                RoundXY();
            }
            private void RoundXY()
            {
              
            }
        }
        public  Vector2 Size { get; private set; }
        private Point _leftUpPoint;
        private Point _rightUpPoint;
        private Point _leftDownPoint;
        private Point _rightDownPoint;
        public BoxCollider()
        {
            StateChangeSize = StateChangeSizeCollider.NotChange;
        }
        public void ChangeSize(Vector2 size)
        {
            //   Size = new Vector2((size.X*(size.X*5/100))/CHANGE_SIZE_VALUE, (size.Y*(size.Y * 5 / 100)) / CHANGE_SIZE_VALUE);
            Size = new Vector2(size.X, size.Y);
            AllBoxColliders.Add(this);
            StateChangeSize = StateChangeSizeCollider.Change;
        }
        public bool HasCollision(Collision collision)
        {
            bool hasCollision=false;
            foreach (var item in AllBoxColliders)
            {
                if (item != this)
                {
                    switch (collision)
                    {
                        case Collision.Left:
                            //hasCollision = 
                            //    (_leftUpPoint.X <= item._rightUpPoint.X && _leftUpPoint.X >= item._leftUpPoint.X &&
                            //    _leftUpPoint.Y <= item._rightUpPoint.Y && _leftUpPoint.Y >= item._leftDownPoint.Y) ||
                            //    (_leftDownPoint.X <= item._rightUpPoint.X && _leftDownPoint.X >= item._leftUpPoint.X &&
                            //    _leftDownPoint.Y <= item._rightUpPoint.Y && _leftDownPoint.Y >= item._leftDownPoint.Y) ? true : hasCollision;
                            hasCollision =
                              (_leftUpPoint.X < item._rightUpPoint.X && _leftUpPoint.X > item._leftUpPoint.X
                              &&
                              ((_leftUpPoint.Y < item._rightUpPoint.Y && _leftUpPoint.Y > item._leftDownPoint.Y)
                              ||
                              (item._leftUpPoint.Y < _rightUpPoint.Y && item._leftUpPoint.Y > _leftDownPoint.Y)
                              ))
                              ||
                              (_leftDownPoint.X < item._rightUpPoint.X && _leftDownPoint.X > item._leftUpPoint.X
                              &&
                              ((_leftDownPoint.Y < item._rightUpPoint.Y && _leftDownPoint.Y > item._leftDownPoint.Y)
                              ||
                              (item._leftDownPoint.Y < _rightUpPoint.Y && item._leftDownPoint.Y > _leftDownPoint.Y)
                              )) ? true : hasCollision;
                            if (hasCollision)
                            {
                                Console.WriteLine("Left");
                            }
                            break;
                            
                        case Collision.Right:
                            hasCollision = 
                               
                               (_rightUpPoint.X < item._rightUpPoint.X && _rightUpPoint.X > item._leftUpPoint.X 
                               &&
                               ((_rightUpPoint.Y < item._rightUpPoint.Y && _rightUpPoint.Y > item._leftDownPoint.Y)
                               ||
                               (item._rightUpPoint.Y < _rightUpPoint.Y && item._rightUpPoint.Y > _leftDownPoint.Y)
                               )) 
                               ||
                               (_rightDownPoint.X < item._rightUpPoint.X && _rightDownPoint.X > item._leftUpPoint.X 
                               &&
                               ((_rightDownPoint.Y < item._rightUpPoint.Y && _rightDownPoint.Y > item._leftDownPoint.Y)
                               ||
                               (item._rightDownPoint.Y < _rightUpPoint.Y && item._rightDownPoint.Y > _leftDownPoint.Y)
                               )) ? true : hasCollision;
                            if (hasCollision)
                            {
                                Console.WriteLine("Right");
                            }
                            break;
                              
                        case Collision.Up:
                            hasCollision = (_rightUpPoint.Y <= item._rightUpPoint.Y && _rightUpPoint.Y >= item._leftDownPoint.Y&&
                              (_rightUpPoint.X <= item._rightUpPoint.X && _rightUpPoint.X >= item._leftDownPoint.X
                              ||
                              item._rightUpPoint.X <= _rightUpPoint.X && item._rightUpPoint.X >= _leftDownPoint.X
                              )) ? true : hasCollision;
                            if (hasCollision)
                            {
                                Console.WriteLine("Up");
                            }
                            break;
                        case Collision.Down:
                            hasCollision = (_rightDownPoint.Y <= item._rightUpPoint.Y && _rightDownPoint.Y >= item._rightDownPoint.Y &&
                              (_rightUpPoint.X <= item._rightUpPoint.X && _rightUpPoint.X >= item._leftDownPoint.X
                              ||
                              item._rightUpPoint.X <= _rightUpPoint.X && item._rightUpPoint.X >= _leftDownPoint.X))
                               //)) ||
                               //(_rightDownPoint.Y <= item._rightUpPoint.Y && _rightDownPoint.Y >= item._leftDownPoint.Y &&
                               //(_rightDownPoint.X <= item._rightUpPoint.X && _rightDownPoint.X >= item._leftDownPoint.X ||
                               //item._rightDownPoint.X <= _rightUpPoint.Y && item._rightDownPoint.X >= _leftDownPoint.X))
                               ? true : hasCollision;
                            if (hasCollision)
                            {
                                Console.WriteLine("Down");
                            }
                            break;
                        default:
                            return false;
                    }
                }
            }
            return hasCollision;
        }
        
        public void UpdateBoxCollider(Vector2 Position)
        {
            _leftUpPoint.MathPosition(Position, Size, false, true);
            _rightUpPoint.MathPosition(Position, Size, false, false);
            _leftDownPoint.MathPosition(Position, Size, true, true);
            _rightDownPoint.MathPosition(Position, Size, true, false);

        }
    }
}
