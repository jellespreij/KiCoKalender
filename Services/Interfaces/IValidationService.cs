using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IValidationService
    {
        string CheckUserInput(string body);
        bool CheckPasswordStrenght(string password);
    }
}
