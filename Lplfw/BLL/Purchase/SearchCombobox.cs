using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.BLL.Purchase
{
    class SearchCombobox
    {
        public int Value { get; set; }
        public string Name { get; set; }

        public SearchCombobox(string _cmbText, int _cmbValue)
        {
            this.Name = _cmbText;
            this.Value = _cmbValue;
        }
    }
}
