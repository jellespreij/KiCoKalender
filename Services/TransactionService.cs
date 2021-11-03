using HttpMultipartParser;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Models;
using Repositories;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TransactionService : ITransactionService
    {
        private ITransactionRepository _transactionRepository;
        private BlobService _blobService;

        public TransactionService(ITransactionRepository transactionRepository, BlobService blobService)
        {
            _transactionRepository = transactionRepository;
            _blobService = blobService;
        }

        public async Task<Transaction> AddTransaction(FilePart file, Transaction transaction)
        {
            CloudBlobContainer container = await _blobService.GetBlobContainer();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("transactions/" + file.FileName);

            try
            {
                using (var filestream = file.Data)
                {
                    blockBlob.UploadFromStream(filestream);
                }
            }
            catch (StorageException ex)
            {
                Console.WriteLine(ex.Message);
            }
             
            transaction.FileName = file.FileName;
            transaction.Url = blockBlob.Uri.ToString();

            return _transactionRepository.Add(transaction).Result;
        }

        public async Task<Transaction> DeleteTransaction(Guid id)
        {
            Transaction transaction = _transactionRepository.GetSingle(id);

            CloudBlobContainer container = await _blobService.GetBlobContainer();

            var blob = container.GetBlobReference("transactions/" + transaction.FileName);
            await blob.DeleteIfExistsAsync();

            return _transactionRepository.Delete(id).Result; 
        }

        public IEnumerable<Transaction> FindTransactionByFamilyId(Guid familyId)
        {
            return _transactionRepository.FindBy(e => e.FamilyId == familyId);
        }

        public Transaction UpdateTransaction(Transaction transaction, Guid id)
        {
            return _transactionRepository.Update(transaction, id).Result;
        }

    }
}
