using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IContactService
    {
        IEnumerable<Contact> FindContactByFamilyId(Guid userId);
        Contact AddContact(Contact address);
        Contact UpdateContact(Contact address, Guid id);
        Contact DeleteContact(Guid id);
    }
}
