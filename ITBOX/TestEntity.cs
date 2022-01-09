using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace ITBOX
{
   public sealed class TestEntity:SolidEntity
    {
        private Vector2 vector;
        public TestEntity():base()
        {
            
        }
        public override void Load()
        {
            Random random = new Random();
            Position = new Vector2(random.Next(-80, 80), random.Next(-80, 0));
            Scale = new Vector2(random.Next(5,10), random.Next(5, 10));
         
            SetColor(random.NextDouble(), random.NextDouble(), random.NextDouble());
            base.Load();
        }
        public override void Update()
        {
           
            base.Update();
        }
    }
}
