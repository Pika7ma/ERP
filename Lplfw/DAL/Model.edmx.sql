










































-- -----------------------------------------------------------
-- Entity Designer DDL Script for MySQL Server 4.1 and higher
-- -----------------------------------------------------------
-- Date Created: 05/22/2018 12:06:46

-- Generated from EDMX file: E:\code\csharp\Lplfw\Lplfw\DAL\Model.edmx
-- Target version: 3.0.0.0

-- --------------------------------------------------



-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- NOTE: if the constraint does not exist, an ignorable error will be reported.
-- --------------------------------------------------


--    ALTER TABLE `ProductClassSet` DROP CONSTRAINT `FK_ProductClassNesting`;

--    ALTER TABLE `ProductSet` DROP CONSTRAINT `FK_ClassOfProduct`;

--    ALTER TABLE `MaterialClassSet` DROP CONSTRAINT `FK_MaterialClassNesting`;

--    ALTER TABLE `MaterialSet` DROP CONSTRAINT `FK_ClassOfMaterial`;

--    ALTER TABLE `RecipeItemSet` DROP CONSTRAINT `FK_RecipeOfProduct`;

--    ALTER TABLE `RecipeItemSet` DROP CONSTRAINT `FK_RecipeItemToMaterial`;

--    ALTER TABLE `UserGroupPrivilegeItemSet` DROP CONSTRAINT `FK_UserGroupPrivilegeItemDescription`;

--    ALTER TABLE `UserGroupPrivilegeItemSet` DROP CONSTRAINT `FK_UserGroupPrivilegeItemUserGroup`;

--    ALTER TABLE `UserSet` DROP CONSTRAINT `FK_UserUserGroup`;

--    ALTER TABLE `StorageSet` DROP CONSTRAINT `FK_PrincipalOfStorage`;

--    ALTER TABLE `ProductStockSet` DROP CONSTRAINT `FK_StockInStorage`;

--    ALTER TABLE `ProductStockSet` DROP CONSTRAINT `FK_ProductInStock`;

--    ALTER TABLE `MaterialStockSet` DROP CONSTRAINT `FK_MaterialInStock`;

--    ALTER TABLE `MaterialStockSet` DROP CONSTRAINT `FK_MaterialOfStorage`;

--    ALTER TABLE `StockInSet` DROP CONSTRAINT `FK_StockInStorage1`;

--    ALTER TABLE `StockInSet` DROP CONSTRAINT `FK_StockInUser`;

--    ALTER TABLE `StockOutSet` DROP CONSTRAINT `FK_StockOutStorage`;

--    ALTER TABLE `StockOutSet` DROP CONSTRAINT `FK_StockOutUser`;

--    ALTER TABLE `ProductStockInItemSet` DROP CONSTRAINT `FK_ProductStockInItemStockIn`;

--    ALTER TABLE `ProductStockInItemSet` DROP CONSTRAINT `FK_ProductStockInItemProduct`;

--    ALTER TABLE `MaterialStockInItemSet` DROP CONSTRAINT `FK_MaterialStockInItemStockIn`;

--    ALTER TABLE `MaterialStockInItemSet` DROP CONSTRAINT `FK_MaterialStockInItemMaterial`;

--    ALTER TABLE `ProductStockOutItemSet` DROP CONSTRAINT `FK_ProductStockOutItemProduct`;

--    ALTER TABLE `ProductStockOutItemSet` DROP CONSTRAINT `FK_ProductStockOutItemStockOut`;

--    ALTER TABLE `MaterialStockOutItemSet` DROP CONSTRAINT `FK_MaterialStockOutItemMaterial`;

--    ALTER TABLE `MaterialStockOutItemSet` DROP CONSTRAINT `FK_MaterialStockOutItemStockOut`;

--    ALTER TABLE `SalesSet` DROP CONSTRAINT `FK_SalesCustomer`;

--    ALTER TABLE `SalesSet` DROP CONSTRAINT `FK_SalesUser`;

--    ALTER TABLE `SalesItemSet` DROP CONSTRAINT `FK_SalesItemSales`;

--    ALTER TABLE `SalesItemSet` DROP CONSTRAINT `FK_SalesItemProduct`;

--    ALTER TABLE `PurchaseSet` DROP CONSTRAINT `FK_PurchaseUser`;

--    ALTER TABLE `PurchaseItemSet` DROP CONSTRAINT `FK_PurchaseItemPurchase`;

--    ALTER TABLE `MaterialPriceSet` DROP CONSTRAINT `FK_MaterialPriceMaterial`;

--    ALTER TABLE `MaterialPriceSet` DROP CONSTRAINT `FK_MaterialPriceSupplier`;

--    ALTER TABLE `PurchaseItemSet` DROP CONSTRAINT `FK_PurchaseItemMaterialPrice`;

--    ALTER TABLE `AssemblyLineSet` DROP CONSTRAINT `FK_AssemblyLineUser`;

--    ALTER TABLE `RequisitionSet` DROP CONSTRAINT `FK_RequisitionAssemblyLine`;

--    ALTER TABLE `RequisitionSet` DROP CONSTRAINT `FK_RequisitionUser`;

--    ALTER TABLE `RequisitionItemSet` DROP CONSTRAINT `FK_RequisitionItemRequisition`;

--    ALTER TABLE `RequisitionItemSet` DROP CONSTRAINT `FK_RequisitionItemMaterialStock`;

--    ALTER TABLE `ReturnSet` DROP CONSTRAINT `FK_ReturnRequisition`;

--    ALTER TABLE `ReturnSet` DROP CONSTRAINT `FK_ReturnUser`;

--    ALTER TABLE `ReturnItemSet` DROP CONSTRAINT `FK_ReturnItemReturn`;

--    ALTER TABLE `ReturnItemSet` DROP CONSTRAINT `FK_ReturnItemMaterialStock`;

--    ALTER TABLE `ProductionSet` DROP CONSTRAINT `FK_ProductionProduct`;

--    ALTER TABLE `ProductionSet` DROP CONSTRAINT `FK_ProductionRequisition`;

--    ALTER TABLE `ProductionSet` DROP CONSTRAINT `FK_ProductionAssemblyLine`;

--    ALTER TABLE `PurchaseQualitySet` DROP CONSTRAINT `FK_PurchaseQualityPurchase`;

--    ALTER TABLE `PurchaseQualitySet` DROP CONSTRAINT `FK_PurchaseQualityUser`;

--    ALTER TABLE `ProductionQualitySet` DROP CONSTRAINT `FK_ProductionQualityProduction`;

--    ALTER TABLE `ProductionQualitySet` DROP CONSTRAINT `FK_ProductionQualityUser`;

--    ALTER TABLE `LogSet` DROP CONSTRAINT `FK_LogUser`;


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------
SET foreign_key_checks = 0;

    DROP TABLE IF EXISTS `ProductClassSet`;

    DROP TABLE IF EXISTS `ProductSet`;

    DROP TABLE IF EXISTS `MaterialClassSet`;

    DROP TABLE IF EXISTS `MaterialSet`;

    DROP TABLE IF EXISTS `RecipeItemSet`;

    DROP TABLE IF EXISTS `PrivilegeSet`;

    DROP TABLE IF EXISTS `UserGroupSet`;

    DROP TABLE IF EXISTS `UserGroupPrivilegeItemSet`;

    DROP TABLE IF EXISTS `UserSet`;

    DROP TABLE IF EXISTS `SupplierSet`;

    DROP TABLE IF EXISTS `CustomerSet`;

    DROP TABLE IF EXISTS `StorageSet`;

    DROP TABLE IF EXISTS `ProductStockSet`;

    DROP TABLE IF EXISTS `MaterialStockSet`;

    DROP TABLE IF EXISTS `StockInSet`;

    DROP TABLE IF EXISTS `StockOutSet`;

    DROP TABLE IF EXISTS `ProductStockInItemSet`;

    DROP TABLE IF EXISTS `MaterialStockInItemSet`;

    DROP TABLE IF EXISTS `ProductStockOutItemSet`;

    DROP TABLE IF EXISTS `MaterialStockOutItemSet`;

    DROP TABLE IF EXISTS `SalesSet`;

    DROP TABLE IF EXISTS `SalesItemSet`;

    DROP TABLE IF EXISTS `PurchaseSet`;

    DROP TABLE IF EXISTS `PurchaseItemSet`;

    DROP TABLE IF EXISTS `MaterialPriceSet`;

    DROP TABLE IF EXISTS `AssemblyLineSet`;

    DROP TABLE IF EXISTS `RequisitionSet`;

    DROP TABLE IF EXISTS `RequisitionItemSet`;

    DROP TABLE IF EXISTS `ReturnSet`;

    DROP TABLE IF EXISTS `ReturnItemSet`;

    DROP TABLE IF EXISTS `ProductionSet`;

    DROP TABLE IF EXISTS `PurchaseQualitySet`;

    DROP TABLE IF EXISTS `ProductionQualitySet`;

    DROP TABLE IF EXISTS `LogSet`;

SET foreign_key_checks = 1;

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------


CREATE TABLE `ProductClassSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Name` longtext NOT NULL, 
	`ParentId` int);

ALTER TABLE `ProductClassSet` ADD PRIMARY KEY (`Id`);





CREATE TABLE `ProductSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Name` longtext NOT NULL, 
	`ClassId` int NOT NULL, 
	`Type` longtext NOT NULL, 
	`Format` longtext NOT NULL, 
	`Unit` longtext NOT NULL, 
	`Price` double NOT NULL);

ALTER TABLE `ProductSet` ADD PRIMARY KEY (`Id`);





CREATE TABLE `MaterialClassSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Name` longtext NOT NULL, 
	`ParentId` int);

ALTER TABLE `MaterialClassSet` ADD PRIMARY KEY (`Id`);





CREATE TABLE `MaterialSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Name` longtext NOT NULL, 
	`Format` longtext NOT NULL, 
	`Unit` longtext NOT NULL, 
	`ClassId` int NOT NULL, 
	`Price` double NOT NULL);

ALTER TABLE `MaterialSet` ADD PRIMARY KEY (`Id`);





CREATE TABLE `RecipeItemSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`ProductId` int NOT NULL, 
	`MaterialId` int NOT NULL);

ALTER TABLE `RecipeItemSet` ADD PRIMARY KEY (`Id`);





CREATE TABLE `PrivilegeSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Description` longtext NOT NULL);

ALTER TABLE `PrivilegeSet` ADD PRIMARY KEY (`Id`);





CREATE TABLE `UserGroupSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Name` longtext NOT NULL);

ALTER TABLE `UserGroupSet` ADD PRIMARY KEY (`Id`);





CREATE TABLE `UserGroupPrivilegeItemSet`(
	`PrivilegeId` int NOT NULL, 
	`UserGroupId` int NOT NULL);

ALTER TABLE `UserGroupPrivilegeItemSet` ADD PRIMARY KEY (`PrivilegeId`, `UserGroupId`);





CREATE TABLE `UserSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Name` longtext NOT NULL, 
	`Password` longtext NOT NULL, 
	`Tel` longtext NOT NULL, 
	`UserGroupId` int NOT NULL);

ALTER TABLE `UserSet` ADD PRIMARY KEY (`Id`);





CREATE TABLE `SupplierSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Name` longtext NOT NULL, 
	`Contact` longtext NOT NULL, 
	`Location` longtext NOT NULL, 
	`Tel` longtext NOT NULL);

ALTER TABLE `SupplierSet` ADD PRIMARY KEY (`Id`);





CREATE TABLE `CustomerSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Name` longtext NOT NULL, 
	`Contact` longtext NOT NULL, 
	`Location` longtext NOT NULL, 
	`Tel` longtext NOT NULL);

ALTER TABLE `CustomerSet` ADD PRIMARY KEY (`Id`);





CREATE TABLE `StorageSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Name` longtext NOT NULL, 
	`UserId` int NOT NULL);

ALTER TABLE `StorageSet` ADD PRIMARY KEY (`Id`);





CREATE TABLE `ProductStockSet`(
	`StorageId` int NOT NULL, 
	`ProductId` int NOT NULL, 
	`Quantity` int NOT NULL, 
	`Location` longtext NOT NULL);

ALTER TABLE `ProductStockSet` ADD PRIMARY KEY (`StorageId`, `ProductId`);





CREATE TABLE `MaterialStockSet`(
	`MaterialId` int NOT NULL, 
	`StorageId` int NOT NULL, 
	`Quantity` int NOT NULL, 
	`Location` longtext NOT NULL);

ALTER TABLE `MaterialStockSet` ADD PRIMARY KEY (`MaterialId`, `StorageId`);





CREATE TABLE `StockInSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`StorageId` int NOT NULL, 
	`UserId` int NOT NULL, 
	`Time` datetime NOT NULL, 
	`Description` longtext);

ALTER TABLE `StockInSet` ADD PRIMARY KEY (`Id`);





CREATE TABLE `StockOutSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`StorageId` int NOT NULL, 
	`UserId` int NOT NULL, 
	`Time` datetime NOT NULL, 
	`Description` longtext);

ALTER TABLE `StockOutSet` ADD PRIMARY KEY (`Id`);





CREATE TABLE `ProductStockInItemSet`(
	`ProductId` int NOT NULL, 
	`StockInId` int NOT NULL, 
	`Quantity` int NOT NULL, 
	`Location` longtext NOT NULL);

ALTER TABLE `ProductStockInItemSet` ADD PRIMARY KEY (`StockInId`, `ProductId`);





CREATE TABLE `MaterialStockInItemSet`(
	`StockInId` int NOT NULL, 
	`MaterialId` int NOT NULL, 
	`Quantity` int NOT NULL, 
	`Location` longtext NOT NULL);

ALTER TABLE `MaterialStockInItemSet` ADD PRIMARY KEY (`StockInId`, `MaterialId`);





CREATE TABLE `ProductStockOutItemSet`(
	`ProductId` int NOT NULL, 
	`StockOutId` int NOT NULL, 
	`Quantity` int NOT NULL, 
	`Location` longtext NOT NULL);

ALTER TABLE `ProductStockOutItemSet` ADD PRIMARY KEY (`ProductId`, `StockOutId`);





CREATE TABLE `MaterialStockOutItemSet`(
	`MaterialId` int NOT NULL, 
	`StockOutId` int NOT NULL, 
	`Quantity` int NOT NULL, 
	`Location` longtext NOT NULL);

ALTER TABLE `MaterialStockOutItemSet` ADD PRIMARY KEY (`MaterialId`, `StockOutId`);





CREATE TABLE `SalesSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`CustomerId` int NOT NULL, 
	`UserId` int NOT NULL, 
	`Status` longtext NOT NULL, 
	`Priority` longtext NOT NULL, 
	`CreateAt` datetime NOT NULL, 
	`FinishedAt` datetime, 
	`Description` longtext);

ALTER TABLE `SalesSet` ADD PRIMARY KEY (`Id`);





CREATE TABLE `SalesItemSet`(
	`SalesId` int NOT NULL, 
	`ProductId` int NOT NULL, 
	`Price` double NOT NULL, 
	`Quantity` int NOT NULL);

ALTER TABLE `SalesItemSet` ADD PRIMARY KEY (`SalesId`, `ProductId`);





CREATE TABLE `PurchaseSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Status` longtext NOT NULL, 
	`Priority` longtext NOT NULL, 
	`CreateAt` datetime NOT NULL, 
	`FinishedAt` datetime NOT NULL, 
	`Description` longtext, 
	`UserId` int NOT NULL);

ALTER TABLE `PurchaseSet` ADD PRIMARY KEY (`Id`);





CREATE TABLE `PurchaseItemSet`(
	`PurchaseId` int NOT NULL, 
	`MaterialId` int NOT NULL, 
	`SupplierId` int NOT NULL, 
	`Price` double NOT NULL, 
	`Quantity` int NOT NULL);

ALTER TABLE `PurchaseItemSet` ADD PRIMARY KEY (`PurchaseId`, `MaterialId`, `SupplierId`);





CREATE TABLE `MaterialPriceSet`(
	`MaterialId` int NOT NULL, 
	`SupplierId` int NOT NULL, 
	`Price` double NOT NULL, 
	`StartQuantity` int NOT NULL, 
	`MaxQuantity` int NOT NULL);

ALTER TABLE `MaterialPriceSet` ADD PRIMARY KEY (`MaterialId`, `SupplierId`);





CREATE TABLE `AssemblyLineSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Name` longtext NOT NULL, 
	`Location` longtext NOT NULL, 
	`UserId` int NOT NULL);

ALTER TABLE `AssemblyLineSet` ADD PRIMARY KEY (`Id`);





CREATE TABLE `RequisitionSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Status` longtext NOT NULL, 
	`CreateAt` datetime NOT NULL, 
	`FinishedAt` datetime, 
	`Description` longtext, 
	`AssemblyLineId` int NOT NULL, 
	`UserId` int NOT NULL);

ALTER TABLE `RequisitionSet` ADD PRIMARY KEY (`Id`);





CREATE TABLE `RequisitionItemSet`(
	`RequisitionId` int NOT NULL, 
	`MaterialId` int NOT NULL, 
	`StorageId` int NOT NULL, 
	`Quantity` int NOT NULL);

ALTER TABLE `RequisitionItemSet` ADD PRIMARY KEY (`MaterialId`, `StorageId`, `RequisitionId`);





CREATE TABLE `ReturnSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Status` longtext NOT NULL, 
	`CreateAt` datetime NOT NULL, 
	`FinishedAt` datetime, 
	`Description` longtext, 
	`RequisitionId` int NOT NULL, 
	`UserId` int NOT NULL);

ALTER TABLE `ReturnSet` ADD PRIMARY KEY (`Id`);





CREATE TABLE `ReturnItemSet`(
	`ReturnId` int NOT NULL, 
	`MaterialId` int NOT NULL, 
	`StorageId` int NOT NULL, 
	`Quantity` int NOT NULL);

ALTER TABLE `ReturnItemSet` ADD PRIMARY KEY (`ReturnId`, `MaterialId`, `StorageId`);





CREATE TABLE `ProductionSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`ProductId` int NOT NULL, 
	`RequisitionId` int NOT NULL, 
	`AssemblyLineId` int NOT NULL, 
	`Status` longtext NOT NULL, 
	`StartAt` datetime NOT NULL, 
	`ThinkFinishedAt` datetime NOT NULL, 
	`FinishedAt` datetime, 
	`Description` longtext);

ALTER TABLE `ProductionSet` ADD PRIMARY KEY (`Id`);





CREATE TABLE `PurchaseQualitySet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`PurchaseId` int NOT NULL, 
	`UserId` int NOT NULL, 
	`Result` longtext NOT NULL, 
	`Time` datetime NOT NULL, 
	`Description` longtext);

ALTER TABLE `PurchaseQualitySet` ADD PRIMARY KEY (`Id`);





CREATE TABLE `ProductionQualitySet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`ProductionId` int NOT NULL, 
	`Result` longtext NOT NULL, 
	`Time` datetime NOT NULL, 
	`UserId` int NOT NULL, 
	`Description` longtext);

ALTER TABLE `ProductionQualitySet` ADD PRIMARY KEY (`Id`);





CREATE TABLE `LogSet`(
	`UserId` int NOT NULL, 
	`Time` datetime NOT NULL, 
	`Operation` longtext NOT NULL, 
	`Description` longtext);

ALTER TABLE `LogSet` ADD PRIMARY KEY (`UserId`, `Time`);







-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------


-- Creating foreign key on `ParentId` in table 'ProductClassSet'

ALTER TABLE `ProductClassSet`
ADD CONSTRAINT `FK_ProductClassNesting`
    FOREIGN KEY (`ParentId`)
    REFERENCES `ProductClassSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_ProductClassNesting'

CREATE INDEX `IX_FK_ProductClassNesting`
    ON `ProductClassSet`
    (`ParentId`);



-- Creating foreign key on `ClassId` in table 'ProductSet'

ALTER TABLE `ProductSet`
ADD CONSTRAINT `FK_ClassOfProduct`
    FOREIGN KEY (`ClassId`)
    REFERENCES `ProductClassSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_ClassOfProduct'

CREATE INDEX `IX_FK_ClassOfProduct`
    ON `ProductSet`
    (`ClassId`);



-- Creating foreign key on `ParentId` in table 'MaterialClassSet'

ALTER TABLE `MaterialClassSet`
ADD CONSTRAINT `FK_MaterialClassNesting`
    FOREIGN KEY (`ParentId`)
    REFERENCES `MaterialClassSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_MaterialClassNesting'

CREATE INDEX `IX_FK_MaterialClassNesting`
    ON `MaterialClassSet`
    (`ParentId`);



-- Creating foreign key on `ClassId` in table 'MaterialSet'

ALTER TABLE `MaterialSet`
ADD CONSTRAINT `FK_ClassOfMaterial`
    FOREIGN KEY (`ClassId`)
    REFERENCES `MaterialClassSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_ClassOfMaterial'

CREATE INDEX `IX_FK_ClassOfMaterial`
    ON `MaterialSet`
    (`ClassId`);



-- Creating foreign key on `ProductId` in table 'RecipeItemSet'

ALTER TABLE `RecipeItemSet`
ADD CONSTRAINT `FK_RecipeOfProduct`
    FOREIGN KEY (`ProductId`)
    REFERENCES `ProductSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_RecipeOfProduct'

CREATE INDEX `IX_FK_RecipeOfProduct`
    ON `RecipeItemSet`
    (`ProductId`);



-- Creating foreign key on `MaterialId` in table 'RecipeItemSet'

ALTER TABLE `RecipeItemSet`
ADD CONSTRAINT `FK_RecipeItemToMaterial`
    FOREIGN KEY (`MaterialId`)
    REFERENCES `MaterialSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_RecipeItemToMaterial'

CREATE INDEX `IX_FK_RecipeItemToMaterial`
    ON `RecipeItemSet`
    (`MaterialId`);



-- Creating foreign key on `PrivilegeId` in table 'UserGroupPrivilegeItemSet'

ALTER TABLE `UserGroupPrivilegeItemSet`
ADD CONSTRAINT `FK_UserGroupPrivilegeItemDescription`
    FOREIGN KEY (`PrivilegeId`)
    REFERENCES `PrivilegeSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;



-- Creating foreign key on `UserGroupId` in table 'UserGroupPrivilegeItemSet'

ALTER TABLE `UserGroupPrivilegeItemSet`
ADD CONSTRAINT `FK_UserGroupPrivilegeItemUserGroup`
    FOREIGN KEY (`UserGroupId`)
    REFERENCES `UserGroupSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_UserGroupPrivilegeItemUserGroup'

CREATE INDEX `IX_FK_UserGroupPrivilegeItemUserGroup`
    ON `UserGroupPrivilegeItemSet`
    (`UserGroupId`);



-- Creating foreign key on `UserGroupId` in table 'UserSet'

ALTER TABLE `UserSet`
ADD CONSTRAINT `FK_UserUserGroup`
    FOREIGN KEY (`UserGroupId`)
    REFERENCES `UserGroupSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_UserUserGroup'

CREATE INDEX `IX_FK_UserUserGroup`
    ON `UserSet`
    (`UserGroupId`);



-- Creating foreign key on `UserId` in table 'StorageSet'

ALTER TABLE `StorageSet`
ADD CONSTRAINT `FK_PrincipalOfStorage`
    FOREIGN KEY (`UserId`)
    REFERENCES `UserSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_PrincipalOfStorage'

CREATE INDEX `IX_FK_PrincipalOfStorage`
    ON `StorageSet`
    (`UserId`);



-- Creating foreign key on `StorageId` in table 'ProductStockSet'

ALTER TABLE `ProductStockSet`
ADD CONSTRAINT `FK_StockInStorage`
    FOREIGN KEY (`StorageId`)
    REFERENCES `StorageSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;



-- Creating foreign key on `ProductId` in table 'ProductStockSet'

ALTER TABLE `ProductStockSet`
ADD CONSTRAINT `FK_ProductInStock`
    FOREIGN KEY (`ProductId`)
    REFERENCES `ProductSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_ProductInStock'

CREATE INDEX `IX_FK_ProductInStock`
    ON `ProductStockSet`
    (`ProductId`);



-- Creating foreign key on `MaterialId` in table 'MaterialStockSet'

ALTER TABLE `MaterialStockSet`
ADD CONSTRAINT `FK_MaterialInStock`
    FOREIGN KEY (`MaterialId`)
    REFERENCES `MaterialSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;



-- Creating foreign key on `StorageId` in table 'MaterialStockSet'

ALTER TABLE `MaterialStockSet`
ADD CONSTRAINT `FK_MaterialOfStorage`
    FOREIGN KEY (`StorageId`)
    REFERENCES `StorageSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_MaterialOfStorage'

CREATE INDEX `IX_FK_MaterialOfStorage`
    ON `MaterialStockSet`
    (`StorageId`);



-- Creating foreign key on `StorageId` in table 'StockInSet'

ALTER TABLE `StockInSet`
ADD CONSTRAINT `FK_StockInStorage1`
    FOREIGN KEY (`StorageId`)
    REFERENCES `StorageSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_StockInStorage1'

CREATE INDEX `IX_FK_StockInStorage1`
    ON `StockInSet`
    (`StorageId`);



-- Creating foreign key on `UserId` in table 'StockInSet'

ALTER TABLE `StockInSet`
ADD CONSTRAINT `FK_StockInUser`
    FOREIGN KEY (`UserId`)
    REFERENCES `UserSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_StockInUser'

CREATE INDEX `IX_FK_StockInUser`
    ON `StockInSet`
    (`UserId`);



-- Creating foreign key on `StorageId` in table 'StockOutSet'

ALTER TABLE `StockOutSet`
ADD CONSTRAINT `FK_StockOutStorage`
    FOREIGN KEY (`StorageId`)
    REFERENCES `StorageSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_StockOutStorage'

CREATE INDEX `IX_FK_StockOutStorage`
    ON `StockOutSet`
    (`StorageId`);



-- Creating foreign key on `UserId` in table 'StockOutSet'

ALTER TABLE `StockOutSet`
ADD CONSTRAINT `FK_StockOutUser`
    FOREIGN KEY (`UserId`)
    REFERENCES `UserSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_StockOutUser'

CREATE INDEX `IX_FK_StockOutUser`
    ON `StockOutSet`
    (`UserId`);



-- Creating foreign key on `StockInId` in table 'ProductStockInItemSet'

ALTER TABLE `ProductStockInItemSet`
ADD CONSTRAINT `FK_ProductStockInItemStockIn`
    FOREIGN KEY (`StockInId`)
    REFERENCES `StockInSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;



-- Creating foreign key on `ProductId` in table 'ProductStockInItemSet'

ALTER TABLE `ProductStockInItemSet`
ADD CONSTRAINT `FK_ProductStockInItemProduct`
    FOREIGN KEY (`ProductId`)
    REFERENCES `ProductSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_ProductStockInItemProduct'

CREATE INDEX `IX_FK_ProductStockInItemProduct`
    ON `ProductStockInItemSet`
    (`ProductId`);



-- Creating foreign key on `StockInId` in table 'MaterialStockInItemSet'

ALTER TABLE `MaterialStockInItemSet`
ADD CONSTRAINT `FK_MaterialStockInItemStockIn`
    FOREIGN KEY (`StockInId`)
    REFERENCES `StockInSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;



-- Creating foreign key on `MaterialId` in table 'MaterialStockInItemSet'

ALTER TABLE `MaterialStockInItemSet`
ADD CONSTRAINT `FK_MaterialStockInItemMaterial`
    FOREIGN KEY (`MaterialId`)
    REFERENCES `MaterialSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_MaterialStockInItemMaterial'

CREATE INDEX `IX_FK_MaterialStockInItemMaterial`
    ON `MaterialStockInItemSet`
    (`MaterialId`);



-- Creating foreign key on `ProductId` in table 'ProductStockOutItemSet'

ALTER TABLE `ProductStockOutItemSet`
ADD CONSTRAINT `FK_ProductStockOutItemProduct`
    FOREIGN KEY (`ProductId`)
    REFERENCES `ProductSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;



-- Creating foreign key on `StockOutId` in table 'ProductStockOutItemSet'

ALTER TABLE `ProductStockOutItemSet`
ADD CONSTRAINT `FK_ProductStockOutItemStockOut`
    FOREIGN KEY (`StockOutId`)
    REFERENCES `StockOutSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_ProductStockOutItemStockOut'

CREATE INDEX `IX_FK_ProductStockOutItemStockOut`
    ON `ProductStockOutItemSet`
    (`StockOutId`);



-- Creating foreign key on `MaterialId` in table 'MaterialStockOutItemSet'

ALTER TABLE `MaterialStockOutItemSet`
ADD CONSTRAINT `FK_MaterialStockOutItemMaterial`
    FOREIGN KEY (`MaterialId`)
    REFERENCES `MaterialSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;



-- Creating foreign key on `StockOutId` in table 'MaterialStockOutItemSet'

ALTER TABLE `MaterialStockOutItemSet`
ADD CONSTRAINT `FK_MaterialStockOutItemStockOut`
    FOREIGN KEY (`StockOutId`)
    REFERENCES `StockOutSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_MaterialStockOutItemStockOut'

CREATE INDEX `IX_FK_MaterialStockOutItemStockOut`
    ON `MaterialStockOutItemSet`
    (`StockOutId`);



-- Creating foreign key on `CustomerId` in table 'SalesSet'

ALTER TABLE `SalesSet`
ADD CONSTRAINT `FK_SalesCustomer`
    FOREIGN KEY (`CustomerId`)
    REFERENCES `CustomerSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_SalesCustomer'

CREATE INDEX `IX_FK_SalesCustomer`
    ON `SalesSet`
    (`CustomerId`);



-- Creating foreign key on `UserId` in table 'SalesSet'

ALTER TABLE `SalesSet`
ADD CONSTRAINT `FK_SalesUser`
    FOREIGN KEY (`UserId`)
    REFERENCES `UserSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_SalesUser'

CREATE INDEX `IX_FK_SalesUser`
    ON `SalesSet`
    (`UserId`);



-- Creating foreign key on `SalesId` in table 'SalesItemSet'

ALTER TABLE `SalesItemSet`
ADD CONSTRAINT `FK_SalesItemSales`
    FOREIGN KEY (`SalesId`)
    REFERENCES `SalesSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;



-- Creating foreign key on `ProductId` in table 'SalesItemSet'

ALTER TABLE `SalesItemSet`
ADD CONSTRAINT `FK_SalesItemProduct`
    FOREIGN KEY (`ProductId`)
    REFERENCES `ProductSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_SalesItemProduct'

CREATE INDEX `IX_FK_SalesItemProduct`
    ON `SalesItemSet`
    (`ProductId`);



-- Creating foreign key on `UserId` in table 'PurchaseSet'

ALTER TABLE `PurchaseSet`
ADD CONSTRAINT `FK_PurchaseUser`
    FOREIGN KEY (`UserId`)
    REFERENCES `UserSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_PurchaseUser'

CREATE INDEX `IX_FK_PurchaseUser`
    ON `PurchaseSet`
    (`UserId`);



-- Creating foreign key on `PurchaseId` in table 'PurchaseItemSet'

ALTER TABLE `PurchaseItemSet`
ADD CONSTRAINT `FK_PurchaseItemPurchase`
    FOREIGN KEY (`PurchaseId`)
    REFERENCES `PurchaseSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;



-- Creating foreign key on `MaterialId` in table 'MaterialPriceSet'

ALTER TABLE `MaterialPriceSet`
ADD CONSTRAINT `FK_MaterialPriceMaterial`
    FOREIGN KEY (`MaterialId`)
    REFERENCES `MaterialSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;



-- Creating foreign key on `SupplierId` in table 'MaterialPriceSet'

ALTER TABLE `MaterialPriceSet`
ADD CONSTRAINT `FK_MaterialPriceSupplier`
    FOREIGN KEY (`SupplierId`)
    REFERENCES `SupplierSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_MaterialPriceSupplier'

CREATE INDEX `IX_FK_MaterialPriceSupplier`
    ON `MaterialPriceSet`
    (`SupplierId`);



-- Creating foreign key on `MaterialId`, `SupplierId` in table 'PurchaseItemSet'

ALTER TABLE `PurchaseItemSet`
ADD CONSTRAINT `FK_PurchaseItemMaterialPrice`
    FOREIGN KEY (`MaterialId`, `SupplierId`)
    REFERENCES `MaterialPriceSet`
        (`MaterialId`, `SupplierId`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_PurchaseItemMaterialPrice'

CREATE INDEX `IX_FK_PurchaseItemMaterialPrice`
    ON `PurchaseItemSet`
    (`MaterialId`, `SupplierId`);



-- Creating foreign key on `UserId` in table 'AssemblyLineSet'

ALTER TABLE `AssemblyLineSet`
ADD CONSTRAINT `FK_AssemblyLineUser`
    FOREIGN KEY (`UserId`)
    REFERENCES `UserSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_AssemblyLineUser'

CREATE INDEX `IX_FK_AssemblyLineUser`
    ON `AssemblyLineSet`
    (`UserId`);



-- Creating foreign key on `AssemblyLineId` in table 'RequisitionSet'

ALTER TABLE `RequisitionSet`
ADD CONSTRAINT `FK_RequisitionAssemblyLine`
    FOREIGN KEY (`AssemblyLineId`)
    REFERENCES `AssemblyLineSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_RequisitionAssemblyLine'

CREATE INDEX `IX_FK_RequisitionAssemblyLine`
    ON `RequisitionSet`
    (`AssemblyLineId`);



-- Creating foreign key on `UserId` in table 'RequisitionSet'

ALTER TABLE `RequisitionSet`
ADD CONSTRAINT `FK_RequisitionUser`
    FOREIGN KEY (`UserId`)
    REFERENCES `UserSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_RequisitionUser'

CREATE INDEX `IX_FK_RequisitionUser`
    ON `RequisitionSet`
    (`UserId`);



-- Creating foreign key on `RequisitionId` in table 'RequisitionItemSet'

ALTER TABLE `RequisitionItemSet`
ADD CONSTRAINT `FK_RequisitionItemRequisition`
    FOREIGN KEY (`RequisitionId`)
    REFERENCES `RequisitionSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_RequisitionItemRequisition'

CREATE INDEX `IX_FK_RequisitionItemRequisition`
    ON `RequisitionItemSet`
    (`RequisitionId`);



-- Creating foreign key on `MaterialId`, `StorageId` in table 'RequisitionItemSet'

ALTER TABLE `RequisitionItemSet`
ADD CONSTRAINT `FK_RequisitionItemMaterialStock`
    FOREIGN KEY (`MaterialId`, `StorageId`)
    REFERENCES `MaterialStockSet`
        (`MaterialId`, `StorageId`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;



-- Creating foreign key on `RequisitionId` in table 'ReturnSet'

ALTER TABLE `ReturnSet`
ADD CONSTRAINT `FK_ReturnRequisition`
    FOREIGN KEY (`RequisitionId`)
    REFERENCES `RequisitionSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_ReturnRequisition'

CREATE INDEX `IX_FK_ReturnRequisition`
    ON `ReturnSet`
    (`RequisitionId`);



-- Creating foreign key on `UserId` in table 'ReturnSet'

ALTER TABLE `ReturnSet`
ADD CONSTRAINT `FK_ReturnUser`
    FOREIGN KEY (`UserId`)
    REFERENCES `UserSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_ReturnUser'

CREATE INDEX `IX_FK_ReturnUser`
    ON `ReturnSet`
    (`UserId`);



-- Creating foreign key on `ReturnId` in table 'ReturnItemSet'

ALTER TABLE `ReturnItemSet`
ADD CONSTRAINT `FK_ReturnItemReturn`
    FOREIGN KEY (`ReturnId`)
    REFERENCES `ReturnSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;



-- Creating foreign key on `MaterialId`, `StorageId` in table 'ReturnItemSet'

ALTER TABLE `ReturnItemSet`
ADD CONSTRAINT `FK_ReturnItemMaterialStock`
    FOREIGN KEY (`MaterialId`, `StorageId`)
    REFERENCES `MaterialStockSet`
        (`MaterialId`, `StorageId`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_ReturnItemMaterialStock'

CREATE INDEX `IX_FK_ReturnItemMaterialStock`
    ON `ReturnItemSet`
    (`MaterialId`, `StorageId`);



-- Creating foreign key on `ProductId` in table 'ProductionSet'

ALTER TABLE `ProductionSet`
ADD CONSTRAINT `FK_ProductionProduct`
    FOREIGN KEY (`ProductId`)
    REFERENCES `ProductSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionProduct'

CREATE INDEX `IX_FK_ProductionProduct`
    ON `ProductionSet`
    (`ProductId`);



-- Creating foreign key on `RequisitionId` in table 'ProductionSet'

ALTER TABLE `ProductionSet`
ADD CONSTRAINT `FK_ProductionRequisition`
    FOREIGN KEY (`RequisitionId`)
    REFERENCES `RequisitionSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionRequisition'

CREATE INDEX `IX_FK_ProductionRequisition`
    ON `ProductionSet`
    (`RequisitionId`);



-- Creating foreign key on `AssemblyLineId` in table 'ProductionSet'

ALTER TABLE `ProductionSet`
ADD CONSTRAINT `FK_ProductionAssemblyLine`
    FOREIGN KEY (`AssemblyLineId`)
    REFERENCES `AssemblyLineSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionAssemblyLine'

CREATE INDEX `IX_FK_ProductionAssemblyLine`
    ON `ProductionSet`
    (`AssemblyLineId`);



-- Creating foreign key on `PurchaseId` in table 'PurchaseQualitySet'

ALTER TABLE `PurchaseQualitySet`
ADD CONSTRAINT `FK_PurchaseQualityPurchase`
    FOREIGN KEY (`PurchaseId`)
    REFERENCES `PurchaseSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_PurchaseQualityPurchase'

CREATE INDEX `IX_FK_PurchaseQualityPurchase`
    ON `PurchaseQualitySet`
    (`PurchaseId`);



-- Creating foreign key on `UserId` in table 'PurchaseQualitySet'

ALTER TABLE `PurchaseQualitySet`
ADD CONSTRAINT `FK_PurchaseQualityUser`
    FOREIGN KEY (`UserId`)
    REFERENCES `UserSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_PurchaseQualityUser'

CREATE INDEX `IX_FK_PurchaseQualityUser`
    ON `PurchaseQualitySet`
    (`UserId`);



-- Creating foreign key on `ProductionId` in table 'ProductionQualitySet'

ALTER TABLE `ProductionQualitySet`
ADD CONSTRAINT `FK_ProductionQualityProduction`
    FOREIGN KEY (`ProductionId`)
    REFERENCES `ProductionSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionQualityProduction'

CREATE INDEX `IX_FK_ProductionQualityProduction`
    ON `ProductionQualitySet`
    (`ProductionId`);



-- Creating foreign key on `UserId` in table 'ProductionQualitySet'

ALTER TABLE `ProductionQualitySet`
ADD CONSTRAINT `FK_ProductionQualityUser`
    FOREIGN KEY (`UserId`)
    REFERENCES `UserSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionQualityUser'

CREATE INDEX `IX_FK_ProductionQualityUser`
    ON `ProductionQualitySet`
    (`UserId`);



-- Creating foreign key on `UserId` in table 'LogSet'

ALTER TABLE `LogSet`
ADD CONSTRAINT `FK_LogUser`
    FOREIGN KEY (`UserId`)
    REFERENCES `UserSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;



-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
