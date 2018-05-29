using System;
using System.Windows.Controls;

namespace Lplfw.DAL
{
    public partial class UserGroup
    {
        /// <summary>
        /// 树状图结构
        /// </summary>
        /// <returns></returns>
        public TreeViewItem GetTreeViewItem()
        {
            var tvi = new TreeViewItem();
            tvi.Header = Name;
            tvi.DataContext = Id;
            tvi.IsSelected = true;
            return tvi;
        }

    }
}