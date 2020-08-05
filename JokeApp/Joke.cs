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
    }
}

