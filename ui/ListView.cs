using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace onwards.ui
{
    public class ListView<T> : ScrollView where T : class, ICellData
    {
        List<ListCell<T>> _cells = new List<ListCell<T>>();
        List<T> _data;

        public T SelectedCellData { get; private set; }

        public Action<T> OnSelectedCell { get; set; }

        public override int VisibleRows { get; }
        public override int TotalRows => _data.Count;

        public ListView(int cellsAmount, int cellHeight)
        {
            VisibleRows = cellsAmount;

            OnScroll = UpdateCells;

            for (var i = 0; i < cellsAmount; i++)
            {
                var cell = new ListCell<T>
                {
                    Pivot = new Vector2(.5f, 0),
                    HorizontalContraint = ViewConstraint.Stretch,
                    Bounds = new Rectangle(16, i * cellHeight + 16, 16, cellHeight),
                    OnSelectCell = SetSelected
                };
                _cells.Add(cell);
                AddChild(cell);

                cell.Active = false;
            }

            StartAt = 0;
            UpdateCells(0);
        }

        public void SetData(List<T> data)
        {
            _data = data;
            SelectedCellData = null;
            StartAt = 0;
            UpdateCells(0);
        }

        public void SetSelected(T cell)
        {
            SelectedCellData = cell;
            UpdateCells(StartAt);
            OnSelectedCell?.Invoke(cell);
        }

        public void SetSelectedByText(string text)
        {
            foreach (var cell in _cells)
            {
                if (cell.CellData.ListCellText == text)
                {
                    SetSelected(cell.CellData);
                    break;
                }
            }
        }

        public void UpdateCells(int startAt)
        {
            foreach (var cell in _cells)
            {
                cell.Active = false;
                cell.Selected = false;
            }

            if (_data == null)
            {
                return;
            }
            
            var j = 0;
            for (var i = startAt; i < _data.Count && j < _cells.Count; i++)
            {
                var cell = _cells[j];
                var dataItem = _data[_data.Count - i - 1];

                cell.Active = true;
                cell.Label.Text = dataItem.ListCellText;
                cell.CellData = dataItem;

                if (dataItem == SelectedCellData)
                {
                    cell.Selected = true;
                }

                j++;
            }
        }
    }
}