USE Single_Stage_MVVM
GO

DELETE FROM Artist
SET IDENTITY_INSERT Artist ON;
INSERT INTO Artist 
	(id, Name)
	VALUES
	('1','RATM'),
	('2','Tool'),
	('3','QOTSA'),
	('4','Beck')
SET IDENTITY_INSERT Artist OFF;
SELECT * FROM Artist ORDER BY id


DELETE FROM Show
SET IDENTITY_INSERT Show ON;
INSERT INTO Show 
	(id, Name, StartTime, EndTime, TicketPrice, SoldOut)
	VALUES
	('1','LAWL','2026-08-06 14:00:00','2026-08-06 20:00:00','40.00','0')
SET IDENTITY_INSERT Show OFF;

SELECT * FROM Show ORDER BY id


DELETE FROM Ticketholder
SET IDENTITY_INSERT Ticketholder ON;
INSERT INTO Ticketholder 
	(id, Name, Birthdate, Email, Discount) 
	VALUES 
	('1', 'Rock Salt','2000-05-20 13:00:00','yahooStillAThing@yahoo.com','0')
SET IDENTITY_INSERT Ticketholder OFF;
SELECT * FROM Ticketholder ORDER BY Name


DELETE FROM ShowAppearance
INSERT INTO ShowAppearance 
	(Description, StartTime, EndTime, Show_id) 
	VALUES 
	('LAWL: First Set' ,'2026-08-06 14:00:00','2026-08-06 17:00:00',1),
	('LAWL: Second Set','2026-08-06 17:00:00','2026-08-06 20:00:00',1)

SELECT * FROM ShowAppearance ORDER BY id


DELETE FROM Appearance
INSERT INTO Appearance 
	(RoyaltyUpFront, RoyaltyAtEnd, Artist_id, ShowAppearance_id) 
	VALUES 
	('3000','9000',1,1)

SELECT * FROM Appearance ORDER BY id
