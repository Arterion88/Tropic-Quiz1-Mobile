using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Quiz1
{
	public partial class MainPage : ContentPage
	{
        /// <summary>
        /// Constructor
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        void OnButtonStartClicked(object sender, EventArgs args)
        {
            Settings.LoadPage(this, new Page3("Test0"));
        }

        void OnButtonSettingsClicked(object sender, EventArgs args)
        {
            Settings.LoadPage(this, new Settings());
        }
    }
}
