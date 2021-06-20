using System;

namespace Organization.Services.Customer.Enumerables
{
    public enum CustomerNoteStatus
    {
        Backlog,
        InProgress,
        Abandoned,
        Completed,
        Deleted,
        CustomerDeleted
    }

    //TODO: create custom mappings in postgres
    /*
    public class CustomerNoteStatus
    {
        //TODO: Confirm with team the validity of status types
        public static readonly CustomerNoteStatus Backlog = new CustomerNoteStatus("Backlog");
        public static readonly CustomerNoteStatus InProgress = new CustomerNoteStatus("InProgress");
        public static readonly CustomerNoteStatus Abandoned = new CustomerNoteStatus("Abandoned");
        public static readonly CustomerNoteStatus Completed = new CustomerNoteStatus("Completed");
        public static readonly CustomerNoteStatus Deleted = new CustomerNoteStatus("Deleted");
        public static readonly CustomerNoteStatus CustomerDeleted = new CustomerNoteStatus("CustomerDeleted");

        public string Value { get; }

        CustomerNoteStatus(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public static CustomerNoteStatus FromString(string value)
        {
            return value switch
            {
                "Backlog" => Backlog,
                "InProgress" => InProgress,
                "Abandoned" => Abandoned,
                "Completed" => Completed,
                "Deleted" => Deleted,
                "CustomerDeleted" => CustomerDeleted,
                _ => throw new ArgumentException($"CustomerNoteStatus '{value}' was not recognised.", nameof(value))
            };
        }
    
    }

    */
}