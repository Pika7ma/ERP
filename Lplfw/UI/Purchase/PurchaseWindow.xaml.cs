using Lplfw.BLL.Purchase;
using Lplfw.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Lplfw.UI.Purchase
{
    /// <summary>
    /// PurchaseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PurchaseWindow : Window
    {
        public PurchaseWindow()
        {
            InitializeComponent();
            RefreshtvPrice();
            var _thread = new Thread(new ThreadStart(Rdpthread));
            _thread.Start();
            var _thread2 = new Thread(new ThreadStart(Rdsthread));
            _thread2.Start();
        }

        private void LoadComponent(object sender, RoutedEventArgs e)
        {
            var _thread = new Thread(new ThreadStart(LoadComponentThread));
            _thread.Start();
        }

        private void LoadComponentThread()
        {
            List<SearchCombobox> _sbbp = new List<SearchCombobox>();
            var _material = new SearchCombobox("材料", 1);
            var _supplier = new SearchCombobox("供应商", 2);
            _sbbp.Add(_material);
            _sbbp.Add(_supplier);

            List<SearchCombobox> _sbbs = new List<SearchCombobox>();
            var _name = new SearchCombobox("名称", 1);
            var _contact = new SearchCombobox("联系人", 2);
            var _location = new SearchCombobox("地址", 3);
            var _tel = new SearchCombobox("电话", 4);
            _sbbs.Add(_name);
            _sbbs.Add(_contact);
            _sbbs.Add(_location);
            _sbbs.Add(_tel);
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                cbbSearchSupplierClass.ItemsSource = _sbbs;
                cbbSearchPriceClass.ItemsSource = _sbbp;
            });
        }

        private void RefreshdgPrice()
        {
            var _thread = new Thread(new ThreadStart(Rdpthread));
            _thread.Start();
        }

        private void Rdpthread()
        {
            using (var _db = new DAL.ModelContainer())
            {
                var _dg = MaterialPriceView.GetAllPrice();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    dgPrice.ItemsSource = _dg;
                });
            }
        }

        private void RefreshdgSupplier()
        {
            var _thread = new Thread(new ThreadStart(Rdsthread));
            _thread.Start();
        }

        private void Rdsthread()
        {
            using (var _db = new DAL.ModelContainer())
            {
                var _dg = _db.SupplierSet.Select(i => i).ToList();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    dgSupplier.ItemsSource = _dg;
                });
            }
        }

        private void BtnShowAll2Click(object sender, RoutedEventArgs e)
        {
            RefreshdgPrice();
        }

        private void BtnShowAll3Click(object sender, RoutedEventArgs e)
        {
            RefreshdgSupplier();
        }

        private void BtnNewPriceClick(object sender, RoutedEventArgs e)
        {
            var _win = new Purchase.NewPriceClass();
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                RefreshdgPrice();
            }
        }

        private void BtnEditPriceClick(object sender, RoutedEventArgs e)
        {
            if (dgPrice.SelectedItem == null)
            {
                MessageBox.Show("需要选中报价条目");
            }
            else
            {
                int _mid = (int)((MaterialPriceView)dgPrice.SelectedItem).mId;
                int _sid = (int)((MaterialPriceView)dgPrice.SelectedItem).sId;
                var _win = new Purchase.EditPriceClass(_mid, _sid);
                var _rtn = _win.ShowDialog();
                if (_rtn == true)
                {
                    RefreshdgPrice();
                }
            }
        }

        private void BtnNewSupplierClick(object sender, RoutedEventArgs e)
        {
            var _win = new Purchase.NewSupplierClass();
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                RefreshdgSupplier();
            }
        }

        private void BtnEditSupplierClick(object sender, RoutedEventArgs e)
        {
            if (dgSupplier.SelectedItem == null)
            {
                MessageBox.Show("需要选中供应商条目");
            }
            else
            {
                int _id = (int)((Supplier)dgSupplier.SelectedItem).Id;
                var _win = new Purchase.EditSupplierClass(_id);
                var _rtn = _win.ShowDialog();
                if (_rtn == true)
                {
                    RefreshdgSupplier();
                }
            }
        }

        private void BtnDeletePriceClick(object sender, RoutedEventArgs e)
        {
            if (dgPrice.SelectedItem == null)
            {
                MessageBox.Show("需要选中报价条目");
            }
            else
            {

                new Thread(new ParameterizedThreadStart(Bdpthread)).Start(new int[] {
                    (int)((MaterialPriceView)dgPrice.SelectedItem).mId,
                    (int)((MaterialPriceView)dgPrice.SelectedItem).sId
                });
            }
        }

        private void Bdpthread(object li)
        {
            try
            {
                var _li = li as int[];
                var _mid = _li[0];
                var _sid = _li[1];
                using (var _db = new DAL.ModelContainer())
                {
                    var _price = _db.MaterialPriceSet.First(i => i.MaterialId == _mid && i.SupplierId == _sid);
                    _db.MaterialPriceSet.Remove(_price);
                    _db.SaveChanges();
                }
                RefreshdgPrice();
                MessageBox.Show("删除成功");
            }
            catch (Exception)
            {
                MessageBox.Show("删除失败");
            }
        }

        private void BtnDeleteSupplierClick(object sender, RoutedEventArgs e)
        {
            if (dgSupplier.SelectedItem == null)
            {
                MessageBox.Show("需要选中供应商条目");
            }
            else
            {
                int _id = (int)((Supplier)dgSupplier.SelectedItem).Id;
                new Thread(new ParameterizedThreadStart(Bdsthread)).Start(_id);
                MessageBox.Show("删除成功");
            }
        }

        private void Bdsthread(object id)
        {
            int _id = (int)id;
            using (var _db = new DAL.ModelContainer())
            {
                if (_db.MaterialPriceSet.Where(i => i.SupplierId == _id).Count() == 0)
                {
                    var _temp = _db.SupplierSet.First(i => i.Id == _id);
                    _db.SupplierSet.Remove(_temp);
                    _db.SaveChanges();
                    RefreshdgSupplier();
                }
                else
                {
                    var _rtn = MessageBox.Show("是否删除其下的所有报价", null, MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (_rtn == MessageBoxResult.OK)
                    {
                        var _temp = _db.SupplierSet.First(i => i.Id == _id);
                        var _temp2 = _db.MaterialPriceSet.Where(i => i.SupplierId == _id).ToList();
                        _db.MaterialPriceSet.RemoveRange(_temp2);
                        _db.SupplierSet.Remove(_temp);
                        _db.SaveChanges();
                        RefreshdgPrice();
                        RefreshdgSupplier();
                    }
                    else
                    {

                    }
                }
            }
        }

        private void RefreshtvPrice()
        {
            tvPrice.Items.Clear();
            MaterialClass.SetTreeFromRoot(ref tvPrice);
        }

        private void Tpthread(object ml)
        {
            var _ml = (List<Material>)ml;
            using (var _db = new DAL.ModelContainer())
            {
                var _dg = MaterialPriceView.GetbyIdList(_ml);
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    dgPrice.ItemsSource = _dg;
                });
            }
        }

        private void BtnNewPrice2Click(object sender, RoutedEventArgs e)
        {
            if (dgSupplier.SelectedItem == null)
            {
                MessageBox.Show("需要选中供应商条目");
            }
            else
            {
                int _sid = (int)((Supplier)dgSupplier.SelectedItem).Id;
                var _win = new Purchase.NewPriceWithSupplierClass(_sid);
                var _rtn = _win.ShowDialog();
                if (_rtn == true)
                {
                    int _id = (int)((Supplier)dgSupplier.SelectedItem).Id;
                    dgMaterialinSupplier.ItemsSource = MaterialPriceView.GetbySupplierId(_id);
                }
            }
        }

        private void BtnEditPrice2Click(object sender, RoutedEventArgs e)
        {
            if (dgMaterialinSupplier.SelectedItem == null)
            {
                MessageBox.Show("需要选中报价条目");
            }
            else
            {
                int _mid = (int)((MaterialPriceView)dgMaterialinSupplier.SelectedItem).mId;
                int _sid = (int)((Supplier)dgSupplier.SelectedItem).Id;
                var _win = new Purchase.EditPriceClass(_mid, _sid);
                var _rtn = _win.ShowDialog();
                if (_rtn == true)
                {
                    int _id = (int)((Supplier)dgSupplier.SelectedItem).Id;
                    dgMaterialinSupplier.ItemsSource = MaterialPriceView.GetbySupplierId(_id);
                }
            }
        }

        private void BtnDeletePrice2Click(object sender, RoutedEventArgs e)
        {
            if (dgMaterialinSupplier.SelectedItem == null)
            {
                MessageBox.Show("需要选中报价条目");
            }
            else
            {
                int _mid = (int)((MaterialPriceView)dgMaterialinSupplier.SelectedItem).mId;
                int _sid = (int)((Supplier)dgSupplier.SelectedItem).Id;
                using (var _db = new DAL.ModelContainer())
                {
                    var _price = _db.MaterialPriceSet.First(i => i.MaterialId == _mid && i.SupplierId == _sid);
                    _db.MaterialPriceSet.Remove(_price);
                    _db.SaveChanges();
                    int _id = (int)((Supplier)dgSupplier.SelectedItem).Id;
                    dgMaterialinSupplier.ItemsSource = MaterialPriceView.GetbySupplierId(_id);
                    MessageBox.Show("删除成功");
                }
            }
        }

        private void BtSearchPriceClassClick(object sender, RoutedEventArgs e)
        {
            switch ((int)cbbSearchPriceClass.SelectedValue)
            {
                case 1: dgPrice.ItemsSource = MaterialPriceView.GetbyMaterialName(searchPrice.Text); break;
                case 2: dgPrice.ItemsSource = MaterialPriceView.GetbySupplierName(searchPrice.Text); break;
            }
        }

        private void BtSearchSupplierClassClick(object sender, RoutedEventArgs e)
        {
            using (var _db = new DAL.ModelContainer())
            {
                switch ((int)cbbSearchSupplierClass.SelectedValue)
                {
                    case 1: dgSupplier.ItemsSource = _db.SupplierSet.Where(i => i.Name == searchSupplier.Text).ToList(); break;
                    case 2: dgSupplier.ItemsSource = _db.SupplierSet.Where(i => i.Contact == searchSupplier.Text).ToList(); break;
                    case 3: dgSupplier.ItemsSource = _db.SupplierSet.Where(i => i.Location == searchSupplier.Text).ToList(); break;
                    case 4: dgSupplier.ItemsSource = _db.SupplierSet.Where(i => i.Tel == searchSupplier.Text).ToList(); break;
                }
            }
        }

        private void TvPriceMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                int _id = (int)((TreeViewItem)tvPrice.SelectedItem).DataContext;
                var _ml = MaterialClass.GetSubClassMaterials(_id);
                new Thread(new ParameterizedThreadStart(Tpthread)).Start(_ml);
            }
            catch (Exception)
            {

            }
        }

        private void DgSupplierMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int _id = (int)((Supplier)dgSupplier.SelectedItem).Id;
            new Thread(new ParameterizedThreadStart(Dsthread)).Start(_id);
        }

        private void Dsthread(object id)
        {
            int _id = (int)id;
            var _temp = MaterialPriceView.GetbySupplierId(_id);
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgMaterialinSupplier.ItemsSource = _temp;
            });
        }
    }
}
