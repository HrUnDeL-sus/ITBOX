using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectLibrary
{
    [Serializable]
    public class Animation
    {
        public string SelectedSprite = "";
        public Dictionary<string, ResourceSprite> Sprites = new Dictionary<string, ResourceSprite>();
    }
}
