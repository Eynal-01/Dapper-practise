﻿CREATE DATABASE GameDB
GO
USE GameDB
GO
CREATE TABLE Players(
[Id] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
[PlayerName] NVARCHAR(30) NOT NULL,
[Score] FLOAT NOT NULL,
[IsStar] BIT NOT NULL DEFAULT(0)
)
GO

INSERT INTO Players([PlayerName], [Score], [IsStar])
VALUES('Lebron James', 99, 1),
('Stephan Curry', 95, 1),
('Maykl Jordan', 94, 0)

SELECT * FROM Players

CREATE PROCEDURE ShowGreaterThan
(@pScore AS FLOAT)
AS
SELECT * FROM Players
WHERE Score>@pScore