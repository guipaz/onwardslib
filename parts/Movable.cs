using Microsoft.Xna.Framework;
using onwardslib;

namespace harvester.parts
{
    public class Movable : IPart
    {
        public Vector2 Force { get; set; }
        public float TerminalSpeed { get; set; } = 100f;

        public void SetForce(Vector2 force)
        {
            Force = force;
        }

        public void SetForceX(float forceX)
        {
            Force = new Vector2(forceX, Force.Y);
        }

        public void SetForceY(float forceY)
        {
            Force = new Vector2(Force.X, forceY);
        }

        public void AddForce(Vector2 force)
        {
            Force += force;
        }
    }
}
