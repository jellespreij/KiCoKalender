using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Models;
using Repositories;
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

        public async Task<Transaction> AddTransaction(Transaction transaction, string localUrl)
        {
            CloudBlobContainer container = await _blobService.GetBlobContainer();

            string[] urlParts = localUrl.Split("\\", System.StringSplitOptions.RemoveEmptyEntries);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("transactions/" + urlParts.Last());

            try
            {
                using (var filestream = System.IO.File.OpenRead(@localUrl))
                {
                    blockBlob.UploadFromStream(filestream);
                }
            }
            catch (StorageException ex)
            {
                Console.WriteLine(ex.Message);
            }

            transaction.Url = blockBlob.Uri.ToString();
            transaction.Name = urlParts.Last();

            return _transactionRepository.Add(transaction).Result;
        }

        public async Task<Transaction> DeleteTransaction(Guid id)
        {
            Transaction transaction = _transactionRepository.GetSingle(id);

            CloudBlobContainer container = await _blobService.GetBlobContainer();

            var blob = container.GetBlobReference("transactions/" + transaction.Name);
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
