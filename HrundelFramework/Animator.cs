using System;
using System.Collections.Generic;
using System.Text;

namespace HrundelFramework
{
    public class Animator
    {
        private readonly Dictionary<string,Animation> _animations;
        private Animation _activeAnimation;
        private int _fps;
        public readonly bool HasAnimations;
        public int Fps { get => _fps; 
            set
            {
                if (value > 60)
                    throw new ArgumentOutOfRangeException(value + " must be less than 61");
                _fps = 60 / value;
            
            }
        }
       
        public Animator(Dictionary<string, Animation> animations)
        {
            _animations = animations;
            HasAnimations=_animations.Count!=0;
        }
        internal void Rendering()
        {
            if (_activeAnimation == null)
                throw new NullReferenceException("_activeAnimation is null");
            _activeAnimation.Rendering(_fps);
        }
        public void SelectAnimation(string name)
        {
            if (_activeAnimation != null&&_activeAnimation!= _animations[name])
                _activeAnimation.ResetTimer();
            _activeAnimation = _animations[name];
        }
    }
}
