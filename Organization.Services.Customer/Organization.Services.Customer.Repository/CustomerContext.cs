using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Organization.Services.Customer.Models;

namespace Organization.Services.Customer.Repository
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options)
        : base(options)
        { }

        public DbSet<Models.Customer> Customers { get; set; }
        public DbSet<CustomerContact> CustomerContacts { get; set; }
        public DbSet<CustomerNote> CustomerNotes { get; set; }
    }
}
