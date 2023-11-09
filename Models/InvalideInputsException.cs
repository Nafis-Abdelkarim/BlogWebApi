namespace BlogWebApi.Models
{
    public class InvalideInputsException : Exception
    {
        public InvalideInputsException() { }

        public InvalideInputsException(string message) : base(message)
        {

        }

        public InvalideInputsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
