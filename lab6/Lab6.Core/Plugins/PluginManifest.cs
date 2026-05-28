using System;
using System.IO;
using System.Xml.Serialization;

namespace Lab6.Core.Plugins
{
    /// <summary>
    /// Side-car file that travels with a plugin DLL. Stores the digital signature
    /// produced by the trusted signer together with activation/expiry timestamps,
    /// so the host can refuse plugins that are either tampered with or used
    /// outside their authorized time window.
    /// </summary>
    [XmlRoot("PluginManifest")]
    public class PluginManifest
    {
        /// <summary>Plugin file name the manifest refers to (no path).</summary>
        public string FileName { get; set; }

        /// <summary>SHA-256 hash of the DLL contents (Base64).</summary>
        public string Sha256 { get; set; }

        /// <summary>RSA signature over the canonical payload (Base64).</summary>
        public string Signature { get; set; }

        /// <summary>UTC moment from which the plugin is considered valid.</summary>
        public DateTime NotBeforeUtc { get; set; }

        /// <summary>UTC moment after which the plugin must be rejected.</summary>
        public DateTime NotAfterUtc { get; set; }

        /// <summary>Free-form signer identity, e.g. e-mail or org name.</summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Canonical bytes that are signed. Includes every field that affects
        /// trust so a single byte change invalidates the signature.
        /// </summary>
        public byte[] BuildSignedPayload()
        {
            var payload = $"{FileName}|{Sha256}|{NotBeforeUtc:o}|{NotAfterUtc:o}|{Issuer}";
            return System.Text.Encoding.UTF8.GetBytes(payload);
        }

        public void Save(string path)
        {
            var serializer = new XmlSerializer(typeof(PluginManifest));
            using (var stream = File.Create(path))
            {
                serializer.Serialize(stream, this);
            }
        }

        public static PluginManifest Load(string path)
        {
            var serializer = new XmlSerializer(typeof(PluginManifest));
            using (var stream = File.OpenRead(path))
            {
                return (PluginManifest)serializer.Deserialize(stream);
            }
        }
    }
}
