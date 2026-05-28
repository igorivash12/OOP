using System;
using System.IO;
using System.Security.Cryptography;

namespace Lab6.Core.Plugins
{
    /// <summary>
    /// Result of verifying a plugin manifest against its DLL.
    /// </summary>
    public class SignatureVerificationResult
    {
        public bool IsValid { get; set; }
        public string Reason { get; set; }
    }

    /// <summary>
    /// Helpers used both by the signing tool and by the host to keep the
    /// signing convention in one place: hash the DLL with SHA-256, sign the
    /// canonical payload with RSA-PKCS#1 v1.5 / SHA-256.
    /// </summary>
    public static class PluginSignature
    {
        /// <summary>Computes Base64 SHA-256 hash of a binary file.</summary>
        public static string ComputeFileHash(string filePath)
        {
            using (var sha = SHA256.Create())
            using (var stream = File.OpenRead(filePath))
            {
                var hash = sha.ComputeHash(stream);
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// Signs a plugin DLL with the supplied private RSA key (XML format
        /// produced by RSACryptoServiceProvider.ToXmlString).
        /// </summary>
        public static PluginManifest Sign(string dllPath, string privateKeyXml, DateTime notBeforeUtc, DateTime notAfterUtc, string issuer)
        {
            var manifest = new PluginManifest
            {
                FileName = Path.GetFileName(dllPath),
                Sha256 = ComputeFileHash(dllPath),
                NotBeforeUtc = notBeforeUtc,
                NotAfterUtc = notAfterUtc,
                Issuer = issuer
            };

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKeyXml);
                var signature = rsa.SignData(manifest.BuildSignedPayload(), new SHA256CryptoServiceProvider());
                manifest.Signature = Convert.ToBase64String(signature);
            }

            return manifest;
        }

        /// <summary>
        /// Verifies the manifest against the DLL and the trusted public key.
        /// Returns a structured failure reason so the host can show a useful
        /// message and skip the plugin instead of crashing.
        /// </summary>
        public static SignatureVerificationResult Verify(string dllPath, PluginManifest manifest, string publicKeyXml)
        {
            if (manifest == null)
                return new SignatureVerificationResult { IsValid = false, Reason = "Manifest is missing." };

            // Time window: refuse plugins outside [NotBefore; NotAfter].
            var now = DateTime.UtcNow;
            if (now < manifest.NotBeforeUtc)
                return new SignatureVerificationResult { IsValid = false, Reason = $"Plugin not yet active (valid from {manifest.NotBeforeUtc:u})." };
            if (now > manifest.NotAfterUtc)
                return new SignatureVerificationResult { IsValid = false, Reason = $"Plugin expired on {manifest.NotAfterUtc:u}." };

            // Integrity: SHA-256 of the DLL must match the manifest claim.
            var actualHash = ComputeFileHash(dllPath);
            if (!string.Equals(actualHash, manifest.Sha256, StringComparison.Ordinal))
                return new SignatureVerificationResult { IsValid = false, Reason = "Plugin file hash does not match the manifest (tampered binary)." };

            // Authenticity: RSA signature over the canonical payload.
            try
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(publicKeyXml);
                    var ok = rsa.VerifyData(manifest.BuildSignedPayload(), new SHA256CryptoServiceProvider(), Convert.FromBase64String(manifest.Signature ?? string.Empty));
                    if (!ok)
                        return new SignatureVerificationResult { IsValid = false, Reason = "RSA signature verification failed." };
                }
            }
            catch (Exception ex)
            {
                return new SignatureVerificationResult { IsValid = false, Reason = "Signature verification error: " + ex.Message };
            }

            return new SignatureVerificationResult { IsValid = true, Reason = "OK" };
        }
    }
}
