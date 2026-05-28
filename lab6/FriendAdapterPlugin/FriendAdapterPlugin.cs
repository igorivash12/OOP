using System;
using System.Windows.Forms;
using FriendPlugin;
using Lab6.Core.Plugins;

namespace FriendAdapterPlugin
{
    /// <summary>
    /// Adapter that lets the host consume a classmate's <see cref="IFriendPlugin"/>
    /// as if it were a regular <see cref="IProcessingPlugin"/>. Translations:
    ///   Caption    -> Name
    ///   IsActive   -> Enabled
    ///   Pack(...)  -> ProcessOnSave(...)
    ///   Unpack(...)-> ProcessOnLoad(...)
    /// The adapter owns no business logic - it only forwards calls.
    /// </summary>
    public class FriendPluginAdapter : IProcessingPlugin
    {
        private readonly IFriendPlugin adaptee;

        public FriendPluginAdapter(IFriendPlugin adaptee)
        {
            this.adaptee = adaptee ?? throw new ArgumentNullException(nameof(adaptee));
        }

        public string Name => adaptee.Caption;
        public string Description => "Adapted classmate plugin";

        public bool Enabled
        {
            get => adaptee.IsActive;
            set => adaptee.IsActive = value;
        }

        public byte[] ProcessOnSave(byte[] input) => adaptee.Pack(input);
        public byte[] ProcessOnLoad(byte[] input) => adaptee.Unpack(input);

        public Control BuildSettingsControl()
        {
            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8) };
            var enabled = new CheckBox { Text = "Enabled", Top = 8, Left = 8, AutoSize = true, Checked = Enabled };
            enabled.CheckedChanged += (s, e) => Enabled = enabled.Checked;
            var label = new Label { Text = "Foreign plugin: " + adaptee.Caption, Top = 40, Left = 8, AutoSize = true };
            panel.Controls.Add(enabled);
            panel.Controls.Add(label);
            return panel;
        }
    }

    /// <summary>
    /// Entry point seen by the host loader. Instantiates the foreign
    /// <see cref="ReverseFriendPlugin"/> and wraps it in the adapter so the
    /// pipeline sees a regular <see cref="IProcessingPlugin"/>.
    /// </summary>
    public class FriendAdapterPluginEntry : IProcessingPlugin
    {
        private readonly FriendPluginAdapter adapter = new FriendPluginAdapter(new ReverseFriendPlugin());

        public string Name => adapter.Name;
        public string Description => adapter.Description;
        public bool Enabled { get => adapter.Enabled; set => adapter.Enabled = value; }
        public byte[] ProcessOnSave(byte[] input) => adapter.ProcessOnSave(input);
        public byte[] ProcessOnLoad(byte[] input) => adapter.ProcessOnLoad(input);
        public Control BuildSettingsControl() => adapter.BuildSettingsControl();
    }
}
