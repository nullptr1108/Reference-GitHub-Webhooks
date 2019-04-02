using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace PrMergeListener
{
    public static class PrMergeHandler
    {
        [FunctionName("PrMergeHandler")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Queue("pr-merge-queue")]CloudQueue outputQueue,
            ILogger log)
        {
            log.LogInformation("A WebHook Event has been caught");
            
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            //Add message to queue and make it visible after a specific time.
            await outputQueue.AddMessageAsync(new CloudQueueMessage(requestBody),
                timeToLive: TimeSpan.FromDays(2),
                initialVisibilityDelay: null,
                options: null,
                operationContext: null);

            dynamic data = JsonConvert.DeserializeObject(requestBody);
            
            return requestBody != null
                ? (ActionResult)new OkObjectResult("WebHook Event Processed")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
