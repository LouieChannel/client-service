using Microsoft.EntityFrameworkCore;

namespace Ascalon.ClientService.DataBaseContext
{
    public partial class ClientServiceContext : DbContext
    {
        public ClientServiceContext()
        {
        }

        public ClientServiceContext(DbContextOptions<ClientServiceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Name=ClientService");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasComment("Список ролей для клиентов");

                entity.Property(e => e.Name).HasComment("Наименование роли");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.HasComment("Список задач");

                entity.Property(e => e.CreatedAt).HasComment("Дата и время создания задания");

                entity.Property(e => e.Description).HasComment("Описание задания");

                entity.Property(e => e.DriverId).HasComment("Идентификатор водителя");

                entity.Property(e => e.EndLatitude).HasComment("Широта конца задания");

                entity.Property(e => e.EndLongitude).HasComment("Долгота конца задания");

                entity.Property(e => e.Entity).HasComment("Основное задание");

                entity.Property(e => e.StartLatitude).HasComment("Широта начала задания");

                entity.Property(e => e.StartLongitude).HasComment("Долгота начала задания");

                entity.Property(e => e.Status).HasComment("Статус задания");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.DriverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tasks_driver_id_fk");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasComment("Список пользователей с их данными");

                entity.Property(e => e.FullName).HasComment("ФИО пользователя");

                entity.Property(e => e.Login).HasComment("Идентификатор для авторизации пользователя");

                entity.Property(e => e.Password).HasComment("Пароль идентификатора пользователя");

                entity.Property(e => e.RoleId).HasComment("Роль пользователя в системе");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("users_role_id_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
