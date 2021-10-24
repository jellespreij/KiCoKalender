using NUnit.Framework;
using Models;
using Services;
using Controllers;
using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using System.Linq;
using Auth.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System.IO;
using Newtonsoft.Json;

namespace NUnitTestingControllers
{
    public class UnitTestUserController
    {
        private List<User> _MockLstUsers;
        private Mock<IUserService> _MockUserService;
        private UserController UserController;
        private Mock<IAuthenticate> _Authenticate;
        private ILogger<UserController> Logger;
        private HttpRequestData req;
        private FunctionContext executionContext;

        [SetUp]
        public void Setup()
        {
           // req = HttpRequestData;
          //  executionContext = new Mock<executionContext>();

            _MockUserService = new Mock<IUserService>();
            _Authenticate = new Mock<IAuthenticate>();

            _MockLstUsers = new List<User>();

            User userOne = new User(Guid.NewGuid(), "Cyprus", "Cypruson", "email.email@gmail.com", "password1", Role.Child, DateTime.Now, "straat 123", "1234AB");
            User userTwo = new User(Guid.NewGuid(), "Crete", "Alebllo", "crete@hotmail.com", "HeelGeheimwachtwoord!", Role.Parent, DateTime.Now, "de hogenveen 12", "2375HY");

            _MockLstUsers.Add(userOne);
            _MockLstUsers.Add(userTwo);

            UserController userController = new UserController(Logger, _MockUserService.Object, _Authenticate.Object);
        }

        [Test]
        public void FindUserByUserId_WhenCalled_Should_ReturnTypeOfUser()
        {
            Mock<HttpRequestData> mockRequest = CreateMockRequest(_MockLstUsers[0]);
            
            // Act
            var resultUser = UserController.FindUserByUserId(mockRequest.Object, _MockLstUsers[0].Id.ToString(), executionContext);

           // var OkResult = resultUser as HttpStatusCode.OK;

            // Assert
            //Assert.That(resultUser, Is.InstanceOf(HttpRespondeData));
            Assert.AreEqual(200, resultUser.Result.StatusCode);
            //Assert.IsNotNull(resultUser);
            //CollectionAssert.AreEqual(resultUser, _MockLstUsers[0]);
        }

        /*
        [Test]
        public void GetDivesPart2_WhenCalled_Should_Return_TypeOf_Of_OkResult()
        {
            // Act
            var resultDives = DivesController.GetDivesPart2();
            var OkResult = resultDives as OkObjectResult;


            // Assert
            Assert.IsNotNull(resultDives);
            Assert.IsNotNull(OkResult);
            Assert.AreEqual(OkResult.Value, _MockLstDives);
            Assert.AreEqual(200, OkResult.StatusCode);
        }

        [Test]
        public void GetDive_Should_Return_Found_Dive()
        {
            //Act
            var resultDives = DivesController.GetDive(1);
            var OkResult = resultDives as OkObjectResult;

            Assert.IsNotNull(resultDives);
            Assert.AreEqual(200, OkResult.StatusCode);
            Assert.AreEqual(OkResult.Value, _MockLstDives[0]);
        }

        [Test]
        public void GetDive_Should_Return_NotFound_Dive_With_Invalid_Id()
        {
            //Act
            var resultDives = _MockLstDivesController.GetDive(3);
            var NotOkResult = resultDives as NotFoundResult;

            Assert.AreEqual(404, NotOkResult.StatusCode);
        }

        [Test]
        public void Post_ValidDive_Should_Return_CreatedResponse()
        {
            // Arrange
            Dive testItem = new Dive(3, "Karpathos", 25, 200);

            // Act
            IActionResult createdResponse = DivesController.Post(testItem);
            var OkCreated = createdResponse as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(createdResponse);
            Assert.AreEqual(201, OkCreated.StatusCode);
            Assert.IsInstanceOf<CreatedAtActionResult>(createdResponse);
        }

        [Test]
        public void Post_InValidDive_Should_Return_BadRequest()
        {
            // Arrange
            Dive testItem = new Dive(3, "Karpathos", -25, 200);
            DivesController.ModelState.AddModelError("divedepth", "Cannot have a negative value for depth");

            // Act
            IActionResult createdResponse = DivesController.Post(testItem);
            var NotOkCreated = createdResponse as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(createdResponse);
            Assert.AreEqual(400, NotOkCreated.StatusCode);
            Assert.IsInstanceOf<BadRequestObjectResult>(createdResponse);
        }
        */

        [TearDown]
        public void TestCleanUp()
        {
            _MockUserService = null;
        }

        private static Mock<HttpRequestData> CreateMockRequest(object body)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);

            var json = JsonConvert.SerializeObject(body);

            sw.Write(json);
            sw.Flush();

            ms.Position = 0;

            var mockRequest = new Mock<HttpRequestData>();
            mockRequest.Setup(x => x.Body).Returns(ms);

            return mockRequest;
        }
    }
}