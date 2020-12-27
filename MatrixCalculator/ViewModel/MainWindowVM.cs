using MatrixCalculator.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MatrixCalculator.ViewModel
{
    class MainWindowVM : BindableBase
    {
        public MatrixManagerVM MatrixManagerVM { get; private set; }
        public CalculatorVM CalculatorViewModel { get; private set; }
        public ObservableCollection<MatrixVM> Matrices { get; private set; } = new ObservableCollection<MatrixVM>() { new MatrixVM(new Matrix(10, 10, "A")), new MatrixVM(new Matrix(6, 4, "B")), new MatrixVM(new Matrix(3, 3, "C")), new MatrixVM(new Matrix(3, 3, "D")), new MatrixVM(new Matrix(1, 6, "E")), new MatrixVM(new Matrix(4, 2, "F")) };
        public MainWindowVM()
        {
            CalculatorViewModel = new CalculatorVM();
            CalculatorViewModel.MatricesArr = Matrices;
            MatrixManagerVM = new MatrixManagerVM();
            MatrixManagerVM.MatrixViewModel = Matrices[0];
            Matrices.ToList().ForEach((m) => m.MatrixSelected += () => {
                MatrixManagerVM.MatrixViewModel = m;
            });

            Matrices.ToList().ForEach((m) => m.DeletedMatrix += () => {
                Matrices.Remove(m);
            });

            Matrices.ToList().ForEach((m) => m.UpdatedMatrix += () => {
                MatrixManagerVM.NumericMatrixCalculate();
            });

            AddMatrix = new DelegateCommand(() => {
                MatrixVM newMatrix = new MatrixVM(new Matrix(3, 3, "S"));
                newMatrix.MatrixSelected += () => {
                    MatrixManagerVM.MatrixViewModel = newMatrix;
                };
                newMatrix.UpdatedMatrix += () => {
                    MatrixManagerVM.NumericMatrixCalculate();
                };
                Matrices.Add(newMatrix);
            });

            CloseApplication = new DelegateCommand(() => { 
                Application.Current.MainWindow.Close();
            });
        }

        public ICommand AddMatrix { get; set; }
        public ICommand CloseApplication { get; set; }

        private string _test;
        public string Test
        {
            get => _test;
            set
            {
                SetProperty(ref _test, value);
                Console.WriteLine(_test); 
            }
        }

    }
}
