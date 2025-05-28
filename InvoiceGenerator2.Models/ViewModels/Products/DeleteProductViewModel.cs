using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator2.Models.ViewModels.Products
{
    public class DeleteProductViewModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Product Code")]
        [StringLength(75)]
        public string Code { get; set; }

        [Display(Name = "Product Name")]
        public string Name { get; set; }
    }
}