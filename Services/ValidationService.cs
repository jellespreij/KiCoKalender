using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services
{
    public class ValidationService : IValidationService
    {
        public string CheckUserInput(string userInput)
        {
            char[] charsToReplace = new char[] {'#', '<', '>', '/', '$', '%', '^', '*', '(', ')' };
            return charsToReplace.Aggregate(userInput, (ch1, ch2) => ch1.Replace(ch2, '-'));
        }

        public bool CheckPasswordStrenght(string password)
        {
            string pattern = @"(?=^.{8,}$)(?=.*\d)(?=.*[!@#$%^&*]+)(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$";

            Regex regex = new Regex(pattern);

            return regex.IsMatch(password);
        }

    }
}
