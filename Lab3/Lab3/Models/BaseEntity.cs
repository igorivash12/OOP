using System.Windows.Forms;

namespace Lab3.Models
{
    public abstract class BaseEntity
    {
        public string Name { get; set; }

        public abstract void FillFromForm(Form1 form);
        public abstract void FillForm(Form1 form);
        public abstract string GetTypeName();
    }
}