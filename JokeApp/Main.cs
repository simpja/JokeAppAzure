using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Dapper;

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

            var joke = new Joke();
            Boolean acceptedJoke = false;
            int retries = 0;
            int maxRetry = 10;
            while (!acceptedJoke && retries < maxRetry)
            {
                // Kjør metoden for å gjøre spørring til APIet
                // JokeAPI.GETRandomJoke();
                joke = JokeAPI.GETRandomJoke();
                acceptedJoke = joke.validateJoke();
            }
            
            // Logger ut vitsen til konsoll:
            log.LogInformation($"Her kommer en vits!");
            log.LogInformation($"{joke.Setup}");
            log.LogInformation($"{joke.Punchline}");

            // Vi henter connecitonString til databasen fra miljøvariabler
            string connectionString = System.Environment.GetEnvironmentVariable($"ConnectionStrings:{"SQLConnectionString"}");

            int Id = joke.Id;
            string JokeType = joke.Type;
            string Setup = joke.Setup;
            string Punchline = joke.Punchline;

            // Oppretter tilkobling til databasen vha. System.Data.SqlClient og åpner denne
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();

                // Definerer spørringen vår og kjører denne
                con.Execute("insert into Jokes (Id, JokeType, Setup, Punchline) values(@Id, @JokeType, @Setup, @Punchline)",
                    new { Id, JokeType, Setup, Punchline });
            }
        }
    }
}
