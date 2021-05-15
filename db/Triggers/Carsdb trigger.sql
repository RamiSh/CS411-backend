CREATE DEFINER=`root`@`localhost` TRIGGER `Main`.`remove_from_ownsandsells` AFTER DELETE ON `CarsDB` FOR EACH ROW
BEGIN
DELETE FROM Owns
WHERE old.RowID = CarID;

DELETE FROM Sells
WHERE old.RowID = CarID;
END