using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Lplfw.DAL;

namespace Lplfw.UI.Bom
{
    /// <summary>
    /// NewRecipeItem.xaml 的交互逻辑
    /// </summary>
    public partial class NewRecipeItem : Window
    {
        private RecipeItemViewModel item;

        public NewRecipeItem()
        {
            InitializeComponent();
            Title = "新建配方条目";
            item = new RecipeItemViewModel();
            btnConfirm.Click += SubmitNewItem;
            SetControls();
        }

        public NewRecipeItem(RecipeItem item)
        {
            InitializeComponent();
            Title = "修改配方条目";
            this.item = new RecipeItemViewModel(item);
            btnConfirm.Click += SubmitEditItem;
            cbMaterial.SelectedValue = item.MaterialId;
            cbClass.IsEnabled = false;
            cbMaterial.IsEnabled = false;
            SetControls();
        }

        private void SetControls()
        {
            cbMaterial.Binding(item, "CbMaterial");
            txtQuantity.Binding(item, "TxtQuantity");
            new Thread(new ThreadStart(SetControlsThread)).Start();
        }

        private void SetControlsThread()
        {
            var _classes = MaterialClass.GetAllClasses(false);
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                cbClass.ItemsSource = _classes;
            });
        }

        private void SelectMaterialClass(object sender, SelectionChangedEventArgs e)
        {
            var _id = cbClass.SelectedValue as int?;
            if (_id == null) return;
            var _materials = MaterialClass.GetSubClassMaterials(_id);
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                cbMaterial.ItemsSource = _materials;
            });
        }

        private void SubmitNewItem(object sender, RoutedEventArgs e)
        {
            if (item.CanSubmit)
            {
                Utils.TempObject = item.Object;
                DialogResult = true;
            }
            else
            {
                txtMessage.Text = item.TxtCheckMessage;
            }
        }

        private void SubmitEditItem(object sender, RoutedEventArgs e)
        {
            if (item.CanSubmit)
            {
                Utils.TempObject = item.Object;
                DialogResult = true;
            }
            else
            {
                txtMessage.Text = item.TxtCheckMessage;
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
