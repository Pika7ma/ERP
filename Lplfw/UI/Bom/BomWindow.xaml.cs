using Lplfw.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using Lplfw.BLL;

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
            new Thread(new ThreadStart(CheckPrivilege)).Start();
        }

        #region 产品管理
        /// <summary>
        /// "显示所有产品"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAllProducts(object sender, RoutedEventArgs e)
        {
            new Thread(new ThreadStart(ShowAllProductsThread)).Start();
        }

        private void ShowAllProductsThread()
        {
            using (var _db = new ModelContainer())
            {
                var _products = _db.ProductSet.ToList();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    dgProduct.ItemsSource = _products;
                    var _node = tvProduct.SelectedItem as TreeViewItem;
                    if (_node != null) _node.IsSelected = false;
                });
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
            if (_rtn == true)
            {
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
                new Thread(new ParameterizedThreadStart(NewProductThread)).Start(_classId);
            }
        }

        private void NewProductThread(object id)
        {
            var _id = id as int?;
            var _products = ProductClass.GetSubClassProducts(_id);
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgProduct.ItemsSource = _products;
            });
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
                new Thread(new ParameterizedThreadStart(EditProductThread)).Start(new int?[] { _classId, _product.Id });
            }
        }

        private void EditProductThread(object obj)
        {
            var _obj = obj as int?[];
            var _classId = _obj[0];
            var _productId = _obj[1];
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgProduct.ItemsSource = ProductClass.GetSubClassProducts((int)_classId);
                dgRecipe.ItemsSource = RecipeView.GetByProductId((int)_productId);
            });
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
                new Thread(new ParameterizedThreadStart(EraseProductThread)).Start(_product.Id);
            }
        }

        private void EraseProductThread(object id)
        {
            try
            {
                var _id = id as int?;
                using (var _db = new ModelContainer())
                {
                    _db.Database.ExecuteSqlCommand("SET FOREIGN_KEY_CHECKS=0;");
                    var _current = _db.ProductSet.FirstOrDefault(i => i.Id == _id);
                    _db.ProductSet.Remove(_current);
                    _db.Database.ExecuteSqlCommand("SET FOREIGN_KEY_CHECKS=1;");
                    _db.SaveChanges();
                }
                Dispatcher.BeginInvoke((Action)delegate () {
                    var _classId = Utils.GetTreeViewSelectedValue(ref tvProduct);
                    dgProduct.ItemsSource = ProductClass.GetSubClassProducts((int)_classId);
                    dgRecipe.ItemsSource = null;
                });
            }
            catch (Exception)
            {
                Dispatcher.BeginInvoke((Action)delegate () {
                    MessageBox.Show("由于此产品与其他数据有重要关联, 无法删除", null, MessageBoxButton.OK, MessageBoxImage.Error);
                });
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
            new Thread(new ParameterizedThreadStart(ChangeProductStatusThread)).Start(_product.Id);
        }

        private void ChangeProductStatusThread(object id)
        {
            var _id = id as int?;
            using (var _db = new ModelContainer())
            {
                var _current = _db.ProductSet.FirstOrDefault(i => i.Id == _id);
                if (_current.Status == "生产")
                {
                    _current.Status = "停产";
                }
                else
                {
                    _current.Status = "生产";
                }
                _db.SaveChanges();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    var _product = dgProduct.SelectedItem as Product;
                    _product.Status = _current.Status;
                    dgProduct.Items.Refresh();
                });
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
            new Thread(new ParameterizedThreadStart(SelectProductClassThread)).Start(_id);
        }

        private void SelectProductClassThread(object id)
        {
            var _id = (int)id;
            var _products = ProductClass.GetSubClassProducts(_id);
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgProduct.ItemsSource = _products;
            });
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
            new Thread(new ParameterizedThreadStart(OnDgProductSelectedChangedThread)).Start(_product.Id);
        }

        private void OnDgProductSelectedChangedThread(object id)
        {
            var _id = (int)id;
            var _recipes = RecipeView.GetByProductId(_id);
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgRecipe.ItemsSource = _recipes;
            });
        }

        /// <summary>
        /// 根据条件模糊查询产品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchProduct(object sender, RoutedEventArgs e)
        {
            if (cbSearchProduct.SelectedValue == null) return;

            new Thread(new ParameterizedThreadStart(SearchProductThread)).Start(new SearchParams
            {
                Class = Utils.GetTreeViewSelectedValue(ref tvProduct),
                Str = txtSearchProduct.Text,
                Type = (int)cbSearchProduct.SelectedValue
            });
        }

        private void SearchProductThread(object value)
        {
            var obj = value as SearchParams;
            var _class = obj.Class;
            var _str = obj.Str;
            var _type = obj.Type;
            List<Product> _products;
            var _list = ProductClass.GetSubClassProducts(_class);
            switch (_type)
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
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgProduct.ItemsSource = _products;
                dgRecipe.ItemsSource = null;
            });
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
            new Thread(new ThreadStart(ShowAllMaterialsThread)).Start();
        }

        private void ShowAllMaterialsThread()
        {
            using (var _db = new ModelContainer())
            {
                var _materials = _db.MaterialSet.ToList();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    dgMaterial.ItemsSource = _materials;
                    var _node = tvProduct.SelectedItem as TreeViewItem;
                    if (_node != null) _node.IsSelected = false;
                });
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

        /// <summary>
        /// "删除材料类别"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                new Thread(new ParameterizedThreadStart(NewMaterialThread)).Start(_classId);
            }
        }

        private void NewMaterialThread(object id)
        {
            var _id = id as int?;
            var _materials = MaterialClass.GetSubClassMaterials((int)_id);
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgMaterial.ItemsSource = _materials;
            });
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
                new Thread(new ParameterizedThreadStart(EditMaterialThread)).Start(_classId);
            }
        }

        private void EditMaterialThread(object id)
        {
            var _classId = id as int?;
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgMaterial.ItemsSource = MaterialClass.GetSubClassMaterials((int)_classId);
            });
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
                new Thread(new ParameterizedThreadStart(EraseMaterialThread)).Start(_material.Id);
            }
        }

        private void EraseMaterialThread(object id)
        {
            try
            {
                var _id = id as int?;
                using (var _db = new ModelContainer())
                {
                    _db.Database.ExecuteSqlCommand("SET FOREIGN_KEY_CHECKS=0;");
                    var _current = _db.MaterialSet.FirstOrDefault(i => i.Id == _id);
                    _db.MaterialSet.Remove(_current);
                    _db.Database.ExecuteSqlCommand("SET FOREIGN_KEY_CHECKS=1;");
                    _db.SaveChanges();
                }
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    var _classId = Utils.GetTreeViewSelectedValue(ref tvMaterial);
                    dgMaterial.ItemsSource = MaterialClass.GetSubClassMaterials((int)_classId);
                });
            }
            catch (Exception)
            {
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    MessageBox.Show("由于此原料与其他数据有重要关联, 无法删除", null, MessageBoxButton.OK, MessageBoxImage.Error);
                });
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
            new Thread(new ParameterizedThreadStart(ChangeMaterialStatusThread)).Start(_material.Id);
        }

        private void ChangeMaterialStatusThread(object id)
        {
            var _id = id as int?;
            using (var _db = new ModelContainer())
            {
                var _current = _db.MaterialSet.FirstOrDefault(i => i.Id == _id);
                if (_current.Status == "可用")
                {
                    _current.Status = "停用";
                }
                else
                {
                    _current.Status = "可用";
                }
                _db.SaveChanges();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    var _material = dgMaterial.SelectedItem as Material;
                    _material.Status = _current.Status;
                    dgMaterial.Items.Refresh();
                });
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
            new Thread(new ParameterizedThreadStart(SelectMaterialClassThread)).Start(_id);
        }

        private void SelectMaterialClassThread(object id)
        {
            var _id = (int)id;
            var _material = MaterialClass.GetSubClassMaterials(_id);
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgMaterial.ItemsSource = _material;
            });
        }


        /// <summary>
        /// 根据条件模糊查询原料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchMaterial(object sender, RoutedEventArgs e)
        {
            if (cbSearchMaterial.SelectedValue == null) return;

            new Thread(new ParameterizedThreadStart(SearchMaterialThread)).Start(new SearchParams
            {
                Class = Utils.GetTreeViewSelectedValue(ref tvMaterial),
                Str = txtSearchMaterial.Text,
                Type = (int)cbSearchMaterial.SelectedValue
            });
        }

        private void SearchMaterialThread(object value)
        {
            var obj = value as SearchParams;
            var _class = obj.Class;
            var _str = obj.Str;
            var _type = obj.Type;
            List<Material> _materials;
            var _list = MaterialClass.GetSubClassMaterials(_class);
            switch (_type)
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
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgMaterial.ItemsSource = _materials;
            });
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
            new Utils.KeyValue { Id=0, Name="状态" },
            new Utils.KeyValue { Id=1, Name="名称" },
            new Utils.KeyValue { Id=2, Name="型号" },
            new Utils.KeyValue { Id=3, Name="规格" }
        };

        static public List<Utils.KeyValue> MaterialFields = new List<Utils.KeyValue>
        {
            new Utils.KeyValue { Id=0, Name="状态" },
            new Utils.KeyValue { Id=1, Name="名称" },
            new Utils.KeyValue { Id=2, Name="规格" }
        };

        class SearchParams
        {
            public int? Class { get; set; }
            public string Str { get; set; }
            public int Type { get; set; }
        }
        #endregion

        #region 权限控制

        private void CheckPrivilege()
        {
            using (var _db = new ModelContainer())
            {
                var _temp = _db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId == 1 && i.UserGroupId == Utils.CurrentUser.UserGroupId);
                if (_temp.Mode == 1)
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        OnlyRead();
                    });
                }
            }
        }
        /// <summary>
        /// 只读
        /// </summary>
        private void OnlyRead()
        {
            btnNewProductClass.Visibility = Visibility.Hidden;
            btnEditProductClass.Visibility = Visibility.Hidden;
            btnDelProductClass.Visibility = Visibility.Hidden;
            btnNewProduct.Visibility = Visibility.Hidden;
            btnEditProduct.Visibility = Visibility.Hidden;
            btnDelProduct.Visibility = Visibility.Hidden;
            btnProduceStatus.Visibility = Visibility.Hidden;
            btnNewMaterialClass.Visibility = Visibility.Hidden;
            btnNewMaterial.Visibility = Visibility.Hidden;
            btnEditMaterialClass.Visibility = Visibility.Hidden;
            btnEditMaterial.Visibility = Visibility.Hidden;
            btnDeleteMaterialClass.Visibility = Visibility.Hidden;
            btnEraseMaterial.Visibility = Visibility.Hidden;
            btnChangeMaterialStatus.Visibility = Visibility.Hidden;
        }
        #endregion

        private void ExportProductSheet(object sender, RoutedEventArgs e)
        {
            var writer = new ExcelWriter(ExcelWriter.Type.Product, dgProduct.ItemsSource as List<Product>);
            var _rtn = writer.Write();
            if (_rtn == true)
            {
                MessageBox.Show("导出成功!", null, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("导出失败!", null, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportMaterialSheet(object sender, RoutedEventArgs e)
        {

        }

        private void ExportRecipeSheet(object sender, RoutedEventArgs e)
        {
            var _item = dgProduct.SelectedItem as Product;
            if (_item == null) return;
            var writer = new ExcelWriter();
            var _rtn = writer.WriteRecipe(_item, dgRecipe.ItemsSource as List<RecipeView>);
            if (_rtn == true)
            {
                MessageBox.Show("导出成功!", null, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("导出失败!", null, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
