using System.Net;

namespace VillaAPI.utils
{
    public class AppError:Exception
    {
        public string status;
        public bool isOperational;
        public int statusCode;

        public AppError(string message = "Some thing Went Very Wrong", HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message)
        {
            this.statusCode = (int)statusCode;
            this.status = statusCode.ToString().StartsWith("4") ? "fail" : "error"; ;
            this.isOperational = true;
        }
    }
}
