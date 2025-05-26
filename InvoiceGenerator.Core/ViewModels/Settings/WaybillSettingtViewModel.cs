using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InvoiceGenerator.Core.ViewModels.Settings
{
    public class WaybillSettingtViewModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Last Waybill Number")]
        public int LastWaybillNumber { get; set; }
    }
}