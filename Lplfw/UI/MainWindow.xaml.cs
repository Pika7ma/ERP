using System.Linq;
using System.Windows;

/// <summary>
/// League of People who Let Father Worry
/// </summary>
namespace Lplfw.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Utils.MainWindow = this;
            Judge();
        }

        private void Judge()
        {
            using (var _db = new DAL.ModelContainer())
            {
                var _list = _db.UserGroupPrivilegeItemSet.Where(i => i.UserGroupId == Utils.CurrentUser.UserGroupId);
                foreach (var i in _list)
                {
                    switch (i.PrivilegeId)
                    {
                        case 1:
                            if (i.Mode == "不可见")
                            {
                                btnBOM.IsEnabled = false;
                            }
                            break;
                        case 2:
                            if (i.Mode == "不可见")
                            {
                                btnInventory.IsEnabled = false;
                            }
                            break;
                        case 3:
                            if (i.Mode == "不可见")
                            {
                                btnOrder.IsEnabled = false;
                            }
                            break;
                        case 4:
                            if (i.Mode == "不可见")
                            {
                                btnProduce.IsEnabled = false;
                            }
                            break;
                        case 5:
                            if (i.Mode == "不可见")
                            {
                                btnPurchase.IsEnabled = false;
                            }
                            break;

                    }
                }
            }
        }

        private void OpenBomWindow(object sender, RoutedEventArgs e)
        {
            Utils.OpenSubwindow(new Bom.BomWindow());
        }

        private void OpenInventoryWindow(object sender, RoutedEventArgs e)
        {
            Utils.OpenSubwindow(new Inventory.InventoryWindow());
        }

        private void OpenOrderWindow(object sender, RoutedEventArgs e)
        {
            Utils.OpenSubwindow(new Order.OrderWindow());
        }

        private void OpenProduceWindow(object sender, RoutedEventArgs e)
        {
            Utils.OpenSubwindow(new Produce.ProduceWindow());
        }

        private void OpenPurchaseWindow(object sender, RoutedEventArgs e)
        {
            Utils.OpenSubwindow(new Purchase.PurchaseWindow());
        }

        private void OpenUserWindow(object sender, RoutedEventArgs e)
        {
            Utils.OpenSubwindow(new User.UserWindow());
        }
    }
}
