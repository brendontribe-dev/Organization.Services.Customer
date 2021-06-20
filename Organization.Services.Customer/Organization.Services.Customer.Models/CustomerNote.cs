using Organization.Services.Customer.Enumerables;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organization.Services.Customer.Models
{
    [Table("customer_note")]
    public class CustomerNote
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid NoteId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime AuthoredDateTime { get; set; }

        //TODO: AuthoredBy should be added for identification purposes
        //Guid == StaffId
        //public Guid AuthoredBy { get; set; }

        //TODO: Should have tags to indicate meaning of a note
        //e.g. is this a financial issue?
        //should the customer be followed up?
        //is this going to be blocking?
        //what implications are there if a note is not resolved?
        //informational or is there a requirement?
        //use noSQL DB to store the notes info in future due to flexibility of definition.
        //need far more info than some FTF to perform meaningful analytics

        public string Contents { get; set; }
        public CustomerNoteStatus Status { get; set; }
    }
}
