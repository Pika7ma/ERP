using System.Windows;

namespace Lplfw.UI.Inventory
{
    /// <summary>
    /// NewStorehouse.xaml 的交互逻辑
    /// </summary>
    public partial class NewStorehouse : Window
    {
        public NewStorehouse(bool isNew)
        {
            InitializeComponent();
            if (isNew)
            {
                Title = "新建仓库";
            }
            else
            {
                Title = "修改仓库";
            }
        }
    }
}
