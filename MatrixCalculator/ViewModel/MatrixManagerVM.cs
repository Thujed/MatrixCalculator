using MatrixCalculator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using System.Security.Cryptography;
using MathNet.Numerics.LinearAlgebra.Factorization;
using MathNet.Numerics.LinearAlgebra;
using System.Windows.Input;
using Prism.Commands;

namespace MatrixCalculator.ViewModel
{
    class MatrixManagerVM : BindableBase
    {
        public MatrixManagerVM() {
            EditModeOn = new DelegateCommand(() => {
                IsEditModeOn = !IsEditModeOn;
            });
        }

        private MatrixVM _matrixViewModel;
        public MatrixVM MatrixViewModel
        {
            get => _matrixViewModel;
            set
            {
                if (_matrixViewModel == value) {
                    return;
                }
                SetProperty(ref _matrixViewModel, value);
                NumericMatrixCalculate();
            }

        }

        private bool _isEditModeOn;
        public bool IsEditModeOn
        {
            get => _isEditModeOn;
            set
            {
                SetProperty(ref _isEditModeOn, value);
            }
        }


        public void NumericMatrixCalculate() {
            TransposeMatrix = null;
            InvMatrix       = null;
            TriangleMatrix  = null;
            KernelMatrix    = null;
            CholeskyMatrix  = null;
            LUUpperMatrix   = null;
            LULowerMatrix   = null;
            PoweredMatrix   = null;
            Det = 0;
            TransposeMatrix = new MatrixVM(new Matrix(MatrixViewModel.Name, MatrixViewModel.NumericsMatrix.Transpose()));
            //Vector<double>[] Kernel = MatrixViewModel.NumericsMatrix.Kernel();
            //Matrix KernelMatrixModel = new Matrix(Kernel[0].Count, Kernel.Length, MatrixViewModel.Name);

            //for (int i = 0; i < Kernel[0].Count; i++)
            //{
            //    for (int j = 0; j < Kernel.Length; j++)
            //    {
            //        KernelMatrixModel[i, j] = Kernel[j][i];
            //    }
            //}
            //KernelMatrix = new MatrixVM(KernelMatrixModel);

            if (MatrixViewModel.Rows == MatrixViewModel.Columns) {
                Det = MatrixViewModel.NumericsMatrix.Determinant();
                InvMatrix = new MatrixVM(new Matrix(MatrixViewModel.Name, MatrixViewModel.NumericsMatrix.Inverse()));
                try {
                    CholeskyMatrix = new MatrixVM(new Matrix(MatrixViewModel.Name, MatrixViewModel.NumericsMatrix.Cholesky().Factor)); 
                }
                catch (Exception) { }
                    
                
                LU<double> LUMatrix = MatrixViewModel.NumericsMatrix.LU();
                LUUpperMatrix = new MatrixVM(new Matrix(MatrixViewModel.Name, LUMatrix.L));
                LULowerMatrix = new MatrixVM(new Matrix(MatrixViewModel.Name, LUMatrix.U));
            }


        }

        public ICommand EditModeOn { get; set; }

        private double _det;
        public double Det
        {
            get => _det;
            set
            {
                SetProperty(ref _det, value);
            }
        }


        private MatrixVM _transposeMatrix;
        public MatrixVM TransposeMatrix
        {
            get => _transposeMatrix;
            set 
            {
                SetProperty(ref _transposeMatrix, value);
            }
        }

        private MatrixVM _invMatrix;
        public MatrixVM InvMatrix
        {
            get => _invMatrix;
            set
            {
                SetProperty(ref _invMatrix, value);
            }
        }

        private MatrixVM _triangleMatrix;
        public MatrixVM TriangleMatrix
        {
            get => _triangleMatrix;
            set
            {
                SetProperty(ref _triangleMatrix, value);
            }
        }

        private MatrixVM _kernelMatrix;
        public MatrixVM KernelMatrix
        {
            get => _kernelMatrix;
            set
            {
                SetProperty(ref _kernelMatrix, value);
            }
        }

        private MatrixVM _choleskyMatrix;
        public MatrixVM CholeskyMatrix
        {
            get => _choleskyMatrix;
            set
            {
                SetProperty(ref _choleskyMatrix, value);
            }
        }

        private MatrixVM _LUUpperMatrix;
        public MatrixVM LUUpperMatrix
        {
            get => _LUUpperMatrix;
            set
            {
                SetProperty(ref _LUUpperMatrix, value);
            }
        }

        private MatrixVM _LULowerMatrix;
        public MatrixVM LULowerMatrix
        {
            get => _LULowerMatrix;
            set
            {
                SetProperty(ref _LULowerMatrix, value);
            }
        }


        private MatrixVM _poweredMatrix;
        public MatrixVM PoweredMatrix
        {
            get => _poweredMatrix;
            set
            {
                SetProperty(ref _poweredMatrix, value);
            }
        }
    }
}
