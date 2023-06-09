USE [master]
GO
/****** Object:  Database [E-Library]    Script Date: 5/23/2023 4:37:47 PM ******/
CREATE DATABASE [E-Library]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'E-Library', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\E-Library.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'E-Library_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\E-Library_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [E-Library] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [E-Library].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [E-Library] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [E-Library] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [E-Library] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [E-Library] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [E-Library] SET ARITHABORT OFF 
GO
ALTER DATABASE [E-Library] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [E-Library] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [E-Library] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [E-Library] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [E-Library] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [E-Library] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [E-Library] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [E-Library] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [E-Library] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [E-Library] SET  DISABLE_BROKER 
GO
ALTER DATABASE [E-Library] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [E-Library] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [E-Library] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [E-Library] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [E-Library] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [E-Library] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [E-Library] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [E-Library] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [E-Library] SET  MULTI_USER 
GO
ALTER DATABASE [E-Library] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [E-Library] SET DB_CHAINING OFF 
GO
ALTER DATABASE [E-Library] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [E-Library] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [E-Library] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [E-Library] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [E-Library] SET QUERY_STORE = ON
GO
ALTER DATABASE [E-Library] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [E-Library]
GO
/****** Object:  Table [dbo].[Books]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Books](
	[bookId] [int] IDENTITY(1,1) NOT NULL,
	[authorId] [int] NOT NULL,
	[title] [varchar](100) NOT NULL,
	[Category] [varchar](100) NOT NULL,
	[Language] [varchar](100) NOT NULL,
	[publisher] [varchar](100) NOT NULL,
	[numberOfPages] [int] NOT NULL,
	[publicationDate] [date] NOT NULL,
	[isbn] [varchar](20) NOT NULL,
	[availableBooks] [int] NOT NULL,
	[totalBooksAvailable] [int] NOT NULL,
	[summary] [varchar](1000) NOT NULL,
 CONSTRAINT [PK__Books__8BE5A10DD759A2D1] PRIMARY KEY CLUSTERED 
(
	[bookId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_ISBN] UNIQUE NONCLUSTERED 
(
	[isbn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Title] UNIQUE NONCLUSTERED 
(
	[title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Authors]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Authors](
	[authorId] [int] IDENTITY(1,1) NOT NULL,
	[firstName] [varchar](50) NOT NULL,
	[lastName] [varchar](50) NOT NULL,
	[nationality] [varchar](50) NOT NULL,
	[dateOfBirth] [date] NOT NULL,
	[dateOfDeath] [date] NULL,
	[biography] [varchar](500) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[authorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_AuthorsWithBooks]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_AuthorsWithBooks]
AS
SELECT a.firstName, a.lastName, b.title
FROM Authors a
INNER JOIN Books b ON a.authorId = b.authorId;
GO
/****** Object:  Table [dbo].[Borrowings]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Borrowings](
	[borrowingId] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NOT NULL,
	[bookId] [int] NOT NULL,
	[borrowDate] [date] NOT NULL,
	[returnDate] [date] NULL,
	[dueDate] [date] NOT NULL,
	[renewalCount] [int] NULL,
	[notes] [varchar](500) NULL,
 CONSTRAINT [PK__Borrowin__9A6787405CC6C7B8] PRIMARY KEY CLUSTERED 
(
	[borrowingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_BooksDetail]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_BooksDetail]
AS
SELECT b.title, CONCAT(a.firstName, ' ', a.lastName) AS author, b.publisher, b.publicationDate,
       b.isbn, b.totalBooksAvailable - COUNT(DISTINCT br.borrowingId) AS availableBooks,
       COUNT(DISTINCT br.borrowingId) AS borrowedBooks
FROM Books b
INNER JOIN Authors a ON b.authorId = a.authorId
LEFT JOIN Borrowings br ON b.bookId = br.bookId AND br.returnDate IS NULL
GROUP BY b.title, CONCAT(a.firstName, ' ', a.lastName), b.publisher, b.publicationDate, b.isbn, b.totalBooksAvailable;
GO
/****** Object:  Table [dbo].[User]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[userId] [int] IDENTITY(1,1) NOT NULL,
	[firstName] [varchar](50) NOT NULL,
	[lastName] [varchar](50) NOT NULL,
	[email] [varchar](100) NOT NULL,
	[phoneNumber] [varchar](20) NOT NULL,
	[address] [varchar](200) NOT NULL,
	[dateOfBirth] [date] NOT NULL,
	[typeId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[userId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_BorrowersDetail]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_BorrowersDetail]
AS
SELECT u.firstName, u.lastName, u.email, u.phoneNumber, COUNT(DISTINCT br.borrowingId) AS borrowedBooks
FROM [User] u
INNER JOIN Borrowings br ON u.userId = br.userId AND br.returnDate IS NULL
GROUP BY u.firstName, u.lastName, u.email, u.phoneNumber;
GO
/****** Object:  View [dbo].[vw_ActiveBorrowings]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_ActiveBorrowings]
AS
SELECT CONCAT(u.firstName, ' ', u.lastName) AS borrowerName, b.title AS bookTitle, u.email, u.phoneNumber, br.dueDate
FROM Borrowings br
INNER JOIN Books b ON br.bookId = b.bookId
INNER JOIN [User] u ON br.userId = u.userId
WHERE br.returnDate IS NULL;
GO
/****** Object:  Table [dbo].[Login]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Login](
	[loginId] [int] IDENTITY(1,1) NOT NULL,
	[email] [varchar](255) NOT NULL,
	[passwordHash] [varbinary](max) NOT NULL,
	[PasswordSalt] [varbinary](max) NOT NULL,
	[lastLogin] [datetime] NULL,
	[lastLogout] [datetime] NULL,
	[currentToken] [varchar](max) NOT NULL,
	[UserId] [int] NULL,
 CONSTRAINT [PK__Login__1F5EF4CF35BC3DBA] PRIMARY KEY CLUSTERED 
(
	[loginId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rating]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rating](
	[RatingId] [int] IDENTITY(1,1) NOT NULL,
	[Feedback] [varchar](250) NULL,
	[StarCount] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[UserId] [int] NOT NULL,
	[BookId] [int] NOT NULL,
 CONSTRAINT [PK_Rating] PRIMARY KEY CLUSTERED 
(
	[RatingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usertype]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usertype](
	[typeId] [int] IDENTITY(1,1) NOT NULL,
	[typeName] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[typeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wishlist]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wishlist](
	[WishlistId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[BooksId] [int] NOT NULL,
 CONSTRAINT [PK_Wishlist] PRIMARY KEY CLUSTERED 
(
	[WishlistId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Borrowings]  WITH CHECK ADD  CONSTRAINT [FK__Borrowing__bookI__4BAC3F29] FOREIGN KEY([bookId])
REFERENCES [dbo].[Books] ([bookId])
GO
ALTER TABLE [dbo].[Borrowings] CHECK CONSTRAINT [FK__Borrowing__bookI__4BAC3F29]
GO
ALTER TABLE [dbo].[Borrowings]  WITH CHECK ADD  CONSTRAINT [FK__Borrowing__userI__4AB81AF0] FOREIGN KEY([userId])
REFERENCES [dbo].[User] ([userId])
GO
ALTER TABLE [dbo].[Borrowings] CHECK CONSTRAINT [FK__Borrowing__userI__4AB81AF0]
GO
ALTER TABLE [dbo].[Login]  WITH CHECK ADD  CONSTRAINT [FK__Login__Userid__71D1E811] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([userId])
GO
ALTER TABLE [dbo].[Login] CHECK CONSTRAINT [FK__Login__Userid__71D1E811]
GO
ALTER TABLE [dbo].[Rating]  WITH CHECK ADD  CONSTRAINT [FK_Rating_Books] FOREIGN KEY([BookId])
REFERENCES [dbo].[Books] ([bookId])
GO
ALTER TABLE [dbo].[Rating] CHECK CONSTRAINT [FK_Rating_Books]
GO
ALTER TABLE [dbo].[Rating]  WITH CHECK ADD  CONSTRAINT [FK_Rating_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([userId])
GO
ALTER TABLE [dbo].[Rating] CHECK CONSTRAINT [FK_Rating_User]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD FOREIGN KEY([typeId])
REFERENCES [dbo].[Usertype] ([typeId])
GO
ALTER TABLE [dbo].[Wishlist]  WITH CHECK ADD  CONSTRAINT [FK_Wishlist_Books] FOREIGN KEY([BooksId])
REFERENCES [dbo].[Books] ([bookId])
GO
ALTER TABLE [dbo].[Wishlist] CHECK CONSTRAINT [FK_Wishlist_Books]
GO
ALTER TABLE [dbo].[Wishlist]  WITH CHECK ADD  CONSTRAINT [FK_Wishlist_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([userId])
GO
ALTER TABLE [dbo].[Wishlist] CHECK CONSTRAINT [FK_Wishlist_User]
GO
ALTER TABLE [dbo].[Authors]  WITH CHECK ADD  CONSTRAINT [CHK_dateOfBirth_dateOfDeath] CHECK  (([dateOfBirth]<[dateOfDeath]))
GO
ALTER TABLE [dbo].[Authors] CHECK CONSTRAINT [CHK_dateOfBirth_dateOfDeath]
GO
ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [CHK_AvailableBooks] CHECK  (([AvailableBooks]>=(0)))
GO
ALTER TABLE [dbo].[Books] CHECK CONSTRAINT [CHK_AvailableBooks]
GO
ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [CHK_AvailableBooks_TotalBooksAvailable] CHECK  (([AvailableBooks]<=[TotalBooksAvailable]))
GO
ALTER TABLE [dbo].[Books] CHECK CONSTRAINT [CHK_AvailableBooks_TotalBooksAvailable]
GO
ALTER TABLE [dbo].[Borrowings]  WITH CHECK ADD  CONSTRAINT [CHK_BorrowDate_ReturnDate] CHECK  (([ReturnDate]>=[BorrowDate]))
GO
ALTER TABLE [dbo].[Borrowings] CHECK CONSTRAINT [CHK_BorrowDate_ReturnDate]
GO
ALTER TABLE [dbo].[Borrowings]  WITH CHECK ADD  CONSTRAINT [CHK_ReturnDate] CHECK  (([ReturnDate]<=getdate()))
GO
ALTER TABLE [dbo].[Borrowings] CHECK CONSTRAINT [CHK_ReturnDate]
GO
ALTER TABLE [dbo].[Rating]  WITH CHECK ADD  CONSTRAINT [CK_StarCount] CHECK  (([StarCount]>(0) AND [StarCount]<=(5)))
GO
ALTER TABLE [dbo].[Rating] CHECK CONSTRAINT [CK_StarCount]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [CHK_DateOfBirth] CHECK  ((datediff(year,[DateOfBirth],getdate())>(18)))
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [CHK_DateOfBirth]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [CHK_PhoneNumber] CHECK  (([PhoneNumber] like '[0-9]%[0-9]%[0-9]'))
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [CHK_PhoneNumber]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [CHK_PhoneNumber07] CHECK  (([PhoneNumber] like '07%'))
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [CHK_PhoneNumber07]
GO
/****** Object:  StoredProcedure [dbo].[BorrowBook]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BorrowBook](
@userId int,
@BookId int,
@BorrowDate datetime,
@DueDate datetime
)
as
begin
DECLARE @AvailableBooks int
select @AvailableBooks = AvailableBooks from Books where BookId=@BookId
if @AvailableBooks >0 
begin
insert into  Borrowings( userId ,BookId ,BorrowDate ,DueDate )
values(@userId ,@BookId ,@BorrowDate ,@DueDate)
update Books set AvailableBooks = @AvailableBooks -1 where BookId=@BookId
end
else
begin
RAISERROR('Book not available for borrowing', 16, 1)
end
end
GO
/****** Object:  StoredProcedure [dbo].[ReturnBook]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ReturnBook](
@BorrowingId int,
@ReturnDate datetime,
@Notes varchar(200)
)
as
begin
DECLARE @BookId int
select @BookId = BookId from Borrowings where BorrowingId = @BorrowingId
update Borrowings set ReturnDate = @ReturnDate , Notes = @Notes where BorrowingId = @BorrowingId
DECLARE @AvailableBooks int
DECLARE @TotalBooksAvailable int
select @AvailableBooks = AvailableBooks from Books where BookId=@BookId
select @TotalBooksAvailable = TotalBooksAvailable from Books where BookId=@BookId
if @AvailableBooks < @TotalBooksAvailable
begin
update Books set AvailableBooks =AvailableBooks +1 where BookId=@BookId
end
else
begin
RAISERROR('All books have already been imported', 16, 1)
end
end
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteAuthor]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteAuthor]
  @authorId INT
AS
BEGIN
  DELETE FROM Authors
  WHERE authorId = @authorId
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteBook]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteBook]
  @bookId INT
AS
BEGIN
  DELETE FROM Books
  WHERE bookId = @bookId
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteRating]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteRating]
    @ratingId INT
AS
BEGIN
    DELETE FROM Rating
    WHERE ratingId = @ratingId;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteUser]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteUser]
  @userId INT
AS
BEGIN
  DELETE FROM [User]
  WHERE userId = @userId
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertAuthor]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertAuthor]
  @firstName VARCHAR(50),
  @lastName VARCHAR(50),
  @nationality VARCHAR(50),
  @dateOfBirth DATE,
  @dateOfDeath DATE,
  @biography VARCHAR(500)
AS
BEGIN
  INSERT INTO Authors (firstName, lastName, nationality, dateOfBirth, dateOfDeath, biography)
  VALUES (@firstName, @lastName, @nationality, @dateOfBirth, @dateOfDeath, @biography)
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertBook]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertBook]
  @title VARCHAR(100),
  @authorId INT,
  @publisher VARCHAR(100),
  @publicationDate DATE,
  @isbn VARCHAR(20),
  @availableBooks INT,
  @totalBooksAvailable INT,
  @numberOfPages INT,
  @summary VARCHAR(500)
AS
BEGIN
  INSERT INTO Books (title, authorId, publisher, publicationDate, isbn, availableBooks, totalBooksAvailable, numberOfPages, summary)
  VALUES (@title, @authorId, @publisher, 
  @publicationDate, @isbn, @availableBooks, @totalBooksAvailable, @numberOfPages, @summary)
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertRating]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertRating]
    @feedback VARCHAR(250) = NULL,
    @starCount INT,
    @date DATE,
    @userId INT,
    @bookId INT
AS
BEGIN
    INSERT INTO Rating (feedback, starCount, date, userId, bookId)
    VALUES (@feedback, @starCount, @date, @userId, @bookId);
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertUser]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertUser]
  @firstName VARCHAR(50),
  @lastName VARCHAR(50),
  @email VARCHAR(100),
  @phoneNumber VARCHAR(20),
  @address VARCHAR(100),
  @dateOfBirth DATE
AS
BEGIN
  INSERT INTO [User] (firstName, lastName, email, phoneNumber, address, dateOfBirth)
  VALUES (@firstName, @lastName, @email, 
  @phoneNumber, @address, @dateOfBirth)
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_SearchBook]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_SearchBook]
  @author VARCHAR(50) = NULL,
  @title VARCHAR(100) = NULL,
  @isbn VARCHAR(20) = NULL,
  @publisher VARCHAR(50) = NULL
AS
BEGIN
  SELECT b.bookId, b.title, a.firstName + ' ' + a.lastName AS author,
         b.isbn, b.publisher, b.publicationDate, b.totalBooksAvailable, b.availableBooks,
         b.numberOfPages, b.summary
  FROM Books b
  INNER JOIN Authors a ON b.authorId = a.authorId
  WHERE (@author IS NULL OR a.firstName LIKE '%' + @author + '%' OR a.lastName LIKE '%' + @author + '%')
    AND (@title IS NULL OR b.title LIKE '%' + @title + '%')
    AND (@isbn IS NULL OR b.isbn LIKE '%' + @isbn + '%')
    AND (@publisher IS NULL OR b.publisher LIKE '%' + @publisher + '%')
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateAuthor]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateAuthor]
  @authorId INT,
  @firstName VARCHAR(50),
  @lastName VARCHAR(50),
  @nationality VARCHAR(50),
  @dateOfBirth DATE,
  @dateOfDeath DATE,
  @biography VARCHAR(500)
AS
BEGIN
  UPDATE Authors
  SET firstName = @firstName,
      lastName = @lastName,
      nationality = @nationality,
      dateOfBirth = @dateOfBirth,
      dateOfDeath = @dateOfDeath,
      biography = @biography
  WHERE authorId = @authorId
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateBook]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateBook]
  @bookId INT,
  @title VARCHAR(100),
  @authorId INT,
  @publisher VARCHAR(100),
  @publicationDate DATE,
  @isbn VARCHAR(20),
  @availableBooks INT,
  @totalBooksAvailable INT,
  @numberOfPages INT,
  @summary VARCHAR(1000)
AS
BEGIN
  UPDATE Books
  SET title = @title,
      authorId = @authorId,
      publisher = @publisher,
      publicationDate = @publicationDate,
      isbn = @isbn,
      availableBooks = @availableBooks,
      totalBooksAvailable = @totalBooksAvailable,
      numberOfPages = @numberOfPages,
      summary = @summary
  WHERE bookId = @bookId
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateRating]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateRating]
    @ratingId INT,
    @feedback VARCHAR(500),
    @starCount INT,
    @date DATE,
    @userId INT,
    @bookId INT
AS
BEGIN
    UPDATE Rating
    SET feedback = @feedback,
        starCount = @starCount,
        date = @date,
        userId = @userId,
        bookId = @bookId
    WHERE ratingId = @ratingId;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateUser]    Script Date: 5/23/2023 4:37:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateUser]
  @userId INT,
  @firstName VARCHAR(50),
  @lastName VARCHAR(50),
  @email VARCHAR(100),
  @phoneNumber VARCHAR(20),
  @address VARCHAR(100),
  @dateOfBirth DATE
AS
BEGIN
  UPDATE [User]
  SET firstName = @firstName,
      lastName = @lastName,
      email = @email,
      phoneNumber = @phoneNumber,
      address = @address,
      dateOfBirth = @dateOfBirth
  WHERE userId = @userId
END;
GO
USE [master]
GO
ALTER DATABASE [E-Library] SET  READ_WRITE 
GO
