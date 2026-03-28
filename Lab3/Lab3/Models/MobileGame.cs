namespace Lab3.Models
{
    public class MobileGame : Game
    {
        public string OS { get; set; }
        public bool HasAds { get; set; }

        public override void FillFromForm(Form1 form)
        {
            Name = form.txtName.Text;
            Price = double.Parse(form.txtPrice.Text);
            Genre = form.txtGenre.Text;
            OS = form.txtExtra1.Text;
            HasAds = bool.Parse(form.txtExtra2.Text);
        }

        public override void FillForm(Form1 form)
        {
            form.txtName.Text = Name;
            form.txtPrice.Text = Price.ToString();
            form.txtGenre.Text = Genre;
            form.txtExtra1.Text = OS;
            form.txtExtra2.Text = HasAds.ToString();

            form.SetLabels("OS:", "Ads:");
        }

        public override string GetTypeName()
        {
            return "MobileGame";
        }
    }
}