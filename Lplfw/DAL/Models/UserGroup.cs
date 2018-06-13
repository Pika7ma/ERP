using System;
using System.Linq;
using System.Windows.Controls;

namespace Lplfw.DAL
{
    public partial class UserGroup
    {
        public TreeViewItem GetTreeViewItem()
        {
            var tvi = new TreeViewItem
            {
                Header = Name,
                DataContext = Id
            };

            return tvi;
        }

        static public void SetTree(ref TreeView treeView)
        {
            using (var _db = new ModelContainer())
            {
                var _results = _db.UserGroupSet.ToList();
                foreach (var _i in _results)
                {
                    treeView.Items.Add(_i.GetTreeViewItem());
                }
            }
        }
    }
}