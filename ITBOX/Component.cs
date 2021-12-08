using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITBOX
{
  public abstract  class Component
    {
        protected  PrefabEntity MainPrefabEntity;
        public void SetActiveMainPrefab(PrefabEntity get)
        {
            MainPrefabEntity = get;
        }
    }
}
