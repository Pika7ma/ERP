using System.Linq;
using System.Windows;

namespace Lplfw.UI.Bom
{
    /// <summary>
    /// NewClass.xaml 的交互逻辑
    /// 新建产品类别\修改产品类别\新建原料类别\修改
    /// </summary>
    public partial class NewClass : Window
    {
        private bool isNew;
        private bool isProduct;
        private int? Id;

        public NewClass(int? index, bool isNew, bool isProduct)
        {
            InitializeComponent();
            this.isNew = isNew;
            this.isProduct = isProduct;
            if (isProduct)
            {
                if (isNew)
                {
                    Title = "新建产品类别";
                    cbClass.ItemsSource = DAL.ProductClass.GetAllClasses(true);
                    cbClass.SelectedValue = index;
                }
                else
                {
                    Title = "修改产品类别";
                    Id = index;
                    cbClass.ItemsSource = DAL.ProductClass.GetClassesExceptSub((int)index);
                    using (var _db = new DAL.ModelContainer())
                    {
                        var _class = _db.ProductClassSet.FirstOrDefault(i => i.Id == index);
                        cbClass.SelectedValue = _class.ParentId;
                        txtName.Text = _class.Name;
                    }
                }
            }
            else
            {
                if (isNew)
                {
                    Title = "新建原料类别";
                    cbClass.ItemsSource = DAL.MaterialClass.GetAllClasses(true);
                    cbClass.SelectedValue = index;
                }
                else
                {
                    Title = "修改原料类别";
                    Id = index;
                    cbClass.ItemsSource = DAL.MaterialClass.GetClassesExceptSub((int)index);
                    using (var _db = new DAL.ModelContainer())
                    {
                        var _class = _db.MaterialClassSet.FirstOrDefault(i => i.Id == index);
                        cbClass.SelectedValue = _class.ParentId;
                        txtName.Text = _class.Name;
                    }
                }
            }
        }


        /// <summary>
        /// 按下"确定"按钮, 创建新的产品\原料类别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitNewProductClass(object sender, RoutedEventArgs e)
        {
            using (var _db = new DAL.ModelContainer())
            {
                if (isProduct)
                {
                    if (isNew)
                    {
                        var _class = new DAL.ProductClass
                        {
                            Name = txtName.Text,
                            ParentId = (int?)cbClass.SelectedValue
                        };
                        _db.ProductClassSet.Add(_class);
                    }
                    else
                    {
                        var _class = _db.ProductClassSet.FirstOrDefault(i => i.Id == Id);
                        _class.Name = txtName.Text;
                        _class.ParentId = cbClass.SelectedValue as int?;
                    }
                } else
                {
                    if (isNew)
                    {
                        var _class = new DAL.MaterialClass
                        {
                            Name = txtName.Text,
                            ParentId = (int?)cbClass.SelectedValue
                        };
                        _db.MaterialClassSet.Add(_class);
                    }
                    else
                    {
                        var _class = _db.MaterialClassSet.FirstOrDefault(i => i.Id == Id);
                        _class.Name = txtName.Text;
                        _class.ParentId = cbClass.SelectedValue as int?;
                    }
                }
                _db.SaveChanges();
                DialogResult = true;
                Close();
            }
        }
    }
}
