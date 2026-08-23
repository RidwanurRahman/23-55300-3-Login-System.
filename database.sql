IF DB_ID(N'db_users') IS NULL
BEGIN
    CREATE DATABASE [db_users];
END
GO

USE [db_users];
GO

IF OBJECT_ID(N'dbo.tbl_users', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tbl_users
    (
        id INT IDENTITY(1,1) PRIMARY KEY,
        username NVARCHAR(50) NOT NULL UNIQUE,
        password VARCHAR(64) NOT NULL
    );
END
GO

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.tbl_users
    WHERE username = N'admin'
)
BEGIN
    INSERT INTO dbo.tbl_users (username, password)
    VALUES
    (
        N'admin',
        CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'admin123'), 2)
    );
END
GO

SELECT id, username, password
FROM dbo.tbl_users
ORDER BY id;
GO
