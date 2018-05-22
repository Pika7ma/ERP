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
