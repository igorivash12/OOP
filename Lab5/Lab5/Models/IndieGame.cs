namespace Lab5.Models
{
    /// <summary>Represents an independently developed game.</summary>
    public class IndieGame : Game
    {
        public string Developer { get; set; }
        public bool IsSoloProject { get; set; }

        public override void FillFromForm(Form1 form)
        {
            Name = form.txtName.Text;
            Price = double.Parse(form.txtPrice.Text);
            Genre = form.txtGenre.Text;
            Developer = form.txtExtra1.Text;
            IsSoloProject = bool.Parse(form.txtExtra2.Text);
        }

        public override void FillForm(Form1 form)
        {
            form.txtName.Text = Name;
            form.txtPrice.Text = Price.ToString();
            form.txtGenre.Text = Genre;
            form.txtExtra1.Text = Developer;
            form.txtExtra2.Text = IsSoloProject.ToString();

            form.SetLabels("Developer:", "Solo:");
        }

        public override string GetTypeName()
        {
            return "IndieGame";
        }
    }
}
