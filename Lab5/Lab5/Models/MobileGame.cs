namespace Lab5.Models
{
    /// <summary>Represents a game released for mobile devices.</summary>
    public class MobileGame : Game
    {
        public string Platform { get; set; }
        public bool HasInAppPurchases { get; set; }

        public override void FillFromForm(Form1 form)
        {
            Name = form.txtName.Text;
            Price = double.Parse(form.txtPrice.Text);
            Genre = form.txtGenre.Text;
            Platform = form.txtExtra1.Text;
            HasInAppPurchases = bool.Parse(form.txtExtra2.Text);
        }

        public override void FillForm(Form1 form)
        {
            form.txtName.Text = Name;
            form.txtPrice.Text = Price.ToString();
            form.txtGenre.Text = Genre;
            form.txtExtra1.Text = Platform;
            form.txtExtra2.Text = HasInAppPurchases.ToString();

            form.SetLabels("Platform:", "In-App:");
        }

        public override string GetTypeName()
        {
            return "MobileGame";
        }
    }
}
