namespace CertAuthentication.DTO
{
    public class VerifySignatureRequest
    {
        public string MerchantId { get; set; }
        public string Payload { get; set; }
        public string Signature { get; set; }
    }
}
