using System.Linq;
using System.Windows;
using Lplfw.DAL;

namespace Lplfw.UI.Bom
{
    /// <summary>
    /// NewMaterial.xaml 的交互逻辑
    /// 新建原料\修改原料
    /// </summary>
    public partial class NewMaterial : Window
    {
        private bool isNew;
        private Material material;
        private bool hasChanged = false;

        public NewMaterial(int? index, bool isNew)
        {
            InitializeComponent();
            this.isNew = isNew;
            cbClass.ItemsSource = MaterialClass.GetAllClasses(false);
            if (isNew)
            {
                Title = "新建原料";
                btnOK.Click += Confirm;
                cbClass.SelectedValue = index;
            }
            else
            {
                Title = "修改产品配方";
                btnOK.Click += SubmitEditMaterial;
                btnContinue.Visibility = Visibility.Hidden;
                using (var _db = new ModelContainer())
                {
                    material = _db.MaterialSet.FirstOrDefault(i => i.Id == index);
                    SetControlsValue();
                }
            }
        }

        /// <summary>
        /// "继续新建"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Continue(object sender, RoutedEventArgs e)
        {
            if (SubmitNewMaterial())
            {
                hasChanged = true;
                SetControlsValue(setNull: true);
                message.Text = "创建成功!关闭窗口后可查看结果";
            }
        }

        /// <summary>
        /// "确定"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Confirm(object sender, RoutedEventArgs e)
        {
            if (SubmitNewMaterial()) DialogResult = true;
        }

        private bool SubmitNewMaterial() {
            if (CheckSumbit())
            {
                using (var _db = new ModelContainer())
                {
                    material = new Material
                    {
                        Name = txtName.Text,
                        Format = txtFormat.Text,
                        Unit = txtUnit.Text,
                        ClassId = (int)cbClass.SelectedValue,
                        Price = double.Parse(txtPrice.Text)
                    };
                    _db.MaterialSet.Add(material);
                    _db.SaveChanges();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 修改原料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitEditMaterial(object sender, RoutedEventArgs e)
        {
            if (CheckSumbit())
            {
                using (var _db = new ModelContainer())
                {
                    material = _db.MaterialSet.FirstOrDefault(i => i.Id == material.Id);
                    material.Name = txtName.Text;
                    material.Format = txtFormat.Text;
                    material.Unit = txtUnit.Text;
                    material.ClassId = (int)cbClass.SelectedValue;
                    material.Price = double.Parse(txtPrice.Text);
                    _db.SaveChanges();
                }
                DialogResult = true;
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
                message.Text = "请选择原料种类!";
            else if (txtName.Text == "")
                message.Text = "请输入原料名称!";
            else if (txtFormat.Text == "")
                message.Text = "请输入原料规格!";
            else if (!double.TryParse(txtPrice.Text, out _temp))
                message.Text = "请输入正确的平均价格!";
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
                txtFormat.Text = null;
                txtUnit.Text = null;
                txtPrice.Text = null;
            }
            else
            {
                cbClass.SelectedValue = material.ClassId;
                txtName.Text = material.Name;
                txtFormat.Text = material.Format;
                txtUnit.Text = material.Unit;
                txtPrice.Text = material.Price.ToString();
            }
        }

        /// <summary>
        /// 在关闭窗口时设置返回值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = hasChanged;
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
