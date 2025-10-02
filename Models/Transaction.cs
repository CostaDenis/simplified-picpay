using System.Text.Json.Serialization;

namespace simplified_picpay.Models
{
    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PayerId { get; set; }
        public string PayerPublicId { get; set; } = string.Empty;

        [JsonIgnore]
        public Account Payer { get; set; } = default!;
        public Guid PayeeId { get; set; }
        public string PayeePublicId { get; set; } = string.Empty;

        [JsonIgnore]
        public Account Payee { get; set; } = default!;
        public decimal Value { get; set; }
    }
}