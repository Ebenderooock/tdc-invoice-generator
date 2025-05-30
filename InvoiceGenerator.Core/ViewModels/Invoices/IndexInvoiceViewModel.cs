﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using InvoiceGenerator.Core.Models;

namespace InvoiceGenerator.Core.ViewModels.Invoices
{
    public class IndexInvoiceViewModel
    {
        [Display(Name = "Id")]
        [StringLength(128)]
        public string Id { get; set; }

        public Client Client { get; set; }

        [Required]
        [Display(Name = "Client")]
        [StringLength(128)]
        public string ClientId { get; set; }

        [Required]
        [Display(Name = "Invoice Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]

        public DateTime InvoiceDate { get; set; }

        [Required]
        [Display(Name = "P/O NUmber")]
        [StringLength(75)]
        public string PoNumber { get; set; }

        [Display(Name = "General Waybill Number")]
        [StringLength(75)]
        public string GeneralWaybillNumber { get; set; }

        [Required]
        [Display(Name = "Branch")]
        [StringLength(3)]
        public string BranchCode { get; set; }
        
        [Required]
        [Display(Name = "Branch")]
        public string BranchName { get; set; }

        [Required]
        [Display(Name = "Status")]
        [StringLength(2)]
        public string Status { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Created User")]
        [StringLength(75)]
        public string CreatedUser { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Modified User")]
        [StringLength(75)]
        public string ModifiedUser { get; set; }

        public DateTime? DeletedDate { get; set; }

        [StringLength(75)]
        public string DeletedUser { get; set; }

        public bool IsDeleted { get; set; }

        public List<InvoiceItem> InvoiceItems { get; set; }
    }
}