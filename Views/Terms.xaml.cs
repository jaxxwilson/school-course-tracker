using SCT.Models;
using SCT.Services;

namespace SCT.Views;

public partial class Terms : ContentPage
{
    public List<Term> ListTerms { get; set; } = new();

    public Terms()
    {
        InitializeComponent();
        BindingContext = this;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadTerms();
    }
    private async Task LoadTerms()
    {
        //await DatabaseService.ClearAll();
        var terms = await DatabaseService.GetTerms();

        if (!terms.Any())
        {
            await DatabaseService.AddTerm("Term 1", DateTime.Today, DateTime.Today.AddMonths(6));
            terms = await DatabaseService.GetTerms();
            var term = terms.FirstOrDefault();
            Course course = await DatabaseService.AddCourse("Course 1", term.Id, DateTime.Today, DateTime.Today.AddMonths(6), "Inactive", "Anika Patel", "555-123-4567", "anika.patel@strimeuniversity.edu");
            await DatabaseService.AddPAAssessment(course.Id, course.StartDate, course.EndDate);
            await DatabaseService.AddOAAssessment(course.Id, course.StartDate, course.EndDate);
        }

        ListTerms = terms.ToList();
        OnPropertyChanged(nameof(ListTerms));
    }

    private async void BtnAddTerm_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TermAddEdit(null));
    }
    private async void BtnReport_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Report());
    }
    private async void Term_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedTerm = e.CurrentSelection.FirstOrDefault() as Term;
        if (selectedTerm != null)
        {
            await Navigation.PushAsync(new TermDetails(selectedTerm));
        }
    }
}