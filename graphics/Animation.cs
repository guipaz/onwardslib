using Microsoft.Xna.Framework;

namespace onwards.graphics
{
    public class Animation
    {
        public string Name { get; }
        public Sprite CurrentSprite { get; private set; }
        public Sprite[] Sprites { get; }
        public float FrameRate { get; set; } = 0.1f;
        public bool Playing { get; private set; }
        public bool Loops { get; set; } = true;
        public bool FlipHorizontally { get; set; }

        int currentIndex;
        float currentLerp;

        public Animation(string name, params Sprite[] sprites)
        {
            Name = name;
            Sprites = sprites;
            CurrentSprite = sprites[0];
        }

        public Animation(string name, OTexture texture, int y, int fromX, int toX, int sizeX, int sizeY)
        {
            Name = name;

            var numberOfSprites = (toX - fromX + sizeX) / sizeX;
            
            Sprites = new Sprite[numberOfSprites];
            
            var i = 0;
            for (var x = fromX; x <= toX; x += sizeX)
            {
                Sprites[i++] = new Sprite(texture, new Rectangle(x, y, sizeX, sizeY));
            }

            CurrentSprite = Sprites[0];
        }

        public void Play()
        {
            Playing = true;
            Reset();
        }

        public void Stop()
        {
            Playing = false;
        }

        public void Reset()
        {
            currentIndex = 0;
            currentLerp = 0;
            CurrentSprite = Sprites[0];
        }

        public void Update()
        {
            if (Playing)
            {
                currentLerp += Engine.Instance.DeltaTime / FrameRate;
                if (currentLerp >= 1)
                {
                    currentLerp = 0;

                    currentIndex++;
                    if (currentIndex >= Sprites.Length)
                    {
                        if (Loops)
                        {
                            currentIndex = 0;
                        }
                        else
                        {
                            Playing = false;
                            return;
                        }
                    }

                    CurrentSprite = Sprites[currentIndex];
                }
            }
        }
    }
}