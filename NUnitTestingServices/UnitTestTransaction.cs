using NUnit.Framework;
using Models;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using Moq;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace NUnitTestingServices
{
    public class UnitTestTransaction
    {
        private Mock<ITransactionRepository> _transactionRepositoryMock;
        private TransactionService _transactionService;
        private List<Transaction> _MockLstTransactions;
        private BlobService _blobService;
        private TransactionDTO _MockTransactionDTO;
        private TransactionUpdateDTO _MockTransactionUpdateDTO;
        private ILogger Logger { get; }

        [SetUp]
        public void Setup()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _blobService = new BlobService((ILogger<BlobService>)Logger);
            _transactionService = new TransactionService((ILogger<TransactionService>)Logger,_transactionRepositoryMock.Object, _blobService);

            _MockLstTransactions = new List<Transaction>();
            Transaction transactionOne = new Transaction(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Kadootjes", "file1.txt", "http://127.0.0.1:10000/devstoreaccount1/powerpuffgirls/Mojojojo.png", 12.45, "was iemand jarig", DateTime.Now, Guid.NewGuid().ToString());
            Transaction transactionTwo = new Transaction(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "school", "file2.txt", "http://127.0.0.1:10000/devstoreaccount1/powerpuffgirls/Mojojojo.png", 12.45, "niewe boeken", DateTime.Now, Guid.NewGuid().ToString());

            _MockTransactionDTO = new TransactionDTO();
            _MockTransactionUpdateDTO = new TransactionUpdateDTO();

            _MockLstTransactions.Add(transactionOne);
            _MockLstTransactions.Add(transactionTwo);
        }
        [Test]
        public void Calling_FindTransactionByFamilyId_ON_ServiceLayer_Should_Call_transactionRepository_and_Return_transactions()
        {
            Guid familyId = _MockLstTransactions[0].FamilyId;
            //Arrange
            _transactionRepositoryMock.Setup(m => m.FindBy(e => e.FamilyId == familyId)).Returns(_MockLstTransactions);

            //act
            IEnumerable<Transaction> result = _transactionService.FindTransactionByFamilyId(familyId);

            //Assert
            Assert.That(result, Is.InstanceOf(typeof(IEnumerable<Transaction>)));
            Assert.IsNotNull(result);
            //Check that the GetSingle method was called once
            _transactionRepositoryMock.Verify(c => c.FindBy(e => e.FamilyId == familyId), Times.Once);
        }

        [TearDown]
        public void TestCleanUp()
        {
            _transactionRepositoryMock = null;
            _MockLstTransactions = null;
        }
    }
}
