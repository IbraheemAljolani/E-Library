using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace E_Library.Models
{
    public partial class ELibraryContext : DbContext
    {
        public ELibraryContext()
        {
        }

        public ELibraryContext(DbContextOptions<ELibraryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<Borrowing> Borrowings { get; set; } = null!;
        public virtual DbSet<Login> Logins { get; set; } = null!;
        public virtual DbSet<Rating> Ratings { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Usertype> Usertypes { get; set; } = null!;
        public virtual DbSet<VwActiveBorrowing> VwActiveBorrowings { get; set; } = null!;
        public virtual DbSet<VwAuthorsWithBook> VwAuthorsWithBooks { get; set; } = null!;
        public virtual DbSet<VwBooksDetail> VwBooksDetails { get; set; } = null!;
        public virtual DbSet<VwBorrowersDetail> VwBorrowersDetails { get; set; } = null!;
        public virtual DbSet<Wishlist> Wishlists { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=BUTCHER\\SQLEXPRESS;Database=E-Library;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.AuthorId).HasColumnName("authorId");

                entity.Property(e => e.Biography)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("biography");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("date")
                    .HasColumnName("dateOfBirth");

                entity.Property(e => e.DateOfDeath)
                    .HasColumnType("date")
                    .HasColumnName("dateOfDeath");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lastName");

                entity.Property(e => e.Nationality)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nationality");
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasIndex(e => e.Isbn, "UQ_ISBN")
                    .IsUnique();

                entity.HasIndex(e => e.Title, "UQ_Title")
                    .IsUnique();

                entity.Property(e => e.BookId).HasColumnName("bookId");

                entity.Property(e => e.AuthorId).HasColumnName("authorId");

                entity.Property(e => e.AvailableBooks).HasColumnName("availableBooks");

                entity.Property(e => e.Category)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Isbn)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("isbn");

                entity.Property(e => e.Language)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumberOfPages).HasColumnName("numberOfPages");

                entity.Property(e => e.PublicationDate)
                    .HasColumnType("date")
                    .HasColumnName("publicationDate");

                entity.Property(e => e.Publisher)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("publisher");

                entity.Property(e => e.Summary)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("summary");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.Property(e => e.TotalBooksAvailable).HasColumnName("totalBooksAvailable");
            });

            modelBuilder.Entity<Borrowing>(entity =>
            {
                entity.Property(e => e.BorrowingId).HasColumnName("borrowingId");

                entity.Property(e => e.BookId).HasColumnName("bookId");

                entity.Property(e => e.BorrowDate)
                    .HasColumnType("date")
                    .HasColumnName("borrowDate");

                entity.Property(e => e.DueDate)
                    .HasColumnType("date")
                    .HasColumnName("dueDate");

                entity.Property(e => e.Notes)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("notes");

                entity.Property(e => e.RenewalCount).HasColumnName("renewalCount");

                entity.Property(e => e.ReturnDate)
                    .HasColumnType("date")
                    .HasColumnName("returnDate");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Borrowings)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Borrowing__bookI__4BAC3F29");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Borrowings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Borrowing__userI__4AB81AF0");
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.ToTable("Login");

                entity.Property(e => e.LoginId).HasColumnName("loginId");

                entity.Property(e => e.CurrentToken)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("currentToken");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.LastLogin)
                    .HasColumnType("datetime")
                    .HasColumnName("lastLogin");

                entity.Property(e => e.LastLogout)
                    .HasColumnType("datetime")
                    .HasColumnName("lastLogout");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Logins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Login__Userid__71D1E811");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.ToTable("Rating");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Feedback)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rating_Books");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rating_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.Address)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("date")
                    .HasColumnName("dateOfBirth");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lastName");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("phoneNumber");

                entity.Property(e => e.TypeId).HasColumnName("typeId");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User__typeId__4316F928");
            });

            modelBuilder.Entity<Usertype>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK__Usertype__F04DF13A7EEB6226");

                entity.ToTable("Usertype");

                entity.Property(e => e.TypeId).HasColumnName("typeId");

                entity.Property(e => e.TypeName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("typeName");
            });

            modelBuilder.Entity<VwActiveBorrowing>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_ActiveBorrowings");

                entity.Property(e => e.BookTitle)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("bookTitle");

                entity.Property(e => e.BorrowerName)
                    .HasMaxLength(101)
                    .IsUnicode(false)
                    .HasColumnName("borrowerName");

                entity.Property(e => e.DueDate)
                    .HasColumnType("date")
                    .HasColumnName("dueDate");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("phoneNumber");
            });

            modelBuilder.Entity<VwAuthorsWithBook>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_AuthorsWithBooks");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lastName");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<VwBooksDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_BooksDetail");

                entity.Property(e => e.Author)
                    .HasMaxLength(101)
                    .IsUnicode(false)
                    .HasColumnName("author");

                entity.Property(e => e.AvailableBooks).HasColumnName("availableBooks");

                entity.Property(e => e.BorrowedBooks).HasColumnName("borrowedBooks");

                entity.Property(e => e.Isbn)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("isbn");

                entity.Property(e => e.PublicationDate)
                    .HasColumnType("date")
                    .HasColumnName("publicationDate");

                entity.Property(e => e.Publisher)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("publisher");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<VwBorrowersDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_BorrowersDetail");

                entity.Property(e => e.BorrowedBooks).HasColumnName("borrowedBooks");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lastName");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("phoneNumber");
            });

            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.ToTable("Wishlist");

                entity.HasOne(d => d.Books)
                    .WithMany(p => p.Wishlists)
                    .HasForeignKey(d => d.BooksId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Wishlist_Books");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Wishlists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Wishlist_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
