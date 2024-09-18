﻿using System.ComponentModel.DataAnnotations.Schema;
using RespectCounter.Domain.Interfaces;

namespace RespectCounter.Domain.Model
{
    public class Comment : Entity
    {
        public string Content { get; set; } = string.Empty;
        
        public Guid? ActivityId { get; set; }
        public virtual Activity? Activity { get; set; }
        public Guid? PersonId { get; set; }
        public virtual Person? Person { get; set; }
        [ForeignKey("Comment")]
        public Guid? ParentId { get; set; }
        public virtual Comment? Parent { get; set; }
        public virtual List<Reaction> Reactions { get; set; } = new();
        public virtual List<Comment> Children { get; set; } = new();
    }
}
