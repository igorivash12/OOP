using Lab5;
using Lab5.Models;

namespace Plugin.BoardGame
{
    /// <summary>
    /// Plugin-provided game type: a physical board game in the hierarchy under Game.
    /// </summary>
    public class BoardGame : Game
    {
        public string Publisher { get; set; }
        public int PlayerCount { get; set; }

        /// <summary>Reads common and board-specific fields from the main form controls.</summary>
        public override void FillFromForm(Form1 form)
        {
            Name        = form.txtName.Text;
            Price       = double.Parse(form.txtPrice.Text);
            Genre       = form.txtGenre.Text;
            Publisher   = form.txtExtra1.Text;
            PlayerCount = int.Parse(form.txtExtra2.Text);
        }

        /// <summary>Writes entity data back to the form and sets dynamic field labels.</summary>
        public override void FillForm(Form1 form)
        {
            form.txtName.Text   = Name;
            form.txtPrice.Text  = Price.ToString();
            form.txtGenre.Text  = Genre;
            form.txtExtra1.Text = Publisher;
            form.txtExtra2.Text = PlayerCount.ToString();

            form.SetLabels("Publisher:", "Players:");
        }

        /// <summary>Returns the type key used by GameFactory and JSON serialization.</summary>
        public override string GetTypeName()
        {
            return "BoardGame";
        }
    }
}
