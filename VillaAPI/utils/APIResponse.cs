using System.Net;

namespace VillaAPI.utils
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public List<string> ErrorsList { get; set; }
        public bool IsSuccess { get; set; } = true;
        public object Result { get; set; }
    }
}
