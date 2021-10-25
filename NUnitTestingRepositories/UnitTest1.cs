using NUnit.Framework;
using Models;
using Repositories;
using System;
using System.Collections.Generic;
using Moq;

namespace NUnitTestingRepositories
{
    public class Tests
    {
        private User _MockUser;
        private Mock<IUserRepository> _userRepositoryMock;
        private IUserRepository _UserRepository;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();

            Guid userId = Guid.NewGuid();
            User userOne = new User(userId, "Cyprus", "Cypruson", "email.email@gmail.com", "password1", Role.Child, DateTime.Now, "straat 123", "1234AB");

            //get single
            _userRepositoryMock.Setup(m => m.GetSingle(It.Is<Guid>(g => g == userId))).Returns(userOne);



            _UserRepository = _userRepositoryMock.Object;
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}