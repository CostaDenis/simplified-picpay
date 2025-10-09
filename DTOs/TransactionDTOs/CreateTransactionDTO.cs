
namespace simplified_picpay.DTOs.TransactionDTOs
{
    public class CreateTransactionDTO
    {
        public Guid PayerId { get; set; }
        public string PayeePublicId { get; set; } = string.Empty;
        public decimal Value { get; set; }
    }
}