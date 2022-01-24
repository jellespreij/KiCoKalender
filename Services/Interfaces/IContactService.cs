using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IContactService
    {
        Contact FindContactByContactId(Guid contactId);
        IEnumerable<Contact> FindContactByFamilyId(Guid familyId);
        IEnumerable<ContactDTO> FindContactDTOByFamilyId(Guid familyId);
        Contact AddContact(Contact address);
        Contact UpdateContact(ContactUpdateDTO addressUpdate, Guid id);
        Contact DeleteContact(Guid id);
    }
}
