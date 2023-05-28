using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using onwards.ecs;
using onwards.graphics;

namespace onwards.components
{
    public class Camera : Component
    {
        public Tag Tag { get; set; } = Tag.None;
        public Tag Mask { get; set; } = Tag.All;
        public int Order { get; set; }
        public RenderTarget2D RenderTarget { get; set; }
        public int UnitInPixels { get; set; } = 16;
        public SamplerState SamplerState { get; set; } = SamplerState.PointClamp;
        public float Zoom { get; set; } = 1;
        public int PixelsPerUnit { get; set; } = 16;
        public Transform Transform { get; private set; }
        public Point Size => RenderTarget?.Bounds.Size ?? Point.Zero;
        public Color ClearColor { get; set; } = Color.Transparent;

        public override void Load()
        {
            Transform = Entity.Get<Transform>();
        }
        
        Vector3 pos3;
        public Matrix Matrix
        {
            get
            {
                pos3.X = Transform.Position.X;
                pos3.Y = Transform.Position.Y;

                double tX = Transform.Position.X * Zoom;
                double tY = Transform.Position.Y * Zoom;

                return Matrix.CreateTranslation(-pos3) * Matrix.CreateScale(Zoom) *
                       Matrix.CreateTranslation(new Vector3((float)(tX - Math.Truncate(tX)),
                           (float)(tY - Math.Truncate(tY)), 0));
            }
        }

        public ClampConfig ClampConfig { get; set; } = new ClampConfig
        {
            clamp = false
        };

        public Vector2 ScreenToWorldPosition(Point point)
        {
            var ratio = 1; //TODO
            var posRatio = new Vector2(Transform.Position.X / ratio, Transform.Position.Y / ratio);
            var pointRatio = new Vector2(point.X / ratio / Zoom, point.Y / ratio / Zoom);

            return posRatio + pointRatio;
        }

        public bool MoveTowards(Vector2 focusPosition)
        {
            return MoveTowards(focusPosition, 0.05f);
        }

        public bool MoveTowards(Vector2 focusPosition, float speed)
        {
            return MoveTowards(focusPosition, speed, false, true);
        }
        
        public bool MoveTowards(Vector2 focusPosition, float speed, bool immediate, bool center)
        {
            Vector2 pos;
            if (center)
            {
                //pos = focusPosition * (RenderContext.UnitInPixels / (float)PixelsPerUnit) - new Vector2(RenderContext.Size.X / 2f / Zoom,
                //    RenderContext.Size.Y / 2f / Zoom);
                pos = Vector2.Zero;
            }
            else
            {
                pos = focusPosition;
            }

            if (immediate)
            {
                Transform.Position = pos;
            }
            else
            {
                const float tolerance = 5;
                var diff = Transform.Position - pos;
                if (Math.Abs(diff.X) > tolerance || Math.Abs(diff.Y) > tolerance)
                {
                    Transform.Position = Vector2.Lerp(Transform.Position, pos, speed);
                }
                else
                {
                    return false;
                }
            }

            if (ClampConfig.clamp)
                Transform.Position = Vector2.Clamp(Transform.Position,
                    ClampConfig.min * (16 / (float)PixelsPerUnit),
                    ClampConfig.max * (16 / (float)PixelsPerUnit) -
                    new Vector2(Engine.Instance.Graphics.Viewport.X, Engine.Instance.Graphics.Viewport.Y));

            return true;
        }

        public virtual void Render(IEnumerable<IRenderer> renderers)
        {
            Draw.SetCurrentCamera(this);

            Engine.Instance.Graphics.Clear(ClearColor);
            Engine.Instance.SpriteBatch.Begin(samplerState: SamplerState, transformMatrix: Matrix);
            foreach (var renderer in renderers)
            {
                if (Mask.Contains(renderer.Tags))
                {
                    renderer.Render();
                }
            }
            Engine.Instance.SpriteBatch.End();
        }
    }

    public class ClampConfig
    {
        public bool clamp;
        public Vector2 min;
        public Vector2 max;
    }
}