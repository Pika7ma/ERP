using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Lplfw.DAL;

namespace Lplfw.UI.Bom
{
    /// <summary>
    /// NewProduct.xaml 的交互逻辑
    /// 新建产品\修改产品
    /// </summary>
    public partial class NewProduct : Window
    {
        private bool isNew;
        private Product product;
        private bool hasChanged = false;
        public List<RecipeItem> RecipeItems { get; set; }
        public List<Material> Materials { get; set; }

        public NewProduct(int? index, bool isNew)
        {
            InitializeComponent();
            this.isNew = isNew;
            DataContext = this; // 传递数据上下文
            Materials = GetAllMaterials();
            cbClass.ItemsSource = ProductClass.GetAllClasses(false);
            if (isNew)
            {
                Title = "新建产品配方";
                btnOK.Click += Confirm;
                cbClass.SelectedValue = index;
                RecipeItems = new List<RecipeItem>();
            }
            else
            {
                Title = "修改产品配方";
                btnOK.Click += SubmitEditProduct;
                btnContinue.Visibility = Visibility.Hidden;
                using (var _db = new ModelContainer())
                {
                    product = _db.ProductSet.FirstOrDefault(i => i.Id == index);
                    RecipeItems = product.RecipeItems.ToList();
                    SetControlsValue();
                }
            }
        }

        private List<Material> GetAllMaterials()
        {
            using (var _db = new ModelContainer()){
                return _db.MaterialSet.Select(i => i).ToList();
            }
        }

        /// <summary>
        /// 删除选中的配方的行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteSelectedItem(object sender, RoutedEventArgs e)
        {
            var _item = dgMaterialItems.SelectedItem as RecipeItem;
            if (_item == null) return;
            RecipeItems.Remove(_item);
            dgMaterialItems.Items.Refresh();
        }

        /// <summary>
        /// 关闭窗口, 不进行改动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// "确定"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Confirm(object sender, RoutedEventArgs e)
        {
            if (SubmitNewProduct()) {
                hasChanged = true;
                Close();
            }
        }

        /// <summary>
        /// "继续新建"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Continue(object sender, RoutedEventArgs e)
        {
            if (SubmitNewProduct())
            {
                hasChanged = true;
                SetControlsValue(setNull: true);
                message.Text = "创建成功!关闭窗口后可查看结果";
            }
        }

        /// <summary>
        /// 提交新产品
        /// </summary>
        private bool SubmitNewProduct()
        {
            if (CheckSumbit())
            {
                using (var _db = new ModelContainer())
                {
                    product = new Product
                    {
                        Name = txtName.Text,
                        Type = txtType.Text,
                        Format = txtFormat.Text,
                        Unit = txtUnit.Text,
                        ClassId = (int)cbClass.SelectedValue,
                        Price = double.Parse(txtPrice.Text)
                    };
                    _db.ProductSet.Add(product);
                    _db.SaveChanges();

                    SortOutRecipeItems();

                    for (int _i = 0; _i < RecipeItems.Count; _i++)
                    {
                        RecipeItems[_i].ProductId = product.Id;
                    }
                    _db.RecipeItemSet.AddRange(RecipeItems);
                    _db.SaveChanges();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 修改产品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitEditProduct(object sender, RoutedEventArgs e)
        {
            if (CheckSumbit())
            {
                using (var _db = new ModelContainer())
                {
                    product = _db.ProductSet.FirstOrDefault(i => i.Id == product.Id);
                    product.Name = txtName.Text;
                    product.Type = txtType.Text;
                    product.Format = txtFormat.Text;
                    product.Unit = txtUnit.Text;
                    product.ClassId = (int)cbClass.SelectedValue;
                    product.Price = double.Parse(txtPrice.Text);
                    var _toDelete = _db.RecipeItemSet.Where(i => i.ProductId == product.Id).ToList();
                    _db.RecipeItemSet.RemoveRange(_toDelete);

                    SortOutRecipeItems();

                    for (int _i = 0; _i < RecipeItems.Count; _i++)
                    {
                        RecipeItems[_i].ProductId = product.Id;
                    }
                    _db.RecipeItemSet.AddRange(RecipeItems);
                    _db.SaveChanges();
                }
                hasChanged = true;
                Close();
            }
        }

        /// <summary>
        /// 检查当前表单是否满足提交条件
        /// </summary>
        /// <returns></returns>
        private bool CheckSumbit()
        {
            double _temp;
            var _rtn = false;
            if (cbClass.SelectedValue == null)
                message.Text = "请选择产品种类!";
            else if (txtName.Text == "")
                message.Text = "请输入产品名称!";
            else if (txtType.Text == "")
                message.Text = "请输入产品型号!";
            else if (txtFormat.Text == "")
                message.Text = "请输入产品规格!";
            else if (txtPrice.Text == "")
                message.Text = "请输入产品价格!";
            else if (!double.TryParse(txtPrice.Text, out _temp))
                message.Text = "请输入正确的产品价格!";
            else
                _rtn = true;

            return _rtn;
        }

        /// <summary>
        /// 设置控件的值
        /// </summary>
        /// <param name="setNull"></param>
        private void SetControlsValue(bool setNull=false)
        {
            if (setNull)
            {
                txtName.Text = null;
                txtType.Text = null;
                txtFormat.Text = null;
                txtUnit.Text = null;
                txtPrice.Text = null;
            }
            else
            {
                cbClass.SelectedValue = product.ClassId;
                txtName.Text = product.Name;
                txtType.Text = product.Type;
                txtFormat.Text = product.Format;
                txtUnit.Text = product.Unit;
                txtPrice.Text = product.Price.ToString();
            }
        }

        /// <summary>
        /// 在关闭窗口时设置返回值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetDialogResult(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = hasChanged;
        }

        private void SortOutRecipeItems()
        {
            var _materials = new List<int>();
            var _recipeItems = new List<RecipeItem>();

            for (int _i = 0; _i < RecipeItems.Count; _i++)
            {
                _materials.Add(RecipeItems[_i].MaterialId);
            }

            _materials = _materials.Distinct().ToList();

            if (_materials.Count == RecipeItems.Count) return;

            for (int _i = 0; _i < RecipeItems.Count; _i++)
            {                
                var _old = RecipeItems[_i];
                var _new = _recipeItems.Find(i => i.MaterialId == _old.MaterialId);
                if (_new == null)
                {
                    _recipeItems.Add(new RecipeItem
                    {
                        MaterialId = _old.MaterialId,
                        Quantity = _old.Quantity
                    });
                }
                else
                {
                    _new.Quantity += _old.Quantity;
                }
            }

            RecipeItems = _recipeItems;
        }
    }
}
