using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Infrastructure.PasswordHashers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Events_WEB_APP.Persistence.Data
{
    public class EventsAppDbContext : DbContext
    {
        public EventsAppDbContext(DbContextOptions<EventsAppDbContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(categoryBuilder =>
            {
                categoryBuilder.ToTable("Categories")
                    .HasKey(c => c.Id);
                categoryBuilder.Property(c => c.Id)
                    .HasColumnName("CategoryID");
                categoryBuilder.Property(c => c.Name)
                    .HasColumnName("Name")
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Event>(eventBuilder =>
            {
                eventBuilder.ToTable("Events")
                    .HasKey(e =>  e.Id);
                eventBuilder.Property(e => e.Id)
                    .HasColumnName("EventID");
                eventBuilder.Property(e => e.Name)
                    .HasColumnName("Name")
                    .HasMaxLength(100);
                eventBuilder.Property(e => e.Description)
                    .HasColumnName("Description")
                    .HasMaxLength(500);
                eventBuilder.Property(e => e.Date)
                    .HasColumnName("Date");
                eventBuilder.Property(e => e.Location)
                    .HasColumnName("Location")
                    .HasMaxLength(50);
                eventBuilder.Property(e => e.CategoryId)
                    .HasColumnName("CategoryID");
                eventBuilder.HasOne(e => e.Category)
                    .WithMany()
                    .HasForeignKey(e => e.CategoryId);
                eventBuilder.Property(e => e.MaxNumOfParticipants)
                    .HasColumnName("MaxNumOfParticipants")
                    .HasMaxLength(10);
                eventBuilder.Property(e => e.ImagePath)
                    .HasColumnName("Image")
                    .HasMaxLength(50);
            });
            
            modelBuilder.Entity<Participant>(participantBuilder =>
            {
                participantBuilder.ToTable("Participants")
                    .HasKey(e => e.Id);
                participantBuilder.Property(p => p.Id)
                    .HasColumnName("ParticipantID");
                participantBuilder.HasOne(p => p.User)
                    .WithMany(u => u.Participants)
                    .HasForeignKey(p => p.UserId);
                participantBuilder.Property(p => p.UserId)
                    .HasColumnName("UserId");
                participantBuilder.Property(p => p.FirstName)
                    .HasColumnName("FirstName")
                    .HasMaxLength(20);
                participantBuilder.Property(p => p.LastName)
                    .HasColumnName("LastName")
                    .HasMaxLength(20);
                participantBuilder.Property(p => p.BirthDate)
                    .HasColumnName("BirthDate");
                participantBuilder.Property(p => p.Email)
                    .HasColumnName("Email")
                    .HasMaxLength(254);
            });

            modelBuilder.Entity<User>(userBuilder =>
            {
                userBuilder.ToTable("Users")
                    .HasKey(e => e.Id);
                userBuilder.Property(u => u.Id)
                    .HasColumnName("UserID");
                userBuilder.Property(u => u.UserName)
                    .HasColumnName("UserName")
                    .HasMaxLength(20);
                userBuilder.Property(u => u.Email)
                    .HasColumnName("Email")
                    .HasMaxLength(254);
                userBuilder.Property(u => u.PasswordHash)
                    .HasColumnName("PasswordHash");
                userBuilder.Property(u => u.RefreshToken)
                    .HasColumnName("RefreshToken");
                userBuilder.HasMany(u => u.Participants)
                    .WithOne(p => p.User)
                    .HasForeignKey(p => p.UserId);
                userBuilder.HasOne(u => u.Role)
                    .WithMany()
                    .HasForeignKey(u => u.RoleId);
            });

            modelBuilder.Entity<Role>(roleBuilder =>
            {
                roleBuilder.ToTable("Roles")
                    .HasKey(r => r.Id);
                roleBuilder.Property(r => r.Id)
                    .HasColumnName("RoleId");
                roleBuilder.Property(r => r.Name)
                    .HasColumnName("Name")
                    .HasMaxLength(20);
            });

            SeedInitialData(modelBuilder);
        }

        private void SeedInitialData(ModelBuilder modelBuilder)
        {
            var adminRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = "Admin"
            };
            var userRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = "User"
            };
            modelBuilder.Entity<Role>().HasData(adminRole, userRole);

            IPasswordHasher passwordHasher = new PasswordHasher();
            var hashedPassword = passwordHasher.Generate("Admin");

            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                UserName = "admin",
                Email = "admin@events.com",
                PasswordHash = hashedPassword,
                RoleId = adminRole.Id,
                RefreshToken = null,
                RefreshTokenExpiry = DateTime.UtcNow.AddYears(1)
            };

            modelBuilder.Entity<User>().HasData(adminUser);
        }

        public void BeginTeansaction()
        {
            Database.BeginTransaction();
        }
        public void CommitTransaction()
        {
            Database.CommitTransaction();
        }
        public void RollbackTransaction()
        {
            Database.RollbackTransaction();
        }
    }
}
