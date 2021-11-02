using HttpMultipartParser;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ITransactionService
    {
        IEnumerable<Transaction> FindTransactionByFamilyId(Guid familyId);
        Task<Transaction> AddTransaction(FilePart file, Transaction transaction);
        Task<Transaction> DeleteTransaction(Guid id);
        Transaction UpdateTransaction(Transaction transaction, Guid id);
    }
}
