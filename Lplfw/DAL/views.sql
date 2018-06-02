/*
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

*/