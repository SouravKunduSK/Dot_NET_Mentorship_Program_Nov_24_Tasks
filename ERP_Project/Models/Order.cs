using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ERP_Project.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;

namespace ERP_Project.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Customer Name is required!")]
        public string CustomerName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity {  get; set; }
        public DateTime? OrderDate { get; set; }

        [Required]
        public int ProductId {  get; set; }

        [JsonIgnore]
        public virtual Product? Product { get; set; }
    }
}
