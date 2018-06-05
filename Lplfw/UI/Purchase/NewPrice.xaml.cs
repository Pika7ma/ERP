using Lplfw.DAL;
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Lplfw.UI.Purchase
{
    /// <summary>
    /// NewPrice.xaml 的交互逻辑
    /// </summary>
    public partial class NewPrice : Window
    {
        private int? classId;
        private MaterialPriceViewModel materialPrice;

        public NewPrice(int? index)
        {
            InitializeComponent();
            materialPrice = new MaterialPriceViewModel();
            Title = "新建报价";
            SetControls();
            classId = index;
            btnConfirm.Click += SubmitNewPrice;
        }

        public NewPrice(MaterialPriceView materialPriceView)
        {
            InitializeComponent();
            materialPrice = new MaterialPriceViewModel(materialPriceView);
            Title = "修改报价";
            SetControls();
            classId = materialPriceView.ClassId;
            cbMaterialClass.IsEnabled = false;
            cbMaterial.IsEnabled = false;
            cbSupplier.IsEnabled = false;
            btnConfirm.Click += SubmitEditPrice;
        }

        private void SetControls()
        {
            cbMaterial.Binding(materialPrice, "CbMaterial");
            cbSupplier.Binding(materialPrice, "CbSupplier");
            txtPrice.Binding(materialPrice, "TxtPrice");
            txtStartQuantity.Binding(materialPrice, "TxtStartQuantity");
            txtMaxQuantity.Binding(materialPrice, "TxtMaxQuantity");
            new Thread(new ParameterizedThreadStart(SetControlsThread)).Start(classId);
        }

        private void SetControlsThread(object obj)
        {
            var _id = obj as int?;
            using (var _db = new ModelContainer())
            {
                var _materialclasses = _db.MaterialClassSet.ToList();
                var _suppliers = _db.SupplierSet.ToList();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    cbMaterialClass.ItemsSource = _materialclasses;
                    cbSupplier.ItemsSource = _suppliers;
                });
            }
        }

        private void SelectMaterialClass(object sender, SelectionChangedEventArgs e)
        {
            var _class = cbMaterialClass.SelectedValue;
            new Thread(new ParameterizedThreadStart(SelectMaterialClassThread)).Start(_class);
        }

        private void SelectMaterialClassThread(object obj)
        {
            var _id = obj as int?;
            using (var _db = new ModelContainer())
            {
                var _materials = MaterialClass.GetSubClassMaterials(_id);
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    cbMaterial.ItemsSource = _materials;
                });
            }
        }

        private void SubmitNewPrice(object sender, RoutedEventArgs e)
        {
            if (materialPrice.CanSubmit)
            {
                var _rtn = materialPrice.CreateNew();
                if (_rtn == true)
                {
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("新建报价失败, 该供应商在该材料上可能已有报价", null, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                txtMessage.Text = materialPrice.TxtCheckMessage;
            }
        }

        private void SubmitEditPrice(object sender, RoutedEventArgs e)
        {
            if (materialPrice.CanSubmit)
            {
                var _rtn = materialPrice.SaveChanges();
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
                txtMessage.Text = materialPrice.TxtCheckMessage;
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
