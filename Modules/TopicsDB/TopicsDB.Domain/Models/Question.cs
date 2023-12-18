using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopicDb.Domain.Models
{
    public class Question
    {
        [Key] public int Id { get; set; }
        public int TopicId { get; set; }

        public string ChatId { get; set; }
        public string MessageId { get; set; }
        public string CorrectAnswer { get; set; }
        public int Price { get; set; }

        [ForeignKey("TopicId")] 
        public Topic Topic { get; set; }
    }
}