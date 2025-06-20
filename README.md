# 🔐 .NET Payload Signer & Verifier Using PEM Certificates

This ASP.NET Core project demonstrates how to:

- ✅ Generate RSA public/private key pair and self-signed X.509 certificate
- ✅ Export keys to `.pem` files (`privateKey.pem`, `publicCert.pem`)
- ✅ Sign string payloads using the private key
- ✅ Verify payload signatures using the public certificate

Built with `System.Security.Cryptography` and compatible with .NET 6, 7, and 8.

---

## 📸 Features

- 🔑 RSA 2048-bit key pair generation
- 📜 PEM file export (PKCS#8 private key and X.509 cert)
- 🖊️ SHA256 payload signing
- 🧾 Signature verification via public certificate
- 🌐 Exposed REST API endpoints for quick use and testing

---

## 📦 Endpoints

### 1. `GET /api/certificate/generate`

Generates and saves:
- `privateKey.pem` (PKCS#8 format)
- `publicCert.pem` (X.509 certificate)

Returns a JSON response containing both keys inline as well.

---

### 2. `POST /api/Merchant/sign-payload`

Signs a payload using the private key.


### 3. `POST /api/Admin/verify-signature`

verify a payload with public key

**Note: Use the same merchantId for all endpoints to test the endpoints.**
