using NUnit.Framework;
using Models;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using Moq;

namespace NUnitTestingServices
{
    public class UnitTestUser
    {

        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IFamilyRepository> _familyRepositoryMock;
        private UserService _userService;
        private List<User> _MockLstUsers;
        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _familyRepositoryMock = new Mock<IFamilyRepository>();
            _userService = new UserService(_userRepositoryMock.Object, _familyRepositoryMock.Object);
            
            _MockLstUsers = new List<User>();
            User userOne = new User(Guid.NewGuid(), "Cyprus", "Cypruson", "email.email@gmail.com", "password1", Role.Child, DateTime.Now, "straat 123", "1234AB");
            User userTwo = new User(Guid.NewGuid(), "Crete", "Alebllo","crete@hotmail.com", "HeelGeheimwachtwoord!", Role.Parent, DateTime.Now, "de hogenveen 12", "2375HY");

            _MockLstUsers.Add(userOne);
            _MockLstUsers.Add(userTwo);
        }

        [Test]
        public void Calling_AddUser_ON_ServiceLayer_Should_Call_UserRepository_and_Add_single_User()
        {
            //Arrange
            _userRepositoryMock.Setup(m => m.Add(_MockLstUsers[0]).Result).Returns(_MockLstUsers[0]);

            //act
            User result = _userService.AddUser(_MockLstUsers[0]);

            //Assert
            Assert.That(result, Is.InstanceOf(typeof(User)));

            //Check that the GetAll method was called once
            _userRepositoryMock.Verify(c => c.Add(_MockLstUsers[0]).Result, Times.Once);
        }

        [Test]
        public void Calling_FindUserByEmail_ON_ServiceLayer_Should_Call_UserRepository_and_Return_single_User()
        {
            //Arrange
            _userRepositoryMock.Setup(m => m.FindUserByEmail(_MockLstUsers[0].Email).Result).Returns(_MockLstUsers[0]);

            //act
            User result = _userService.FindUserByEmail(_MockLstUsers[0].Email);

            //Assert
            Assert.That(result, Is.InstanceOf(typeof(User)));

            //Check that the GetSingle method was called once
            _userRepositoryMock.Verify(c => c.FindUserByEmail(_MockLstUsers[0].Email).Result, Times.Once);
        }

        [Test]
        public void Calling_FindUserByUserId_ON_ServiceLayer_Should_Call_UserRepository_and_Return_single_User()
        {
            //Arrange
            _userRepositoryMock.Setup(m => m.GetSingle(_MockLstUsers[0].Id)).Returns(_MockLstUsers[0]);

            //act
            User result = _userService.FindUserByUserId(_MockLstUsers[0].Id);

            //Assert
            Assert.That(result, Is.InstanceOf(typeof(User)));

            //Check that the GetSingle method was called once
            _userRepositoryMock.Verify(c => c.GetSingle(_MockLstUsers[0].Id), Times.Once);
        }

        [Test]
        public void Calling_DeleteUserByUserId_ON_ServiceLayer_Should_Call_UserRepository_and_Return_deleted_User()
        {
            //Arrange
            _userRepositoryMock.Setup(m => m.Delete(_MockLstUsers[0].Id).Result).Returns(_MockLstUsers[0]);

            //act
            User result = _userService.DeleteUser(_MockLstUsers[0].Id).Result;

            //Assert
            Assert.That(result, Is.InstanceOf(typeof(User)));

            //Check that the delete method was called once
            _userRepositoryMock.Verify(c => c.Delete(_MockLstUsers[0].Id), Times.Once);
        }

        [Test]
        public void Calling_UpdateUserByUserId_ON_ServiceLayer_Should_Call_UserRepository_and_Return_updated_User()
        {
            //Arrange
            _userRepositoryMock.Setup(m => m.Update(_MockLstUsers[0], _MockLstUsers[0].Id).Result).Returns(_MockLstUsers[0]);

            //act
            User result = _userService.UpdateUser(_MockLstUsers[0], _MockLstUsers[0].Id);

            //Assert
            Assert.That(result, Is.InstanceOf(typeof(User)));

            //Check that the delete method was called once
            _userRepositoryMock.Verify(c => c.Update(_MockLstUsers[0], _MockLstUsers[0].Id), Times.Once);
        }

        [TearDown]
        public void TestCleanUp()
        {
            _userRepositoryMock = null;
            _MockLstUsers = null;
        }
    }
}