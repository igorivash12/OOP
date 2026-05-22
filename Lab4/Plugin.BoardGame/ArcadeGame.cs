using Lab4;
using Lab4.Models;

namespace Plugin.BoardGame
{
    /// <summary>
    /// Plugin-provided game type: classic arcade cabinet game in the Game hierarchy.
    /// </summary>
    public class ArcadeGame : Game
    {
        public string CabinetModel { get; set; }
        public int YearReleased { get; set; }

        /// <summary>Reads common and arcade-specific fields from the main form controls.</summary>
        public override void FillFromForm(Form1 form)
        {
            Name = form.txtName.Text;
            Price = double.Parse(form.txtPrice.Text);
            Genre = form.txtGenre.Text;
            CabinetModel = form.txtExtra1.Text;
            YearReleased = int.Parse(form.txtExtra2.Text);
        }

        /// <summary>Writes entity data back to the form and sets dynamic field labels.</summary>
        public override void FillForm(Form1 form)
        {
            form.txtName.Text = Name;
            form.txtPrice.Text = Price.ToString();
            form.txtGenre.Text = Genre;
            form.txtExtra1.Text = CabinetModel;
            form.txtExtra2.Text = YearReleased.ToString();

            form.SetLabels("Cabinet:", "Year:");
        }

        /// <summary>Returns the type key used by GameFactory and JSON serialization.</summary>
        public override string GetTypeName()
        {
            return "ArcadeGame";
        }
    }
}
