// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrMergeHandler.cs" company="">
//   
// </copyright>
// <summary>
//   TODO The pr merge handler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PrMergeListener
    {
        #region

        using System;
        using System.IO;
        using System.Threading.Tasks;

        using Microsoft.AspNetCore.Http;
        using Microsoft.AspNetCore.Mvc;
        using Microsoft.Azure.WebJobs;
        using Microsoft.Azure.WebJobs.Extensions.Http;
        using Microsoft.Extensions.Logging;
        using Microsoft.WindowsAzure.Storage.Queue;

        using Newtonsoft.Json;

        #endregion

        /// <summary>
        /// TODO The pr merge handler.
        /// </summary>
        public static class PrMergeHandler
            {
                /// <summary>
                /// TODO The run.
                /// </summary>
                /// <param name="req">
                /// TODO The req.
                /// </param>
                /// <param name="outputQueue">
                /// TODO The output queue.
                /// </param>
                /// <param name="log">
                /// TODO The log.
                /// </param>
                /// <returns>
                /// The <see cref="Task"/>.
                /// </returns>
                [FunctionName("PrMergeHandler")] public static async Task<IActionResult> Run(
                    [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
                    HttpRequest req,
                    [Queue("pr-merge-queue")] CloudQueue outputQueue,
                    ILogger log)
                    {
                        log.LogInformation("A WebHook Event has been caught");

                        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                        // Add message to queue and make it visible after a specific time.
                        await outputQueue.AddMessageAsync(new CloudQueueMessage(requestBody), TimeSpan.FromDays(2), null, null, null);

                        dynamic data = JsonConvert.DeserializeObject(requestBody);

                        return requestBody != null ? (ActionResult)new OkObjectResult("WebHook Event Processed") : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
                    }
            }
    }
