using MatrixCalculator.Workers;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MatrixCalculator.ViewModel
{
    class CalculatorVM : BindableBase
    {
        public CalculatorVM()
        {
            PolishCowCalculator polishCow = new PolishCowCalculator();
            Expression = "";
            NumberOne = new DelegateCommand(() => {
                Expression += "1";
                CurrentCaretIndex = Expression.Length;
            });

            NumberTwo = new DelegateCommand(() => {
                Expression += "2";
                CurrentCaretIndex = Expression.Length;
            });

            NumberThree = new DelegateCommand(() => {
                Expression += "3";
                CurrentCaretIndex = Expression.Length;
            });

            NumberFive = new DelegateCommand(() => {
                Expression += "5";
                CurrentCaretIndex = Expression.Length;
            });

            NumberSix = new DelegateCommand(() => {
                Expression += "6";
                CurrentCaretIndex = Expression.Length;
            });

            NumberSeven = new DelegateCommand(() => {
                Expression += "7";
                CurrentCaretIndex = Expression.Length;
            });

            NumberEight = new DelegateCommand(() => {
                Expression += "8";
                CurrentCaretIndex = Expression.Length;
            });

            NumberNine = new DelegateCommand(() => {
                Expression += "9";
                CurrentCaretIndex = Expression.Length;
            });

            NumberZero = new DelegateCommand(() => {
                Expression += "0";
                CurrentCaretIndex = Expression.Length;
            });

            Dot = new DelegateCommand(() => {
                if (Expression.Length != 0 && Expression.LastIndexOf('.') != Expression.Length - 1) {
                    Expression += ".";
                    CurrentCaretIndex = Expression.Length;
                }
            });

            Plus = new DelegateCommand(() => {
                if (Expression.Length != 0 && Expression.LastIndexOf('+') != Expression.Length - 1) {
                    Expression += "+";
                    CurrentCaretIndex = Expression.Length;
                }
            });

            Minus = new DelegateCommand(() => {
                if (Expression.Length != 0 && Expression.LastIndexOf('-') != Expression.Length - 1) {
                    Expression += "-";
                    CurrentCaretIndex = Expression.Length;
                }
            });

            Multiply = new DelegateCommand(() => {
                if (Expression.Length != 0 && Expression.LastIndexOf('*') != Expression.Length - 1) {
                    Expression += "*";
                    CurrentCaretIndex = Expression.Length;
                }
            });

            Divide = new DelegateCommand(() => {
                if (Expression.Length != 0 && Expression.LastIndexOf('/') != Expression.Length - 1) {
                    Expression += "/";
                    CurrentCaretIndex = Expression.Length;
                }
            });

            OpenBracket = new DelegateCommand(() => {
                Expression += "(";
                CurrentCaretIndex = Expression.Length;
            });

            Equal = new DelegateCommand(() => {
                Result = polishCow.Calculate(Expression, MatricesArr.ToArray());
            });

            CloseBracket = new DelegateCommand(() => {
                int countOpenBrackets = 0;
                int countClouseBrackets = 0;
                foreach (char sym in Expression) {
                    if (sym == '(')
                        countOpenBrackets++;
                    if (sym == ')')
                        countClouseBrackets++;
                }
                if (!(countOpenBrackets <= countClouseBrackets)) { 
                    Expression += ")";
                    CurrentCaretIndex = Expression.Length;
                }
            });

            Clear = new DelegateCommand(() => {
                Expression = "";
            });

            DeleteSym = new DelegateCommand(() => {
                Expression = Expression.Remove(Expression.Length - 1);
                CurrentCaretIndex = Expression.Length;
            });

            EnterPressed = new DelegateCommand(() => { 
                Result = polishCow.Calculate(Expression, MatricesArr.ToArray());
            });
        }

        private int _currentCaretIndex;
        public int CurrentCaretIndex
        {
            get => _currentCaretIndex;
            set
            {
                SetProperty(ref _currentCaretIndex, value);
                Console.WriteLine(value);
            }
        }

        private ObservableCollection<MatrixVM> _matricesArr;
        public ObservableCollection<MatrixVM> MatricesArr
        {
            get => _matricesArr;
            set
            {
                SetProperty(ref _matricesArr, value);
            }
        }
        private bool _isResultString;
        public bool IsResultString
        {
            get => _isResultString;
            set
            {
                SetProperty(ref _isResultString, value);
            }
        }



        private string _expression;
        public string Expression
        {
            get => _expression;
            set
            {
                SetProperty(ref _expression, value);
            }
        }

        private object _result;
        public object Result
        {
            get => _result;
            set
            {
                SetProperty(ref _result, value);
                IsResultString = value is string;
            }
        }



        #region Commands
        public ICommand Power { get; set; }
        public ICommand Percent { get; set; }
        public ICommand OpenBracket { get; set; }
        public ICommand CloseBracket { get; set; }
        public ICommand Clear { get; set; }
        public ICommand DeleteSym { get; set; }
        public ICommand RowsCount { get; set; }
        public ICommand ColumnsCount { get; set; }
        public ICommand MatrixDet { get; set; }
        public ICommand MatrixRank { get; set; }
        public ICommand NormMatrix { get; set; }
        public ICommand DiagMatrix { get; set; }
        public ICommand TransposeMatrix { get; set; }
        public ICommand InvMatrix { get; set; }
        public ICommand NumberOne { get; set; }
        public ICommand NumberTwo { get; set; }
        public ICommand NumberThree { get; set; }
        public ICommand NumberFour { get; set; }
        public ICommand NumberFive { get; set; }
        public ICommand NumberSix { get; set; }
        public ICommand NumberSeven { get; set; }
        public ICommand NumberEight { get; set; }
        public ICommand NumberNine { get; set; }
        public ICommand NumberZero { get; set; }
        public ICommand ChangeSign { get; set; }
        public ICommand Dot { get; set; }
        public ICommand Plus { get; set; }
        public ICommand Minus { get; set; }
        public ICommand Multiply { get; set; }
        public ICommand Divide { get; set; }
        public ICommand Matrix { get; set; }
        public ICommand Equal { get; set; }
        public ICommand EnterPressed { get; set; }
        #endregion
    }
}
