namespace Lab3.Models
{
    public class VRGame : Game
    {
        public string VRDevice { get; set; }
        public bool RequiresControllers { get; set; }

        public override void FillFromForm(Form1 form)
        {
            Name = form.txtName.Text;
            Price = double.Parse(form.txtPrice.Text);
            Genre = form.txtGenre.Text;
            VRDevice = form.txtExtra1.Text;
            RequiresControllers = bool.Parse(form.txtExtra2.Text);
        }

        public override void FillForm(Form1 form)
        {
            form.txtName.Text = Name;
            form.txtPrice.Text = Price.ToString();
            form.txtGenre.Text = Genre;
            form.txtExtra1.Text = VRDevice;
            form.txtExtra2.Text = RequiresControllers.ToString();

            form.SetLabels("Device:", "Controllers:");
        }

        public override string GetTypeName()
        {
            return "VRGame";
        }
    }
}