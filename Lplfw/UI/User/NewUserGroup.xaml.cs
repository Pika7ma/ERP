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
        /// <summary>
        /// 初始化窗口
        /// </summary>
        public NewUserGroup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 确认按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK(object sender, RoutedEventArgs e)
        {
            if (txtGroup.Text != "")
            {
                var _usergroup = new DAL.UserGroup
                {
                    Name = txtGroup.Text
                };
                using (var _db = new DAL.ModelContainer())
                {
                    _db.UserGroupSet.Add(_usergroup);
                    _db.SaveChanges();
                    for (int i = 0; i < _db.PrivilegeSet.Count(); i++)
                    {
                        _db.UserGroupPrivilegeItemSet.Add(new DAL.UserGroupPrivilegeItem
                        {
                            PrivilegeId = i + 1,
                            UserGroupId = _usergroup.Id,
                            Mode = "可修改"
                        });
                    }
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


    }
}