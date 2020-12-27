using MatrixCalculator.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Matrix = MatrixCalculator.Model.Matrix;
using MathNet.Numerics.LinearAlgebra.Complex;

namespace MatrixCalculator.ViewModel
{
    class MatrixVM : BindableBase
    {
        private Matrix _matrix;
        public MatrixVM(Matrix matrix)
        {
            this._matrix = matrix;
            _matrix.PropertyChanged += (o, e) => {
                RaisePropertyChanged(e.PropertyName);
            };

            SelectMatrix = new DelegateCommand(() => {
                MatrixSelected?.Invoke();
            });

            for (int i = 0; i < matrix.Rows; i++) {
                for (int k = 0; k < matrix.Columns; k++) {
                    CellValueVM cellValue = new CellValueVM();
                    if (double.IsNaN(matrix[i, k])) {
                        //cellValue.Value = (i + k) % 2 == 0 ? "🗙" : "𐩒";
                        cellValue.Value = "🗙";
                    } else { 
                        cellValue.Value = matrix[i, k].ToString();
                    }
                    cellValue.RowIndex = i;
                    cellValue.ColumnIndex = k;
                    MatrixValues.Add(cellValue);
                    cellValue.PropertyChanged += CellValuePropertyChanged;
                }
            }
            

            AddRow = new DelegateCommand(() => {
                if (Rows < 10) { 
                    matrix.SetNewSize(Rows + 1, Columns, true);
                    for (int i = 0; i < Columns; i++) {
                        CellValueVM cellValue = new CellValueVM();
                        cellValue.Value = "0";
                        cellValue.RowIndex = Rows - 1;
                        cellValue.ColumnIndex = i;
                        cellValue.PropertyChanged += CellValuePropertyChanged;
                        MatrixValues.Add(cellValue);
                    }
                    UpdatedMatrix?.Invoke();
                }

            });

            AddColumn = new DelegateCommand(() => {
                if(Columns < 10) { 
                    matrix.SetNewSize(Rows, Columns + 1, true);
                    for (int i = 0; i < Rows; i++) {
                        CellValueVM cellValue = new CellValueVM();
                        cellValue.Value = "0";
                        cellValue.RowIndex = i;
                        cellValue.ColumnIndex = Columns - 1;
                        cellValue.PropertyChanged += CellValuePropertyChanged;
                        MatrixValues.Insert((Columns - 1) + ((Columns - 1) * i) + i, cellValue);
                    }
                    UpdatedMatrix?.Invoke();
                }
            });

            DeleteRow = new DelegateCommand(() => {
                if (Rows > 1) { 
                    int tmp = Columns * Rows - 1;
                    matrix.SetNewSize(Rows - 1, Columns, false);
                    for (int i = 0; i < Columns; i++) {
                        MatrixValues.RemoveAt(tmp);
                        tmp--;
                    }
                    UpdatedMatrix?.Invoke();
                }
            });

            DeleteColumn = new DelegateCommand(() => {
                if (Columns > 1) { 
                    matrix.SetNewSize(Rows, Columns - 1, false);
                    for (int i = 0; i < Rows; i++) {
                        Console.WriteLine("Count:" + MatrixValues.Count);
                        Console.WriteLine("index:" + ((Columns + (Columns * i)) + 1));
                        MatrixValues.RemoveAt(Columns + (Columns * i));
                    }
                    UpdatedMatrix?.Invoke();
                }
            });

            DeleteMatrix = new DelegateCommand(() => {
                DeletedMatrix?.Invoke();
            });
        }

        public ICommand AddRow { get; set; }
        public ICommand AddColumn { get; set; }
        public ICommand DeleteRow { get; set; }
        public ICommand DeleteColumn { get; set; }
        public ICommand SelectMatrix { get; set; }
        public ICommand DeleteMatrix { get; set; }
        public event Action DeletedMatrix;
        public event Action UpdatedMatrix;

        public ObservableCollection<CellValueVM> MatrixValues { get; private set; } = new ObservableCollection<CellValueVM>();
        public event Action MatrixSelected;
        
        public void CellValuePropertyChanged(object o, PropertyChangedEventArgs args) {
            _matrix[(o as CellValueVM).RowIndex, (o as CellValueVM).ColumnIndex] = Convert.ToDouble((o as CellValueVM).Value.Replace('.',','));
            UpdatedMatrix?.Invoke();
        }


        public int Columns => _matrix.Columns;
        public int Rows => _matrix.Rows;

        public string Name {
            get => _matrix.Name;
            set {
                _matrix.Name = value;
            }
        }

        public Matrix<double> NumericsMatrix {
            get => _matrix.NumericsMatrix;
            set {
                _matrix.NumericsMatrix = value;
            }
        }

        #region Overriden
        public static MatrixVM operator +(MatrixVM matrix1, MatrixVM matrix2)
        {
            return new MatrixVM(matrix1._matrix + matrix2._matrix);
        }

        public static MatrixVM operator +(MatrixVM matrix1, double num)
        {
            return new MatrixVM(matrix1._matrix + num);
        }

        public static MatrixVM operator -(MatrixVM matrix1, MatrixVM matrix2)
        {
            return new MatrixVM(matrix1._matrix - matrix2._matrix);
        }

        public static MatrixVM operator -(MatrixVM matrix1, double num)
        {
            return new MatrixVM(matrix1._matrix - num);
        }

        public static MatrixVM operator *(MatrixVM matrix1, MatrixVM matrix2)
        {
            return new MatrixVM(matrix1._matrix * matrix2._matrix);
        }

        public static MatrixVM operator *(MatrixVM matrix1, double num)
        {
            return new MatrixVM(matrix1._matrix * num);
        }
        #endregion
    }
}
