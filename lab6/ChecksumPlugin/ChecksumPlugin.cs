using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using Lab6.Core.Plugins;

namespace ChecksumPlugin
{
    /// <summary>
    /// Variant-5 plugin: prepends a length-prefixed cryptographic hash to the
    /// saved stream and re-verifies it on load. Detects accidental file
    /// corruption вЂ” any single bit change in the payload makes the load fail
    /// loudly instead of silently returning damaged data.
    ///
    /// On-disk layout (all little-endian):
    ///   [int32 hashLen][hashLen bytes hash][rest = payload]
    /// </summary>
    public class ChecksumProcessingPlugin : IProcessingPlugin
    {
        public string Name => "Checksum";
        public string Description => "Adds a hash checksum and verifies it on load.";
        // Default to enabled so the Decorator log fills up on the very first
        // save without the user having to remember to tick the checkbox.
        public bool Enabled { get; set; } = true;

        /// <summary>Hash algorithm chosen by the user in settings.</summary>
        public string Algorithm { get; set; } = "SHA256";

        private HashAlgorithm CreateHash()
        {
            switch ((Algorithm ?? "SHA256").ToUpperInvariant())
            {
                case "MD5": return MD5.Create();
                case "SHA1": return SHA1.Create();
                case "SHA512": return SHA512.Create();
                default: return SHA256.Create();
            }
        }

        public byte[] ProcessOnSave(byte[] input)
        {
            using (var hasher = CreateHash())
            {
                var hash = hasher.ComputeHash(input);
                using (var output = new MemoryStream())
                using (var writer = new BinaryWriter(output))
                {
                    writer.Write(hash.Length);
                    writer.Write(hash);
                    writer.Write(input);
                    return output.ToArray();
                }
            }
        }

        public byte[] ProcessOnLoad(byte[] input)
        {
            using (var inStream = new MemoryStream(input))
            using (var reader = new BinaryReader(inStream))
            {
                int hashLen = reader.ReadInt32();
                var hashStored = reader.ReadBytes(hashLen);
                var payload = reader.ReadBytes((int)(inStream.Length - inStream.Position));
                using (var hasher = CreateHash())
                {
                    var actual = hasher.ComputeHash(payload);
                    if (!ByteArraysEqual(actual, hashStored))
                        throw new InvalidDataException("Checksum mismatch: file is corrupted or modified.");
                }
                return payload;
            }
        }

        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
                if (a[i] != b[i]) return false;
            return true;
        }

        public Control BuildSettingsControl()
        {
            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8) };

            var enabled = new CheckBox { Text = "Enabled", Top = 8, Left = 8, AutoSize = true, Checked = Enabled };
            enabled.CheckedChanged += (s, e) => Enabled = enabled.Checked;

            var label = new Label { Text = "Hash algorithm:", Top = 40, Left = 8, AutoSize = true };
            var combo = new ComboBox { Top = 60, Left = 8, Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            combo.Items.AddRange(new object[] { "MD5", "SHA1", "SHA256", "SHA512" });
            combo.SelectedItem = Algorithm;
            combo.SelectedIndexChanged += (s, e) => Algorithm = (string)combo.SelectedItem;

            panel.Controls.Add(enabled);
            panel.Controls.Add(label);
            panel.Controls.Add(combo);
            return panel;
        }
    }
}
