using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ContactInfoApp.Shared.Models;

namespace ContactInfoApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputerVisionController : ControllerBase
    {
        private readonly IComputerVisionClient _computerVisionClient;

        public ComputerVisionController(IComputerVisionClient computerVisionClient)
        {
            _computerVisionClient = computerVisionClient;
        }

        [HttpPost(nameof(Ocr))]
        public async Task<ActionResult<string>> Ocr([FromBody] OcrRequestModel model)
        {
            return await GetTextImage(new MemoryStream(Convert.FromBase64String(model.Image)));
        }

        private async Task<string> GetTextImage(Stream imageStream)
        {
            const float scaleVal = 3f;

            using var imgBmp = new Bitmap(imageStream);
            SetPixelColor(imgBmp, false);

            using var imgBmpScaled = new Bitmap(imgBmp, new Size((int)(imgBmp.Width * scaleVal), (int)(imgBmp.Height * scaleVal)));
            SetPixelColor(imgBmpScaled);

            var imageMemoryStream = new MemoryStream();
            imgBmpScaled.Save(imageMemoryStream, ImageFormat.Jpeg);

            imageMemoryStream.Seek(0, SeekOrigin.Begin);
            return await ReadImage(imageMemoryStream);
        }

        private async Task<string> ReadImage(Stream imageStream)
        {
            // Read text from URL
            var textHeaders = await _computerVisionClient.ReadInStreamAsync(imageStream, language: "en");
            // After the request, get the operation location (operation ID)
            var operationLocation = textHeaders.OperationLocation;

            // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            var operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            ReadOperationResult results; 
            do
            {
                results = await _computerVisionClient.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                    results.Status == OperationStatusCodes.NotStarted));

            var textUrlFileResults = results.AnalyzeResult.ReadResults;
            return (from page in textUrlFileResults from line in page.Lines select line.Text).FirstOrDefault();
        }

        private static void SetPixelColor(Bitmap imgBmp, bool hasBeenCleared = true)
        {
            var bgColor = Color.White;
            var textColor = Color.Black;
            for (var x = 0; x < imgBmp.Width; x++)
            {
                for (var y = 0; y < imgBmp.Height; y++)
                {
                    var pixel = imgBmp.GetPixel(x, y);
                    var isCloserToWhite = hasBeenCleared ? ((pixel.R + pixel.G + pixel.B) / 2) > 180 : ((pixel.R + pixel.G + pixel.B) / 2) > 120;
                    imgBmp.SetPixel(x, y, isCloserToWhite ? bgColor : textColor);
                }
            }
        }
    }
}
