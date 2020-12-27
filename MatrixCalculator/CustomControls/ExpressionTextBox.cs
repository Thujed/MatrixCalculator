using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MatrixCalculator.CustomControls
{
    class ExpressionTextBox : TextBox
    {
        public ExpressionTextBox() : base()
        {
            this.SelectionChanged += ExpressionTextBox_SelectionChanged;
            this.KeyDown += ExpressionTextBox_KeyDown;
        }

        private void ExpressionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                EnterPressed?.Execute(null);
        }

        private void ExpressionTextBox_SelectionChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (CurrentCaretIndex != CaretIndex)
            {
                CurrentCaretIndex = CaretIndex;
            }
        }


        public int CurrentCaretIndex
        {
            get { return (int)GetValue(CurrentCaretIndexProperty); }
            set { 
                SetValue(CurrentCaretIndexProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentCaretndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentCaretIndexProperty =
            DependencyProperty.Register("CurrentCaretIndex", typeof(int), typeof(ExpressionTextBox), new PropertyMetadata(0, CurrentCaretIndexUpdated));

        private static void CurrentCaretIndexUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TextBox).CaretIndex = (int)e.NewValue;
        }




        public ICommand EnterPressed
        {
            get { return (ICommand)GetValue(EnterPressedProperty); }
            set { SetValue(EnterPressedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EnterPressed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnterPressedProperty =
            DependencyProperty.Register("EnterPressed", typeof(ICommand), typeof(ExpressionTextBox), new PropertyMetadata(null));


    }
}
