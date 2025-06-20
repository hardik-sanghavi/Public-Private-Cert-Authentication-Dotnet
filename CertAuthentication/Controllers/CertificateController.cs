using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CertAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateController : ControllerBase
    {

        [HttpPost("generate")]
        public IActionResult GenerateCertificate([FromQuery] string merchantId)
        {
            using RSA rsa = RSA.Create(2048);

            var req = new CertificateRequest(
                $"CN={merchantId}",
                rsa,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);

            req.CertificateExtensions.Add(
                new X509BasicConstraintsExtension(false, false, 0, false));
            req.CertificateExtensions.Add(
                new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, false));
            req.CertificateExtensions.Add(
                new X509SubjectKeyIdentifierExtension(req.PublicKey, false));

            var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(1));

            // Convert private key to PEM
            string privateKeyPem = ExportPrivateKeyToPEM(rsa);

            // Convert public key (certificate) to PEM
            string publicCertPem = ExportPublicCertToPEM(cert);

            string password = "YourPfxPassword";
            byte[] certPfx = cert.Export(X509ContentType.Pfx, password);

            // Export public certificate only (as .cer)
            byte[] certPublic = cert.Export(X509ContentType.Cert);

            var outputDir = Path.Combine("Certificates", merchantId);
            System.IO.File.WriteAllBytes(Path.Combine(outputDir, $"{merchantId}_myCert.pfx"), certPfx);
            System.IO.File.WriteAllBytes(Path.Combine(outputDir, $"{merchantId}_myCert.cer"), certPublic);

            // Save to files (optional)
            System.IO.File.WriteAllText(Path.Combine(outputDir, $"{merchantId}_privateKey.pem"), privateKeyPem);
            System.IO.File.WriteAllText(Path.Combine(outputDir, $"{merchantId}_publicCert.pem"), publicCertPem);

            return Ok(new
            {
                message = "Certificate generated.",
                privateKey = privateKeyPem,
                publicCertificate = publicCertPem
            });
        }

        private string ExportPrivateKeyToPEM(RSA rsa)
        {
            var builder = new StringBuilder();
            var privateKeyBytes = rsa.ExportPkcs8PrivateKey();
            builder.AppendLine("-----BEGIN PRIVATE KEY-----");
            builder.AppendLine(Convert.ToBase64String(privateKeyBytes, Base64FormattingOptions.InsertLineBreaks));
            builder.AppendLine("-----END PRIVATE KEY-----");
            return builder.ToString();
        }

        private string ExportPublicCertToPEM(X509Certificate2 cert)
        {
            var builder = new StringBuilder();
            builder.AppendLine("-----BEGIN CERTIFICATE-----");
            builder.AppendLine(Convert.ToBase64String(cert.RawData, Base64FormattingOptions.InsertLineBreaks));
            builder.AppendLine("-----END CERTIFICATE-----");
            return builder.ToString();
        }
    }
}
