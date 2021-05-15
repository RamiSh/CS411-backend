CREATE DEFINER=`root`@`localhost` TRIGGER `Main`.`Sells_BEFORE_INSERT` BEFORE INSERT ON `Sells` FOR EACH ROW
BEGIN
	IF NEW.UserID not in (
		select A.UserID
		From Users A
		where (NEW.UserID = A.UserID and NEW.UserID = A.UserID)
	)
    THEN
	signal sqlstate '45000' set message_text = 'that user does not exist';

	END IF;

	IF NEW.CarID not in (
		select A.RowID
		From CarsDB A
		where (NEW.CarID = A.RowID and NEW.CarID = A.RowID)
	)
    THEN
	signal sqlstate '45000' set message_text = 'That car does not exist';

	END IF;
END