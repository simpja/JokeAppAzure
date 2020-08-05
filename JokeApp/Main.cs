using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace JokeApp
{
    public static class Main
    {
        [FunctionName("Main")]
        public static void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            APIController JokeAPI = new APIController();
            log.LogInformation($"API URL is: {JokeAPI.APIURL}");

            // Kjør metoden for å gjøre spørring til APIet
            // JokeAPI.GETRandomJoke();
            var joke = JokeAPI.GETRandomJoke();

            // Logger ut vitsen til konsoll:
            log.LogInformation($"Her kommer en vits!");
            log.LogInformation($"{joke.Setup}");
            log.LogInformation($"{joke.Punchline}");
        }
    }
}
