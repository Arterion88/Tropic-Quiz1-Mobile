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
    public partial class Page1 : ContentPage
    {
        #region Variables

        #region Questions
        public List<Question> Questions;
        public int currentQuestionNumber = 0;
        public int CurrentQuestionNumber
        {
            get { return currentQuestionNumber; }
            set
            {
                if (value >= Questions.Count)
                    value = -1;
                currentQuestionNumber = value;
            }
        }
        public Question CurrentQ { get { return Questions[CurrentQuestionNumber]; } }
        #endregion

        #region Constants
        const string Answer = "answer";
        #endregion

        public List<Button> BtnAnswers
        {
            get
            {
                List<Button> result = new List<Button>();
                for (int i = 0; i < CurrentQ.Answers.Count; i++)
                    result.Add(this.FindByName<Button>(Answer + i));
                return result;
            }
        }

        public string SelectedAnswer
        {
            get
            {
                string result = "";
                for (int i = 0; i < BtnAnswers.Count; i++)
                    if (!BtnAnswers[i].IsEnabled) result = BtnAnswers.Where(x => x.IsEnabled).ToList()[0].Text;
                return result;
            }
        }

        public string TestName { get { return Questions[0].TestName; } }

        public bool Revision { get; set; }

        #endregion

        #region Constructors
        public Page1(string testName, bool randomize)
        {
            InitializeComponent();

            object obj = Application.Current.Resources[testName];
            List<string> test = new List<string>((string[])obj);
            Questions = Question.GetQuestionsFromResource(test, randomize);

            this.Revision = false;

            CurrentQuestionNumber = 0;
            LoadQuestion();

        }

        public Page1(List<Question> questions)
        {
            InitializeComponent();

            Questions = questions;

            this.Revision = true;
            btnFinish.Text = "Zpět do Menu";

            CurrentQuestionNumber = 0;
            LoadQuestion();
        }
        #endregion

        #region Button Events

        void OnBtnBackClicked(object sender, EventArgs args)
        {
            if (CurrentQuestionNumber == 0)
                return;
            CurrentQuestionNumber--;
            LoadQuestion();
        }

        void OnBtnNextClicked(object sender, EventArgs args)
        {
            if (CurrentQuestionNumber == Questions.Count - 1)
                return;
            CurrentQuestionNumber++;
            LoadQuestion();
        }

        void OnBtnFinnishClicked(object sender, EventArgs args)
        {
            if (Revision)
            {
                MainPage mPage = new MainPage();
                Navigation.PushAsync(mPage);
                NavigationPage.SetHasNavigationBar(mPage, false);
                return;
            }

            List<Question> missingAnswers = Question.GetMissingAnswers(Questions);
            if (missingAnswers.Count > 0)
            {
                string alertText;
                if (missingAnswers.Count > 1)
                {
                    alertText = "Neodpověděl jsi na " + missingAnswers.Count + " otázky: " + Environment.NewLine + Environment.NewLine;
                    for (int i = 0; i < missingAnswers.Count; i++)
                        alertText += Questions.IndexOf(missingAnswers[i]) + 1 + ", ";

                    alertText.Substring(alertText.Length - 2);
                }
                else
                    alertText = "Neodpověděl jsi na otázku číslo " + (Questions.IndexOf(missingAnswers[0]) + 1);

                DisplayAlert("Chybí odpověd!!", alertText, "Ok");
            }
            else
            {
                ScorePage scPage = new ScorePage(Questions);
                Navigation.PushAsync(scPage);
                NavigationPage.SetHasNavigationBar(scPage, false);
            }

        }

        void BtnAnswerClick(object sender, EventArgs args)
        {
            if (Revision) return;

            BtnAnswers.ForEach(x => x.IsEnabled = true);
            ((Button)sender).IsEnabled = false;
            CurrentQ.TestAnswer = BtnAnswers.IndexOf((Button)sender);
        }

       
        #endregion

        #region Methods
        void LoadQuestion()
        {
            promptValue.Text = (Questions.IndexOf(CurrentQ) + 1) + ". " + CurrentQ.Prompt;

            for (int i = 0; i < CurrentQ.Answers.Count; i++)
            {
                Button btnAnswer = this.FindByName<Button>(Answer + i);
                btnAnswer.Text = CurrentQ.Answers[i];
                btnAnswer.BackgroundColor = Color.Default;
                if (!Revision)
                {
                    if (CurrentQ.TestAnswer == -1)
                        btnAnswer.IsEnabled = true;
                    else
                        btnAnswer.IsEnabled = !(i == CurrentQ.TestAnswer);
                }
                else
                {
                    if (CurrentQ.TestAnswer == i)
                        btnAnswer.BackgroundColor = Color.Red;
                    if (CurrentQ.CorrectAnswer == i)
                        btnAnswer.BackgroundColor = Color.Green;
                }
            }


            #region Back and Next Visibility
            if (CurrentQuestionNumber == 0)
                btnBack.IsVisible = false;
            else if (CurrentQuestionNumber == Questions.Count - 1)
                btnNext.IsVisible = false;
            else
                btnBack.IsVisible = btnNext.IsVisible = true;
            #endregion

        }
        #endregion

    }
}