using Microsoft.Extensions.Configuration;
using Models;
using Repositories;
using Repositories.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using Services.Interfaces;
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
        private IUserRepository _userRepository;
        public IConfiguration _configuration { get; }

        public FamilyService(IFamilyRepository familyRepository, IUserRepository userRepository, IConfiguration configuration)
        {
            _familyRepository = familyRepository;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public void AddUserToFamily(User user, Guid id)
        {
            _familyRepository.AddUserToFamily(user, id);
        }

        public void AddFolderToFamily(Folder folder, Guid id)
        {
            _familyRepository.AddFolderToFamily(folder, id);
        }

        public Family DeleteFamily(Guid id)
        {
            return _familyRepository.Delete(id).Result;
        }

        public Family FindFamilyByFamilyId(Guid familyId)
        {
            return _familyRepository.GetSingle(familyId);
        }

        public Family AddFamily(Family family, Guid userId)
        {
            Family addedFamily = _familyRepository.Add(family).Result;
            User userToAdd = _userRepository.GetSingle(userId);

            addedFamily = _familyRepository.AddUserToFamily(userToAdd, family.Id).Result;

            return addedFamily;
        }

        public async void SendMail(string toEmail)
        {
            var apikey = _configuration.GetConnectionString("sendGridKey");
            var client = new SendGridClient(apikey);
            var from = new EmailAddress("kimvangelder@kpnmail.nl");
            var subject = "Uitnodiging KiCoKalender";
            var to = new EmailAddress(toEmail);
            var plainTextContent = "Ik zou graag willen dat je de KiCoKalender download zodat wij beter kunnen communiceren tijdens de scheiding.";
            var htmlContent = "<h3>Beste Ontvanger ..</h3><p>Ik zou graag willen dat je de KiCoKalender download zodat wij beter kunnen communiceren tijdens de scheiding.</p></n><p>Vriendelijke groet, "+ from.Email+"</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await client.SendEmailAsync(msg);
        }
    }
}
