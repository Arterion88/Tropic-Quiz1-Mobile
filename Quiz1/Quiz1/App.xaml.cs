using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Quiz1
{
	public partial class App : Application
	{
        public App()
        {
            InitializeComponent();

            MainPage mPage = new MainPage();
            MainPage = new NavigationPage(mPage);
            NavigationPage.SetHasNavigationBar(mPage, false);

            #region Language Initialization
            if(!Settings.Properties.TryGetValue(Settings.LanguageKey, out object language))
                Settings.Properties[Settings.LanguageKey] = language;
            #endregion
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
