using System;

namespace ContactInfoApp.Shared.Models
{
    public class CommentModel
    {
        public string Author { get; set; }

        public string AuthorImage { get; set; }

        public string Body { get; set; }

        public int Liked { get; set; }

        public int Disliked { get; set; }

        public DateTime Date { get; set; }
    }
}
