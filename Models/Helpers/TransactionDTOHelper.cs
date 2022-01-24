using System.Collections.Generic;
using System.Linq;

namespace Models.Helpers
{
    public static class TransactionDTOHelper
    {
        public static IEnumerable<TransactionDTO> ToDTO(IEnumerable<Transaction> transactions)
        {
            List<TransactionDTO> transactionDTOs = new();

            foreach (Transaction transaction in transactions) 
            {
                transactionDTOs.Add( new TransactionDTO {
                    Id = transaction.Id,
                    Name = transaction.Name,
                    FileName = transaction.FileName,
                    Description = transaction.Description,
                    Date = transaction.Date,
                    Amount = transaction.Amount,
                    Url = transaction.Url
                });
            }

            return transactionDTOs.AsEnumerable();
        }
    }
}