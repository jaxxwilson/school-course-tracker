using SCT.Models;
using SCT.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SCT.Views;

public partial class Report : ContentPage
{
    private List<Term> allTerms = new();
    private List<Course> allCourses = new();
    private List<Label> allLabels = new();
    
	public Report()
	{
		InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetReportData();
    }

    private async Task GetReportData()
    {
        allTerms = (List<Term>)await DatabaseService.GetTerms();
        allCourses = (List<Course>)await DatabaseService.GetCourses();
        await LoadReport();
    }

    private async Task LoadReport()
    {
        ReportData.Children.Clear();
        allLabels.Clear();

        Label titleLbl = new Label
        {
            Text = "Tracker Report",
            FontAttributes = FontAttributes.Bold,
            FontSize = 22,
            TextColor = Colors.Black,
            Margin = new Thickness(0,10,0,20)
        };

        ReportData.Children.Add(titleLbl);
        allLabels.Add(titleLbl);

        foreach (Term term in allTerms)
        {
            var termLbl = new Label
            {
                Text = term.Title,
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                TextColor = Colors.Black,
                Margin = new Thickness(0,10,0,0)
            };
            ReportData.Children.Add(termLbl);
            allLabels.Add(termLbl);

            var termDatesLbl = new Label
            {
                Text = $"{term.StartDate:d} - {term.EndDate:d}",
                FontSize = 16,
                Margin = new Thickness(0, 0, 0, 8)
            };
            ReportData.Children.Add(termDatesLbl);
            allLabels.Add(termDatesLbl);

            var matchingCourses = allCourses
                .Where(c => c.TermId == term.Id)
                .ToList();

            if (!matchingCourses.Any())
            {
                var noCourseLbl = new Label
                {
                    Text = "no courses",
                    Margin = new Thickness(30,2,0,0)
                };
                ReportData.Children.Add(noCourseLbl);
                allLabels.Add(noCourseLbl);
            }
            else
            {
                foreach(var course in matchingCourses)
                {
                    var courseLbl = new Label
                    {
                        Text = course.Title,
                        FontSize = 16,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Colors.Black,
                        Margin = new Thickness(30, 2, 0, 0)
                    };
                    ReportData.Children.Add(courseLbl);
                    allLabels.Add(courseLbl);

                    var courseDatesLbl = new Label
                    {
                        Text = $"{course.StartDate:d} - { course.EndDate:d}",
                        Margin = new Thickness(30, 0, 0, 5)
                    };
                    ReportData.Children.Add(courseDatesLbl);
                    allLabels.Add(courseDatesLbl);

                    var pa = await DatabaseService.GetPerformanceAssessment(course.Id);
                    var oa = await DatabaseService.GetObjectiveAssessment(course.Id);

                    if (pa != null)
                    {
                        var paLbl = new Label
                        {
                            Text = pa.Title,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Colors.Black,
                            Margin = new Thickness(60, 2, 0, 0)
                        };
                        ReportData.Children.Add(paLbl);
                        allLabels.Add(paLbl);

                        var paDatesLbl = new Label
                        {
                            Text = $"{pa.StartDate:d} - {pa.EndDate:d}",
                            FontSize = 14,
                            Margin = new Thickness(60, 0, 0, 5)
                        };
                        ReportData.Children.Add(paDatesLbl);
                        allLabels.Add(paDatesLbl);
                    }
                    if (oa != null)
                    {
                        var oaLbl = new Label
                        {
                            Text = oa.Title,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Colors.Black,
                            Margin = new Thickness(60, 2, 0, 0)
                        };
                        ReportData.Children.Add(oaLbl);
                        allLabels.Add(oaLbl);

                        var oaDatesLbl = new Label
                        {
                            Text = $"{oa.StartDate:d} - {oa.EndDate:d}",
                            FontSize = 14,
                            Margin = new Thickness(60, 0, 0, 10)
                        };
                        ReportData.Children.Add(oaDatesLbl);
                        allLabels.Add(oaDatesLbl);
                    }
                }
            }
        }
        var timeStampLbl = new Label
        {
            Text = $"Generated on {DateTime.Now:G}",
            FontSize = 14,
            Margin = new Thickness(10, 40, 0, 10)
        };
        ReportData.Children.Add(timeStampLbl);
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.ToLower() ?? "";

        foreach (Label lbl in allLabels)
        {
            if (!string.IsNullOrEmpty(searchText)
                && lbl.Text.ToLower().Contains(searchText))
            {
                lbl.TextColor = Colors.DarkSalmon;
            }
            else
            {
                lbl.TextColor = Colors.Black;
            }
        }
    }

    private void searchBar_SearchButtonPressed(object sender, EventArgs e)
    {
        searchBar.Unfocus();
    }

    private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
    {
        if (searchBar.IsFocused)
        {
            searchBar.Unfocus();
        }
    }

    private async void BtnHome_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Terms());
    }

}