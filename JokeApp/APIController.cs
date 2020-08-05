using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace JokeApp
{
    class APIController
    {
        // A class to control our Joke API
        // Define some properties of our class
        public string APIURL;

        // Create constructor to set variables
        public APIController()
        {
            // Let's put the URL in our environmental variables to make it easier to update it later if needed
            this.APIURL = System.Environment.GetEnvironmentVariable("RandomJokeAPIURL", EnvironmentVariableTarget.Process);
        }

        public Joke GETRandomJoke()
        {
            // Vi definerer først en ny RestClient med endepunktet vi henter fra miljøvariabelen i konstruktøren over
            var client = new RestClient(this.APIURL);

            // Deretter definerer vi en RestRequest som holder detaljene for spørringen vår
            // En RestRequest hører til en RestClient og får det generelle endepunktet fra denne
            // Det første argumentet er URL-parametre der vi i vårt eksempel ikke trenger noen fordi vi skal rett til base-URLen
            // Det andre argumentet forteller hvilken metode vi benytter
            var request = new RestRequest("", Method.GET);

            // Da kan vi kjøre spørringen vår!
            IRestResponse response = client.Execute(request);

            // La oss hente ut content fra responsen
            var content = response.Content;
            // Og la oss videre opprette en instans av Joke-klassen vår
            Joke joke = new Joke();
            // Til slutt parser vi innholdet fra APIet til formen av Joke-klassen vår og lagrer dette i joke-variabelen vår
            joke = JsonConvert.DeserializeObject<Joke>(content);

            // Returner vitsen, en instans av typen/klassen Joke
            return joke;

        }
    }
}
