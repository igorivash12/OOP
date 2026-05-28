namespace Lab5.Models
{
    /// <summary>Represents an online multiplayer game requiring a network connection.</summary>
    public class OnlineGame : Game
    {
        public string ServerRegion { get; set; }
        public int MaxOnlinePlayers { get; set; }

        public override void FillFromForm(Form1 form)
        {
            Name = form.txtName.Text;
            Price = double.Parse(form.txtPrice.Text);
            Genre = form.txtGenre.Text;
            ServerRegion = form.txtExtra1.Text;
            MaxOnlinePlayers = int.Parse(form.txtExtra2.Text);
        }

        public override void FillForm(Form1 form)
        {
            form.txtName.Text = Name;
            form.txtPrice.Text = Price.ToString();
            form.txtGenre.Text = Genre;
            form.txtExtra1.Text = ServerRegion;
            form.txtExtra2.Text = MaxOnlinePlayers.ToString();

            form.SetLabels("Region:", "Max Players:");
        }

        public override string GetTypeName()
        {
            return "OnlineGame";
        }
    }
}
