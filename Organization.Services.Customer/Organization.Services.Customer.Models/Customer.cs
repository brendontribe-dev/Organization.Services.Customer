using Organization.Services.Customer.Enumerables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Organization.Services.Customer.Models
{
    //TODO: tidy up namespaces and class names to mitigate conflicts
    //Shouldnt need to use Models.Customer

    [Table("customer")]
    public class Customer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid CustomerId { get; set; }
        public CustomerStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PreferredName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
