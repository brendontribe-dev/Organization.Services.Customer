using System;
using System.Collections.Generic;
using System.Text;

namespace Organization.Services.Customer.Models
{
    //TODO: Populate result fields
    //TODO: Object should be replaced with something better
    public interface IResult { }
    public class CustomerNoteSuccessResult : IResult
    {
        public string Message { get; set; }
        public object Payload { get; set; }
    }

    public class CustomerNoteFailureResult : IResult
    {
        public string Message { get; set; }
    }

    public class CustomerSuccessResult : IResult
    {
        public string Message { get; set; }
        public object Payload { get; set; }

    }

    public class CustomerFailureResult : IResult
    {
        public string Message { get; set; }
    }
}
