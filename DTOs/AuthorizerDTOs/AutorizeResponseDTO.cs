
namespace simplified_picpay.Dtos.AuthorizerDTOs
{
    public class AutorizeResponseDTO
    {
        public string Status { get; set; } = string.Empty;
        public AuthorizeDataDTO Data { get; set; } = new();
    }
}