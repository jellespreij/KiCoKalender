using Models;
using Repositories;
using SendGrid;
using SendGrid.Helpers.Mail;
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

        public FamilyService(IFamilyRepository familyRepository, IUserRepository userRepository)
        {
            _familyRepository = familyRepository;
            _userRepository = userRepository;
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

            //addedFamily.Users.Add(userToAdd);
            return addedFamily;
        }

        public async void SendMail(string toEmail)
        {
            var apiKey = "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";

            //var apiKey = Environment.GetEnvironmentVariable("emailSender");
            var client = new SendGridClient(apiKey);
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
