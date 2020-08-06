using System;
using System.Collections.Generic;
using System.Text;

namespace JokeApp
{
    public class Joke
    {
        private int id;
        private string type;
        private string setup;
        private string punchline;

        public int Id { get => id; set => id = value; }
        public string Type { get => type; set => type = value; }
        public string Setup { get => setup; set => setup = value; }
        public string Punchline { get => punchline; set => punchline = value; }

        public Boolean validateJoke()
        {
            // Initial validation true indicating a pass
            // We make a straight forward if-else check to validate different lengths
            // You may implement this function as you please. 
            // I've chose to do it simple, explicit and un-elegant for the sake of teaching
            var validation = true;
            
            if (this.Id < 0) // Validate non-negative Id
            {
                validation = false;
            } else if (this.Type.Length > 255 
                || this.Setup.Length > 255 
                || this.Punchline.Length > 255) // validate length doesn't exceed the 255 char limit in our db
            {
                validation = false;
            }
            return validation;
        }
    }

    
}

