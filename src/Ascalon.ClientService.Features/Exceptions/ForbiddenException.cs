using System;

namespace Ascalon.ClientService.Features.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException() : base() { }

        public ForbiddenException(string message) : base(message) { }
    }

    public class ForbiddenResponse
    {
        public string Message { get; set; }
    }
}
