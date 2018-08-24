using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using QuizLibrary;

namespace Quiz1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScorePage : ContentPage
    {
        List<Question> Questions;
        public ScorePage(List<Question> questions)
        {
            InitializeComponent();

            this.Questions = questions;
            label2.Text = "Dokončil jsi test" + Environment.NewLine + Environment.NewLine + "Odpověděl jsi správně na " + Questions.FindAll(x => x.IsCorrectAnswer == true).Count + " ze " + questions.Count + " odpovědí!";
        }

        void OnRevisionButtonClicked(object sender, EventArgs args) => Settings.LoadPage(this, new Page3(Questions)); // Load Quiz for control

        void OnMMButtonClicked(object sender, EventArgs args) => Settings.LoadPage(this, new MainPage()); //Load Main Page

        protected override bool OnBackButtonPressed() => true; //Disable Back Button
    }
}