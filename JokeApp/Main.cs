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
            while (!acceptedJoke)
            {
                // Kj�r metoden for � gj�re sp�rring til APIet
                // JokeAPI.GETRandomJoke();
                joke = JokeAPI.GETRandomJoke();
                acceptedJoke = joke.validateJoke();

                if (retries >= maxRetry)
                {
                    // Hvis vi bruker mer enn maxRetry fors�k p� � finne en godkjent vits avslutter vi kj�ringen
                    log.LogInformation($"After {maxRetry} subsequent tries, function couldn't retrieve a joke that passed validation.");
                    return;
                }
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

                var exists = con.ExecuteScalar<bool>("select count(distinct 1) from Jokes where Id=@id", joke);

                // If any jokes with the same jokeId existed in the API, we don't insert anything
                if (!exists)
                {
                    // Definerer sp�rringen v�r for insertion og kj�rer denne
                    con.Execute("insert into Jokes (Id, JokeType, Setup, Punchline) values(@Id, @Type, @Setup, @Punchline)",
                        joke);
                }
            }
        }
    }
}
