using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixCalculator.ViewModel
{
    class CellValueVM : BindableBase
    {
        public CellValueVM()
        {
            
        }

        private string _value;
        public string Value
        {
            get => _value;
            set
            {
                SetProperty(ref _value, value);
            }
        }

        private int _rowIndex;
        public int RowIndex { 
            get => _rowIndex; 
            set {
                _rowIndex = value;
                IAmNotInFirstRow = _rowIndex != 0;
            } 
        }

        private int _columnIndex;
        public int ColumnIndex { 
            get => _columnIndex;
            set {
                _columnIndex = value;
                IAmNotInFirstColumn = _columnIndex != 0;
            }
        }
        public bool IAmNotInFirstColumn { get; set; }
        public bool IAmNotInFirstRow { get; set; }
        public bool Agregator => !IAmNotInFirstColumn && !IAmNotInFirstRow;
    }
}
