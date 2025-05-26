using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TDC_Invoice_Generator.Models
{
    public class ClientBranch
    {
        [Key]
        [Display(Name = "Id")]
        [StringLength(128)]
        [Required]
        public string Id { get; set; }

        [ForeignKey("Client")]
        [Display(Name = "ClientId")]
        [StringLength(128)]
        [Required]
        public string ClientId { get; set; }
        [ForeignKey("Branch")]
        [Display(Name = "BranchId")]
        [StringLength(128)]
        [Required]
        public string BranchId { get; set; }



        public Branch Branch { get; set; }
        public Client Client{ get; set; }
    }
}