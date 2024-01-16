using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TopicDb.Domain.Models.QuestionAttachments;

namespace TopicDb.Domain.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TopicId { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string CorrectAnswer { get; set; }

        // Optional 
        public Picture? Picture { get; set; }

        // Optional 
        public Music? Music { get; set; }

        [Required]
        public int Price { get; set; }

        [ForeignKey("TopicId")]
        public Topic Topic { get; set; }
    }
}