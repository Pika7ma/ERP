/*
-- 产品配方的材料名
create view recipeview as
select
	recipeitemset.ProductId,
	recipeitemset.MaterialId,
    materialset.Name,
    recipeitemset.Quantity,
    materialset.Unit
from recipeitemset join materialset
	on recipeitemset.MaterialId = materialset.Id;
*/