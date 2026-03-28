namespace Lab3.Models
{
    public class ConsoleGame : Game
    {
        public string Console { get; set; }
        public int MaxPlayers { get; set; }

        public override void FillFromForm(Form1 form)
        {
            Name = form.txtName.Text;
            Price = double.Parse(form.txtPrice.Text);
            Genre = form.txtGenre.Text;
            Console = form.txtExtra1.Text;
            MaxPlayers = int.Parse(form.txtExtra2.Text);
        }

        public override void FillForm(Form1 form)
        {
            form.txtName.Text = Name;
            form.txtPrice.Text = Price.ToString();
            form.txtGenre.Text = Genre;
            form.txtExtra1.Text = Console;
            form.txtExtra2.Text = MaxPlayers.ToString();

            form.SetLabels("Console:", "Players:");
        }

        public override string GetTypeName()
        {
            return "ConsoleGame";
        }
    }
}