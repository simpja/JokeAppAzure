using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Dapper;
using System.Configuration;

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
                // Kj�r metoden for � gj�re sp�rring til APIet
                // JokeAPI.GETRandomJoke();
                joke = JokeAPI.GETRandomJoke();
                acceptedJoke = joke.validateJoke();
            }
            
            // Logger ut vitsen til konsoll:
            log.LogInformation($"Her kommer en vits!");
            log.LogInformation($"{joke.Setup}");
            log.LogInformation($"{joke.Punchline}");

            // Vi henter connectionString til databasen fra milj�variabler
            string connectionString = System.Environment.GetEnvironmentVariable("SQLConnectionString", EnvironmentVariableTarget.Process);

            // Oppretter tilkobling til databasen vha. System.Data.SqlClient og �pner denne
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();

                // Definerer sp�rringen v�r og kj�rer denne
                con.Execute("insert into Jokes (Id, JokeType, Setup, Punchline) values(@Id, @Type, @Setup, @Punchline)",
                    joke);
            }
        }
    }
}
