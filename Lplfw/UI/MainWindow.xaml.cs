using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

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

        private void ImageMouseEnter(object sender, MouseEventArgs e)
        {
            Image ig = sender as Image;
            if (ig.Tag.ToString() == "close")
            {
                Uri ur = new Uri("images/close1.png", UriKind.Relative);
                BitmapImage bp = new BitmapImage(ur);
                ig.Source = bp;
            }
            else
            {
                Uri ur = new Uri("images/mini1.png", UriKind.Relative);
                BitmapImage bp = new BitmapImage(ur);
                ig.Source = bp;
            }
        }

        private void ImageMouseLeave(object sender, MouseEventArgs e)
        {
            Image ig = sender as Image;
            if (ig.Tag.ToString() == "close")
            {
                Uri ur = new Uri("images/close0.png", UriKind.Relative);
                BitmapImage bp = new BitmapImage(ur);
                ig.Source = bp;
            }
            else
            {
                Uri ur = new Uri("images/mini0.png", UriKind.Relative);
                BitmapImage bp = new BitmapImage(ur);
                ig.Source = bp;
            }
        }

        private void ImageMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image ig = sender as Image;
            if (ig.Tag.ToString() == "close")
            {
                this.Close();
            }
            else this.WindowState = System.Windows.WindowState.Minimized;
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

        private void MoveWindow(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
