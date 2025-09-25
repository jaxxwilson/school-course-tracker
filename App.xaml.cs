using SCT.Views;

namespace SCT
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var main = new Terms();
            var navPage = new NavigationPage(main)
            {
                BarBackgroundColor = Colors.LightBlue,
                BarTextColor = Colors.White
            };
            MainPage = navPage;
        }
    }
}
