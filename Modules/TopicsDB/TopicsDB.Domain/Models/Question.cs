using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopicDb.Domain.Models
{
    public class Question
    {
        [Key] public int Id { get; set; }
        public int TopicId { get; set; }

        public string Text { get; set; }
        public string PictureUrl { get; set; }
        public string MusicUrl { get; set; }
        public string CorrectAnswer { get; set; }

        [ForeignKey("TopicId")] 
        public Topic Topic { get; set; }
    }
}