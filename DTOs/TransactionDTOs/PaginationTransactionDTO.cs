
namespace simplified_picpay.DTOs.TransactionDTOs
{
    public class PaginationTransactionDTO
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;
    }
}