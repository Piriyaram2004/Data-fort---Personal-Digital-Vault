using Microsoft.EntityFrameworkCore;
using PersonalDigitalVault.API.Models;

namespace PersonalDigitalVault.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ==============================
        // DbSets - 8 Core Tables
        // ==============================

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Credential> Credentials { get; set; }
        public DbSet<ShareLink> ShareLinks { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==============================
            // ROLE
            // ==============================

            modelBuilder.Entity<Role>()
                .HasKey(r => r.RoleId);

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.RoleName)
                .IsUnique();


            // ==============================
            // USER
            // ==============================

            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);


            // ==============================
            // PASSWORD RESET TOKEN
            // ==============================

            modelBuilder.Entity<PasswordResetToken>()
                .HasKey(t => t.TokenId);

            modelBuilder.Entity<PasswordResetToken>()
                .HasOne(t => t.User)
                .WithMany(u => u.PasswordResetTokens)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.NoAction);


            // ==============================
            // FOLDER
            // ==============================

            modelBuilder.Entity<Folder>()
                .HasKey(f => f.FolderId);

            modelBuilder.Entity<Folder>()
                .HasOne(f => f.User)
                .WithMany(u => u.Folders)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Self-reference
            modelBuilder.Entity<Folder>()
                .HasOne(f => f.ParentFolder)
                .WithMany(f => f.SubFolders)
                .HasForeignKey(f => f.ParentFolderId)
                .OnDelete(DeleteBehavior.NoAction);

            // Root folder duplicate prevention
            modelBuilder.Entity<Folder>()
                .HasIndex(f => new
                {
                    f.UserId,
                    f.NormalizedFolderName
                })
                .IsUnique()
                .HasFilter("[ParentFolderId] IS NULL AND [IsDeleted] = 0");

            // Child folder duplicate prevention
            modelBuilder.Entity<Folder>()
                .HasIndex(f => new
                {
                    f.UserId,
                    f.ParentFolderId,
                    f.NormalizedFolderName
                })
                .IsUnique()
                .HasFilter("[ParentFolderId] IS NOT NULL AND [IsDeleted] = 0");


            // ==============================
            // DOCUMENT
            // ==============================

            modelBuilder.Entity<Document>()
                .HasKey(d => d.DocumentId);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.User)
                .WithMany(u => u.Documents)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.Folder)
                .WithMany(f => f.Documents)
                .HasForeignKey(d => d.FolderId)
                .OnDelete(DeleteBehavior.NoAction);

            // Root document duplicate prevention
            modelBuilder.Entity<Document>()
                .HasIndex(d => new
                {
                    d.UserId,
                    d.NormalizedFileName
                })
                .IsUnique()
                .HasFilter("[FolderId] IS NULL AND [IsDeleted] = 0");

            // Document inside folder duplicate prevention
            modelBuilder.Entity<Document>()
                .HasIndex(d => new
                {
                    d.UserId,
                    d.FolderId,
                    d.NormalizedFileName
                })
                .IsUnique()
                .HasFilter("[FolderId] IS NOT NULL AND [IsDeleted] = 0");


            // ==============================
            // CREDENTIAL
            // ==============================

            modelBuilder.Entity<Credential>()
                .HasKey(c => c.CredentialId);

            modelBuilder.Entity<Credential>()
                .HasOne(c => c.User)
                .WithMany(u => u.Credentials)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Credential>()
                .HasOne(c => c.Folder)
                .WithMany(f => f.Credentials)
                .HasForeignKey(c => c.FolderId)
                .OnDelete(DeleteBehavior.NoAction);


            // ==============================
            // SHARE LINK
            // ==============================

            modelBuilder.Entity<ShareLink>()
                .HasKey(s => s.ShareLinkId);

            modelBuilder.Entity<ShareLink>()
                .HasOne(s => s.User)
                .WithMany(u => u.ShareLinks)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ShareLink>()
                .HasOne(s => s.Document)
                .WithMany(d => d.ShareLinks)
                .HasForeignKey(s => s.DocumentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ShareLink>()
                .HasIndex(s => s.ShareToken)
                .IsUnique();


            // ==============================
            // AUDIT LOG
            // ==============================

            modelBuilder.Entity<AuditLog>()
                .HasKey(a => a.AuditLogId);

            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}