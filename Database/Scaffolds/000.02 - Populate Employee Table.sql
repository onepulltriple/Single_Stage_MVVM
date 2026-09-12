USE Single_Stage_MVVM
GO

DELETE FROM Employee
INSERT INTO Employee
	(Username, Password)
	VALUES
	('admin','$2a$11$lLCjSYzZ7oE/7k1xDyn17eHPGyXgM0e7AiCQ7g934I6FrADoQytBe')

SELECT * FROM Employee ORDER BY id