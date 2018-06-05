using Lplfw.DAL;
using System.Windows;

namespace Lplfw.UI.Purchase
{
    /// <summary>
    /// NewSupplier.xaml 的交互逻辑
    /// </summary>
    public partial class NewSupplier : Window
    {
        private SupplierViewModel supplier;

        public NewSupplier()
        {
            InitializeComponent();
            supplier = new SupplierViewModel();
            SetControls();
            btnConfirm.Click += SubmitNewSupplier;
        }

        public NewSupplier(Supplier supplier)
        {
            InitializeComponent();
            this.supplier = new SupplierViewModel(supplier);
            SetControls();
            btnConfirm.Click += SubmitEditSupplier;
        }

        private void SetControls()
        {
            txtName.Binding(supplier, "TxtName");
            txtContact.Binding(supplier, "TxtContact");
            txtLocation.Binding(supplier, "TxtLocation");
            txtTel.Binding(supplier, "TxtTel");
        }

        private void SubmitNewSupplier(object sender, RoutedEventArgs e)
        {
            if (supplier.CanSubmit)
            {
                var _rtn = supplier.CreateNew();
                if (_rtn == true)
                {
                    DialogResult = true;
                }
                else
                {
                    DialogResult = false;
                }
            }
            else
            {
                txtMessage.Text = supplier.TxtCheckMessage;
            }
        }

        private void SubmitEditSupplier(object sender, RoutedEventArgs e)
        {
            if (supplier.CanSubmit)
            {
                var _rtn = supplier.SaveChanges();
                if (_rtn == true)
                {
                    DialogResult = true;
                }
                else
                {
                    DialogResult = false;
                }
            }
            else
            {
                txtMessage.Text = supplier.TxtCheckMessage;
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
