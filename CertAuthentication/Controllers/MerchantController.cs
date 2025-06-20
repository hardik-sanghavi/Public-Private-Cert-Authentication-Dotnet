using CertAuthentication.DTO;
using CertAuthentication.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace CertAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantController : ControllerBase
    {
        [HttpPost("sign-payload")]
        public IActionResult SignPayload([FromBody] SignPayloadRequst signPayloadRequst)
        {
            var outputDir = DirectoryUtility.GetMerchantCertDirectory(signPayloadRequst.MerchantId);

            // Read private key from PEM file (you can also store in secret store or DB)
            var privateKeyPem = System.IO.File.ReadAllText(Path.Combine(outputDir, $"{signPayloadRequst.MerchantId}_privateKey.pem"));

            // Sign payload
            string signature = SignatureUtility.RSASignPayload(signPayloadRequst.Payload, privateKeyPem);

            return Ok(new
            {
                signPayloadRequst.Payload,
                signature
            });
        }        
    }
}
