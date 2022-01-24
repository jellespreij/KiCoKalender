using HttpMultipartParser;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITransactionService
    {
        Transaction FindTransactionByTransactionId(Guid transactionId);
        IEnumerable<Transaction> FindTransactionByFamilyId(Guid familyId);
        IEnumerable<TransactionDTO> FindTransactionDTOByFamilyId(Guid familyId);
        Task<Transaction> AddTransaction(FilePart file, Transaction transaction);
        Task<Transaction> DeleteTransaction(Guid id);
        Transaction UpdateTransaction(TransactionUpdateDTO transactionUpdate, Guid id);
    }
}
