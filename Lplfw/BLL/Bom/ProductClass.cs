﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Lplfw.DAL
{
    public partial class ProductClass
    {
        /// <summary>
        /// 设置TreeView控件显示所有分类
        /// </summary>
        /// <param name="treeView">TreeView控件</param>
        static public void SetTreeFromRoot(ref TreeView treeView)
        {
            using (var _db = new ModelContainer())
            {
                var _results = _db.ProductClassSet.Where(i => i.ParentId == null);
                foreach (var _i in _results)
                {
                    treeView.Items.Add(_i.GetTreeViewItem());
                }
            }
        }

        /// <summary>
        /// 获取当前ProductClass的TreeViewItem, 包含所有的子类
        /// </summary>
        /// <returns>当前ProductClass的TreeViewItem</returns>
        public TreeViewItem GetTreeViewItem()
        {
            var _tvi = new TreeViewItem
            {
                Header = Name,
                DataContext = Id,
                IsExpanded = true
            };

            foreach (var _i in Chidren)
            {
                _tvi.Items.Add(_i.GetTreeViewItem());
            }

            return _tvi;
        }

        /// <summary>
        /// 获取指定ProductClass及所有子类的Id列表
        /// </summary>
        /// <param name="index">指定ProductClass的Id</param>
        /// <returns>指定ProductClass及所有子类的Id列表</returns>
        static public List<int> GetSubClassIndexs(int index)
        {
            using (var _db = new ModelContainer())
            {
                var _dSubIndexs = _db.ProductClassSet.Where(i => i.ParentId == index).Select(i => i.Id).ToList();
                var _sumIndexs = new List<int>(_dSubIndexs);
                _sumIndexs.Add(index);

                foreach (var _subIndex in _dSubIndexs)
                {
                    var _tempList = GetSubClassIndexs(_subIndex);
                    _sumIndexs = _sumIndexs.Union(_tempList).ToList();
                }
                return _sumIndexs;
            }
        }

        /// <summary>
        /// 获取指定分类下所有产品
        /// </summary>
        /// <param name="index">指定分类的Id</param>
        /// <returns>当前分类下所有产品的列表</returns>
        static public List<Product> GetSubClassProducts(int? index)
        {
            if (index == null)
            {
                using (var _db = new ModelContainer())
                {
                    return _db.ProductSet.ToList();
                }
            }
            else
            {
                var classList = GetSubClassIndexs((int)index);
                using (var _db = new ModelContainer())
                {
                    var _sumList = new List<Product>();
                    foreach (int id in classList)
                    {
                        var _temp = _db.ProductSet.Where(i => i.ClassId == id).ToList();
                        _sumList = _sumList.Union(_temp).ToList();
                    }
                    return _sumList;
                }
            }
        }

        /// <summary>
        /// 获取所有产品类别的列表, 可以选择是否添加一个'- 根分类 -'的标签
        /// </summary>
        /// <param name="containRoot">是否包含根分类标签</param>
        /// <returns>所有产品类别的列表</returns>
        static public List<ProductClass> GetAllClasses(bool containRoot)
        {
            using (var _db = new ModelContainer())
            {
                if (containRoot)
                {
                    var _classes = new List<ProductClass>();
                    var _root = new ProductClass();
                    _root.Name = "- 根分类 -";
                    _root.ParentId = null;
                    _classes.Add(_root);
                    var _temp = _db.ProductClassSet.Select(i => i).ToList();
                    _classes = _classes.Concat(_temp).ToList();
                    return _classes;
                }
                else
                    return _db.ProductClassSet.Select(i => i).ToList();
            }
        }

        /// <summary>
        /// 获取除某分类及子类以外的所有分类
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        static public List<ProductClass> GetClassesExceptSub(int index)
        {
            var list = GetAllClasses(true);
            var sub = GetSubClassIndexs(index);
            var classes = new List<ProductClass>();
            foreach (var i in list)
            {
                if (sub.FindIndex(x => x == i.Id) == -1)
                {
                    classes.Add(i);
                }
            }
            return classes;
        }
    }
}
