using Lplfw.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Lplfw.UI.User
{

    public class DataItem
    {
        public int PrivilegeId { get; set; }
        public String Name { get; set; }
        public String Mode { get; set; }
    }

    public class DataItem2
    {
        public String Name { get; set; }
    }

    /// <summary>
    /// UserWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UserWindow : Window
    {
        /// <summary>
        /// 初始化窗口
        /// </summary>
        public UserWindow()
        {
            InitializeComponent();
            var _thread1 = new Thread(new ThreadStart(RefreshdgUsers));
            _thread1.Start();
            var _thread2 = new Thread(new ThreadStart(RefreshtvUserGroups));
            _thread2.Start();
            var _thread3 = new Thread(new ThreadStart(RefreshPrivilege));
            _thread3.Start();
            DataContext = this;
            TheModes = new List<DataItem2>
            {
                new DataItem2{Name="不可见"},
                new DataItem2{Name="只读"},
                new DataItem2{Name="可修改"}
            };
            Datas = new List<DataItem>();
            var _thread = new Thread(new ThreadStart(Refresh));
            _thread.Start();

        }


        #region 权限控制

        private void Refresh()
        {
            using (var _db = new ModelContainer())
            {
                var _temp1 = _db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId == 6 && i.UserGroupId == Utils.CurrentUser.UserGroupId);
                var _temp2 = _db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId == 7 && i.UserGroupId == Utils.CurrentUser.UserGroupId);

                if (_temp1.Mode == "只读")
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        OnlyRead1();
                    });
                }
                else if (_temp1.Mode == "不可见")
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        tiEditPassword.Visibility = Visibility.Hidden;
                        tiEditPassword.IsSelected = false;
                    });

                }

                if (_temp2.Mode == "只读")
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        OnlyRead2();
                    });
                }
                else if (_temp2.Mode == "不可见")
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        tiUserGroupAdmin.Visibility = Visibility.Hidden;
                        tiUserAdmin.Visibility = Visibility.Hidden;
                    });

                }
            }
        }
        /// <summary>
        /// 只读1
        /// </summary>
        private void OnlyRead1()
        {
            btnCancel.IsEnabled = false;
            btnSure.IsEnabled = false;
        }
        /// <summary>
        /// 只读2
        /// </summary>
        private void OnlyRead2()
        {
            btnNewGroup.Visibility = Visibility.Hidden;
            btnDelGroup.Visibility = Visibility.Hidden;
            btnNewUser.Visibility = Visibility.Hidden;
            btnDelUser.Visibility = Visibility.Hidden;
            tvGroups.IsEnabled = false;
            btnEditPassword.IsEnabled = false;
            btnCancel2.IsEnabled = false;
            btnSure2.IsEnabled = false;
        }


        #endregion

        public List<DataItem2> TheModes { get; set; }

        public List<DataItem> Datas { get; set; }

        private void RefreshPrivilege()
        {
            using (var _db = new ModelContainer())
            {
                if (_db.PrivilegeSet.Count() == 0)
                {
                    DAL.Privilege.Init();
                }
            }
        }
        /// <summary>
        /// 刷新用户显示
        /// </summary>
        private void RefreshdgUsers()
        {
            using (var _db = new DAL.ModelContainer())
            {
                var _users = _db.UserSet.Select(i => i).ToList();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    dgUsers.ItemsSource = _users;
                    dgUsers.SelectedIndex = 0;
                });
            }

        }

        /// <summary>
        /// 刷新用户组显示
        /// </summary>
        private void RefreshtvUserGroups()
        {
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                tvUserGroups.Items.Clear();
                tvGroups.Items.Clear();
            });
            using (var _db = new DAL.ModelContainer())
            {
                var _usergroup = _db.UserGroupSet.Select(i => i).ToList();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    foreach (var i in _usergroup)
                    {
                        tvUserGroups.Items.Add(i.GetTreeViewItem());
                        tvGroups.Items.Add(i.GetTreeViewItem());
                    }
                    btnChoose.ItemsSource = _usergroup;
                    btnChoose.SelectedIndex = 0;
                });
            }

        }

        /// <summary>
        /// 用户管理中的“显示全部”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShowAllClick(object sender, RoutedEventArgs e)
        {
            var _thread1 = new Thread(new ThreadStart(RefreshdgUsers));
            _thread1.Start();
            var _thread2 = new Thread(new ThreadStart(RefreshtvUserGroups));
            _thread2.Start();
        }

        /// <summary>
        /// 用户组管理中的“新建用户组”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNewUserAllClick(object sender, RoutedEventArgs e)
        {
            var _win = new NewUserGroup();
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                var _thread2 = new Thread(new ThreadStart(RefreshtvUserGroups));
                _thread2.Start();
            }
        }

        /// <summary>
        /// 用户管理中的“新建用户”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNewUserClick(object sender, RoutedEventArgs e)
        {
            var _win = new NewUser();
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                var _thread1 = new Thread(new ThreadStart(RefreshdgUsers));
                _thread1.Start();
            }
        }

        /// <summary>
        /// 按钮“删除用户”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelUserClick(object sender, RoutedEventArgs e)
        {
            //需要先交接工作,即其他部门都没有工作属于此用户
            try
            {
                if (dgUsers.SelectedItem == null)
                {
                    MessageBox.Show("需要选中用户");
                }
                else if ((int)((DAL.User)dgUsers.SelectedItem).Id == Utils.CurrentUser.Id)
                {
                    MessageBox.Show("不能删除自己");
                }
                else
                {
                    int _id = (int)((DAL.User)dgUsers.SelectedItem).Id;
                    var _thread = new Thread(new ParameterizedThreadStart(DelUserClick));
                    _thread.Start(_id);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("无法删除！");
            }
        }

        /// <summary>
        /// 用户管理中的“删除用户”读取数据
        /// </summary>
        private void DelUserClick(Object id)
        {
            var _id = id as int?;
            using (var _db = new DAL.ModelContainer())
            {
                var _user = _db.UserSet.First(i => i.Id == _id);
                _db.UserSet.Remove(_user);
                _db.SaveChanges();
                RefreshdgUsers();
                MessageBox.Show("已删除！");
            }

        }

        /// <summary>
        /// 用户组管理中的“删除用户组”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelUserGroupClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tvGroups.SelectedItem == null)
                {
                    MessageBox.Show("需要选中用户组");
                }
                else
                {
                    int _id = (int)((TreeViewItem)tvGroups.SelectedItem).DataContext;
                    var _rtn = MessageBox.Show("是否删除其下的用户", null, MessageBoxButton.OKCancel, MessageBoxImage.Warning);

                    if (_rtn == MessageBoxResult.OK)
                    {
                        var _thread = new Thread(new ParameterizedThreadStart(DelUserGroupClick));
                        _thread.Start(_id);
                    }

                }
            }
            catch (Exception)
            {
                MessageBox.Show("无法删除！");
            }
        }

        /// <summary>
        /// 用户组管理中的“删除用户组”读取数据
        /// </summary>
        /// <param name="id"></param>
        private void DelUserGroupClick(Object id)
        {
            var _id = id as int?;
            using (var _db = new DAL.ModelContainer())
            {
                if (_db.UserSet.FirstOrDefault(i => i.Id == Utils.CurrentUser.Id && i.UserGroupId == _id) != null)
                {
                    MessageBox.Show("无法删除自己所在用户组！");
                }
                else
                {
                    var _temp = _db.UserGroupSet.First(i => i.Id == _id);
                    var _temp1 = _db.UserGroupPrivilegeItemSet.Where(i => i.UserGroupId == _id).ToList();
                    var _temp2 = _db.UserSet.Where(i => i.UserGroupId == _id).ToList();
                    _db.UserSet.RemoveRange(_temp2);//可能需要先交接工作！
                    _db.UserGroupPrivilegeItemSet.RemoveRange(_temp1);
                    _db.UserGroupSet.Remove(_temp);
                    _db.SaveChanges();
                    RefreshtvUserGroups();
                    RefreshdgUsers();
                    MessageBox.Show("已删除");
                }
            }
        }

        /// <summary>
        /// 用户组管理中用于显示选中用户组权限及修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TvGroupsMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int _id = (int)((TreeViewItem)tvGroups.SelectedItem).DataContext;
            using (var _db = new DAL.ModelContainer())
            {
                var _privileges = _db.UserGroupPrivilegeItemSet.Where(i => i.UserGroupId == _id);
                var _temps = _db.PrivilegeSet;
                var _name = _db.UserGroupSet.First(i => i.Id == _id).Name;
                Datas.RemoveAll(i => true);
                foreach (var i in _privileges)
                {
                    Datas.Add(new DataItem { PrivilegeId = i.PrivilegeId, Name = _temps.First(u => u.Id == i.PrivilegeId).Description, Mode = i.Mode });
                }
                dgPrivilege.Items.Refresh();
                tbGroupid.Text = _id.ToString();
                tbGroup.Text = _name;
                btnChange.IsEnabled = true;
            }
        }

        /// <summary>
        /// 用户组管理中“确认修改”，用于修改权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnChangeClick(object sender, RoutedEventArgs e)
        {
            int _id = int.Parse(tbGroupid.Text);
            List<DataItem> _list = (List<DataItem>)dgPrivilege.ItemsSource;
            var _temp1 = new List<UserGroupPrivilegeItem>();

            foreach (var i in _list)
            {
                _temp1.Add(new UserGroupPrivilegeItem { PrivilegeId = i.PrivilegeId, UserGroupId = _id, Mode = i.Mode });
            }

            var _thread = new Thread(new ParameterizedThreadStart(ChangeClick));
            _thread.Start(new { id = _id, temp1 = _temp1 });
            MessageBox.Show("ok");
            tbGroupid.Text = "";
            tbGroup.Text = "";
            btnChange.IsEnabled = false;
            Datas.RemoveAll(i => true);
            dgPrivilege.Items.Refresh();
        }

        /// <summary>
        /// "确认修改"的数据读取
        /// </summary>
        /// <param name="ob"></param>
        private void ChangeClick(object ob)
        {
            var _id = (int)((dynamic)ob).id;
            var _temp1 = (List<DAL.UserGroupPrivilegeItem>)((dynamic)ob).temp1;
            using (var _db = new DAL.ModelContainer())
            {
                var _temp = _db.UserGroupPrivilegeItemSet.Where(i => i.UserGroupId == _id).ToList();
                _db.UserGroupPrivilegeItemSet.RemoveRange(_temp);
                _db.UserGroupPrivilegeItemSet.AddRange(_temp1);
                _db.SaveChanges();
            }
        }

        /// <summary>
        /// 用户管理中显示选中用户组下的用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TvUserGroupsMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int _id = (int)((TreeViewItem)tvUserGroups.SelectedItem).DataContext;
            var _thread = new Thread(new ParameterizedThreadStart(UserGroupsMouseDoubleClick));
            _thread.Start(_id);
        }

        /// <summary>
        /// 用户管理中显示选中用户组下的用户的读取数据
        /// </summary>
        /// <param name="id"></param>
        private void UserGroupsMouseDoubleClick(object id)
        {
            int _id = (int)id;
            using (var _db = new DAL.ModelContainer())
            {
                var _users = _db.UserSet.Where(i => i.UserGroupId == _id).Select(i => i).ToList();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    dgUsers.ItemsSource = _users;
                    dgUsers.SelectedIndex = 0;
                });

            }
        }

        /// <summary>
        /// 修改用户小窗口中的“取消”按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel2Click(object sender, RoutedEventArgs e)
        {
            btnName.Text = "";
            btnPhone.Text = "";
            btnChoose.SelectedIndex = 0;
        }

        /// <summary>
        /// 修改用户小窗口中的“重置密码”，将改为“111111”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditPasswordClick(object sender, RoutedEventArgs e)
        {
            if (dgUsers.SelectedItem == null)
            {
                MessageBox.Show("需要选中用户");
            }
            else
            {
                int _id = (int)((DAL.User)dgUsers.SelectedItem).Id;
                var _thread = new Thread(new ParameterizedThreadStart(EditPasswordClick));
                _thread.Start(_id);
            }
        }

        /// <summary>
        /// "重置密码"的读取数据
        /// </summary>
        /// <param name="id"></param>
        private void EditPasswordClick(object id)
        {
            var _id = (int)id;
            using (var _db = new DAL.ModelContainer())
            {
                var _user = _db.UserSet.First(i => i.Id == _id);
                _user.Password = DAL.User.Encryption("111111");
                _db.SaveChanges();
                MessageBox.Show("密码已被重置为 111111 ！");
                if (Utils.CurrentUser.Id == _id)
                {
                    Utils.CurrentUser = _user;
                }
            }

        }

        /// <summary>
        /// 修改用户信息的“确定”按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSure2Click(object sender, RoutedEventArgs e)
        {
            if (btnName.Text != "" && btnPhone.Text != "" && btnChoose.SelectedItem != null)
            {
                if (dgUsers.SelectedItem == null)
                {
                    MessageBox.Show("需要选中用户");
                }
                else
                {
                    int _id = (int)((DAL.User)dgUsers.SelectedItem).Id;
                    var _thread = new Thread(new ParameterizedThreadStart(Sure2Click));
                    _thread.Start(new { id = _id, name = btnName.Text, tel = btnPhone.Text, groupid = (int)btnChoose.SelectedValue });
                    btnName.Text = "";
                    btnPhone.Text = "";
                    btnChoose.SelectedIndex = 0;
                }
            }
            else
            {
                MessageBox.Show("格式错误！");
                btnName.Text = "";
                btnPhone.Text = "";
                btnChoose.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// 修改用户信息的“确定”按钮的数据读取
        /// </summary>
        /// <param name="ob"></param>
        private void Sure2Click(object ob)
        {
            var _id = (int)((dynamic)ob).id;
            var _name = (String)((dynamic)ob).name;
            var _tel = (String)((dynamic)ob).tel;
            var _groupid = (int)((dynamic)ob).groupid;
            using (var _db = new DAL.ModelContainer())
            {
                var _user = _db.UserSet.First(i => i.Id == _id);
                _user.Name = _name;
                _user.Tel = _tel;
                _user.UserGroupId = _groupid;
                _db.SaveChanges();
                RefreshdgUsers();
                MessageBox.Show("修改成功！");
                if (Utils.CurrentUser.Id == _id)
                {
                    Utils.CurrentUser = _user;
                }
            }
        }

        /// <summary>
        /// 修改密码取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            OldPassword.Password = "";
            NewPassword.Password = "";
            RepetePassword.Password = "";
        }
        /// <summary>
        /// 修改密码确认按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSureClick(object sender, RoutedEventArgs e)
        {
            var _user = Utils.CurrentUser;
            String _thepass = DAL.User.Decrypt(_user.Password);
            if (_thepass != OldPassword.Password)
            {
                MessageBox.Show("原密码错误！");
                OldPassword.Password = "";
                NewPassword.Password = "";
                RepetePassword.Password = "";

            }
            else
            {
                if (NewPassword.Password != RepetePassword.Password)
                {
                    MessageBox.Show("确认密码前后不一致！");
                    OldPassword.Password = "";
                    NewPassword.Password = "";
                    RepetePassword.Password = "";

                }
                else
                {
                    var _thread = new Thread(new ParameterizedThreadStart(Sure_Click));
                    _thread.Start(NewPassword.Password);
                    OldPassword.Password = "";
                    NewPassword.Password = "";
                    RepetePassword.Password = "";

                }
            }
        }
        /// <summary>
        /// 修改密码确认按钮读取数据
        /// </summary>
        /// <param name="pass"></param>
        private void Sure_Click(Object pass)
        {
            String _pass = (String)pass;
            using (var _db = new ModelContainer())
            {
                var _user = _db.UserSet.First(i => i.Id == Utils.CurrentUser.Id);
                _user.Password = DAL.User.Encryption(_pass);
                _db.SaveChanges();
                Utils.CurrentUser.Password = _user.Password;
                MessageBox.Show("OK");
            }
        }

    }
}
