using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MatrixCalculator.CustomControls
{
    class MatrixNumbersTextBox : TextBox
    {
        public MatrixNumbersTextBox() : base()
        {
            this.PreviewTextInput += MatrixNumbersTextBox_PreviewTextInput;
            this.PreviewKeyDown += MatrixNumbersTextBox_PreviewKeyDown;
        }

        private void MatrixNumbersTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Back && this.Text.Length == 1)
                e.Handled = true;
        }

        private void MatrixNumbersTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text[0]) && e.Text[0] != '.') {
                e.Handled = true;
            }
        }
    }
}
