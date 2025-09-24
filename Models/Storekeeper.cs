namespace simplified_picpay.Models
{
    public class Storekeeper
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AccountId { get; set; }
        public Account Account { get; set; } = default!;
        public string CNPJ { get; set; } = string.Empty;
    }
}