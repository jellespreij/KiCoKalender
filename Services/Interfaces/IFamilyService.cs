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
        Family FindFamilyByFamilyId(Guid familyId);
        Family AddFamily(Family family, Guid userId);
        void AddUserToFamily(User user, Guid id);
        void AddFolderToFamily(Folder folder, Guid id);
        Family DeleteFamily(Guid id);
        void SendMail(string toEmail);
    }
}
