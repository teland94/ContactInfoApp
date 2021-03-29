using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ContactInfoApp.Server.Configuration;
using ContactInfoApp.Shared;
using GetContactAPI;
using GetContactAPI.Exceptions;
using GetContactAPI.Models;
using Microsoft.Extensions.Options;

namespace ContactInfoApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private GetContactSettings GetContactSettings { get; }

        public ContactController(IOptions<GetContactSettings> getContactOptions)
        {
            GetContactSettings = getContactOptions.Value;
        }

        public async Task<ActionResult<Contact>> Get(string phoneNumber)
        {
            try
            {
                var api = new GetContact(new Data(
                    GetContactSettings.Token,
                    GetContactSettings.AesKey
                ));

                var phoneInfo = await api.GetByPhoneAsync(phoneNumber, CancellationToken.None, "UA");
                var phoneInfoResponse = phoneInfo.Response;

                DetailsResult tagsInfoResponse = null;
                if (!phoneInfo.Response.LimitedResult)
                {
                    var tagsInfo = await api.GetTagsAsync(phoneNumber, CancellationToken.None, "UA");
                    tagsInfoResponse = tagsInfo.Response;
                }

                return new Contact
                {
                    DisplayName = phoneInfoResponse.Profile.DisplayName,
                    Tags = tagsInfoResponse?.Tags.Select(t => t.Tag),
                    TagCount = phoneInfoResponse.Profile.TagCount
                };
            }
            catch (GetContactRequestException ex)
            {
                return StatusCode((int)ex.StatusCode, "GetContact Request Error");
            }
        }
    }
}
