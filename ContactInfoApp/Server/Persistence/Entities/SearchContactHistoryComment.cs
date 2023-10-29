using System;

namespace ContactInfoApp.Server.Persistence.Entities
{
    public class SearchContactHistoryComment : EntityBase
    {
        public string Author { get; set; }

        public string AuthorImage { get; set; }

        public string Body { get; set; }

        public int Liked { get; set; }

        public int Disliked { get; set; }

        public DateTime Date { get; set; }

        public SearchContactHistory SearchContactHistory { get; set; }
        public int SearchContactHistoryId { get; set; }
    }
}
