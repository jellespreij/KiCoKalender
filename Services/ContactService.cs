using Models;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ContactService : IContactService
    {
        private IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public Contact AddContact(Contact contact)
        {
            return _contactRepository.Add(contact).Result;
        }

        public Contact DeleteContact(Guid id)
        {
            return _contactRepository.Delete(id).Result;
        }

        public IEnumerable<Contact> FindContactByFamilyId(Guid familyId)
        {
            return _contactRepository.FindBy(e => e.FamilyId == familyId);
        }

        public Contact UpdateContact(Contact contact, Guid id)
        {
            return _contactRepository.Update(contact, id).Result;
        }
    }
}
