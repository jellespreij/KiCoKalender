using Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class FamilyService : IFamilyService
    {
        private IFamilyRepository _familyRepository;

        public FamilyService(IFamilyRepository familyRepository)
        {
            _familyRepository = familyRepository;
        }
        public void AddFamily(Family family)
        {
            _familyRepository.Add(family);
        }

        public void DeleteFamily(Family family)
        {
            _familyRepository.Delete(family);
        }

        public IEnumerable<Family> FindFamilyByUserIdAndRole(long userId, Role role)
        {
            //return _familyRepository.FindBy(e => e.Id == 1);
            return _familyRepository.FindBy(e => e.Parents.Any(x => x.Id == userId));
        }

        public void UpdateFamily(Family family)
        {
            _familyRepository.Update(family);
        }
    }
}
