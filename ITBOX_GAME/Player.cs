using System;
using System.Collections.Generic;
using System.Text;
using HrundelFramework;
using OpenTK;
using HrundelFramework.Input;
namespace ITBOX_GAME
{
    class Player:PhysicalEntity,ICollider
    {
       
        private float speed=2;
        public Player() : base("player")
        {

        }
        public override void Load()
        {
            MyAnimator.Fps = 12;
            MyAnimator.SelectAnimation("stand");
            base.Load();
        }
        void ICollider.CollisionHasOccurred(List<Entity> entities)
        {

        }
        public override void Update()
        {

            if (KeyManager.KeyPressed(Key.D))
            {
                Position += new Vector2(0.1f, 0);
                MyAnimator.SelectAnimation("run");
            }
            else if (KeyManager.KeyPressed(Key.A))
            {
                Position -= new Vector2(0.1f, 0);
                MyAnimator.SelectAnimation("run");
            }
            else if(KeyManager.KeyPressed(Key.Space) && IsCollision)
            {
                AddImpulse(new Vector2(0, 2));
                MyAnimator.SelectAnimation("stand");
            }
            else
            {
                MyAnimator.SelectAnimation("stand");
            }
            if (KeyManager.KeyPressed(Key.Add))
            {
                speed++;
                Console.WriteLine(speed);
            }
            if (KeyManager.KeyPressed(Key.Minus))
            {
                speed--;
                Console.WriteLine(speed);
            }
            if (Position.Y < -200)
            {
                Position = Vector2.Zero;
            }
            base.LateUpdate();
        }

        
    }
}
