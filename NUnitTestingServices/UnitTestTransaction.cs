﻿using NUnit.Framework;
using Models;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Linq.Expressions;
using System.IO;

namespace NUnitTestingServices
{
    public class UnitTestTransaction
    {
        private Mock<ITransactionRepository> _transactionRepositoryMock;
        private TransactionService _transactionService;
        private List<Transaction> _MockLstTransactions;
        private BlobService _blobService;

        [SetUp]
        public void Setup()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _blobService = new BlobService();
            _transactionService = new TransactionService(_transactionRepositoryMock.Object, _blobService);

            _MockLstTransactions = new List<Transaction>();
            Transaction transactionOne = new Transaction(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Kadootjes", "http://127.0.0.1:10000/devstoreaccount1/powerpuffgirls/Mojojojo.png", 12.45, "was iemand jarig", DateTime.Now, Guid.NewGuid().ToString());
            Transaction transactionTwo = new Transaction(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "school", "http://127.0.0.1:10000/devstoreaccount1/powerpuffgirls/Mojojojo.png", 12.45, "niewe boeken", DateTime.Now, Guid.NewGuid().ToString());
            
            _MockLstTransactions.Add(transactionOne);
            _MockLstTransactions.Add(transactionTwo);
        }

        [Test]
        public void Calling_AddTransaction_ON_ServiceLayer_Should_Call_transactionRepository_and_Add_single_transaction()
        {
            //Arrange
            string localUrl = "-url-";
            _transactionRepositoryMock.Setup(m => m.Add(_MockLstTransactions[0]).Result).Throws(new Exception());

            //act
            Transaction result = _transactionService.AddTransaction(_MockLstTransactions[0], localUrl).Result;

            //Assert
            Assert.That(result, Is.InstanceOf(typeof(Transaction)));
            Assert.Throws<FileNotFoundException>();

            //Check that the GetAll method was called once
            //_transactionRepositoryMock.Verify(c => c.Add(_MockLstTransactions[0]).Result, Times.Once);
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

        [Test]
        public void Calling_DeleteTransaction_ON_ServiceLayer_Should_Call_transactionRepository_and_Return_deleted_Transaction()
        {
            Guid Id = _MockLstTransactions[0].Id;
            //Arrange 
            _transactionRepositoryMock.Setup(m => m.GetSingle(Id)).Returns(_MockLstTransactions[0]);
            _transactionRepositoryMock.Setup(m => m.Delete(Id).Result).Returns(_MockLstTransactions[0]);

            //act
            Transaction result = _transactionService.DeleteTransaction(Id).Result;

            //Assert
            Assert.That(result, Is.InstanceOf(typeof(Transaction)));

            //Check that the delete method was called once
            _transactionRepositoryMock.Verify(c => c.Delete(Id), Times.Once);
        }

        [Test]
        public void Calling_UpdateTransaction_ON_ServiceLayer_Should_Call_TransactionRepository_and_Return_updated_Transaction()
        {
            //Arrange
            _transactionRepositoryMock.Setup(m => m.Update(_MockLstTransactions[0], _MockLstTransactions[0].Id).Result).Returns(_MockLstTransactions[0]);

            //act
            Transaction result = _transactionService.UpdateTransaction(_MockLstTransactions[0], _MockLstTransactions[0].Id);

            //Assert
            Assert.That(result, Is.InstanceOf(typeof(Transaction)));

            //Check that the delete method was called once
            _transactionRepositoryMock.Verify(c => c.Update(_MockLstTransactions[0], _MockLstTransactions[0].Id), Times.Once);
        }

        [TearDown]
        public void TestCleanUp()
        {
            _transactionRepositoryMock = null;
            _MockLstTransactions = null;
        }
    }
}
