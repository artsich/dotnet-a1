using System.Runtime.Serialization;

namespace DevTool.Entrities
{
    [DataContract]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
