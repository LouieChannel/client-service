using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ascalon.ClientService.DataBaseContext
{
    [Table("users")]
    public partial class User
    {
        public User()
        {
            TaskDrivers = new HashSet<Task>();
            TaskLogists = new HashSet<Task>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("login")]
        [StringLength(50)]
        public string Login { get; set; }
        [Required]
        [Column("password")]
        [StringLength(50)]
        public string Password { get; set; }
        [Column("role_id")]
        public int RoleId { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(150)]
        public string FullName { get; set; }
        [Column("dumper_id")]
        public int? DumperId { get; set; }

        [ForeignKey(nameof(RoleId))]
        [InverseProperty("Users")]
        public virtual Role Role { get; set; }
        [InverseProperty(nameof(Task.Driver))]
        public virtual ICollection<Task> TaskDrivers { get; set; }
        [InverseProperty(nameof(Task.Logist))]
        public virtual ICollection<Task> TaskLogists { get; set; }
    }
}
