use erp;



-- 产品种类
insert into productclassset values(0, "类别1", null);
insert into productclassset values(0, "类别2", null);
insert into productclassset values(0, "类别1-1", 1);
insert into productclassset values(0, "类别1-2", 1);

-- 原料种类
insert into materialclassset values(0, "材料类别1", null);
insert into materialclassset values(0, "材料类别2", null);
insert into materialclassset values(0, "材料类别1-1", 1);

-- 原料
insert into materialset values(0, "圆石", "3x3m", "个", 3, 10, "可用", 0);
insert into materialset values(0, "橡木", "3x3m", "个", 2, 10, "可用", 0);
insert into materialset values(0, "白桦木", "3x3m", "个", 2, 10, "可用", 0);

-- 产品
insert into productset values(0, "T-001型警用机器人", 3, "T-001", "2m", "个", 3000, "生产");
insert into productset values(0, "T-002型警用机器人", 3, "T-002", "2m", "个", 3000, "生产");
insert into productset values(0, "Z-003型民用机器人", 4, "Z-003", "2m", "个", 3000, "生产");
insert into productset values(0, "Z-004型民用机器人", 4, "Z-004", "2m", "个", 3000, "生产");

-- 产品配方
insert into recipeitemset values(1, 1, 10);
insert into recipeitemset values(1, 2, 5);
insert into recipeitemset values(2, 1, 5);
insert into recipeitemset values(2, 2, 10);

-- 用户
insert into usergroupset values(0, "管理员");
insert into privilegeset values(1, "BOM管理");
insert into privilegeset values(2, "仓库管理");
insert into privilegeset values(3, "订单管理");
insert into privilegeset values(4, "生产管理");
insert into privilegeset values(5, "采购管理");
insert into privilegeset values(6, "用户管理");
insert into usergroupprivilegeitemset values(1, 1, 2);
insert into usergroupprivilegeitemset values(2, 1, 2);
insert into usergroupprivilegeitemset values(3, 1, 2);
insert into usergroupprivilegeitemset values(4, 1, 2);
insert into usergroupprivilegeitemset values(5, 1, 2);
insert into usergroupprivilegeitemset values(6, 1, 2);
insert into userset values(0, "admin", "ixTfreDyxMU=", "-", 1);

-- 供货商
insert into supplierset values(0, "南开大学", "张三", "天津市某区某大街", "400-000-0000");
