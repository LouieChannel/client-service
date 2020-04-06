using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ascalon.ClientService.DataBaseContext
{
    [Table("tasks")]
    public partial class Task
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("driver_id")]
        public int DriverId { get; set; }
        [Required]
        [Column("description")]
        [StringLength(100)]
        public string Description { get; set; }
        [Column("start_longitude")]
        public float StartLongitude { get; set; }
        [Column("start_latitude")]
        public float StartLatitude { get; set; }
        [Column("end_longitude")]
        public float EndLongitude { get; set; }
        [Column("end_latitude")]
        public float EndLatitude { get; set; }
        [Column("status")]
        public short Status { get; set; }
        [Required]
        [Column("entity")]
        [StringLength(100)]
        public string Entity { get; set; }
        [Column("created_at", TypeName = "timestamp with time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("logist_id")]
        public int LogistId { get; set; }

        [ForeignKey(nameof(DriverId))]
        [InverseProperty(nameof(User.TaskDrivers))]
        public virtual User Driver { get; set; }
        [ForeignKey(nameof(LogistId))]
        [InverseProperty(nameof(User.TaskLogists))]
        public virtual User Logist { get; set; }
    }
}
