using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IFamilyService
    {
        IEnumerable<Family> FindFamilyByUserIdAndRole(long userId, Role role);
        void AddFamily(Family family);
        void UpdateFamily(Family family);
        void DeleteFamily(Family family);
        void SendMail(string toEmail);
    }
}
