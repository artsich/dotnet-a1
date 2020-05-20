using System.Collections;
using System.Collections.Generic;

namespace Http
{
    public class RequestMessageHeader
    {
        public RequestLine RequestLine { get; set; }

        public IDictionary<string, object> Headers { get; set; }

        public string Version { get; set; }
    }
}
