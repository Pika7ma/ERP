using Lplfw.DAL;
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
        private ProductClassViewModel productClass;
        private MaterialClassViewModel materialClass;

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
                    productClass = new ProductClassViewModel {
                        CbParentClass = index
                    };
                }
                else
                {
                    Title = "修改产品类别";
                    cbClass.ItemsSource = DAL.ProductClass.GetClassesExceptSub((int)index);
                    productClass = new ProductClassViewModel(index);
                }
                cbClass.Binding(productClass, "CbParentClass");
                txtName.Binding(productClass, "TxtName");
            }
            else
            {
                if (isNew)
                {
                    Title = "新建原料类别";
                    cbClass.ItemsSource = DAL.MaterialClass.GetAllClasses(true);
                    materialClass = new MaterialClassViewModel
                    {
                        CbParentClass = index
                    };
                }
                else
                {
                    Title = "修改原料类别";
                    cbClass.ItemsSource = DAL.MaterialClass.GetClassesExceptSub((int)index);
                    materialClass = new MaterialClassViewModel(index);
                }
                cbClass.Binding(materialClass, "CbParentClass");
                txtName.Binding(materialClass, "TxtName");
            }
        }


        /// <summary>
        /// 按下"确定"按钮, 创建新的产品\原料类别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitNewProductClass(object sender, RoutedEventArgs e)
        {

            if (isProduct)
            {
                if (isNew) productClass.CreateNew();
                else productClass.SaveChanges();
            }
            else
            {
                if (isNew) materialClass.CreateNew();
                else materialClass.SaveChanges();
            }
            DialogResult = true;
        }
    }
}
