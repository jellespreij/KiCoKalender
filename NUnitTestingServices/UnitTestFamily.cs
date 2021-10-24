﻿using NUnit.Framework;
using Models;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace NUnitTestingServices
{
    public class UnitTestFamily
    {
        private Mock<IFamilyRepository> _familyRepositoryMock;
        private FamilyService _familyService;
        private List<Family> _MockLstFamily;
        [SetUp]
        public void Setup()
        {
            _familyRepositoryMock = new Mock<IFamilyRepository>();
            _familyService = new FamilyService(_familyRepositoryMock.Object);

            List<User> _MockLstUsers = new List<User>();
            User userOne = new User(Guid.NewGuid(), "Cyprus", "Cypruson", "email.email@gmail.com", "password1", Role.Child, DateTime.Now, "straat 123", "1234AB");
            User userTwo = new User(Guid.NewGuid(), "Crete", "Alebllo", "crete@hotmail.com", "HeelGeheimwachtwoord!", Role.Parent, DateTime.Now, "de hogenveen 12", "2375HY");

            _MockLstUsers.Add(userOne);
            _MockLstUsers.Add(userTwo);

            _MockLstFamily = new List<Family>();
            Family familyOne = new Family(Guid.NewGuid(), _MockLstUsers, "De barends");
            Family familyTwo = new Family(Guid.NewGuid(), _MockLstUsers, "Jansen");

            _MockLstFamily.Add(familyOne);
            _MockLstFamily.Add(familyTwo);
        }

        [Test]
        public void Calling_AddFamily_ON_ServiceLayer_Should_Call_familyRepository_and_Add_family()
        {
            //Arrange
            _familyRepositoryMock.Setup(m => m.Add(_MockLstFamily[0]).Result).Returns(_MockLstFamily[0]);

            //act
            Family result = _familyService.AddFamily(_MockLstFamily[0]);

            //Assert
            Assert.That(result, Is.InstanceOf(typeof(Family)));

            //Check that the GetAll method was called once
            _familyRepositoryMock.Verify(c => c.Add(_MockLstFamily[0]).Result, Times.Once);
        }

        [Test]
        public void Calling_FindFamilyByFamilyId_ON_ServiceLayer_Should_Call_FamilyRepository_and_Return_single_Family()
        {
            //Arrange
            _familyRepositoryMock.Setup(m => m.GetSingle(_MockLstFamily[0].Id)).Returns(_MockLstFamily[0]);

            //act
            Family result = _familyService.FindFamilyByFamilyId(_MockLstFamily[0].Id);

            //Assert
            Assert.That(result, Is.InstanceOf(typeof(Family)));

            //Check that the GetSingle method was called once
            _familyRepositoryMock.Verify(c => c.GetSingle(_MockLstFamily[0].Id), Times.Once);
        }

        [Test]
        public void Calling_DeleteFamily_ON_ServiceLayer_Should_Call_FamilyRepository_and_Return_deleted_Family()
        {
            //Arrange
            _familyRepositoryMock.Setup(m => m.Delete(_MockLstFamily[1].Id).Result).Returns(_MockLstFamily[1]);

            //act
            Family result = _familyService.DeleteFamily(_MockLstFamily[1].Id);

            //Assert
            Assert.That(result, Is.InstanceOf(typeof(Family)));

            //Check that the delete method was called once
            _familyRepositoryMock.Verify(c => c.Delete(_MockLstFamily[1].Id), Times.Once);
        }

        [Test]
        public void Calling_UpdateFamily_ON_ServiceLayer_Should_Call_FamilyRepository_and_Return_updated_Family()
        {
            //Arrange
            _familyRepositoryMock.Setup(m => m.Update(_MockLstFamily[1], _MockLstFamily[1].Id).Result).Returns(_MockLstFamily[1]);

            //act
            Family result = _familyService.UpdateFamily(_MockLstFamily[1], _MockLstFamily[1].Id);

            //Assert
            Assert.That(result, Is.InstanceOf(typeof(Family)));

            //Check that the delete method was called once
            _familyRepositoryMock.Verify(c => c.Update(_MockLstFamily[1], _MockLstFamily[1].Id), Times.Once);
        }

        [Test]
        public void Calling_AddUserToFamily_ON_ServiceLayer_Should_Call_FamilyRepository_and_addUser()
        {
            //Arrange
            User userAdd = new User(Guid.NewGuid(), "Jos", "dag", "crete@hotmail.com", "HeelGeheimwachtwoord!", Role.Parent, DateTime.Now, "de hogenveen 12", "2375HY");
            _familyRepositoryMock.Setup(m => m.AddUserToFamily(userAdd, _MockLstFamily[1].Id));

            //act
            _familyService.AddUserToFamily(userAdd, _MockLstFamily[1].Id);

            //Assert
            //Check that the delete method was called once
            _familyRepositoryMock.Verify(c => c.AddUserToFamily(userAdd, _MockLstFamily[1].Id), Times.Once);
        }

        [TearDown]
        public void TestCleanUp()
        {
            _familyRepositoryMock = null;
            _MockLstFamily = null;
        }
    }
}
