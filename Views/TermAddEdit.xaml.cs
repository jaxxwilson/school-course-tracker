using SCT.Models;
using SCT.Services;

namespace SCT.Views;

public partial class TermAddEdit : ContentPage
{
    private Term? Term;
    public TermAddEdit(Term? term = null)
    {
        InitializeComponent();
        Term = term;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (Term != null)
        {
            TermTitle.Text = Term.Title;
            StartDate.Date = Term.StartDate;
            EndDate.Date = Term.EndDate;
        }
        else
        {
            TermTitle.Text = string.Empty;
            StartDate.Date = DateTime.Today;
            EndDate.Date = DateTime.Today.AddMonths(6);
        }
    }
    private async void BtnHome_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Terms());
    }
    private async void SaveBtn_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(TermTitle.Text))
        {
            await DisplayAlert("No Title", "Enter a Term title before saving", "OK");
            return;
        }
        if (StartDate.Date >= EndDate.Date)
        {
            await DisplayAlert("Invalid Dates", "The end date must be after the start date", "OK");
            return;
        }
        if (Term == null)
        {
            string title = Helper.Sanitize(TermTitle.Text);
            DateTime startDate = StartDate.Date;
            DateTime endDate = EndDate.Date;
            Term = await DatabaseService.AddTerm(title, startDate, endDate);
        }
        else
        {
            Term.Title = Helper.Sanitize(TermTitle.Text);
            Term.StartDate = StartDate.Date;
            Term.EndDate = EndDate.Date;
            await DatabaseService.UpdateTerm(Term.Id, Term.Title, Term.StartDate, Term.EndDate);
        }

        await Navigation.PushAsync(new Terms());
    }

    private async void CancelBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}