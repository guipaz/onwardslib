using System.Collections.Generic;

namespace onwards
{
    public interface IScene
    {
        void OnEnter();
        void Draw();
        void Update();
        void ResolutionChanged();
    }
}