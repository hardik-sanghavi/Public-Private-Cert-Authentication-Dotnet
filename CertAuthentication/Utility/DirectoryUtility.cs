namespace CertAuthentication.Utility
{
    public static class DirectoryUtility
    {
        public static string GetMerchantCertDirectory(string merchantId)
        {
            return Path.Combine("Certificates", merchantId);
        }
    }
}
