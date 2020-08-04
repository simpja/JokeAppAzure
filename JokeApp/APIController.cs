using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
