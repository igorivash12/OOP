namespace Lab3.Models
{
    public class PCGame : Game
    {
        public string Platform { get; set; }
        public int RequiredRAM { get; set; }

        public override void FillFromForm(Form1 form)
        {
            Name = form.txtName.Text;
            Price = double.Parse(form.txtPrice.Text);
            Genre = form.txtGenre.Text;
            Platform = form.txtExtra1.Text;
            RequiredRAM = int.Parse(form.txtExtra2.Text);
        }

        public override void FillForm(Form1 form)
        {
            form.txtName.Text = Name;
            form.txtPrice.Text = Price.ToString();
            form.txtGenre.Text = Genre;
            form.txtExtra1.Text = Platform;
            form.txtExtra2.Text = RequiredRAM.ToString();

            form.SetLabels("Platform:", "RAM:");
        }

        public override string GetTypeName()
        {
            return "PCGame";
        }
    }
}