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

        public async void SendMail(string toEmail)
        {
            var apiKey = "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";

            //var apiKey = Environment.GetEnvironmentVariable("emailSender");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("565459@student.inholland.nl");
            var subject = "Inivitation KiCoKalender";
            var to = new EmailAddress(toEmail);
            var plainTextContent = "Ik zou graag willen dat je de KiCoKalende download zodat wij beter kunnen communiceren tijdens de scheiding.";
            var htmlContent = "<h3>Dear ..</h3><p>Ik zou graag willen dat je de KiCoKalende download zodat wij beter kunnen communiceren tijdens de scheiding.</p></n><p>Vriendelijke groet, "+ from.Email+"</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await client.SendEmailAsync(msg);
        }

        public void UpdateFamily(Family family)
        {
            _familyRepository.Update(family);
        }
    }
}
