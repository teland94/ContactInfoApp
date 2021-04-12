using System;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ContactInfoApp.Server.Persistence.Entities;
using GetContactAPI;
using GetContactAPI.Exceptions;
using GetContactAPI.Models;
using System.Text.Json;
using ContactInfoApp.Server.Persistence;
using ContactInfoApp.Shared.Models;

namespace ContactInfoApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private GetContact Api { get; }
        private AppDbContext DbContext { get; }

        public ContactController(GetContact getContact,
            AppDbContext dbContext)
        {
            Api = getContact;
            DbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<ContactModel>> Get(string phoneNumber)
        {
            try
            {
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

                var phoneInfo = await Api.GetByPhoneAsync(phoneNumber, CancellationToken.None, "UA");
                var phoneInfoResponse = phoneInfo.Response;

                DetailsResult tagsInfoResponse = null;
                if (!phoneInfoResponse.LimitedResult && phoneInfoResponse.Profile.TagCount > 0)
                {
                    var tagsInfo = await Api.GetTagsAsync(phoneNumber, CancellationToken.None, "UA");
                    tagsInfoResponse = tagsInfo.Response;
                }

                var profile = phoneInfoResponse.Profile;
                var contact = new ContactModel
                {
                    PhoneNumber = profile.PhoneNumber,
                    DisplayName = profile.DisplayName,
                    IsSpam = phoneInfoResponse.SpamInfo.Degree != "none",
                    Tags = tagsInfoResponse?.Tags.Select(t => t.Tag).ToList(),
                    TagCount = profile.TagCount
                };

                await AddSearchContactToHistoryAsync(contact, remoteIpAddress);

                return contact;
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
                await Api.SendValidationCodeAsync(validationCode, CancellationToken.None);

                return Ok();
            }
            catch (GetContactRequestException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ErrorInfo.Response);
            }
        }

        private async Task AddSearchContactToHistoryAsync(ContactModel contact, string ipAddress)
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var searchContactHistoryItem = new SearchContactHistory
            {
                Date = DateTime.UtcNow,
                IpAddress = ipAddress,
                PhoneNumber = contact.PhoneNumber,
                DisplayName = contact.DisplayName,
                IsSpam = contact.IsSpam,
                Tags = JsonSerializer.Serialize(contact.Tags, jsonSerializerOptions),
                TagCount = contact.TagCount
            };

            await DbContext.AddAsync(searchContactHistoryItem);
            await DbContext.SaveChangesAsync();
        }
    }
}
