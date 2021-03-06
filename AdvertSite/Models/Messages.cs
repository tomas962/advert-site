﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvertSite.Models
{
    [DisplayName("pranešimas")]
    public partial class Messages
    {
        public Messages()
        {
            UsersHasMessages = new HashSet<UsersHasMessages>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Tema")]
        [Required]
        [MaxLength(100)]
        public string Subject { get; set; }
        [DisplayName("Pranešimas")]
        [Required]
        [MaxLength(1000)]
        public string Text { get; set; }
        public DateTime DateSent { get; set; }

        public ICollection<UsersHasMessages> UsersHasMessages { get; set; }
    }
}
