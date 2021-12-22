using NUnit.Framework;
using Models;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using Moq;
using System.Linq;
using Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace NUnitTestingServices
{
    public class UnitTestAsset
    {
        private Mock<IFolderRepository> _folderRepositoryMock;
        private Mock<IAssetRepository> _assetRepositoryMock;
        private AssetService _assetService;
        private List<Asset> _MockLstAssets;
        private BlobService _blobService;
        private Folder _MockFolder;
        private ILogger Logger { get; }

        [SetUp]
        public void Setup()
        {
            _folderRepositoryMock = new Mock<IFolderRepository>();
            _assetRepositoryMock = new Mock<IAssetRepository>();
            _blobService = new BlobService((ILogger<BlobService>)Logger);
            _assetService = new AssetService((ILogger<AssetService>)Logger, _assetRepositoryMock.Object, _folderRepositoryMock.Object, _blobService);
            _MockLstAssets = new List<Asset>();

            Asset assetOne = new Asset(Guid.NewGuid(), "picture.png", "its a picture", DateTime.Now, "-url-", Guid.NewGuid().ToString());
            _MockLstAssets.Add(assetOne);

            _MockFolder = new Folder(Guid.NewGuid(), "images");
        }

        [Test]
        public void Calling_FindAssetByFolderId_ON_ServiceLayer_Should_Call_UserRepository_and_Return_Assets()
        {
            Guid folderId = _MockFolder.Id;

            //Arrange
            _assetRepositoryMock.Setup(m => m.FindBy(e => e.FolderId == folderId)).Returns(_MockLstAssets); 

            //act
            IEnumerable<Asset> result = _assetService.FindAssetsByFolderId(folderId);

            //Assert
            Assert.That(result, Is.InstanceOf(typeof(IEnumerable<Asset>)));
            Assert.IsNotNull(result);
            //Check that the GetSingle method was called once
            _assetRepositoryMock.Verify(c => c.FindBy(e => e.FolderId == folderId), Times.Once);
        }

        [Test]
        public void Calling_UpdateUserByUserId_ON_ServiceLayer_Should_Call_UserRepository_and_Return_updated_User()
        {
            //Arrange
            _assetRepositoryMock.Setup(m => m.Update(_MockLstAssets[0], _MockLstAssets[0].Id).Result).Returns(_MockLstAssets[0]);

            //act
            Asset result = _assetService.UpdateAsset(_MockLstAssets[0], _MockLstAssets[0].Id);

            //Assert
            Assert.That(result, Is.InstanceOf(typeof(Asset)));

            //Check that the delete method was called once
            _assetRepositoryMock.Verify(c => c.Update(_MockLstAssets[0], _MockLstAssets[0].Id), Times.Once);
        }

        [TearDown]
        public void TestCleanUp()
        {
            _folderRepositoryMock = null;
            _assetRepositoryMock = null;
            _MockLstAssets = null;
        }
    }

    interface IFileSystem
    {
        bool FileExists(string fileName);
        DateTime GetCreationDate(string fileName);
    }
}