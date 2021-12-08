using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
namespace ITBOX
{
 public class BoxScr:Script
    {
        
        public override void Update()
        {
          
            (MainPrefab.GetComponent<Transform>() as Transform).Position += new Vector3(0, -0.1f, 0);
          
            base.Update();
        }
    }
}
