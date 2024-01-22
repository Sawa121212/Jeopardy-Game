using System.ComponentModel.DataAnnotations;

namespace Users.Domain
{
    public record User
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Nick { get; set; }
        public StateUserEnum State { get; set; }
    }
}
