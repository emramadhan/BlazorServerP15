CREATE DATABASE BookDB
GO

USE BookDB
GO

CREATE TABLE [dbo].[Publisher](
   [Id]       [Int] IDENTITY(0,1),
   [Name]     [VarChar](20),
   [City]     [VarChar](20),
   [Country]  [VarChar](20),
   CONSTRAINT [pkPublisher] PRIMARY KEY ([Id])
)
GO

CREATE TABLE [dbo].[Author](
   [Id]    [Int] IDENTITY(1,1),
   [FName] [VarChar](20),
   [LName] [VarChar](20),
   [Phone] [VarChar](16),
   CONSTRAINT [pkAuthor] PRIMARY KEY ([Id])
)
GO

CREATE TABLE [dbo].[Book](
   [ISBN]      [BigInt],
   [Title]     [VarChar](80),
   [PubYear]   [SmallInt],
   [PurchDate] [Date],
   [PubId]     [Int] DEFAULT 0,
   CONSTRAINT  [pkBook] PRIMARY KEY ([ISBN]),
   CONSTRAINT  [fkBookPub] FOREIGN KEY ([PubId])
   REFERENCES  [dbo].[Publisher]([Id])
   ON UPDATE CASCADE ON DELETE SET DEFAULT
)
GO

CREATE TABLE [dbo].[BookAuthor](
   [ISBN]      [BigInt],
   [AuthorId]  [Int],
   [AuthorOrd] [TinyInt],
   CONSTRAINT  [pkBookAuthor] PRIMARY KEY ([ISBN],[AuthorId]),
   CONSTRAINT  [fkBookAuthor_Book] FOREIGN KEY([ISBN])
   REFERENCES  [dbo].[Book] ([ISBN]) 
   ON UPDATE CASCADE ON DELETE CASCADE,
   CONSTRAINT  [fkBookAuthor_Author] FOREIGN KEY ([AuthorId])
   REFERENCES  [dbo].[Author] ([Id])
   ON UPDATE CASCADE ON DELETE CASCADE
)
GO

INSERT INTO Publisher([Name], [City], [Country]) 
VALUES ('Unknown', NULL, NULL),
       ('Unsri Press', 'Palembang', 'Indonesia'),
       ('Gramedia', 'Jakarta', 'Indonesia'),
       ('Apress', 'New York', 'USA'),
       ('UIGM Press', 'Palembang', 'Indonesia'),
       ('Packt', 'Birmingham', 'UK'),
       ('Rafah Press', 'Palembang', 'Indonesia'),
       ('O’Reilly','Sebastopol','USA'),
       ('Informatika', 'Bandung', 'Indonesia'),
       ('Pearson', 'London', 'UK'),
       ('Cengage', 'Boston', 'USA'),
       ('Noerfikri', 'Palembang', 'Indonesia'),
       ('Iris Press','Bandung','Indonesia')
GO

INSERT INTO dbo.Book (ISBN, PubYear, PurchDate, Title, PubId)
VALUES (9789791339957, 2013, '2019-02-12', 'Pranata Sosial', 6),
       (9780134802749, 2018, '2020-10-14', 'Database Processing', 9),
       (9786024474348, 2019, '2019-10-01', 'Desain Basis Data Akademik Perguruan Tinggi', 11),
       (9781305576766, 2015, '2018-12-31', 'NoSQL Web Development with Apache Cassandra', 10),
       (9780135191767, 2020, '2020-06-30', 'Using MIS',9),
       (9781789619768, 2020, '2020-08-31', 'Modern Web Development with ASP.NET Core 3', 5),
       (9789793053585, 2009, '2015-03-25', 'Mengenal Pola Huruf Arab',12),
       (9781492056812, 2020, '2020-09-27', 'Programming C# 8.0',7),
       (9781783989201, 2015, '2017-07-23', 'Learning Apache Cassandra', 5),
       (9781484259276, 2020, '2020-10-17', 'Microsoft Blazor: Building Web App in .NET', 3),
       (9786020338682, 2017, '2018-08-10', 'Disruption', 2)
GO

INSERT INTO Author([FName], [LName], [Phone])
VALUES ('Rhenald', 'Kasali', 'UNKNOWN'),
       ('David M.', 'Kroenke ', 'UNKNOWN'),
       ('M.', 'Ramadhan', '081122334455'),
       ('Randall J.', 'Boyle', 'UNKNOWN'),
       ('Deepak', 'Vohra', 'UNKNOWN'),
       ('Hamidah', 'Akil', '08123456789'),
       ('David','Auer','UNKNOWN'),
       ('Ricardo', 'Peres', 'UNKNOWN'),
       ('Ian', 'Griffiths', 'UNKNOWN'),
       ('Mat', 'Brown', 'UNKNOWN'),
       ('Peter', 'Himschoot', 'UNKNOWN')
GO 

INSERT INTO [dbo].[BookAuthor]([ISBN],[AuthorId],[AuthorOrd])
VALUES (9780134802749, 2, 1),(9780134802749, 7, 2),
       (9789791339957, 6, 1),(9781789619768, 8, 1),
       (9781305576766, 5, 1),(9781492056812, 9, 1),
       (9780135191767, 2, 1),(9780135191767, 4, 2),
       (9786020338682, 1, 1),(9789793053585, 3, 1),
       (9786024474348, 3, 1),(9781783989201, 10, 1),
       (9781484259276, 11, 1)
GO

CREATE PROCEDURE [dbo].[spAddPublisher]
                 @Name    VarChar(20), 
                 @City    VarChar(20), 
                 @Country VarChar(20)
       AS
BEGIN    
   DECLARE @Id Int;
   INSERT  INTO Publisher([Name], [City], [Country]) 
   VALUES  (@Name, @City, @Country);
   SET     @Id = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE [dbo].[spUpdatePublisher]
                 @Id      Int, 
                 @Name    VarChar(20), 
                 @City    VarChar(20), 
                 @Country VarChar(20)
       AS
UPDATE Publisher 
SET [Name]    = @Name, 
    [City]    = @City, 
    [Country] = @Country 
WHERE [Id] = @Id
GO

CREATE PROCEDURE [dbo].[spAddBook]
                 @ISBN      Bigint, 
                 @Title     VarChar(80), 
                 @PubYear   SmallInt,
                 @PurchDate Date,
                 @PubId     Int 
       AS
INSERT INTO dbo.Book (ISBN, Title, PubYear, PurchDate, PubId)
VALUES (@ISBN, @Title, @PubYear, @PurchDate, @PubId)
GO

CREATE PROCEDURE [dbo].[spUpdateBook]
                 @ISBN      Bigint, 
                 @Title     VarChar(80), 
                 @PubYear   SmallInt,
                 @PurchDate Date,
                 @PubId     Int,
                 @Pk        Bigint
       AS
UPDATE Book 
SET [ISBN]      = @ISBN,
    [Title]     = @Title, 
    [PubYear]   = @PubYear,
    [PurchDate] = @PurchDate,
    [PubId]     = @PubId
WHERE [ISBN] = @Pk
GO

CREATE PROCEDURE [dbo].[spAddAuthor]
                 @FName VarChar(20), 
                 @LName VarChar(20), 
                 @Phone VarChar(20)
       AS
BEGIN    
   DECLARE @Id as Int;
   INSERT  INTO Author([FName], [LName], [Phone]) 
   VALUES  (@FName, @LName, @Phone);
   SET     @Id = SCOPE_IDENTITY();   
   SELECT  @Id AS AuthorId;   
END
GO

CREATE PROCEDURE [dbo].[spUpdateAuthor]
                 @Id    Int, 
                 @FName VarChar(20), 
                 @LName VarChar(20), 
                 @Phone VarChar(20)
       AS
UPDATE Author 
SET [FName] = @FName, 
    [LName] = @LName, 
    [Phone] = @Phone 
WHERE  [Id] = @Id
GO

CREATE PROCEDURE [dbo].[spAddBookAuthor]
                @ISBN      BigInt, 
                @AuthorId  Int, 
                @AuthorOrd TinyInt
      AS
INSERT INTO [dbo].[BookAuthor]([ISBN],[AuthorId],[AuthorOrd])
VALUES (@ISBN, @AuthorId, @AuthorOrd)
GO

CREATE PROCEDURE [dbo].[spUpdateBookAuthor]
                @ISBN      BigInt, 
                @AuthorId  Int, 
                @AuthorOrd TinyInt
      AS
UPDATE BookAuthor 
SET [AuthorOrd] = @AuthorOrd
WHERE [ISBN] = @ISBN and AuthorId = @AuthorId
GO

CREATE FUNCTION dbo.AuthorOfBook (@ISBN BigInt) RETURNS Table AS
       RETURN 
SELECT ISBN, AuthorId AuId, AuthorOrd
FROM BookAuthor WHERE ISBN=@ISBN
GO

CREATE FUNCTION dbo.BookAuthorName (@ISBN BigInt) Returns Table AS
       RETURN
SELECT ISBN, Id AuthorId, AuthorOrd, FName + ' ' + LName AuthorName
FROM Author LEFT OUTER JOIN dbo.AuthorOfBook (@ISBN) On Id = AuId