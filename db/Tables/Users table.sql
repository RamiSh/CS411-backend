CREATE TABLE `Users` (
  `UserID` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(45) NOT NULL,
  `Password` varchar(45) NOT NULL,
  `email` varchar(45) NOT NULL,
  PRIMARY KEY (`UserID`),
  UNIQUE KEY `UserName_UN` (`Username`),
  UNIQUE KEY `Email_UN` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=2001 DEFAULT CHARSET=latin1