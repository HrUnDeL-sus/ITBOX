using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
namespace ProjectLibrary
{
    [Serializable]
    public class Sprite
    {
        public readonly ResourceSprite MainSprite;
        public Sprite(ResourceSprite mainSprite)
        {
            MainSprite = mainSprite;
          
        }
    }
}
