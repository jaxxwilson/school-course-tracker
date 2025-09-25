using SCT.Models;
using SCT.Services;

namespace SCT.Views;

public partial class TermDetails : ContentPage
{
    public Term Term { get; set; }
    public List<Course> TermCourses { get; set; } = new();
    public TermDetails(Term term)
    {
        InitializeComponent();
        Term = term;
        BindingContext = this;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadCourses();
    }
    private async Task LoadCourses()
    {
        var courses = await DatabaseService.GetCoursesForTerm(Term.Id);
        TermCourses = courses.ToList();
        OnPropertyChanged(nameof(TermCourses));
    }
    private async void Course_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var course = e.CurrentSelection.FirstOrDefault() as Course;
        if (course != null)
        {
            await Navigation.PushAsync(new CourseDetails(course));
        }
    }
    private async void BtnHome_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Terms());
    }
    private async void BtnEditTerm_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TermAddEdit(Term));
    }
    private async void BtnDeleteTerm_Clicked(object sender, EventArgs e)
    {
        bool yes = await DisplayAlert("Delete Term and Courses", "Do you want to delete the term along with its courses?", "Yes", "No");
        if (yes)
        {
            await DatabaseService.RemoveCourses(Term.Id);
            await DatabaseService.RemoveTerm(Term.Id);
            await Navigation.PushAsync(new Terms());
        }
    }
    private async void BtnAddCourse_Clicked(object sender, EventArgs e)
    {
        int termId = Term.Id;
        DateTime startDate = Term.StartDate;
        DateTime endDate = Term.EndDate;
        int count = TermCourses.Count;
        int i = count + 1;

        if (count >= 6)
        {
            await DisplayAlert("Too many Courses", "Can't add more than 6 courses for a term.", "OK");
            return;
        }

        Course course = await DatabaseService.AddCourse($"Course {i}", termId, startDate, endDate, "Inactive", "Anika Patel", "555-123-4567", "anika.patel@strimeuniversity.edu");
        var listCourses = await DatabaseService.GetCoursesForTerm(termId);

        await Helper.DateNotify(course.Id, course.Title, course.StartDate, course.EndDate);
        TermCourses = listCourses.ToList();
        OnPropertyChanged(nameof(TermCourses));
    }
}
