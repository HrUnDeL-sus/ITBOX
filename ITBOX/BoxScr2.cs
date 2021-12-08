using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace ITBOX
{
    public class BoxScr2 : Script
    {
        public override void Start()
        {
            
            base.Start();
        }
       
        public override void Update()
        {
            (MainPrefab.GetComponent<Transform>() as Transform).Position += new Vector3(0, 0, 0);
            base.Update();
        }
    }
}
