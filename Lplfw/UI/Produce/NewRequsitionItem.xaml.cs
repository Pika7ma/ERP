using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lplfw.UI.Produce
{
    /// <summary>
    /// NewRequsitionItem.xaml 的交互逻辑
    /// </summary>
    public partial class NewRequsitionItem : Window
    {
        public NewRequsitionItem(bool isNew)
        {
            InitializeComponent();
            if (isNew)
            {
                Title = "新建物料单条目";
            }
            else
            {
                Title = "修改物料单条目";
            }
        }
    }
}
