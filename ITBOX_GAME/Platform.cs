using System;
using System.Collections.Generic;
using System.Text;
using HrundelFramework;
using HrundelFramework.Input;
using OpenTK;
namespace ITBOX_GAME
{
    class Platform:SolidEntity
    {
        private float z;
        private Vector2 posRand;
        public Platform():base("platform")
        {

        }
        public override void Load()
        {
            Random random = new Random();
           
            base.Load();
        }
        public void Push()
        {
           
        }
        public override void Update()
        {

            Position -= new Vector2(0.1f, 0);

            base.Update();
        }

        
    }
}