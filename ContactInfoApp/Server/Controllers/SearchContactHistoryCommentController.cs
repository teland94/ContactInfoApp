using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactInfoApp.Server.Persistence;
using ContactInfoApp.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactInfoApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchContactHistoryCommentController : ControllerBase
    {
        private AppDbContext DbContext { get; }

        public SearchContactHistoryCommentController(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }

        [HttpGet(nameof(GetBySearchContactHistoryId))]
        public Task<List<SearchContactHistoryCommentModel>> GetBySearchContactHistoryId(int searchContactHistoryId)
        {
            return DbContext.SearchContactHistoryComments
                .Where(c => c.SearchContactHistoryId == searchContactHistoryId)
                .Select(c => new SearchContactHistoryCommentModel
                {
                    Author = c.Author,
                    AuthorImage = c.AuthorImage,
                    Body = c.Body,
                    Liked = c.Liked,
                    Disliked = c.Disliked,
                    Date = c.Date,
                    SearchContactHistoryId = c.SearchContactHistoryId
                })
                .ToListAsync();
        }
    }
}
