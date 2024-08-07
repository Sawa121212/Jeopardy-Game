﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TopicDb.Domain.Models
{
    public class Topic
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Optional
        public List<Question>? Questions { get; set; }
    }
}