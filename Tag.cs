using System;

namespace onwards
{
    public readonly struct Tag : IEquatable<Tag>
    {
        public static Tag None = new Tag(uint.MinValue);
        public static Tag All = new Tag(uint.MaxValue);

        public uint Value { get; }

        public Tag(uint value)
        {
            Value = value;
        }
        
        public bool Equals(Tag other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is Tag other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (int)Value;
        }

        public static Tag operator &(Tag a, Tag b)
        {
            return new Tag(a.Value & b.Value);
        }

        public static Tag operator |(Tag a, Tag b)
        {
            return new Tag(a.Value | b.Value);
        }

        public static bool operator ==(Tag a, Tag b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(Tag a, Tag b)
        {
            return !(a == b);
        }

        public bool Contains(Tag a)
        {
            return (this & a) == a;
        }
    }
}