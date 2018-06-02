using Lplfw.DAL;
using System;
using System.Linq;
using System.Threading;
using System.Windows;


namespace Lplfw.UI.User
{
    /// <summary>
    /// NewUser.xaml 的交互逻辑
    /// </summary>
    public partial class NewUser : Window
    {
        /// <summary>
        /// 初始化窗口
        /// </summary>
        public NewUser()
        {
            InitializeComponent();
            var _thread = new Thread(new ThreadStart(GiveUserGroup));
            _thread.Start();
        }

        /// <summary>
        /// 读取用户组
        /// </summary>
        private void GiveUserGroup()
        {
            using (var _db = new DAL.ModelContainer())
            {
                var _userGroups = _db.UserGroupSet.Select(i => i).ToList();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    cbUsergroup.ItemsSource = _userGroups;
                    cbUsergroup.SelectedIndex = 0;
                });
            }
        }

        /// <summary>
        /// 确认按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOK(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text != "" && txtPassword.Text != "" && txtTel.Text != "" && cbUsergroup.SelectedValue != null)
            {
                var _user = new DAL.User
                {
                    Name = txtUsername.Text,
                    Password = DAL.User.Encryption(txtPassword.Text),
                    Tel = txtTel.Text,
                    UserGroupId = (int)cbUsergroup.SelectedValue
                };
                using (var _db = new ModelContainer())
                {
                    if (_db.UserSet.FirstOrDefault(i => i.Name == txtUsername.Text) == null)
                    {
                        _db.UserSet.Add(_user);
                        _db.SaveChanges();
                        DialogResult = true;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("已有此用户！");
                    }
                }

            }
            else
            {
                MessageBox.Show("wrong");
            }
        }

    }
}