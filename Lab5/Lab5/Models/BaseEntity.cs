using System.Windows.Forms;

namespace Lab5.Models
{
    /// <summary>Root of the entity hierarchy; ties models to the main form UI.</summary>
    public abstract class BaseEntity
    {
        public string Name { get; set; }

        /// <summary>Reads user input from form controls into this entity.</summary>
        public abstract void FillFromForm(Form1 form);

        /// <summary>Writes entity data to form controls and sets dynamic labels.</summary>
        public abstract void FillForm(Form1 form);

        /// <summary>Returns the factory/serialization type key for this instance.</summary>
        public abstract string GetTypeName();
    }
}
