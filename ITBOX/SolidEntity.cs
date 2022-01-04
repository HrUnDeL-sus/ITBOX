using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITBOX
{
   public abstract class SolidEntity:Entity
    {
        public sealed override Vector2 Position { get => base.Position; protected set => base.Position = value; }
    }
}