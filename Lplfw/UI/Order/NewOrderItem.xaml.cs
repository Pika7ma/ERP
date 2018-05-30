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

namespace Lplfw.UI.Order
{
    /// <summary>
    /// NewOrderItem.xaml 的交互逻辑
    /// </summary>
    public partial class NewOrderItem : Window
    {
        public NewOrderItem(bool isNew)
        {
            InitializeComponent();
            if (isNew)
            {
                Title = "新建订单项目";
            }
            else
            {
                Title = "修改订单项目";
            }
        }
    }
}
