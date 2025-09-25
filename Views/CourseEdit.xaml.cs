//using static Java.Util.Jar.Attributes;
using SCT.Models;
using SCT.Services;

namespace SCT.Views;

public partial class CourseEdit : ContentPage
{
    public Course Course { get; set; }
    public CourseEdit(Course course)
    {
        InitializeComponent();
        Course = course;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        CourseTitle.Text = Course.Title;
        StartDate.Date = Course.StartDate;
        EndDate.Date = Course.EndDate;
        Status.SelectedItem = Course.Status;
        Name.Text = Course.Instructor;
        Phone.Text = Course.Phone;
        Email.Text = Course.Email;
        Notes.Text = Course.Notes;
    }

    private async void HomeBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Terms());
    }
    private async void CancelBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    private async void SaveBtn_Clicked(object sender, EventArgs e)
    {
        int termId = Course.TermId;
        string title = Helper.Sanitize(CourseTitle.Text);
        DateTime startDate = StartDate.Date;
        DateTime endDate = EndDate.Date;
        string status = Helper.Sanitize((string)Status.SelectedItem);
        string instructor = Helper.Sanitize(Name.Text);
        string phone = Helper.Sanitize(Phone.Text);
        string email = Helper.Sanitize(Email.Text);
        string notes = Helper.Sanitize(Notes.Text);

        if (string.IsNullOrEmpty(title))
        {
            await DisplayAlert("No Title", "Enter a Term title before saving", "OK");
            return;
        }
        if (startDate >= endDate)
        {
            await DisplayAlert("Invalid Dates", "The end date must be after the start date", "OK");
            return;
        }
        if ((instructor.Length == 0 || phone.Length == 0 || email.Length == 0) && (instructor != null || phone != null || email != null))
        {
            await DisplayAlert("Empty value", "Check the instructor name, phone, and email are entered before saving.", "OK");
            return;
        }
        if (!Helper.IsValidEmail(email))
        {
            await DisplayAlert("Invalid email", "Enter a valid email", "OK");
            return;
        }
        if (!Helper.IsValidPhone(phone))
        {
            await DisplayAlert("Invalid phone", "Enter a valid phone", "OK");
            return;
        }

        if (Course == null)
        {
            Course = await DatabaseService.AddCourse(title, termId, startDate, endDate, status, instructor, phone, email, notes);
        }
        else
        {
            Course.Title = title;
            Course.StartDate = startDate;
            Course.EndDate = endDate;
            Course.Status = status;
            Course.Instructor = instructor;
            Course.Phone = phone;
            Course.Email = email;
            Course.Notes = notes;
            await DatabaseService.UpdateCourse(Course.Id, Course.Title, Course.StartDate, Course.EndDate, Course.Status, Course.Instructor, Course.Phone, Course.Email, Course.Notes);
        }

        await Helper.DateNotify(Course.Id, Course.Title, Course.StartDate, Course.EndDate);
        await Navigation.PushAsync(new CourseDetails(Course));
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
}