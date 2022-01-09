using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace ITBOX
{
  public   class PlayerEntity:PhysicalEntity
    {
        public override void Load()
        {
            Position = new Vector2(-20, -20);
            Scale = new Vector2(1, 5);
            base.Load();
        }
        public override void LateUpdate()
        {
            Position += new Vector2(1, 0);
            base.LateUpdate();
        }
    }
}
