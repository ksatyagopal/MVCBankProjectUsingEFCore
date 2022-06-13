using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MVCBankProjectUsingEFCore.Models
{
    public partial class SharpBankContext : DbContext
    {
        public SharpBankContext()
        {
        }

        public SharpBankContext(DbContextOptions<SharpBankContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Authenticate> Authenticates { get; set; }
        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        public virtual DbSet<MoneyTransaction> MoneyTransactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SharpBank;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Authenticate>(entity =>
            {
                entity.HasKey(e => e.UserName)
                    .HasName("PK__Authenti__C9F28457C980068F");

                entity.ToTable("Authenticate");

                entity.Property(e => e.UserName)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("password");
            });

            modelBuilder.Entity<BankAccount>(entity =>
            {
                entity.HasKey(e => e.AccountNumber)
                    .HasName("PK__BankAcco__BE2ACD6E284A30C9");

                entity.Property(e => e.AccountHolderName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfCreation).HasColumnType("datetime");

                entity.Property(e => e.TypeOfAccount)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MoneyTransaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId)
                    .HasName("PK__MoneyTra__55433A4B44E5E36A");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.TaccountNumber).HasColumnName("TAccountNumber");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.HasOne(d => d.TaccountNumberNavigation)
                    .WithMany(p => p.MoneyTransactions)
                    .HasForeignKey(d => d.TaccountNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MoneyTran__TAcco__3A81B327");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
