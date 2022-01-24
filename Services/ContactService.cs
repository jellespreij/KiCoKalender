using Models;
using Models.Helpers;
using Repositories;
using Repositories.Interfaces;
using Services;
using Services.Interfaces;
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

        public Contact FindContactByContactId(Guid contactId)
        {
            return _contactRepository.GetSingle(contactId);
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

        public IEnumerable<ContactDTO> FindContactDTOByFamilyId(Guid familyId)
        {
            IEnumerable<Contact> contacts = FindContactByFamilyId(familyId);

            return ContactDTOHelper.ToDTO(contacts);
        }

        public Contact UpdateContact(ContactUpdateDTO contactUpdate, Guid id)
        {
            Contact contactToUpdate = FindContactByContactId(id);

            contactToUpdate.Name = contactUpdate.Name;
            contactToUpdate.PhoneNumber = contactUpdate.PhoneNumber;
            contactToUpdate.LastName = contactUpdate.LastName;
            contactToUpdate.City = contactUpdate.City;
            contactToUpdate.Address = contactUpdate.Address;
            contactToUpdate.Postcode = contactUpdate.Postcode;
            contactToUpdate.Email = contactUpdate.Email;

            return _contactRepository.Update(contactToUpdate, id).Result;
        }
    }
}
