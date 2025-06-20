using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CertAuthentication.Utility
{
    public class SignatureUtility
    {
        public static string RSASignPayload(string payload, string privateKeyPem)
        {
            // Load RSA private key from PEM
            using RSA rsa = RSA.Create();
            rsa.ImportFromPem(privateKeyPem.ToCharArray());

            // Convert payload to bytes
            byte[] dataBytes = Encoding.UTF8.GetBytes(payload);

            // Sign using SHA256
            byte[] signature = rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            // Return Base64 signature
            return Convert.ToBase64String(signature);
        }

        public static bool VerifySignature(string payload, string signatureBase64, string publicCertPem)
        {
            
            // Load public key from PEM certificate
            var cert = new X509Certificate2(Encoding.UTF8.GetBytes(publicCertPem));

            using RSA rsa = cert.GetRSAPublicKey(); // ✅ Use the built-in method to extract RSA public key

            if (rsa == null)
                throw new InvalidOperationException("Failed to extract public key from certificate.");

            //rsa.ImportFromPem(cert.GetPublicKeyString().ToCharArray());

            byte[] dataBytes = Encoding.UTF8.GetBytes(payload);
            byte[] signatureBytes = Convert.FromBase64String(signatureBase64);

            return rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}
