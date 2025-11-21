using System;
using System.Collections.Generic;
using TermSuggester.BackOfficeEngine;

namespace TermSuggester
{
    class Program
    {
        static void Main()
        {
            List<string> choices = new List<string> { "gros", "gras", "graisse", "agressif", "go", "ros", "gro" };
            int numberOfSuggestions = 2;
            
            TermSuggestions suggester = new TermSuggestions();
            IEnumerable<string> result = suggester.GetSuggestions("gros", choices, numberOfSuggestions);

            Console.WriteLine(string.Join(", ", result));
        }
    }
}