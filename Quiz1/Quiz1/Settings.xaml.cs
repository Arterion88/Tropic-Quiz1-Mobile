using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Quiz1
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public static readonly List<string> Languages = new List<string>() { "Čeština", "Slovenština" };

        #region DictionaryKeys
        public const string LanguageKey = "Language";
        public const string RandomKey = "Random";
        #endregion

        public static IDictionary<string, object> Properties = Application.Current.Properties;
       
        public Settings()
        {
            InitializeComponent();

            Languages.ForEach(x => Picker1.Items.Add(x));
            if (Properties.TryGetValue(LanguageKey, out object item))
                Picker1.SelectedItem = item;
            else
                Picker1.SelectedIndex = 0;

        } //Constructor

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Application.Current.Properties[LanguageKey] = Picker1.SelectedItem.ToString();
            Application.Current.SavePropertiesAsync();
        }

        private void OnBtnBackClicked(object sender, EventArgs e) => LoadPage(this, new MainPage());

        public static void LoadPage(Page prevPage,Page page,bool HasNavigationBar=false)
        {
            prevPage.Navigation.PushAsync(page);
            NavigationPage.SetHasNavigationBar(page, HasNavigationBar);
        }
    }
}