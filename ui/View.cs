using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using onwards.graphics;

namespace onwards.ui
{
    public enum ViewConstraint
    {
        Start,
        Center,
        End,
        Stretch
    }

    public class View
    {
        public bool ConsumedController { get; set; }
        public bool SlatedForRemoval { get; set; }
        public bool Active { get; set; } = true;
        public Rectangle LocalBounds { get; protected set; }
        public List<View> Children { get; } = new List<View>();

        public ViewConstraint HorizontalContraint { get; set; }
        public ViewConstraint VerticalContraint { get; set; }
        public Vector2 Pivot { get; set; } = new Vector2(.5f);

        protected Rectangle globalBounds;
        protected bool dirty;

        View parent;
        Color _debugColor;
        List<View> toRemove = new List<View>();
        List<View> toAdd = new List<View>();

        public View Parent
        {
            get => parent;
            set
            {
                parent = value;
                dirty = true;
            }
        }

        public Rectangle Bounds
        {
            get
            {
                if (dirty)
                {
                    UpdateBounds();
                }
                return globalBounds;
            }
            set
            {
                LocalBounds = value;
                dirty = true;
            }
        }

        public View()
        {
            _debugColor = new Color(ProcMaster.Debug.Roll(255), ProcMaster.Debug.Roll(255), ProcMaster.Debug.Roll(255));
        }

        public virtual void UpdateBounds()
        {
            dirty = false;

            var parentBounds = Rectangle.Empty;

            if (Parent != null)
            {
                parentBounds = Parent.Bounds;
            }

            globalBounds = CalculateBounds(parentBounds);

            foreach (var child in Children)
            {
                child.UpdateBounds();
            }
        }

        Rectangle CalculateBounds(Rectangle parentBounds)
        {
            var x = 0;
            var y = 0;
            var width = LocalBounds.Width;
            var height = LocalBounds.Height;

            switch (HorizontalContraint)
            {
                case ViewConstraint.Start:
                    x = parentBounds.X + LocalBounds.X - (int)(LocalBounds.Width * Pivot.X);
                    break;
                case ViewConstraint.Center:
                    x = parentBounds.X + parentBounds.Width / 2 + LocalBounds.X - (int)(LocalBounds.Width * Pivot.X);
                    break;
                case ViewConstraint.End:
                    x = parentBounds.X + parentBounds.Width - LocalBounds.X - (int)(LocalBounds.Width * Pivot.X);
                    break;
                case ViewConstraint.Stretch:
                    x = parentBounds.X + LocalBounds.X;
                    width = parentBounds.X + parentBounds.Width - x - LocalBounds.Width;
                    break;
            }

            switch (VerticalContraint)
            {
                case ViewConstraint.Start:
                    y = parentBounds.Y + LocalBounds.Y - (int)(LocalBounds.Height * Pivot.Y);
                    break;
                case ViewConstraint.Center:
                    y = parentBounds.Y + parentBounds.Height / 2 + LocalBounds.Y - (int)(LocalBounds.Height * Pivot.Y);
                    break;
                case ViewConstraint.End:
                    y = parentBounds.Y + parentBounds.Height - LocalBounds.Y - (int)(LocalBounds.Height * Pivot.Y);
                    break;
                case ViewConstraint.Stretch:
                    y = parentBounds.Y + LocalBounds.Y;
                    height = parentBounds.Y + parentBounds.Height - y - LocalBounds.Height;
                    break;
            }

            return new Rectangle(x, y, width, height);
        }
        
        public void AddChild(View view)
        {
            toAdd.Add(view);
        }

        public void RemoveChild(View view)
        {
            toRemove.Add(view);
        }

        public virtual void Draw()
        {
            //TODO debug
            if (Input.Keyboard.Down(Keys.LeftShift))
            {
                Engine.Instance.SpriteBatch.Draw(ColorTexture.Get(_debugColor), Bounds, Color.White * 0.3f);
            }

            foreach (var child in Children)
            {
                if (child.Active)
                {
                    child.Draw();
                }
            }
        }

        public virtual void Update()
        {
            ConsumedController = false;

            if (!Active)
            {
                return;
            }

            for (var i = Children.Count - 1; i >= 0; i--)
            {
                var child = Children[i];

                if (!ConsumedController)
                {
                    child.Update();
                }

                if (child.SlatedForRemoval)
                {
                    toRemove.Add(child);
                }

                ConsumedController |= child.ConsumedController;
            }

            foreach (var add in toAdd)
            {
                add.Parent = this;
                Children.Add(add);
            }

            foreach (var remove in toRemove)
            {
                if (remove.Parent == this)
                {
                    remove.Parent = null;
                }

                Children.Remove(remove);
            }

            toAdd.Clear();
            toRemove.Clear();
        }

        protected bool IsMouseInside => Bounds.Contains(Input.Mouse.Position);

        protected bool ClickedInside(int id = 0)
        {
            return IsMouseInside && Input.Mouse.Clicked(id);
        }
    }
}