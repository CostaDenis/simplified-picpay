
namespace simplified_picpay.DTOs.NotifyDTOs
{
    public class NotifyResponseDTO
    {
        public string Status { get; set; } = string.Empty;
        public NotifyDataDTO Data { get; set; } = new();
    }
}