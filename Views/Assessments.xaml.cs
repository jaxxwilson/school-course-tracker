using SCT.Models;
using SCT.Services;

namespace SCT.Views;

public partial class Assessments : ContentPage
{
    public Course Course { get; set; }
    public ObjectiveAssessment ObjectiveAssessment { get; set; }
    public PerformanceAssessment PerformanceAssessment { get; set; }
    public Assessments(Course course)
    {
        InitializeComponent();
        Course = course;
        BindingContext = this;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAssessments();
    }
    private async Task LoadAssessments()
    {
        int courseId = Course.Id;
        PerformanceAssessment = await DatabaseService.GetPerformanceAssessment(courseId);
        ObjectiveAssessment = await DatabaseService.GetObjectiveAssessment(courseId);

        AssessmentVisibility();
    }
    private void AssessmentVisibility()
    {
        OnPropertyChanged(nameof(PerformanceAssessment));
        OnPropertyChanged(nameof(ObjectiveAssessment));

        AddPA.IsVisible = PerformanceAssessment == null;
        DeletePA.IsVisible = PerformanceAssessment != null;
        LblPADash.IsVisible = PerformanceAssessment != null;
        OnPropertyChanged(nameof(AddPA));
        OnPropertyChanged(nameof(DeletePA));

        AddOA.IsVisible = ObjectiveAssessment == null;
        DeleteOA.IsVisible = ObjectiveAssessment != null;
        LblOADash.IsVisible = ObjectiveAssessment != null;
        OnPropertyChanged(nameof(AddOA));
        OnPropertyChanged(nameof(DeleteOA));
    }
    private async void BtnOverview_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CourseDetails(Course));
    }
    private async void BtnHome_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Terms());
    }
    private async void BtnEdit_Clicked(object sender, EventArgs e)
    {
        if (PerformanceAssessment is null && ObjectiveAssessment is null)
        {
            await DisplayAlert("", "No Assessments exist to edit.", "OK");
            return;
        }
        await Navigation.PushAsync(new AssessmentEdit(PerformanceAssessment, ObjectiveAssessment));
    }

    private async void BtnAddPA_Clicked(object sender, EventArgs e)
    {
        if (PerformanceAssessment is null)
        {
            PerformanceAssessment = await DatabaseService.AddPAAssessment(Course.Id, Course.StartDate, Course.EndDate);
            await LoadAssessments();
            await Helper.DateAssessmentNotify(PerformanceAssessment.Id, PerformanceAssessment.Title, PerformanceAssessment.StartDate, PerformanceAssessment.EndDate);
            //await Helper.DateNotify(Course.Id, Course.Title, Course.StartDate, Course.EndDate);
        }
        else
        {
            await DisplayAlert("Existing PA", "A Performance Assessment already exists for this course.", "OK");
        }
    }
    private async void BtnDeletePA_Clicked(object sender, EventArgs e)
    {
        if (PerformanceAssessment != null)
        {
            await DatabaseService.DeletePAAssessment(PerformanceAssessment);
            await LoadAssessments();
        }
    }
    private async void BtnAddOA_Clicked(object sender, EventArgs e)
    {
        if (ObjectiveAssessment is null)
        {
            ObjectiveAssessment = await DatabaseService.AddOAAssessment(Course.Id, Course.StartDate, Course.EndDate);
            await LoadAssessments();
            await Helper.DateAssessmentNotify(ObjectiveAssessment.Id, ObjectiveAssessment.Title, ObjectiveAssessment.StartDate, ObjectiveAssessment.EndDate);
        }
        else
        {
            await DisplayAlert("Existing OA", "An Objective Assessment already exists for this course.", "OK");
        }
    }
    private async void BtnDeleteOA_Clicked(object sender, EventArgs e)
    {
        if (ObjectiveAssessment != null)
        {
            await DatabaseService.DeleteOAAssessment(ObjectiveAssessment);
            await LoadAssessments();
        }
    }
}