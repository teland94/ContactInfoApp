﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ContactInfoApp.Server.Persistence;
using ContactInfoApp.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactInfoApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchContactHistoryController : ControllerBase
    {
        private AppDbContext DbContext { get; }

        public SearchContactHistoryController(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<SearchContactHistoryModel>> Get()
        {
            var searchContactHistoryItems = await DbContext.SearchContactHistory
                .OrderByDescending(sch => sch.Date)
                .ToListAsync();
            return searchContactHistoryItems.Select(sch => new SearchContactHistoryModel
            {
                Id = sch.Id,
                Date = sch.Date,
                IpAddress = sch.IpAddress,
                PhoneNumber = sch.PhoneNumber,
                DisplayName = sch.DisplayName,
                IsSpam = sch.IsSpam,
                Tags = sch.Tags != null ? JsonSerializer.Deserialize<IEnumerable<string>>(sch.Tags) : null,
                TagCount = sch.TagCount
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var searchContactHistoryItem = await DbContext.SearchContactHistory.FindAsync(id);
            if (searchContactHistoryItem == null)
            {
                return NotFound();
            }

            DbContext.SearchContactHistory.Remove(searchContactHistoryItem);
            await DbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
