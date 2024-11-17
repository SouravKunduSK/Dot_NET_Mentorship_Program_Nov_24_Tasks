using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP_Project.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice{ get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Stock{ get; set; }

        [JsonIgnore]
        public virtual List<Order>? Orders { get; set; }
    }
}
