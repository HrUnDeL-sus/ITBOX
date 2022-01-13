﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrundelFramework
{
   public abstract class SolidEntity:Entity
    {
        private Collider _collider = new Collider();
        public SolidEntity()
        {
          
        }
        public override void LateUpdate()
        {
            _collider.UpdatePosition(Position, base.Scale);
            base.LateUpdate();
        }
        public sealed override Vector2 Position 
        { get => base.Position;
            protected set
            {
                Vector2 offset = base.Position - value;
             
                foreach (var otherSolidEntity in MapManager.GetLoadingMap().FindEntityTypeInRadius<SolidEntity>(value, 10000))
                {
                    Collider otherCollider = otherSolidEntity._collider;
                   
                    if (offset.Y != 0&& (_collider.LeftLine< otherCollider.RightLine&&_collider.LeftLine>otherCollider.LeftLine)|| (otherCollider.LeftLine < _collider.RightLine && otherCollider.LeftLine > _collider.LeftLine))
                    {
                        if (offset.Y>0&&((_collider.DownLine < otherCollider.UpLine && _collider.DownLine > otherCollider.DownLine)|| _collider.DownLine == otherCollider.UpLine))
                            value.Y = otherCollider.UpLine.Y+base.Scale.Y/2;
                        else if (offset.Y < 0 && ((_collider.UpLine > otherCollider.DownLine && _collider.UpLine < otherCollider.UpLine) || _collider.UpLine == otherCollider.DownLine))
                            value.Y = otherCollider.DownLine.Y - base.Scale.Y/2;
                        _collider.UpdatePosition(value, base.Scale);
                    }
                  else  if(offset.X != 0 && (_collider.UpLine < otherCollider.UpLine && _collider.UpLine > otherCollider.DownLine) || (otherCollider.UpLine < _collider.UpLine && otherCollider.UpLine > _collider.DownLine))
                    {
                        if (offset.X > 0 && ((_collider.LeftLine < otherCollider.RightLine && _collider.LeftLine > otherCollider.LeftLine) || _collider.LeftLine == otherCollider.RightLine))
                            value.X = otherCollider.RightLine.X+base.Scale.X/2;
                        else if (offset.X < 0 && ((_collider.RightLine > otherCollider.LeftLine && _collider.RightLine < otherCollider.RightLine)|| _collider.RightLine == otherCollider.LeftLine))
                            value.X = otherCollider.LeftLine.X - base.Scale.X / 2;
                        _collider.UpdatePosition(value, base.Scale);
                    }


                }
                base.Position = value;
            }
               
        
        }
    }
}