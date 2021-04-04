using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ContactInfoApp.Shared;
using GetContactAPI;
using GetContactAPI.Exceptions;
using GetContactAPI.Models;

namespace ContactInfoApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private GetContact Api { get; }

        public ContactController(GetContact getContact)
        {
            Api = getContact;
        }

        [HttpGet]
        public async Task<ActionResult<Contact>> Get(string phoneNumber)
        {
            try
            {
                var phoneInfo = await Api.GetByPhoneAsync(phoneNumber, CancellationToken.None, "UA");
                var phoneInfoResponse = phoneInfo.Response;

                DetailsResult tagsInfoResponse = null;
                if (!phoneInfoResponse.LimitedResult && phoneInfoResponse.Profile.TagCount > 0)
                {
                    var tagsInfo = await Api.GetTagsAsync(phoneNumber, CancellationToken.None, "UA");
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
    }
}
