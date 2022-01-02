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
    public enum PositionPoint
    {
        Up,
        Down,
        Left,
        Right
    }
  public  class BoxCollider:Component
    {
        public static List<BoxCollider> AllBoxColliders = new List<BoxCollider>();
        public StateChangeSizeCollider StateChangeSize { get; private set; }
      
        class Point
        {
           public float X { get; private set; }
           public float Y { get; private set; }
            public void MathPosition(Transform transform, PositionPoint positionPoint)
            {
                Vector2 Position = new Vector2(transform.Position.X, transform.Position.Y);
                Vector2 Size =new Vector2(transform.Scale.X, transform.Scale.Y);
                switch (positionPoint)
                {
                    case PositionPoint.Up:
                        Y =Position.Y + Size.Y / 3;
                        X = Position.X;
                        break;
                    case PositionPoint.Down:
                        Y = Position.Y - Size.Y / 3;
                        X = Position.X;
                        break;
                    case PositionPoint.Left:
                        X =Position.X - Size.X / 3;
                        Y = Position.Y;
                        break;
                    case PositionPoint.Right:
                        X = Position.X + Size.X / 3;
                        Y = Position.Y;
                        break;
                    default:
                        break;
                }
               
               
                RoundXY();
            }
            private void RoundXY()
            {
                //X = (float)Math.Round(X, 2);
                //Y = (float)Math.Round(Y, 2);
            }
        }
        public  Vector2 Size { get; private set; }
        private Point _upPoint=new Point();
        private Point _downPoint = new Point();
        private Point _rightPoint = new Point();
        private Point _leftPoint = new Point();
        public BoxCollider()
        {
            StateChangeSize = StateChangeSizeCollider.NotChange;
        }
        public void ChangeSize(Vector2 size)
        {
            //   Size = new Vector2((size.X*(size.X*5/100))/CHANGE_SIZE_VALUE, (size.Y*(size.Y * 5 / 100)) / CHANGE_SIZE_VALUE);
            Size = new Vector2(size.X, size.Y);
            Console.WriteLine("{0} {1}", Size.X, Size.Y);
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
                            hasCollision = _leftPoint.X < item._rightPoint.X && _leftPoint.X > item._leftPoint.X;
                                //&&
                                // ((_upPoint.Y > item._downPoint.Y && _upPoint.Y < item._upPoint.Y) ||
                                // (_downPoint.Y > item._downPoint.Y && _downPoint.Y < item._upPoint.Y))
                            if (hasCollision)
                            {
                                Console.WriteLine("Left");
                            }
                            break;
                            
                        case Collision.Right:
                            hasCollision = _rightPoint.X < item._rightPoint.X && _rightPoint.X > item._leftPoint.X;
                                //(((_upPoint.Y > item._downPoint.Y && _upPoint.Y < item._upPoint.Y) ||
                                //(_downPoint.Y > item._downPoint.Y && _downPoint.Y < item._upPoint.Y))||
                                //((item._upPoint.Y > _downPoint.Y && item._upPoint.Y < _upPoint.Y) ||
                                //(item._downPoint.Y > _downPoint.Y && item._downPoint.Y < _upPoint.Y)))
                            if (hasCollision)
                            {
                                Console.WriteLine("Right");
                            }
                            break;
                              
                        case Collision.Up:
                            if (hasCollision)
                            {
                                Console.WriteLine("Up");
                            }
                            break;
                        case Collision.Down:
                           
                            if(hasCollision)
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
            Transform transform = MainPrefabEntity.GetComponent<Transform>();
            _leftPoint.MathPosition(transform, PositionPoint.Left);
            _rightPoint.MathPosition(transform, PositionPoint.Right);
            _downPoint.MathPosition(transform, PositionPoint.Down);
            _upPoint.MathPosition(transform, PositionPoint.Up);

        }
    }
}
