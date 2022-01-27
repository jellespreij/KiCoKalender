using NUnit.Framework;
using Models;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using Moq;
using System.Linq;
using Repositories.Interfaces;

namespace NUnitTestingServices
{
    public class UnitTestContact
    {

        private Mock<IContactRepository> _contactRepositoryMock;
        private ContactService _contactService;
        private List<Contact> _MockLstContacts;
        private ContactDTO _MockContactDTO;
        private ContactUpdateDTO _MockContactUpdateDTO;
        [SetUp]
        public void Setup()
        {
            _contactRepositoryMock = new Mock<IContactRepository>();
            _contactService = new ContactService(_contactRepositoryMock.Object);

            _MockLstContacts = new List<Contact>();
            Contact contactOne = new Contact(Guid.NewGuid(), Guid.NewGuid(), "0658652143", ContactType.family, "Oom bert", "Amsterdam", "De hagenland 11", "2154LO", Guid.NewGuid().ToString());
            Contact contactTwo = new Contact(Guid.NewGuid(), Guid.NewGuid(), "0674582136", ContactType.family, "Oom jan", "Zaandam", "De korf 11", "2154PO", Guid.NewGuid().ToString());

            _MockContactDTO = new ContactDTO();
            _MockContactUpdateDTO = new ContactUpdateDTO();

            _MockLstContacts.Add(contactOne);
            _MockLstContacts.Add(contactTwo);
        }

        [Test]
        public void Calling_AddContact_ON_ServiceLayer_Should_Call_ContactRepository_and_Add_single_Contact()
        {
            //Arrange
            _contactRepositoryMock.Setup(m => m.Add(_MockLstContacts[0]).Result).Returns(_MockLstContacts[0]);

            //act
            Contact result = _contactService.AddContact(_MockLstContacts[0]);

            //Assert
            Assert.That(result, Is.InstanceOf(typeof(Contact)));

            //Check that the GetAll method was called once
            _contactRepositoryMock.Verify(c => c.Add(_MockLstContacts[0]).Result, Times.Once);
        }

        [Test]
        public void Calling_FindContactByFamilyId_ON_ServiceLayer_Should_Call_ContactRepository_and_Return_Contacts()
        {
            Guid familyId = _MockLstContacts[0].FamilyId;
            //Arrange
            _contactRepositoryMock.Setup(m => m.FindBy(e => e.FamilyId == familyId)).Returns(_MockLstContacts);
            
            //act
            IEnumerable <Contact> result = _contactService.FindContactByFamilyId(familyId);

            //Assert
            Assert.That(result, Is.InstanceOf(typeof(IEnumerable<Contact>)));
            Assert.AreEqual(result.Count(), 2);
            Assert.IsNotNull(result);
            //Check that the GetSingle method was called once
            _contactRepositoryMock.Verify(c => c.FindBy(e => e.FamilyId == familyId), Times.Once);
        }

        [Test]
        public void Calling_DeleteContact_ON_ServiceLayer_Should_Call_ContactRepository_and_Return_deleted_Contact()
        {
            //Arrange
            _contactRepositoryMock.Setup(m => m.Delete(_MockLstContacts[0].Id).Result).Returns(_MockLstContacts[0]);

            //act
            Contact result = _contactService.DeleteContact(_MockLstContacts[0].Id);

            //Assert
            Assert.That(result, Is.InstanceOf(typeof(Contact)));

            //Check that the delete method was called once
            _contactRepositoryMock.Verify(c => c.Delete(_MockLstContacts[0].Id), Times.Once);
        }

        [TearDown]
        public void TestCleanUp()
        {
            _contactRepositoryMock = null;
            _MockLstContacts = null;
        }
    }
}

