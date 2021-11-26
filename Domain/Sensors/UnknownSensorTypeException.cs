using System;

namespace nostromo_muthur.Domain.Sensors
{
    public class UnknownSensorTypeException : ApplicationException
    {
        public UnknownSensorTypeException() : base() { }
        public UnknownSensorTypeException(string? message) : base(message) { }
        public UnknownSensorTypeException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
