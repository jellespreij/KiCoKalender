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
    public class AddressService : IAddressService
    {
        private IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public void AddAddress(Address address)
        {
            _addressRepository.Add(address);
        }

        public void DeleteAddress(Address address)
        {
            _addressRepository.Delete(address);
        }

        public IEnumerable<Address> FindAddressByFamilyId(long familyId)
        {
            return _addressRepository.FindBy(e => e.FamilyId == familyId);
        }

        public IEnumerable<Address> FindAddressByUserId(long userId)
        {
            return _addressRepository.FindBy(e => e.UserId == userId);
        }

        public void UpdateAddress(Address address)
        {
            _addressRepository.Update(address);
        }
    }
}
