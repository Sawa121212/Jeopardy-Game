using System.ComponentModel.DataAnnotations;

namespace Users.Domain
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public StateEnum State { get; set; }
    }
}
