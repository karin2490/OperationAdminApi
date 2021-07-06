using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OperationAdminDB.Models;

#nullable disable

namespace OperationAdminDB.Data
{
    public partial class OperationAdminContext : DbContext
    {
        public OperationAdminContext()
        {
        }

        public OperationAdminContext(DbContextOptions<OperationAdminContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<ModuleByRole> ModuleByRoles { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<PermissionOnModuleByRole> PermissionOnModuleByRoles { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<TeamByUser> TeamByUsers { get; set; }
        public virtual DbSet<TeamLog> TeamLogs { get; set; }
        public virtual DbSet<Token> Tokens { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }

      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ClientName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.OperationResp)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Teams");
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.Property(e => e.ModuleDescrip).IsRequired();
            });

            modelBuilder.Entity<ModuleByRole>(entity =>
            {
                entity.ToTable("ModuleByRole");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.ModuleByRoles)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModuleByRole_Modules");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.ModuleByRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModuleByRole_Roles");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("Permission");

                entity.Property(e => e.PermissionDescrip)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<PermissionOnModuleByRole>(entity =>
            {
                entity.HasKey(e => e.PermissionOnModuleByRoleId)
                    .HasName("PK__Permissi__B6444C34ECB8FCE9");

                entity.ToTable("PermissionOnModuleByRole");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.PermissionOnModuleByRoles)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PermissionOnModuleByRole_Modules");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.PermissionOnModuleByRoles)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PermissionOnModuleByRole_Permission");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.PermissionOnModuleByRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PermissionOnModuleByRole_Roles");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleDescrip)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.Property(e => e.TeamName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TeamByUser>(entity =>
            {
                entity.ToTable("TeamByUser");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamByUsers)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamByUser_Teams");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TeamByUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamByUser_Users");
            });

            modelBuilder.Entity<TeamLog>(entity =>
            {
                entity.ToTable("TeamLog");

                entity.Property(e => e.DateActivity).HasColumnType("datetime");

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.TeamName).IsRequired();

                entity.Property(e => e.UserName).IsRequired();

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamLogs)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamLog_Teams");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TeamLogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamLog_Users");
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.AdmissionDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PassEncrypted).IsRequired();

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Users_Account");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Roles");
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(e => e.ProfileId)
                    .HasName("PK__UserProf__290C88E459BFA058");

                entity.ToTable("UserProfile");

                entity.Property(e => e.EnglishLevel).HasMaxLength(10);

                entity.Property(e => e.LinkCv).HasColumnName("LinkCV");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserProfiles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProfile_Users");
            });

            
        }

       
    }
}
