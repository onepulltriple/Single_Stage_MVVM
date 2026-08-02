USE MASTER
GO

DROP DATABASE Single_Stage_MVVM
CREATE DATABASE Single_Stage_MVVM
GO

USE Single_Stage_MVVM
GO

ALTER AUTHORIZATION ON DATABASE::Single_Stage_MVVM TO sa;

CREATE TABLE Employee (
	id int PRIMARY KEY IDENTITY(1,1),
	Username varchar(50) NOT NULL UNIQUE,
	Password varchar(255) NOT NULL
	)

CREATE TABLE Artist (
	id int PRIMARY KEY IDENTITY(1,1),
	Name varchar(50) NOT NULL UNIQUE
	)

CREATE TABLE Show (
	id int PRIMARY KEY IDENTITY(1,1),
	Name varchar(50) NOT NULL,
	StartTime datetime NOT NULL,
	EndTime datetime NOT NULL,
	TicketPrice decimal,
	SoldOut bit NOT NULL DEFAULT 0
	)

CREATE TABLE ShowAppearance (
	id int PRIMARY KEY IDENTITY(1,1),
	Description varchar(100) NOT NULL,
	StartTime datetime NOT NULL,
	EndTime datetime NOT NULL,
	Show_id int NOT NULL,
	CONSTRAINT FK_ParentShowChildShowAppearance
		FOREIGN KEY (Show_id) 
			REFERENCES Show(id)
			ON DELETE CASCADE
	)

CREATE TABLE Appearance (
	id int PRIMARY KEY IDENTITY(1,1),
	RoyaltyUpFront decimal,
	RoyaltyAtEnd decimal,
	Artist_id int NOT NULL,
	CONSTRAINT FK_ParentArtistChildAppearance
		FOREIGN KEY (Artist_id) 
			REFERENCES Artist(id)
			ON DELETE CASCADE,
	ShowAppearance_id int NOT NULL,
	CONSTRAINT FK_ParentShowAppearanceChildAppearance
		FOREIGN KEY (ShowAppearance_id) 
			REFERENCES ShowAppearance(id)
			ON DELETE CASCADE
	)

CREATE TABLE Seat (
	id int PRIMARY KEY IDENTITY(1,1),
	Row char NOT NULL,
	Number int NOT NULL
	)

CREATE TABLE Ticketholder (
	id int PRIMARY KEY IDENTITY(1,1),
	Name varchar(50) NOT NULL,
	Birthdate datetime NOT NULL,
	Email varchar(100) NOT NULL UNIQUE,
	Discount bit NOT NULL DEFAULT 0
	)

CREATE TABLE Ticket (
	id int PRIMARY KEY IDENTITY(1,1),
	Ticketholder_id int NOT NULL,
	CONSTRAINT FK_ParentTicketholderChildTicket
		FOREIGN KEY (Ticketholder_id) 
			REFERENCES Ticketholder(id)
			ON DELETE CASCADE,
	Seat_id int NOT NULL,
	CONSTRAINT FK_ParentSeatChildTicket
		FOREIGN KEY (Seat_id) 
			REFERENCES Seat(id)
			ON DELETE CASCADE,
	Show_id int NOT NULL,
	CONSTRAINT FK_ParentShowChildTicket
		FOREIGN KEY (Show_id) 
			REFERENCES Show(id)
			ON DELETE CASCADE
	)
