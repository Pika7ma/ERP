using System.Windows;
using Lplfw.DAL;

namespace Lplfw.UI.Order
{
    /// <summary>
    /// GenerateRequsition.xaml 的交互逻辑
    /// </summary>
    public partial class GenerateRequsition : Window
    {
        private RequisitionViewModel requisition;

        public GenerateRequsition(int salesId)
        {
            InitializeComponent();

            requisition = new RequisitionViewModel(Utils.CurrentUser.Id, salesId);
            txtCode.Binding(requisition, "TxtCode");
            txtDescription.Binding(requisition, "TxtDescription");
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            if (requisition.CanSubmit)
            {
                var _rtn = requisition.CreateNew();
                DialogResult = _rtn;
            }
            else
            {
                txtMessage.Text = requisition.TxtCheckMessage;
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
