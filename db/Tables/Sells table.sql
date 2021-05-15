CREATE TABLE `Sells` (
  `ListingID` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `UserID` int(11) unsigned NOT NULL,
  `CarID` int(11) unsigned NOT NULL,
  `DateListed` date DEFAULT NULL,
  `DateSold` date DEFAULT NULL,
  `PriceListed` double DEFAULT NULL,
  `PriceSold` double DEFAULT NULL,
  PRIMARY KEY (`ListingID`),
  UNIQUE KEY `ListingID_UNIQUE` (`ListingID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1