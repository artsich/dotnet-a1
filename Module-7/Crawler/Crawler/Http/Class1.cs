using System;

namespace Http
{

    public class HttpRequest
    {
        public RequestMessageHeader MessageHeader { get; set; }

        public Request BlankLine { get; set; }

        public RequestBody GetRequestBody { get; set; }
    }

    public class HttpResponse
    {

    }

}
