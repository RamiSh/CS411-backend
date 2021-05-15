CREATE
    ALGORITHM = UNDEFINED
    DEFINER = `root`@`localhost`
    SQL SECURITY DEFINER
VIEW `Main`.`car_listings` AS
    SELECT
        `c`.`RowID` AS `RowID`,
        `c`.`region` AS `region`,
        `c`.`price` AS `price`,
        `c`.`year` AS `year`,
        `c`.`manufacturer` AS `manufacturer`,
        `c`.`model` AS `model`,
        `c`.`condition` AS `condition`,
        `c`.`cylinders` AS `cylinders`,
        `c`.`fuel` AS `fuel`,
        `c`.`odometer` AS `odometer`,
        `c`.`title_status` AS `title_status`,
        `c`.`transmission` AS `transmission`,
        `c`.`VIN` AS `VIN`,
        `c`.`drive` AS `drive`,
        `c`.`size` AS `size`,
        `c`.`type` AS `type`,
        `c`.`paint_color` AS `paint_color`,
        `c`.`state` AS `state`,
        `c`.`posting_date` AS `posting_date`,
        `s`.`ListingID` AS `ListingID`,
        `s`.`UserID` AS `UserID`,
        `s`.`CarID` AS `CarID`,
        `s`.`DateListed` AS `DateListed`,
        `s`.`DateSold` AS `DateSold`,
        `s`.`PriceListed` AS `PriceListed`,
        `s`.`PriceSold` AS `PriceSold`
    FROM
        (`Main`.`Sells` `s`
        JOIN `Main`.`CarsDB` `c` ON ((`c`.`RowID` = `s`.`CarID`)))