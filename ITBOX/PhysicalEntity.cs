using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace ITBOX
{
  public abstract  class PhysicalEntity:SolidEntity
    {
        public override void LateUpdate()
        {
            Position -= new Vector2(0,1);
            base.LateUpdate();
        }
    }
}
