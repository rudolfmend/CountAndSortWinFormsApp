using System.Drawing;
using System;
using System.Windows.Forms;

namespace CountAndSortWinFormsAppNetFr4
{
    public class DataGridViewCheckBoxHeaderCell : DataGridViewColumnHeaderCell
    {
        private bool isChecked = false;
        public event Action<object, bool> OnCheckBoxClicked;

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds,
            int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue,
            string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value,
                formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

            int checkBoxSize = 15;
            Rectangle checkBoxLocation = new Rectangle(
                cellBounds.Location.X + (cellBounds.Width - checkBoxSize) / 2,
                cellBounds.Location.Y + (cellBounds.Height - checkBoxSize) / 2,
                checkBoxSize, checkBoxSize);

            ButtonState state = isChecked ? ButtonState.Checked : ButtonState.Normal;
            ControlPaint.DrawCheckBox(graphics, checkBoxLocation, state);
        }

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseClick(e);
            isChecked = !isChecked;
            OnCheckBoxClicked?.Invoke(this, isChecked);
            this.DataGridView.InvalidateCell(this);
        }
    }
}
