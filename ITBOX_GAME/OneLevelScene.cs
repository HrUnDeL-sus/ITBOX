using System;
using System.Collections.Generic;
using System.Text;
using HrundelFramework;
using OpenTK;
namespace ITBOX_GAME
{
    class OneLevelScene : Map
    {
        public OneLevelScene():base("level_1",new Camera(200))
        {

        }
        protected override void EntitiesInitialization()
        {
            //    AddEntity(new Platform(), new EntityProperties(new Vector2(0,-60), new Vector2(100, 20), "platform", new ColorF(1, 1, 1, 1)));
            Random random = new Random();

          
                AddEntity(new Platform(), new EntityProperties(new Vector2(random.Next(-150,150), random.Next(-150,150)), new Vector2(15, 5), "platform", new ColorF((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), 1)));
           
            //  AddEntity(new Platform(), new EntityProperties(new Vector2(-20,10), new Vector2(2, 30), "platform", new ColorF(1, 1, 1, 1)));
            AddEntity(new Player(), new EntityProperties(new Vector2(0,6), new Vector2(2, 5), "Player", new ColorF(0f, 0f, 0f, 0f)));
        }
    }
}
