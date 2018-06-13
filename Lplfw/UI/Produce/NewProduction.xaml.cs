using System;
using System.Linq;
using System.Windows;
using Lplfw.DAL;
using System.Threading;
using System.Windows.Controls;

namespace Lplfw.UI.Produce
{
    /// <summary>
    /// NewProduction.xaml 的交互逻辑
    /// </summary>
    public partial class NewProduction : Window
    {
        private ProductionViewModel production;

        public NewProduction()
        {
            InitializeComponent();

            production = new ProductionViewModel(Utils.CurrentUser.Id);
            SetControls();
        }

        private void SetControls()
        {
            txtCode.Binding(production, "TxtCode");
            cbProduct.Binding(production, "CbProduct");
            dpThinkFinishedAt.Binding(production, "DpThinkFinishedAt");
            txtDescription.Binding(production, "TxtDescription");
            dpThinkFinishedAt.Value = DateTime.Now;
            new Thread(new ThreadStart(SetControlsThread)).Start();
        }

        private void SetControlsThread()
        {
            var _items = ProductClass.GetAllClasses(false);
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                cbClass.ItemsSource = _items;
            });
        }

        private void SelectProductClass(object sender, SelectionChangedEventArgs e)
        {
            var _class = cbClass.SelectedValue as int?;
            if (_class == null) return;
            cbProduct.ItemsSource = ProductClass.GetSubClassProducts(_class);
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            if (production.CanSubmit)
            {
                var _rtn = production.CreateNew();
                DialogResult = _rtn;
            }
            else
            {
                txtMessage.Text = production.TxtCheckMessage;
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
