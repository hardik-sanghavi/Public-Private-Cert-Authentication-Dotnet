using CertAuthentication.DTO;
using CertAuthentication.Utility;
using Microsoft.AspNetCore.Mvc;

namespace CertAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        [HttpPost("verify-signature")]
        public IActionResult VerifySignature([FromBody] VerifySignatureRequest request)
        {
            var outputDir = DirectoryUtility.GetMerchantCertDirectory(request.MerchantId);
            var publicCertPem = System.IO.File.ReadAllText(Path.Combine(outputDir, $"{request.MerchantId}_publicCert.pem"));
            bool isValid = SignatureUtility.VerifySignature(request.Payload, request.Signature, publicCertPem);

            return Ok(new
            {
                request.Payload,
                request.Signature,
                isValid
            });
        }
    }
}
