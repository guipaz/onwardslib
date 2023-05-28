using System;
using onwards.graphics;
using onwards.utils;

namespace onwards.ui
{
    public sealed class ListCell<T> : Button where T : ICellData
    {
        public T CellData { get; set; }

        public Action<T> OnSelectCell { get; set; }

        public ListCell()
        {
            Label.Text = "Cell";

            BackgroundSprite = new NineSlicedSprite(TextureLoader.Get("ui"), 24, 0, 8);
            SelectedBackgroundSprite = new NineSlicedSprite(TextureLoader.Get("ui"), 48, 0, 8);

            OnClick = _ => OnSelectCell?.Invoke(CellData);
        }
    }
}