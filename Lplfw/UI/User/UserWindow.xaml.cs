using Lplfw.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Lplfw.UI.User
{
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
        }

        /// <summary>
        /// 刷新权限显示
        /// </summary>
        private void RefreshPrivilege()
        {
            using (var _db = new DAL.ModelContainer())
            {
                if (_db.PrivilegeSet.Count() == 0)
                {
                    DAL.Privilege.Init();
                }
                var _ones = _db.PrivilegeSet.Where(i => i.Id > 0 && i.Id <= 3).Select(i => i).ToList();
                var _twos = _db.PrivilegeSet.Where(i => i.Id > 3 && i.Id <= 6).Select(i => i).ToList();
                var _threes = _db.PrivilegeSet.Where(i => i.Id > 6 && i.Id <= 9).Select(i => i).ToList();
                var _fours = _db.PrivilegeSet.Where(i => i.Id > 9 && i.Id <= 12).Select(i => i).ToList();
                var _fives = _db.PrivilegeSet.Where(i => i.Id > 12 && i.Id <= 15).Select(i => i).ToList();
                var _sixs = _db.PrivilegeSet.Where(i => i.Id > 15 && i.Id <= 18).Select(i => i).ToList();
                var _sevens = _db.PrivilegeSet.Where(i => i.Id > 18 && i.Id <= 21).Select(i => i).ToList();
                var _eights = _db.PrivilegeSet.Where(i => i.Id > 21 && i.Id <= 24).Select(i => i).ToList();
                var _nines = _db.PrivilegeSet.Where(i => i.Id > 24 && i.Id <= 27).Select(i => i).ToList();
                var _tens = _db.PrivilegeSet.Where(i => i.Id > 27 && i.Id <= 30).Select(i => i).ToList();
                var _elevens = _db.PrivilegeSet.Where(i => i.Id > 30 && i.Id <= 33).Select(i => i).ToList();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    one.ItemsSource = _one.ItemsSource = _ones;
                    two.ItemsSource = _two.ItemsSource = _twos;
                    three.ItemsSource = _three.ItemsSource = _threes;
                    four.ItemsSource = _four.ItemsSource = _fours;
                    five.ItemsSource = _five.ItemsSource = _fives;
                    six.ItemsSource = _six.ItemsSource = _sixs;
                    seven.ItemsSource = _seven.ItemsSource = _sevens;
                    eight.ItemsSource = _eight.ItemsSource = _eights;
                    nine.ItemsSource = _nine.ItemsSource = _nines;
                    ten.ItemsSource = _ten.ItemsSource = _tens;
                    eleven.ItemsSource = _eleven.ItemsSource = _elevens;
                    one.SelectedIndex = _one.SelectedIndex = 0;
                    two.SelectedIndex = _two.SelectedIndex = 0;
                    three.SelectedIndex = _three.SelectedIndex = 0;
                    four.SelectedIndex = _four.SelectedIndex = 0;
                    five.SelectedIndex = _five.SelectedIndex = 0;
                    six.SelectedIndex = _six.SelectedIndex = 0;
                    seven.SelectedIndex = _seven.SelectedIndex = 0;
                    eight.SelectedIndex = _eight.SelectedIndex = 0;
                    nine.SelectedIndex = _nine.SelectedIndex = 0;
                    ten.SelectedIndex = _ten.SelectedIndex = 0;
                    eleven.SelectedIndex = _eleven.SelectedIndex = 0;
                });
            }
        }

        /// <summary>
        /// 刷新权限显示，交换过后的
        /// </summary>
        private void _RefreshPrivilege()
        {
            one.SelectedIndex = _one.SelectedIndex = 0;
            two.SelectedIndex = _two.SelectedIndex = 0;
            three.SelectedIndex = _three.SelectedIndex = 0;
            four.SelectedIndex = _four.SelectedIndex = 0;
            five.SelectedIndex = _five.SelectedIndex = 0;
            six.SelectedIndex = _six.SelectedIndex = 0;
            seven.SelectedIndex = _seven.SelectedIndex = 0;
            eight.SelectedIndex = _eight.SelectedIndex = 0;
            nine.SelectedIndex = _nine.SelectedIndex = 0;
            ten.SelectedIndex = _ten.SelectedIndex = 0;
            eleven.SelectedIndex = _eleven.SelectedIndex = 0;

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
            if (dgUsers.SelectedItem == null)
            {
                MessageBox.Show("需要选中用户");
            }
            else
            {
                int _id = (int)((DAL.User)dgUsers.SelectedItem).Id;
                var _thread = new Thread(new ParameterizedThreadStart(DelUserClick));
                _thread.Start(_id);
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

        /// <summary>
        /// 用户组管理中的“删除用户组”读取数据
        /// </summary>
        /// <param name="id"></param>
        private void DelUserGroupClick(Object id)
        {
            var _id = id as int?;
            using (var _db = new DAL.ModelContainer())
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

        /// <summary>
        /// 用户组管理中用于显示选中用户组权限及修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TvGroupsMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int _id = (int)((TreeViewItem)tvGroups.SelectedItem).DataContext;
            var _thread = new Thread(new ParameterizedThreadStart(GroupsMouseDoubleClick));
            _thread.Start(_id);
        }
        /// <summary>
        /// 用户组管理中用于显示选中用户组权限及修改的读取数据
        /// </summary>
        /// <param name="id"></param>
        private void GroupsMouseDoubleClick(object id)
        {
            var _id = id as int?;
            using (var _db = new DAL.ModelContainer())
            {
                var _onei = (_db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId > 0 && i.PrivilegeId <= 3 && i.UserGroupId == _id).PrivilegeId - 1) % 3;
                var _twoi = (_db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId > 3 && i.PrivilegeId <= 6 && i.UserGroupId == _id).PrivilegeId - 1) % 3;
                var _threei = (_db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId > 6 && i.PrivilegeId <= 9 && i.UserGroupId == _id).PrivilegeId - 1) % 3;
                var _fouri = (_db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId > 9 && i.PrivilegeId <= 12 && i.UserGroupId == _id).PrivilegeId - 1) % 3;
                var _fivei = (_db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId > 12 && i.PrivilegeId <= 15 && i.UserGroupId == _id).PrivilegeId - 1) % 3;
                var _sixi = (_db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId > 15 && i.PrivilegeId <= 18 && i.UserGroupId == _id).PrivilegeId - 1) % 3;
                var _seveni = (_db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId > 18 && i.PrivilegeId <= 21 && i.UserGroupId == _id).PrivilegeId - 1) % 3;
                var _eighti = (_db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId > 21 && i.PrivilegeId <= 24 && i.UserGroupId == _id).PrivilegeId - 1) % 3;
                var _ninei = (_db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId > 24 && i.PrivilegeId <= 27 && i.UserGroupId == _id).PrivilegeId - 1) % 3;
                var _teni = (_db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId > 27 && i.PrivilegeId <= 30 && i.UserGroupId == _id).PrivilegeId - 1) % 3;
                var _eleveni = (_db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId > 30 && i.PrivilegeId <= 33 && i.UserGroupId == _id).PrivilegeId - 1) % 3;
                var _name = _db.UserGroupSet.First(i => i.Id == _id).Name;
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    one.SelectedIndex = _onei;
                    two.SelectedIndex = _twoi;
                    three.SelectedIndex = _threei;
                    four.SelectedIndex = _fouri;
                    five.SelectedIndex = _fivei;
                    six.SelectedIndex = _sixi;
                    seven.SelectedIndex = _seveni;
                    eight.SelectedIndex = _eighti;
                    nine.SelectedIndex = _ninei;
                    ten.SelectedIndex = _teni;
                    eleven.SelectedIndex = _eleveni;
                    tbGroupid.Text = _id.ToString();
                    tbGroup.Text = _name;
                    btnChange.IsEnabled = true;
                });
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
            var _temp1 = new List<UserGroupPrivilegeItem>() {
                new UserGroupPrivilegeItem { PrivilegeId = (int)_one.SelectedValue, UserGroupId = _id },
                new UserGroupPrivilegeItem { PrivilegeId = (int)_two.SelectedValue, UserGroupId = _id },
                new UserGroupPrivilegeItem { PrivilegeId = (int)_three.SelectedValue, UserGroupId = _id },
                new UserGroupPrivilegeItem { PrivilegeId = (int)_four.SelectedValue, UserGroupId = _id },
                new UserGroupPrivilegeItem { PrivilegeId = (int)_five.SelectedValue, UserGroupId = _id },
                new UserGroupPrivilegeItem { PrivilegeId = (int)_six.SelectedValue, UserGroupId = _id },
                new UserGroupPrivilegeItem { PrivilegeId = (int)_seven.SelectedValue, UserGroupId = _id },
                new UserGroupPrivilegeItem { PrivilegeId = (int)_eight.SelectedValue, UserGroupId = _id },
                new UserGroupPrivilegeItem { PrivilegeId = (int)_nine.SelectedValue, UserGroupId = _id },
                new UserGroupPrivilegeItem { PrivilegeId = (int)_ten.SelectedValue, UserGroupId = _id },
                new UserGroupPrivilegeItem { PrivilegeId = (int)_eleven.SelectedValue, UserGroupId = _id },
            };

            var _thread = new Thread(new ParameterizedThreadStart(ChangeClick));
            _thread.Start(new { id = _id, temp1 = _temp1 });
            MessageBox.Show("ok");
            tbGroupid.Text = "";
            tbGroup.Text = "";
            btnChange.IsEnabled = false;
            _RefreshPrivilege();
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
            }
        }


    }
}
