using Organization.Services.Customer.Enumerables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Organization.Services.Customer.Models
{
    [Table("customer_contact")]

    public class CustomerContact
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ContactId { get; set; }
        public Guid CustomerId { get; set; }
        public CustomerContactType ContactType { get; set; }
        public string ContactEntry { get; set; }
        public bool IsPrimaryContact { get; set; }
        public bool RequiresValidation { get; set; }
    }
}
