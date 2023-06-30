using Microsoft.Xna.Framework;
using onwardslib.utils;

namespace onwardslib.ui
{
    public class View
    {
        public View Parent
        {
            get => _parent;
            set
            {
                _parent = value;
                UpdateBounds();
            }
        }
        public Rectangle LocalBounds => _localBounds;
        public Rectangle Bounds
        {
            get => _bounds;
            set
            {
                _localBounds = value;
                UpdateBounds();
            }
        }
        public IEnumerable<View> Children => _children;
        public Color DebugColor { get; set; } = new Color(Utils.Random.Next(0, 256), Utils.Random.Next(0, 256), Utils.Random.Next(0, 256)); //TODO debug only

        View _parent;
        Rectangle _localBounds;
        Rectangle _bounds;
        bool _circularReferenceCheck;
        
        List<View> _children = new();
        List<View> _toAdd = new();
        List<View> _toRemove = new();

        protected void UpdateBounds()
        {
            if (_circularReferenceCheck)
            {
                throw new Exception("Circular reference happing; someone has an ancestor as child");
            }

            _bounds = new Rectangle((Parent?.Bounds.X ?? 0) + _localBounds.X, (Parent?.Bounds.Y ?? 0) + _localBounds.Y, _localBounds.Width, _localBounds.Height);
            
            _circularReferenceCheck = true;
            foreach (var child in Children)
            {
                child.UpdateBounds();
            }
            _circularReferenceCheck = false;
        }

        public void AddChild(View view)
        {
            _toAdd.Add(view);
        }

        public void RemoveChild(View view)
        {
            _toRemove.Add(view);
        }

        public void RefreshChildren()
        {
            foreach (var view in _toAdd)
            {
                view.Parent = this;
                _children.Add(view);
            }

            foreach (var view in _toRemove)
            {
                if (view.Parent == this)
                {
                    view.Parent = null;
                }
                
                _children.Remove(view);
            }

            _toAdd.Clear();
            _toRemove.Clear();

            foreach (var child in _children)
            {
                child.RefreshChildren();
            }
        }
    }
}
