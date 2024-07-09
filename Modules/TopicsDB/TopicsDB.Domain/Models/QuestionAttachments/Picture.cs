using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopicDb.Domain.Models.QuestionAttachments
{
    public class Picture
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PictureQuestionId { get; set; }

        [Required]
        public long ChatId { get; set; }

        [Required]
        public long MessageId { get; set; }

        [ForeignKey("PictureQuestionId")]
        public Question Question { get; set; }
    }
}