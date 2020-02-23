using System.Web.Mvc;

namespace Sandbox
{
    [Authorize]
    public class RightController : Controller
    {
    }

    public class WrongControl : Controller
    {
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
