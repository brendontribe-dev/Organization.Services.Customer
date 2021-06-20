using System;

namespace Organization.Services.Customer.Enumerables
{
    public enum CustomerStatus
    {
        Prospective,
        Current,
        NonActive
    }
    /*
    public class CustomerStatus
    {
        
        //TODO: Confirm with team the validity of status types
        //If a customer is prospective they are not yet active and thus are also NonActive - is this me being pedantic? probably
        public static readonly CustomerStatus Prospective = new CustomerStatus("Prospective");
        public static readonly CustomerStatus Current = new CustomerStatus("Current");
        public static readonly CustomerStatus NonActive = new CustomerStatus("NonActive");
     
        public string Value { get; }

        CustomerStatus(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public static CustomerStatus FromString(string value)
        {
            return value switch
            {
                "Prospective" => Prospective,
                "Current" => Current,
                "Abandoned" => NonActive,
                _ => throw new ArgumentException($"CustomerStatus '{value}' was not recognised.", nameof(value))
            };
        }
    }   
            */
}
