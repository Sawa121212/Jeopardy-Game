using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopicDb.Domain.Models.QuestionAttachments
{
    public class Music
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Required]
        public long ChatId { get; set; }

        [Required]
        public long MessageId { get; set; }

        [ForeignKey("QuestionId")]
        public Question Question { get; set; }
    }
}