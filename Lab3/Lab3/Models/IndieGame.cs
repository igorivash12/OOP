namespace Lab3.Models
{
    public class IndieGame : Game
    {
        public string Studio { get; set; }
        public int TeamSize { get; set; }

        public override void FillFromForm(Form1 form)
        {
            Name = form.txtName.Text;
            Price = double.Parse(form.txtPrice.Text);
            Genre = form.txtGenre.Text;
            Studio = form.txtExtra1.Text;
            TeamSize = int.Parse(form.txtExtra2.Text);
        }

        public override void FillForm(Form1 form)
        {
            form.txtName.Text = Name;
            form.txtPrice.Text = Price.ToString();
            form.txtGenre.Text = Genre;
            form.txtExtra1.Text = Studio;
            form.txtExtra2.Text = TeamSize.ToString();

            form.SetLabels("Studio:", "Team:");
        }

        public override string GetTypeName()
        {
            return "IndieGame";
        }
    }
}