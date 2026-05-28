using System;
using System.IO;
using System.Security.Cryptography;
using Lab6.Core.Plugins;

namespace PluginSigner
{
    /// <summary>
    /// Command-line utility used to (1) generate an RSA key pair, (2) sign a
    /// plugin DLL producing the side-car *.manifest.xml, (3) verify an
    /// existing manifest.
    ///
    /// Usage:
    ///     PluginSigner.exe genkeys &lt;private.xml&gt; &lt;public.xml&gt;
    ///     PluginSigner.exe sign    &lt;plugin.dll&gt; &lt;private.xml&gt; &lt;notBeforeUtc&gt; &lt;notAfterUtc&gt; &lt;issuer&gt;
    ///     PluginSigner.exe verify  &lt;plugin.dll&gt; &lt;public.xml&gt;
    /// </summary>
    internal static class Program
    {
        private static int Main(string[] args)
        {
            try
            {
                if (args.Length == 0) { PrintUsage(); return 1; }

                switch (args[0].ToLowerInvariant())
                {
                    case "genkeys":
                        return GenKeys(args);
                    case "sign":
                        return Sign(args);
                    case "verify":
                        return Verify(args);
                    default:
                        PrintUsage();
                        return 1;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("ERROR: " + ex.Message);
                return 2;
            }
        }

        /// <summary>Creates a fresh RSA 2048 key pair and saves both XMLs.</summary>
        private static int GenKeys(string[] args)
        {
            if (args.Length < 3) { PrintUsage(); return 1; }
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                File.WriteAllText(args[1], rsa.ToXmlString(true));
                File.WriteAllText(args[2], rsa.ToXmlString(false));
            }
            Console.WriteLine($"Generated key pair: {args[1]} (private), {args[2]} (public)");
            return 0;
        }

        /// <summary>Signs the DLL and writes the matching .manifest.xml.</summary>
        private static int Sign(string[] args)
        {
            if (args.Length < 6) { PrintUsage(); return 1; }
            var dll = args[1];
            var privKeyXml = File.ReadAllText(args[2]);
            var notBefore = DateTime.Parse(args[3]).ToUniversalTime();
            var notAfter = DateTime.Parse(args[4]).ToUniversalTime();
            var issuer = args[5];

            var manifest = PluginSignature.Sign(dll, privKeyXml, notBefore, notAfter, issuer);
            var manifestPath = dll + ".manifest.xml";
            manifest.Save(manifestPath);
            Console.WriteLine($"Signed. Manifest written to {manifestPath}");
            return 0;
        }

        /// <summary>Re-verifies a signed plugin to confirm trust is intact.</summary>
        private static int Verify(string[] args)
        {
            if (args.Length < 3) { PrintUsage(); return 1; }
            var dll = args[1];
            var publicKeyXml = File.ReadAllText(args[2]);
            var manifest = PluginManifest.Load(dll + ".manifest.xml");
            var result = PluginSignature.Verify(dll, manifest, publicKeyXml);
            Console.WriteLine(result.IsValid ? "VALID" : "INVALID: " + result.Reason);
            return result.IsValid ? 0 : 3;
        }

        private static void PrintUsage()
        {
            Console.WriteLine("PluginSigner usage:");
            Console.WriteLine("  PluginSigner genkeys <private.xml> <public.xml>");
            Console.WriteLine("  PluginSigner sign    <plugin.dll> <private.xml> <notBeforeUtc> <notAfterUtc> <issuer>");
            Console.WriteLine("  PluginSigner verify  <plugin.dll> <public.xml>");
            Console.WriteLine();
            Console.WriteLine("Example dates (ISO 8601): 2026-01-01T00:00:00Z");
        }
    }
}
