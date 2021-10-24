using NUnit.Framework;
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
    public class UnitTestAppointment
    {
        private Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private AppointmentService _appointmentService;
        private List<Appointment> _MockLstAppointment;

        [SetUp]
        public void Setup()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _appointmentService = new AppointmentService(_appointmentRepositoryMock.Object);

            Guid familyGuid = Guid.NewGuid();

            _MockLstAppointment = new List<Appointment>();
            Appointment appointmentOne = new Appointment(Guid.NewGuid(), familyGuid, Guid.NewGuid(), Invited.everyone, Accepted.accepted, "Tandarts", false, "Wortelkanaal behandeling", DateTime.Now, Guid.NewGuid().ToString());
            Appointment appointmentTwo = new Appointment(Guid.NewGuid(), familyGuid, Guid.NewGuid(), Invited.everyone, Accepted.accepted, "Feest",  false, "Feestje bij truus", DateTime.Now,  Guid.NewGuid().ToString());

            _MockLstAppointment.Add(appointmentOne);
            _MockLstAppointment.Add(appointmentTwo);
        }

        [Test]
        public void Calling_AddAppointment_ON_ServiceLayer_Should_Call_AppointmentRepository_and_Add_single_Appointment()
        {
            //Arrange
            _appointmentRepositoryMock.Setup(m => m.Add(_MockLstAppointment[0]).Result).Returns(_MockLstAppointment[0]);

            //act
            Appointment result = _appointmentService.AddAppointment(_MockLstAppointment[0]);

            //Assert
            Assert.That(result, Is.InstanceOf(typeof(Appointment)));

            //Check that the add method was called once
            _appointmentRepositoryMock.Verify(c => c.Add(_MockLstAppointment[0]).Result, Times.Once);
        }

        [Test]
        public void Calling_DeleteAppoint_ON_ServiceLayer_Should_Call_AppointmentRepository_and_Return_Deleted_Appointment()
        {
            //Arrange
            _appointmentRepositoryMock.Setup(m => m.Delete(_MockLstAppointment[1].Id).Result).Returns(_MockLstAppointment[1]);

            //act
            Appointment result = _appointmentService.DeleteAppointment(_MockLstAppointment[1].Id);

            //Assert
            Assert.That(result, Is.InstanceOf(typeof(Appointment)));

            //Check that the delete method was called once
            _appointmentRepositoryMock.Verify(c => c.Delete(_MockLstAppointment[1].Id).Result, Times.Once);
        }

        [Test]
        public void Calling_FindAppointmentByFamilyIdAndUserId_ON_ServiceLayer_Should_Call_AppointmentRepository_and_Return_list_Appointment()
        {
            Guid userId = _MockLstAppointment[0].UserId;
            Guid familyId = _MockLstAppointment[0].FamilyId;

            //Arrange
            _appointmentRepositoryMock.Setup(m => m.FindBy(e => e.FamilyId == familyId && (e.Privacy == false || (e.UserId == userId && e.Privacy == true)))).Returns(_MockLstAppointment);

            //act
            IEnumerable<Appointment> result = _appointmentService.FindAppointmentByFamilyIdAndUserId(familyId, userId);

            //Assert
            //Assert.That(result, Is.InstanceOf(typeof(IEnumerable<Appointment>)));
            Assert.AreEqual(result.Count(), 2);

            //Check that the find method was called once
            _appointmentRepositoryMock.Verify(c => c.FindBy(e => e.FamilyId == familyId && (e.Privacy == false || (e.UserId == userId && e.Privacy == true))), Times.Once);
        }

        [Test]
        public void Calling_UpdateAppointment_ON_ServiceLayer_Should_Call_ApointmentRepository_and_Return_updated_Appointment()
        {
            //Arrange
            _appointmentRepositoryMock.Setup(m => m.Update(_MockLstAppointment[1], _MockLstAppointment[1].Id).Result).Returns(_MockLstAppointment[1]);

            //act
            Appointment result = _appointmentService.UpdateAppointment(_MockLstAppointment[1], _MockLstAppointment[1].Id);

            //Assert
            Assert.That(result, Is.InstanceOf(typeof(Appointment)));

            //Check that the update method was called once
            _appointmentRepositoryMock.Verify(c => c.Update(_MockLstAppointment[1], _MockLstAppointment[1].Id), Times.Once);
        }

        [TearDown]
        public void TestCleanUp()
        {
            _appointmentRepositoryMock = null;
            _MockLstAppointment = null;
        }
    }
}
