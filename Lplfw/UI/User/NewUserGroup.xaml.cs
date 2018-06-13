using Lplfw.DAL;
using System;
using System.Linq;
using System.Windows;

namespace Lplfw.UI.User
{
    /// <summary>
    /// NewUserGroup.xaml 的交互逻辑
    /// </summary>
    public partial class NewUserGroup : Window
    {
        private UserGroupViewModel group;

        public NewUserGroup()
        {
            InitializeComponent();
            group = new UserGroupViewModel();
            txtName.Binding(group, "TxtName");
        }

        /// <summary>
        /// 确认按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Confirm(object sender, RoutedEventArgs e)
        {
            if (group.CanSubmit)
            {
                var _rtn = group.CreateNew();
                DialogResult = _rtn;
            }
            else
            {
                txtMessage.Text = group.TxtCheckMessage;
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}