using SCT.Models;
using SCT.Services;

namespace SCT.Views;

public partial class CourseDetails : ContentPage
{
    public Course Course { get; set; }
    public CourseDetails(Course course)
    {
        InitializeComponent();
        Course = course;
        BindingContext = this;
    }
    private async void BtnAssessments_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Assessments(Course));
    }
    private async void BtnHome_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Terms());
    }
    private async void BtnEditCourse_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CourseEdit(Course));
    }
    private async void BtnDeleteCourse_Clicked(object sender, EventArgs e)
    {
        bool yes = await DisplayAlert("Delete Course", "Do you want to delete this course?", "Yes", "No");
        if (yes)
        {
            Term Term = await DatabaseService.GetTerm(Course.TermId);
            await DatabaseService.RemoveCourse(Course.Id);
            await Navigation.PushAsync(new TermDetails(Term));
        }
    }
    private async void BtnShareNotes_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Course.Notes))
        {
            await DisplayAlert("Empty Notes", "Must have notes to share.", "OK");
            return;
        }
        await Share.RequestAsync(new ShareTextRequest
        {
            Text = Course.Notes,
            Title = $"Notes for {Course.Title}"
        });
    }
}