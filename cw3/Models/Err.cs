using Newtonsoft.Json;

namespace cw3.Models
{
    public class Err
    {
        public int StatusCode { get; set; }


        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
