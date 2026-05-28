using System;
using System.Diagnostics;
using System.Windows.Forms;
using Lab6.Core.Plugins;

namespace Lab6.Core.Patterns
{
    /// <summary>
    /// Decorator that wraps any <see cref="IProcessingPlugin"/> and emits a
    /// log line with byte counts and elapsed milliseconds on every save and
    /// load call. The inner plugin is unaware it is being instrumented.
    ///
    /// Pattern justification: we want optional, composable instrumentation
    /// (timing, byte counts, error capture) without touching the dozen+
    /// individual plugin implementations. Inheritance would require editing
    /// each one; a Decorator preserves the original contract and lets us
    /// stack additional decorators later (encryption-counter, audit-trail,
    /// etc.) without breaking polymorphism.
    /// </summary>
    public class LoggingProcessingPlugin : IProcessingPlugin
    {
        private readonly IProcessingPlugin inner;
        private readonly Action<string> sink;

        public LoggingProcessingPlugin(IProcessingPlugin inner, Action<string> sink)
        {
            this.inner = inner ?? throw new ArgumentNullException(nameof(inner));
            this.sink = sink ?? (_ => { });
        }

        public string Name => inner.Name;
        public string Description => inner.Description + " (logged)";

        public bool Enabled
        {
            get => inner.Enabled;
            set => inner.Enabled = value;
        }

        public byte[] ProcessOnSave(byte[] input)
        {
            var sw = Stopwatch.StartNew();
            var result = inner.ProcessOnSave(input);
            sw.Stop();
            sink($"[{inner.Name}] save: {input.Length} -> {result.Length} bytes in {sw.ElapsedMilliseconds} ms");
            return result;
        }

        public byte[] ProcessOnLoad(byte[] input)
        {
            var sw = Stopwatch.StartNew();
            var result = inner.ProcessOnLoad(input);
            sw.Stop();
            sink($"[{inner.Name}] load: {input.Length} -> {result.Length} bytes in {sw.ElapsedMilliseconds} ms");
            return result;
        }

        public Control BuildSettingsControl() => inner.BuildSettingsControl();
    }
}
