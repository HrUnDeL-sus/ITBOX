using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace ITBOX
{
   public sealed class TestEntity:Entity
    {
        public TestEntity():base(new Vector2(new Random().Next(-5,5), new Random().Next(-5, 5)), Vector2.One)
        {

        }
    }
}
