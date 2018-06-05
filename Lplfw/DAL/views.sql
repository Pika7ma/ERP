use erp;

-- 产品配方的材料名
drop view if exists recipeview;
create view recipeview as
select
	recipeitemset.ProductId,
	recipeitemset.MaterialId,
    materialset.Name,
    recipeitemset.Quantity,
    materialset.Unit
from recipeitemset join materialset
	on recipeitemset.MaterialId = materialset.Id;

-- 采购单的用户名
drop view if exists purchaseview;
create view purchaseview as
select purchaseset.Id,
	   purchaseset.Status,
	   purchaseset.Priority,
	   purchaseset.CreateAt,
	   purchaseset.FinishedAt,
	   purchaseset.Description,
	   userset.Name,
	   userset.Id as UserId
from purchaseset join userset
on purchaseset.UserId = userset.Id;

-- 采购单条目的原料名和供货商名
drop view if exists purchaseitemview;
create view purchaseitemview as
select purchaseitemset.PurchaseId,
	   purchaseitemset.MaterialId,
	   purchaseitemset.Quantity,
	   purchaseitemset.SupplierId,
	   purchaseitemset.Price,
	   materialset.Name as MaterialName,
	   supplierset.Name as SupplierName
from (purchaseitemset join materialset 
on purchaseitemset.MaterialId = materialset.Id) join supplierset
on purchaseitemset.SupplierId = supplierset.Id;

-- 仓库信息显示管理员名字
drop view if exists storageview;
create view storageview as
select storageset.*,
	   userset.Name as UserName
from storageset join userset
on storageset.UserId = userset.Id;

-- 各仓库的材料库存和成本
drop view if exists materialstockview;
create view materialstockview as
select materialstockset.MaterialId,
	   materialstockset.StorageId,
       materialstockset.Quantity,
       materialstockset.Location,
       materialset.Name as MaterialName,
       storageset.Name as StorageName,
       materialset.Price,
       Price * Quantity as Cost
from (materialstockset join materialset 
on materialstockset.MaterialId = materialset.Id) join storageset
on materialstockset.StorageId = storageset.Id;

-- 全部仓库的原料库存和成本
drop view if exists materialstockallview;
create view materialstockallview as
select materialstockview.MaterialId,
       sum(materialstockview.Quantity) as Quantity,
       materialstockview.MaterialName,
       materialstockview.Price,
       Price * Quantity as Cost
from materialstockview
group by MaterialId;

-- 各仓库的产品库存和成本
drop view if exists productstockview;
create view productstockview as
select productstockset.ProductId,
	   productstockset.StorageId,
       productstockset.Quantity,
       productstockset.Location,
       productset.Name as ProductName,
       storageset.Name as StorageName,
       productset.Price,
       Price * Quantity as Cost
from (productstockset join productset 
on productstockset.ProductId = productset.Id) join storageset
on productstockset.StorageId = storageset.Id;

-- 全部仓库的产品库存和成本
drop view if exists productstockallview;
create view productstockallview as
select productstockview.ProductId,
       sum(productstockview.Quantity) as Quantity,
       productstockview.ProductName,
       productstockview.Price,
       Price * Quantity as Cost
from productstockview
group by ProductId;

-- 入库单
drop view if exists stockinview;
create view stockinview as
select stockinset.*,
	   storageset.Name as StorageName,
	   userset.Name as UserName
from (stockinset join storageset
on stockinset.StorageId = storageset.Id) join userset
on stockinset.UserId = userset.Id;

-- 出库单
drop view if exists stockoutview;
create view stockoutview as
select stockoutset.*,
	   storageset.Name as StorageName,
	   userset.Name as UserName
from (stockoutset join storageset
on stockoutset.StorageId = storageset.Id) join userset
on stockoutset.UserId = userset.Id;

-- 出入库条目
drop view if exists materialstockinitemview;
create view materialstockinitemview as
select materialstockinitemset.*,
	   materialset.Name as MaterialName
from materialstockinitemset join materialset
on materialstockinitemset.MaterialId = materialset.Id;

drop view if exists materialstockoutitemview;
create view materialstockoutitemview as
select materialstockoutitemset.*,
	   materialset.Name as MaterialName
from materialstockoutitemset join materialset
on materialstockoutitemset.MaterialId = materialset.Id;

drop view if exists productstockinitemview;
create view productstockinitemview as
select productstockinitemset.*,
	   productset.Name as ProductName
from productstockinitemset join productset
on productstockinitemset.ProductId = productset.Id;

drop view if exists productstockoutitemview;
create view productstockoutitemview as
select productstockoutitemset.*,
	   productset.Name as ProductName
from productstockoutitemset join productset
on productstockoutitemset.ProductId = productset.Id;