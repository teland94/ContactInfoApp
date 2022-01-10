using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ContactInfoApp.Server.Persistence.Entities;
using GetContactAPI;
using GetContactAPI.Exceptions;
using System.Text.Json;
using ContactInfoApp.Server.Configuration;
using ContactInfoApp.Server.Persistence;
using ContactInfoApp.Shared.Models;
using Microsoft.Extensions.Options;

namespace ContactInfoApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private GetContact Api { get; }
        private GetContactSettings GetContactSettings { get; }
        private AppDbContext DbContext { get; }

        public ContactController(GetContact getContact,
            IOptions<GetContactSettings> getContactSettingsAccessor,
            AppDbContext dbContext)
        {
            Api = getContact;
            GetContactSettings = getContactSettingsAccessor.Value;
            DbContext = dbContext;
        }

        [HttpGet(nameof(Search))]
        public async Task<ActionResult<ContactModel>> Search(string phoneNumber)
        {
            try
            {
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

                var phoneInfo = await Api.GetByPhoneAsync(phoneNumber, GetContactSettings.CountryCode);
                var phoneInfoResponse = phoneInfo.Response;

                var profile = phoneInfoResponse.Profile;
                var contact = new ContactModel
                {
                    PhoneNumber = profile.PhoneNumber,
                    DisplayName = profile.DisplayName,
                    IsSpam = phoneInfoResponse.SpamInfo.Degree != "none",
                    LimitedResult = phoneInfoResponse.LimitedResult,
                    TagCount = profile.TagCount,
                    CommentCount = phoneInfoResponse.Comments.CommentCount
                };

                contact.Id = await AddSearchContactToHistoryAsync(contact, remoteIpAddress);

                return contact;
            }
            catch (GetContactRequestException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ErrorInfo.Response);
            }
        }

        [HttpGet(nameof(NumberDetail))]
        public async Task<ActionResult<NumberDetailModel>> NumberDetail(string phoneNumber, int? contactId = null)
        {
            try
            {
                var tagsInfo = await Api.GetTagsAsync(phoneNumber, GetContactSettings.CountryCode);

                var tags = tagsInfo.Response.Tags.Select(t => t.Tag).ToList();

                if (contactId != null)
                {
                    await UpdateSearchContactHistoryAsync(contactId.Value, tags);
                }

                return new NumberDetailModel
                {
                    Tags = tags
                };
            }
            catch (GetContactRequestException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ErrorInfo.Response);
            }
        }

        [HttpGet(nameof(Comments))]
        public async Task<ActionResult<CommentsModel>> Comments(string phoneNumber, int? contactId = null)
        {
            try
            {
                var commentsInfo = await Api.GetCommentsAsync(phoneNumber);

                var comments = commentsInfo.Response.Comments.Select(c => new CommentModel
                {
                    Author = c.Author,
                    AuthorImage = c.AuthorImage,
                    Body = c.Body,
                    Liked = c.Liked,
                    Disliked = c.Disliked,
                    Date = c.Date
                }).ToList();

                //if (contactId != null)
                //{
                //    await UpdateSearchContactHistoryAsync(contactId.Value, tags);
                //}

                return new CommentsModel
                {
                    Comments = comments
                };
            }
            catch (GetContactRequestException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ErrorInfo.Response);
            }
        }

        [HttpGet(nameof(VerifyCode))]
        public async Task<ActionResult> VerifyCode(string validationCode)
        {
            try
            {
                await Api.SendValidationCodeAsync(validationCode);

                return Ok();
            }
            catch (GetContactRequestException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ErrorInfo.Response);
            }
        }

        private async Task UpdateSearchContactHistoryAsync(int contactId, IEnumerable<string> tags)
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var contact = await DbContext.SearchContactHistory.FindAsync(contactId);

            contact.Tags = JsonSerializer.Serialize(tags, jsonSerializerOptions);

            DbContext.Update(contact);
            await DbContext.SaveChangesAsync();
        }

        private async Task<int> AddSearchContactToHistoryAsync(ContactModel contact, string ipAddress)
        {
            var searchContactHistoryItem = new SearchContactHistory
            {
                Date = DateTime.UtcNow,
                IpAddress = ipAddress,
                PhoneNumber = contact.PhoneNumber,
                DisplayName = contact.DisplayName,
                IsSpam = contact.IsSpam,
                TagCount = contact.TagCount
            };

            var entry = await DbContext.AddAsync(searchContactHistoryItem);
            await DbContext.SaveChangesAsync();

            return entry.Entity.Id;
        }
    }
}
