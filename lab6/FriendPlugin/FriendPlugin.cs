using System;

namespace FriendPlugin
{
    /// <summary>
    /// Plugin contract designed by a classmate. It is a processing-style
    /// plugin (operates on byte arrays before save and after load), but the
    /// naming and shape are deliberately different from our own
    /// <c>IProcessingPlugin</c>:
    ///   - methods are called <c>Pack</c> / <c>Unpack</c>;
    ///   - the user-facing label sits in <c>Caption</c>, not in <c>Name</c>;
    ///   - there is an extra <c>IsActive</c> flag instead of <c>Enabled</c>.
    /// The host cannot consume this directly, which is what motivates the
    /// adapter in <c>FriendAdapterPlugin</c>.
    /// </summary>
    public interface IFriendPlugin
    {
        string Caption { get; }
        bool IsActive { get; set; }
        byte[] Pack(byte[] data);
        byte[] Unpack(byte[] data);
    }

    /// <summary>
    /// Concrete classmate plugin: reverses the byte array. Reverse is its
    /// own inverse, so <c>Pack</c> and <c>Unpack</c> share the same logic.
    /// Trivial on purpose - the point of this lab is the adapter, not the
    /// transform.
    /// </summary>
    public class ReverseFriendPlugin : IFriendPlugin
    {
        public string Caption => "Friend / Reverse bytes";
        public bool IsActive { get; set; }

        public byte[] Pack(byte[] data) => Reverse(data);
        public byte[] Unpack(byte[] data) => Reverse(data);

        private static byte[] Reverse(byte[] input)
        {
            var output = new byte[input.Length];
            for (int i = 0; i < input.Length; i++) output[i] = input[input.Length - 1 - i];
            return output;
        }
    }
}
