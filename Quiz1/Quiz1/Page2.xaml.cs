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
	public partial class Page2 : ContentPage
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

        public List<View> BtnAnswers
        {
            get
            {
                List<View> result = new List<View>();
                for (int i = 0; i < CurrentQ.Answers.Count; i++)
                    result.Add(this.FindByName<View>(Answer + i));
                return result;
            }
        }

        public string TestName { get { return Questions[0].TestName; } }

        public bool Revision { get; set; } = false;

        #endregion

        #region Constructors
        public Page2(object test)
        {
            InitializeComponent();
            if (test is string)
            {
                object obj = Application.Current.Resources[(string)test];
                List<string> testQ = new List<string>((string[])obj);
                Questions = Question.GetQuestionsFromResource(testQ, true);                                              
            }
            else if (test is List<Question>)
            {
                Questions = (List<Question>)test;

                this.Revision = gridNavigation.IsVisible = true;
                btnFinish.Text = "Zpět do Menu";
            }
            else throw new NotSupportedException();

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
            if (Revision) Settings.LoadPage(this, new MainPage());

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
                Settings.LoadPage(this, new ScorePage(Questions));

        }

        void BtnAnswerClick(object sender, EventArgs args)
        {
            if (Revision) return;

            BtnAnswers.ForEach(x => x.IsEnabled = true);
            ((View)sender).IsEnabled = false;
            CurrentQ.TestAnswer = Grid.GetColumn((View)sender) + Grid.GetRow((View)sender);
        }


        #endregion

        #region Methods
        void LoadQuestion()
        {
            promptValue.Text = (Questions.IndexOf(CurrentQ) + 1) + ". " + CurrentQ.Prompt;

            #region Adding Answer Controls

            GridAnswers.Children.Clear();
            BtnAnswers.Clear();

            for (int i = 0; i < CurrentQ.Answers.Count; i++)
            {
                View control;
                if (CurrentQ.Answers[i].StartsWith("@"))
                {
                    #region Image
                    control = new Image() { Source = CurrentQ.Answers[i].Substring(1) };
                    ((Image)control).GestureRecognizers.Add(new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            BtnAnswerClick(control, EventArgs.Empty);
                        })
                    });
                    #endregion
                }
                else
                {
                    #region Button
                    control = new Button() { Text = CurrentQ.Answers[i], BackgroundColor = Color.Default };
                    ((Button)control).Clicked += BtnAnswerClick;
                    #endregion
                }

                const int columns = 2;
                Grid.SetRow(control, i / columns);
                Grid.SetColumn(control, i % columns);

                if (!Revision)
                {
                    if (CurrentQ.TestAnswer == -1)
                        control.IsEnabled = true;
                    else
                        control.IsEnabled = !(i == CurrentQ.TestAnswer);
                }
                else
                {
                    if (CurrentQ.TestAnswer == i)
                        control.BackgroundColor = Color.Red;
                    if (CurrentQ.CorrectAnswer == i)
                        control.BackgroundColor = Color.Green;
                }

                BtnAnswers.Add(control);
                GridAnswers.Children.Add(control);
            } 
            #endregion

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