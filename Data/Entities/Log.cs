namespace Data.Entities
{
    public class Log
    {
        public Log(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
