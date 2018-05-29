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
                    Password = txtPassword.Text,
                    Tel = txtTel.Text,
                    UserGroupId = (int)cbUsergroup.SelectedValue
                };
                using (var _db = new ModelContainer())
                {
                    _db.UserSet.Add(_user);
                    _db.SaveChanges();
                }
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("wrong");
            }
        }

        /// <summary>
        /// 确认按钮读取数据
        /// </summary>
        /// <param name="ob"></param>
        private void OK(object ob)
        {
            var _username = (String)((dynamic)ob).name;
            var _password = (String)((dynamic)ob).ps;
            var _tel = (String)((dynamic)ob).phone;
            var _groupid = (int)((dynamic)ob).groupid;
            var _user = new DAL.User
            {
                Name = _username,
                Password = _password,
                Tel = _tel,
                UserGroupId = _groupid
            };
            using (var _db = new ModelContainer())
            {
                _db.UserSet.Add(_user);
                _db.SaveChanges();
            }
        }
    }
}