using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAddressService
    {
        IEnumerable<Address> FindAddressByUserId(long userId);
        IEnumerable<Address> FindAddressByFamilyId(long userId);
        void AddAddress(Address address);
        void UpdateAddress(Address address);
        void DeleteAddress(Address address);
    }
}
