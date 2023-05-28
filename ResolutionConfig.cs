using System;

namespace onwards
{
    public readonly struct ResolutionConfig : IComparable<ResolutionConfig>, IEquatable<ResolutionConfig>
    {
        public int Width { get; }
        public int Height { get; }
        public bool IsFullScreen { get; }

        public ResolutionConfig(int width, int height, bool fullscreen)
        {
            Width = width;
            Height = height;
            IsFullScreen = fullscreen;
        }

        public int CompareTo(ResolutionConfig other)
        {
            var widthComparison = Width.CompareTo(other.Width);
            if (widthComparison != 0) return widthComparison;
            var heightComparison = Height.CompareTo(other.Height);
            if (heightComparison != 0) return heightComparison;
            return IsFullScreen.CompareTo(other.IsFullScreen);
        }

        public bool Equals(ResolutionConfig other)
        {
            return Width == other.Width && Height == other.Height && IsFullScreen == other.IsFullScreen;
        }

        public override bool Equals(object obj)
        {
            return obj is ResolutionConfig other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Width, Height, IsFullScreen);
        }
    }
}