using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITBOX
{
 public abstract  class Script
    {
        protected PrefabEntity MainPrefab { get; private set; }
       
        public void SetActiveMainPrefab(PrefabEntity prefab)
        {
            MainPrefab = prefab;
        }
        public Script(PrefabEntity prefab = null)
        {
            MainPrefab = prefab;
        }
        public virtual void Start()
        {
            
        }
        public virtual void Update()
        {
           
        }
    }
}
