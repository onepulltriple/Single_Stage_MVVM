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
    ('1',  'LAWL', '2026-08-03 04:00:00', '2026-08-03 20:00:00', '35.00', '0'),
    ('2',  'ROFL', '2026-08-05 09:00:00', '2026-08-05 11:00:00', '50.00', '0'),
    ('3',  'OOPS', '2026-08-07 14:00:00', '2026-08-07 20:00:00', '25.00', '0'),
    ('4',  'GGEZ', '2026-08-08 19:00:00', '2026-08-08 23:00:00', '65.00', '0'),
    ('5',  'ACAB', '2026-08-09 08:00:00', '2026-08-09 10:00:00', '30.00', '0'),

    ('6',  'HEYH', '2026-08-10 04:00:00', '2026-08-10 20:00:00', '45.00', '0'),
    ('7',  'ABCD', '2026-08-11 09:00:00', '2026-08-11 11:00:00', '20.00', '0'),
    ('8',  'RUFF', '2026-08-12 14:00:00', '2026-08-12 20:00:00', '55.00', '0'),
    ('9',  'CMON', '2026-08-14 19:00:00', '2026-08-14 23:00:00', '75.00', '0'),
    ('10', 'A#$A', '2026-08-15 08:00:00', '2026-08-15 10:00:00', '40.00', '0'),

    ('11', 'SHOK', '2026-08-17 04:00:00', '2026-08-17 20:00:00', '60.00', '0'),
    ('12', 'RUND', '2026-08-20 09:00:00', '2026-08-20 11:00:00', '30.00', '0'),
    ('13', 'MCMC', '2026-08-23 14:00:00', '2026-08-23 20:00:00', '85.00', '0'),
    ('14', 'BBOY', '2026-08-27 19:00:00', '2026-08-27 23:00:00', '45.00', '0'),
    ('15', 'BOTZ', '2026-08-30 08:00:00', '2026-08-30 10:00:00', '70.00', '0');
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
SET IDENTITY_INSERT ShowAppearance ON;
INSERT INTO ShowAppearance 
	(id, Description, StartTime, EndTime, Show_id) 
	VALUES 
	('1', 'LAWL: First Set' ,'2026-08-06 14:00:00','2026-08-06 17:00:00',1),
	('2', 'LAWL: Second Set','2026-08-06 17:00:00','2026-08-06 20:00:00',1)
SET IDENTITY_INSERT ShowAppearance OFF;

SELECT * FROM ShowAppearance ORDER BY id


DELETE FROM Appearance
SET IDENTITY_INSERT Appearance ON;
INSERT INTO Appearance 
	(id, RoyaltyUpFront, RoyaltyAtEnd, Artist_id, ShowAppearance_id) 
	VALUES 
	('1','3000','9000',1,1)
SET IDENTITY_INSERT Appearance OFF;

SELECT * FROM Appearance ORDER BY id
