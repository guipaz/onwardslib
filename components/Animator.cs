using System.Collections.Generic;
using onwards.ecs;
using onwards.graphics;

namespace onwards.components
{
    public class Animator : Component, IUpdater
    {
        public Animation CurrentAnimation { get; private set; }

        Dictionary<string, Animation> animations { get; } = new Dictionary<string, Animation>();
        SpriteRenderer spriteRenderer;

        public override void Load()
        {
            spriteRenderer = Entity.Get<SpriteRenderer>();
        }

        public void Add(Animation animation)
        {
            animations[animation.Name] = animation;
        }

        public void Set(string name)
        {
            if (animations.ContainsKey(name) && CurrentAnimation?.Name != name)
            {
                CurrentAnimation = animations[name];
                CurrentAnimation.Play();
            }
        }

        public void Update()
        {
            if (CurrentAnimation != null)
            {
                CurrentAnimation.Update();
                if (CurrentAnimation.Playing)
                {
                    spriteRenderer.Sprite = CurrentAnimation.CurrentSprite;
                    spriteRenderer.FlipHorizontally = CurrentAnimation.FlipHorizontally;
                }
            }
        }
    }
}