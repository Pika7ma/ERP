using System;
using System.Linq;
using System.Windows;
using Lplfw.DAL;

namespace Lplfw.UI.Bom
{
    /// <summary>
    /// DeleteClass.xaml 的交互逻辑
    /// "删除产品类别"\"删除原料类别"公用
    /// </summary>
    public partial class DeleteClass : Window
    {
        private int? id;
        private bool isProduct;

        public DeleteClass(int? index, bool isProduct)
        {
            InitializeComponent();
            id = index;
            this.isProduct = isProduct;
            SetControls();
        }

        /// <summary>
        /// 按条件删除相应的类别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOK(object sender, RoutedEventArgs e)
        {
            var _reserve = (bool)rdReserve.IsChecked;
            var _deleteSub = _reserve ? (bool)ckbDeleteSubClass.IsChecked : true;
            var _mark = (bool)ckbMark.IsChecked;
            // _reserve & ?_deleteSub & ?_mark
            // !_reserve & _deleteSub
            try
            {
                using (var _db = new ModelContainer())
                {
                    _db.Database.ExecuteSqlCommand("SET FOREIGN_KEY_CHECKS=0;");
                    #region Product
                    if (isProduct)
                    {
                        if (!_deleteSub)
                        {
                            var _newParent = (int)cbTransferTo.SelectedValue;
                            var _subclasses = _db.ProductClassSet.Where(i => i.ParentId == id).ToList();
                            for (int _i = 0; _i < _subclasses.Count; _i++)
                            {
                                _subclasses[_i].ParentId = _newParent;
                            }

                            if (_mark)
                            {
                                var _subclassIndexes = ProductClass.GetSubClassIndexs((int)id);
                                for (int _i = 0; _i < _subclassIndexes.Count; _i++)
                                {
                                    var _id = _subclassIndexes[_i];
                                    var _products = _db.ProductSet.Where(i => i.ClassId == _id).ToList();
                                    for (int _j = 0; _j < _products.Count; _j++)
                                    {
                                        _products[_j].Status = "停产";
                                    }
                                }
                            }
                        }
                        else
                        {
                            var _subclassIndexes = ProductClass.GetSubClassIndexs((int)id);
                            for (int _i = 0; _i < _subclassIndexes.Count; _i++)
                            {
                                var _id = _subclassIndexes[_i];
                                var _products = _db.ProductSet.Where(i => i.ClassId == _id).ToList();
                                if (_reserve)
                                {
                                    for (int _j = 0; _j < _products.Count; _j++)
                                    {
                                        if (_mark) _products[_j].Status = "停产";
                                        _products[_j].ClassId = (int)cbTransferTo.SelectedValue;
                                    }
                                }
                                else
                                {
                                    _db.ProductSet.RemoveRange(_products);
                                }

                            }
                        }
                        var _toDelete = _db.ProductClassSet.FirstOrDefault(i => i.Id == id);
                        _db.ProductClassSet.Remove(_toDelete);
                    }
                    #endregion
                    #region Material
                    else
                    {
                        if (!_deleteSub)
                        {
                            var _newParent = (int)cbTransferTo.SelectedValue;
                            var _subclasses = _db.MaterialClassSet.Where(i => i.ParentId == id).ToList();
                            for (int _i = 0; _i < _subclasses.Count; _i++)
                            {
                                _subclasses[_i].ParentId = _newParent;
                            }

                            if (_mark)
                            {
                                var _subclassIndexes = MaterialClass.GetSubClassIndexs((int)id);
                                for (int _i = 0; _i < _subclassIndexes.Count; _i++)
                                {
                                    var _id = _subclassIndexes[_i];
                                    var _materials = _db.MaterialSet.Where(i => i.ClassId == _id).ToList();
                                    for (int _j = 0; _j < _materials.Count; _j++)
                                    {
                                        _materials[_j].Status = "停用";
                                    }
                                }
                            }

                        }
                        else
                        {
                            var _subclassIndexes = MaterialClass.GetSubClassIndexs((int)id);
                            for (int _i = 0; _i < _subclassIndexes.Count; _i++)
                            {
                                var _id = _subclassIndexes[_i];
                                var _materials = _db.MaterialSet.Where(i => i.ClassId == _id).ToList();
                                if (_reserve)
                                {
                                    for (int _j = 0; _j < _materials.Count; _j++)
                                    {
                                        if (_mark) _materials[_j].Status = "停用";
                                        _materials[_j].ClassId = (int)cbTransferTo.SelectedValue;
                                    }
                                }
                                else
                                {
                                    _db.MaterialSet.RemoveRange(_materials);
                                }

                            }
                        }
                        var _toDelete = _db.MaterialClassSet.FirstOrDefault(i => i.Id == id);
                        _db.MaterialClassSet.Remove(_toDelete);
                    }
                    #endregion
                    _db.Database.ExecuteSqlCommand("SET FOREIGN_KEY_CHECKS=1;");
                    _db.SaveChanges();
                }
                DialogResult = true;
            }
            catch (Exception)
            {
                MessageBox.Show("由于当前类别的内容关联过多, 无法删除", null, MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void BtnCancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 根据删除的是产品或原料, 设置相应的文字
        /// </summary>
        private void SetControls() {
            var _content = isProduct ? "产品" : "原料";
            var _status = isProduct ? "停产" : "停用";
            Title = $"删除{_content}类别";
            gbReserve.Header = $"保留{_content}";
            gbDelete.Header = $"删除{_content}";
            rdReserve.Content = $"将{_content}(/子种类)转移到";
            ckbMark.Content = $"将{_content}标记为{_status}";
            rdDelete.Content = $"删除所有子分类, 删除{_content}";

            if (isProduct)
            {
                cbTransferTo.ItemsSource = ProductClass.GetClassesExceptSub((int)id);
            }
            else
            {
                cbTransferTo.ItemsSource = MaterialClass.GetClassesExceptSub((int)id);
            }
        }
    }
}
