﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using InvoiceGenerator.Core.Models;

namespace InvoiceGenerator.Core.ViewModels.Invoices
{
    public class CreateInvoiceViewModel
    {        

        [Required]
        [Display(Name = "Client")]
        [StringLength(128)]
        public string ClientId { get; set; }

        [Required]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [Display(Name = "P/O Number")]
        [StringLength(75)]
        public string PoNumber { get; set; }

        [Display(Name = "Transporter P/O Number")]
        [StringLength(75)]
        public string TransporterPoNumber { get; set; }

        [Display(Name = "Driver")]
        [StringLength(75)]
        public string Driver { get; set; }

        [Display(Name = "Vehicle Registration Number")]
        [StringLength(75)]
        public string VehicleRegistration { get; set; }

        [Required]
        [Display(Name = "From Branch")]
        [StringLength(3)]
        public string BranchCode { get; set; }
        
        [Required]
        [Display(Name = "Status")]
        [StringLength(10)]
        public string Status { get; set; }
        
        [Display(Name = "Transporter")]
        [StringLength(128)]
        public string TransporterId { get; set; }

        public List<SelectListItem> StatusItems { get; } = new List<SelectListItem>
        {
                new SelectListItem {Text = "Pending", Value = "PD"},
                new SelectListItem {Text = "Delivered", Value = "DL"}
        };

        public List<CreateInvoiceItemViewModel> InvoiceItems { get; set; }

        public List<Branch> Branches { get; set; }
        public List<Product> Products { get; set; }
        
        public List<Transporter> Transporters { get; set; }

        public IEnumerable<Client> Clients { get; set; }
        
        public bool SaveAndDownload { get; set; }
        
        public bool HasSaved { get; set; }

        public CreateInvoiceViewModel()
        {
            InvoiceItems = new List<CreateInvoiceItemViewModel>();
        }

    }

    public class InputTags
    {
        public string value { get; set; }
    }
}