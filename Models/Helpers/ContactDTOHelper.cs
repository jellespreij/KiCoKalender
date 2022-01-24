using System.Collections.Generic;
using System.Linq;

namespace Models.Helpers
{
    public static class ContactDTOHelper
    {
        public static IEnumerable<ContactDTO> ToDTO(IEnumerable<Contact> contacts)
        {
            List<ContactDTO> contactsDTOs = new();

            foreach (Contact contact in contacts)
            {
                contactsDTOs.Add(new ContactDTO
                {
                    Id = contact.Id,
                    Name = contact.Name,
                    PhoneNumber = contact.PhoneNumber,
                    LastName = contact.LastName,
                    City = contact.City,
                    Address = contact.Address,
                    Postcode = contact.Postcode,
                    Email = contact.Email,
                    ContactType = contact.ContactType,
                    Created = contact.Created
                });
            }

            return contactsDTOs.AsEnumerable();
        }
    }
}