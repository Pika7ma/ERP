using Lplfw.DAL;
using Lplfw.Bll.Bom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace Lplfw.UI.Bom
{
    /// <summary>
    /// BomWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BomWindow : Window
    {
        public BomWindow()
        {
            InitializeComponent();
            cbSearchProduct.ItemsSource = ProductFields;
            cbSearchProduct.SelectedValue = 1;
            cbSearchMaterial.ItemsSource = MaterialFields;
            cbSearchMaterial.SelectedValue = 1;
        }

        #region 产品管理
        /// <summary>
        /// "显示所有产品"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAllProducts(object sender, RoutedEventArgs e)
        {
            using (var _db = new ModelContainer())
            {
                var _products = _db.ProductSet.ToList();
                dgProduct.ItemsSource = _products;
                var _node = tvProduct.SelectedItem as TreeViewItem;
                if (_node != null) _node.IsSelected = false;
            }
        }

        /// <summary>
        /// "新建产品类别"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewProductClass(object sender, RoutedEventArgs e)
        {
            var _classId = Utils.GetTreeViewSelectedValue(ref tvProduct);
            var _win = new NewClass(_classId, true, true);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                RefreshTvProduct();
            }
        }

        /// <summary>
        /// "修改产品类别"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditProductClass(object sender, RoutedEventArgs e)
        {
            var _classId = Utils.GetTreeViewSelectedValue(ref tvProduct);
            if (_classId == null) return;
            var _win = new NewClass(_classId, false, true);
            var _rtn = _win.ShowDialog();
            if (_rtn == true){
                RefreshTvProduct();
            }
        }

        /// <summary>
        /// "删除产品类别"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteProductClass(object sender, RoutedEventArgs e)
        {
            var _classId = Utils.GetTreeViewSelectedValue(ref tvProduct);
            if (_classId == null) return;
            var _win = new DeleteClass(_classId, true);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                RefreshTvProduct();
                dgProduct.ItemsSource = null;
            }
        }

        /// <summary>
        /// "新建产品"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewProduct(object sender, RoutedEventArgs e)
        {
            var _classId = Utils.GetTreeViewSelectedValue(ref tvProduct);
            var _win = new NewProduct(_classId, true);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                dgProduct.ItemsSource = ProductClass.GetSubClassProducts((int)_classId);
            }
        }

        /// <summary>
        /// "修改产品"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditProduct(object sender, RoutedEventArgs e)
        {
            var _product = dgProduct.SelectedItem as Product;
            if (_product == null) return;
            var _win = new NewProduct(_product.Id, false);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                var _classId = Utils.GetTreeViewSelectedValue(ref tvProduct);
                dgProduct.ItemsSource = ProductClass.GetSubClassProducts((int)_classId);
                dgRecipe.ItemsSource = RecipeView.GetByProductId(_product.Id);
            }
        }

        /// <summary>
        /// "删除产品"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EraseProduct(object sender, RoutedEventArgs e)
        {
            var _product = dgProduct.SelectedItem as Product;
            if (_product == null) return;
            var _rtn = MessageBox.Show($"真的要删除产品 {_product.Name} 吗? 可能会导致严重后果!", null, MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (_rtn == MessageBoxResult.OK)
            {
                try
                {
                    using (var _db = new ModelContainer())
                    {
                        _db.Database.ExecuteSqlCommand("SET FOREIGN_KEY_CHECKS=0;");
                        var _current = _db.ProductSet.FirstOrDefault(i => i.Id == _product.Id);
                        _db.ProductSet.Remove(_current);
                        _db.Database.ExecuteSqlCommand("SET FOREIGN_KEY_CHECKS=1;");
                        _db.SaveChanges();
                    }
                    var _classId = Utils.GetTreeViewSelectedValue(ref tvProduct);
                    dgProduct.ItemsSource = ProductClass.GetSubClassProducts((int)_classId);
                    dgRecipe.ItemsSource = null;
                }
                catch (Exception)
                {
                    MessageBox.Show("由于此产品与其他数据有重要关联, 无法删除", null, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// 修改产品"生产\停产"状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeProductStatus(object sender, RoutedEventArgs e)
        {
            var _product = dgProduct.SelectedItem as Product;
            if (_product == null) return;
            using (var _db = new ModelContainer())
            {
                var _current = _db.ProductSet.FirstOrDefault(i => i.Id == _product.Id);
                if (_current.Status == "生产")
                {
                    _current.Status = "停产";
                }
                else {
                    _current.Status = "生产";
                }
                _db.SaveChanges();
                _product.Status = _current.Status;
                dgProduct.Items.Refresh();
            }
        }

        /// <summary>
        /// 选择了TreeView中的产品种类, 在DataGrid中显示相应的产品列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectProductClass(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (tvProduct.SelectedItem == null) return;
            var _id = (int)Utils.GetTreeViewSelectedValue(ref tvProduct);
            dgProduct.ItemsSource = ProductClass.GetSubClassProducts(_id);
        }

        /// <summary>
        /// 产品DataGrid的选择项被改变的时候, 在配方DataGrid中显示相应的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDgProductSelectedChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var _product = dgProduct.SelectedItem as Product;
            if (_product == null) return;
            dgRecipe.ItemsSource = RecipeView.GetByProductId(_product.Id);
        }

        /// <summary>
        /// 根据条件模糊查询产品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchProduct(object sender, RoutedEventArgs e)
        {
            if (cbSearchProduct.SelectedValue == null) return;
            var _str = txtSearchProduct.Text;
            List<Product> _products;
            var _class = Utils.GetTreeViewSelectedValue(ref tvProduct);
            var _list = ProductClass.GetSubClassProducts(_class);
            switch (cbSearchProduct.SelectedValue)
            {
                case 0:
                    _products = _list.Where(i => i.Status.Contains(_str)).ToList();
                    break;
                case 1:
                    _products = _list.Where(i => i.Name.Contains(_str)).ToList();
                    break;
                case 2:
                    _products = _list.Where(i => i.Type.Contains(_str)).ToList();
                    break;
                case 3:
                    _products = _list.Where(i => i.Format.Contains(_str)).ToList();
                    break;
                default:
                    _products = null;
                    break;
            }
            dgProduct.ItemsSource = _products;
            dgRecipe.ItemsSource = null;
        }
        #endregion

        #region 材料管理
        /// <summary>
        /// "显示所有原料"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAllMaterials(object sender, RoutedEventArgs e)
        {
            using (var _db = new ModelContainer())
            {
                dgMaterial.ItemsSource = _db.MaterialSet.ToList();
                var _node = tvProduct.SelectedItem as TreeViewItem;
                if (_node != null) _node.IsSelected = false;
            }
        }

        /// <summary>
        /// "新建原料类别"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewMaterialClass(object sender, RoutedEventArgs e)
        {
            var _classId = Utils.GetTreeViewSelectedValue(ref tvMaterial);
            var _win = new NewClass(_classId, true, false);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                RefreshTvMaterial();
            }
        }

        /// <summary>
        /// "修改原料类别"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditMaterialClass(object sender, RoutedEventArgs e)
        {
            var _classId = Utils.GetTreeViewSelectedValue(ref tvMaterial);
            if (_classId == null) return;
            var _win = new NewClass(_classId, false, false);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                RefreshTvMaterial();
            }
        }

        //TODO: 删除材料类别
        private void DeleteMaterialClass(object sender, RoutedEventArgs e)
        {
            var _classId = Utils.GetTreeViewSelectedValue(ref tvMaterial);
            if (_classId == null) return;
            var _win = new DeleteClass(_classId, false);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                RefreshTvMaterial();
                dgMaterial.ItemsSource = null;
            }
        }

        /// <summary>
        /// "新建材料"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewMaterial(object sender, RoutedEventArgs e)
        {
            var _classId = Utils.GetTreeViewSelectedValue(ref tvMaterial);
            var _win = new NewMaterial(_classId, true);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                dgMaterial.ItemsSource = MaterialClass.GetSubClassMaterials((int)_classId);
            }
        }

        /// <summary>
        /// "修改材料"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditMaterial(object sender, RoutedEventArgs e)
        {
            var _material = dgMaterial.SelectedItem as Material;
            if (_material == null) return;
            var _win = new NewMaterial(_material.Id, false);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                var _classId = Utils.GetTreeViewSelectedValue(ref tvMaterial);
                dgMaterial.ItemsSource = MaterialClass.GetSubClassMaterials((int)_classId);
            }
        }

        /// <summary>
        /// "删除材料"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EraseMaterial(object sender, RoutedEventArgs e)
        {
            var _material = dgMaterial.SelectedItem as Material;
            if (_material == null) return;
            var _rtn = MessageBox.Show($"真的要删除原料 {_material.Name} 吗? 可能会导致严重后果!", null, MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (_rtn == MessageBoxResult.OK)
            {
                try
                {
                    using (var _db = new ModelContainer())
                    {
                        _db.Database.ExecuteSqlCommand("SET FOREIGN_KEY_CHECKS=0;");
                        var _current = _db.MaterialSet.FirstOrDefault(i => i.Id == _material.Id);
                        _db.MaterialSet.Remove(_current);
                        _db.Database.ExecuteSqlCommand("SET FOREIGN_KEY_CHECKS=1;");
                        _db.SaveChanges();
                    }
                    var _classId = Utils.GetTreeViewSelectedValue(ref tvMaterial);
                    dgMaterial.ItemsSource = MaterialClass.GetSubClassMaterials((int)_classId);
                }
                catch (Exception)
                {
                    MessageBox.Show("由于此原料与其他数据有重要关联, 无法删除", null, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        /// <summary>
        /// 修改材料"可用\停用"状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeMaterialStatus(object sender, RoutedEventArgs e)
        {
            var _material = dgMaterial.SelectedItem as Material;
            if (_material == null) return;
            using (var _db = new ModelContainer())
            {
                var _current = _db.MaterialSet.FirstOrDefault(i => i.Id == _material.Id);
                if (_current.Status == "可用")
                {
                    _current.Status = "停用";
                }
                else
                {
                    _current.Status = "可用";
                }
                _db.SaveChanges();
                _material.Status = _current.Status;
                dgMaterial.Items.Refresh();
            }
        }

        /// <summary>
        /// 选择了TreeView中的原料种类, 在DataGrid中显示相应的原料列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectMaterialClass(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (tvMaterial.SelectedItem == null) return;
            var _id = (int)Utils.GetTreeViewSelectedValue(ref tvMaterial);
            dgMaterial.ItemsSource = MaterialClass.GetSubClassMaterials(_id);
        }


        /// <summary>
        /// 根据条件模糊查询原料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchMaterial(object sender, RoutedEventArgs e)
        {
            if (cbSearchMaterial.SelectedValue == null) return;
            var _str = txtSearchMaterial.Text;
            List<Material> _materials;
            var _class = Utils.GetTreeViewSelectedValue(ref tvMaterial);
            var _list = MaterialClass.GetSubClassMaterials(_class);
            switch (cbSearchProduct.SelectedValue)
            {
                case 0:
                    _materials = _list.Where(i => i.Status.Contains(_str)).ToList();
                    break;
                case 1:
                    _materials = _list.Where(i => i.Name.Contains(_str)).ToList();
                    break;
                case 2:
                    _materials = _list.Where(i => i.Format.Contains(_str)).ToList();
                    break;
                default:
                    _materials = null;
                    break;
            }
            dgMaterial.ItemsSource = _materials;
            dgRecipe.ItemsSource = null;
        }

        /// <summary>
        /// 从Excel表格中导入原料信息到数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportExcel(object sender, RoutedEventArgs e)
        {
            var _reader = new ExcelReader();
            if (_reader.OpenFileDialog())
            {
                try
                {
                    _reader.ReadFile();
                    var _list = _reader.Materials;
                    using (var _db = new ModelContainer())
                    {
                        _db.MaterialSet.AddRange(_list);
                        _db.SaveChanges();
                        var _classId = Utils.GetTreeViewSelectedValue(ref tvMaterial);
                        dgMaterial.ItemsSource = MaterialClass.GetSubClassMaterials((int)_classId);
                        MessageBox.Show("导入成功!");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("读取文件失败!", null, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("打开文件时发生错误!", null, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Utils
        /// <summary>
        /// 刷新产品种类的TreeView
        /// </summary>
        private void RefreshTvProduct()
        {
            tvProduct.Items.Clear();
            ProductClass.SetTreeFromRoot(ref tvProduct);
        }

        /// <summary>
        /// 刷新原料种类的TreeView
        /// </summary>
        private void RefreshTvMaterial()
        {
            tvMaterial.Items.Clear();
            MaterialClass.SetTreeFromRoot(ref tvMaterial);
        }

        /// <summary>
        /// TabControl的页面跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabRouter(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if (tabProduct.IsSelected)
                {
                    RefreshTvProduct();
                }
                else if (tabMaterial.IsSelected)
                {
                    RefreshTvMaterial();
                }
            }
        }

        static public List<Utils.KeyValue> ProductFields = new List<Utils.KeyValue>
        {
            new Utils.KeyValue { ID=0, Name="状态" },
            new Utils.KeyValue { ID=1, Name="名称" },
            new Utils.KeyValue { ID=2, Name="型号" },
            new Utils.KeyValue { ID=3, Name="规格" }
        };

        static public List<Utils.KeyValue> MaterialFields = new List<Utils.KeyValue>
        {
            new Utils.KeyValue { ID=0, Name="状态" },
            new Utils.KeyValue { ID=1, Name="名称" },
            new Utils.KeyValue { ID=2, Name="规格" }
        };
        #endregion

    }
}
