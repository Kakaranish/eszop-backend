using System;

namespace Common.Utilities.Exceptions
{
    public class NotFoundException : Exception
    {
        private const string BaseMessage = "NOT FOUND";

        public NotFoundException() : base(BaseMessage)
        {
        }

        public NotFoundException(string? message) : base($"{BaseMessage}: {message}")
        {
        }
    }
}
