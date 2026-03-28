namespace Lab3.Models
{
    public class OnlineGame : Game
    {
        public int ServerCount { get; set; }
        public bool Subscription { get; set; }

        public override void FillFromForm(Form1 form)
        {
            Name = form.txtName.Text;
            Price = double.Parse(form.txtPrice.Text);
            Genre = form.txtGenre.Text;
            ServerCount = int.Parse(form.txtExtra1.Text);
            Subscription = bool.Parse(form.txtExtra2.Text);
        }

        public override void FillForm(Form1 form)
        {
            form.txtName.Text = Name;
            form.txtPrice.Text = Price.ToString();
            form.txtGenre.Text = Genre;
            form.txtExtra1.Text = ServerCount.ToString();
            form.txtExtra2.Text = Subscription.ToString();

            form.SetLabels("Servers:", "Subscription:");
        }

        public override string GetTypeName()
        {
            return "OnlineGame";
        }
    }
}