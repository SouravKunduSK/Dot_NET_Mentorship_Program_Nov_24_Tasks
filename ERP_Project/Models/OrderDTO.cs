using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_Project.Models
{
    public class OrderDTO
    {
        [Required(ErrorMessage = "Product Id is required!")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Customer Name is required!")]
        public string CustomerName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Required(ErrorMessage = "Quantity is required!")]
        public decimal Quantity { get; set; }
    }
}
