using System.ComponentModel.DataAnnotations;

namespace Users.Domain.Models
{
    public record User
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Nick { get; set; }

        public StateUserEnum State { get; set; }
    }
}