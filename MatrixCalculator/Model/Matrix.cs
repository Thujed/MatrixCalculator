using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Prism.Mvvm;

namespace MatrixCalculator.Model
{
    class Matrix : BindableBase
    {
        public Matrix(int rows, int columns, string name)
        {
            this._name = name;
            Values = new double[rows, columns];
            NumericsMatrix = DenseMatrix.Create(rows, columns, 0);
        }

        public Matrix(string name, Matrix<double> numericMatrix)
        {
            this._name = name;
            NumericsMatrix = numericMatrix;
            Values = new double[numericMatrix.RowCount, numericMatrix.ColumnCount];
            for (int i = 0; i < Values.GetLength(0); ++i)
            {
                for (int j = 0; j < Values.GetLength(1); ++j)
                {
                    Values[i, j] = Math.Round(Convert.ToDouble(numericMatrix[i, j]), 4);

                }
            }
        }

        public static Matrix operator +(Matrix matrix1, Matrix matrix2) {
            Matrix resultMatrix = new Matrix("C", matrix1.NumericsMatrix.Add(matrix2.NumericsMatrix));
            return resultMatrix;
        }

        public static Matrix operator +(Matrix matrix1, double num) {
            Matrix resultMatrix = new Matrix("C", matrix1.NumericsMatrix.Add(num));
            return resultMatrix;
        }

        public static Matrix operator -(Matrix matrix1, Matrix matrix2) {
            Matrix resultMatrix = new Matrix("C", matrix1.NumericsMatrix.Subtract(matrix2.NumericsMatrix));
            return resultMatrix;
        }

        public static Matrix operator -(Matrix matrix1, double num) {
            Matrix resultMatrix = new Matrix("C", matrix1.NumericsMatrix.Subtract(num));
            return resultMatrix;
        }

        public static Matrix operator *(Matrix matrix1, Matrix matrix2) {
            Matrix resultMatrix = new Matrix("C", matrix1.NumericsMatrix.Multiply(matrix2.NumericsMatrix));
            return resultMatrix;
        }

        public static Matrix operator *(Matrix matrix1, double num)  {
            Matrix resultMatrix = new Matrix("C", matrix1.NumericsMatrix.Multiply(num));
            return resultMatrix;
        }


        public double this[int i, int j] { 
            get => Values[i, j];
            set {
                Values[i, j] = value;
                NumericsMatrix[i, j] = (double)value;
            }
        }

        public void SetNewSize(int rows, int columns, bool flag) {
            double[,] oldMatrix = Values;
            Values = new double[rows, columns];
            double[,] doubleValues = new double[rows, columns];


            if (flag) { 
                for (int i = 0; i < oldMatrix.GetLength(0); ++i)
                {
                    for (int j = 0; j < oldMatrix.GetLength(1); ++j)
                    {
                        Values[i, j] = oldMatrix[i, j];
                    }
                }
            } else {
                for (int i = 0; i < Values.GetLength(0); ++i)
                {
                    for (int j = 0; j < Values.GetLength(1); ++j)
                    {
                        Values[i, j] = oldMatrix[i, j];
                    }
                }
            }
            for (int i = 0; i < Values.GetLength(0); ++i)
            {
                for (int j = 0; j < Values.GetLength(1); ++j)
                {
                    doubleValues[i, j] = Convert.ToDouble(Values[i, j]);
                }
            }
            NumericsMatrix = DenseMatrix.OfArray(doubleValues);
            RaisePropertyChanged("Columns");
            RaisePropertyChanged("Rows");
        }

        public int Rows => Values.GetLength(0);
        public int Columns => Values.GetLength(1);

        private double[,] _values;
        public double[,] Values
        {
            get => _values;
            set
            {
                SetProperty(ref _values, value);
            }
        }
        private Matrix<double> _numericsMatrix;
        public Matrix<double> NumericsMatrix
        {
            get => _numericsMatrix;
            set
            {
                SetProperty(ref _numericsMatrix, value);
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
            }
        }
    }
}
