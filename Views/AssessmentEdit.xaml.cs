//using static Android.Icu.Text.CaseMap;
using SCT.Models;
using SCT.Services;

namespace SCT.Views;

public partial class AssessmentEdit : ContentPage
{
    private PerformanceAssessment PA;
    private ObjectiveAssessment OA;
    public AssessmentEdit(PerformanceAssessment pa, ObjectiveAssessment oa)
    {
        InitializeComponent();
        PA = pa;
        OA = oa;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (PA != null)
        {
            PATitle.Text = PA.Title;
            PAStartDate.Date = PA.StartDate;
            PAEndDate.Date = PA.EndDate;
        }
        else
        {
            PATitle.IsVisible = false;
            PAStartDate.IsVisible = false;
            PAEndDate.IsVisible = false;
            LblPATitle.IsVisible = false;
            LblPAStartDate.IsVisible = false;
            LblPAEndDate.IsVisible = false;
        }

        if (OA != null)
        {
            OATitle.Text = OA.Title;
            OAStartDate.Date = OA.StartDate;
            OAEndDate.Date = OA.EndDate;
        }
        else
        {
            OATitle.IsVisible = false;
            OAStartDate.IsVisible = false;
            OAEndDate.IsVisible = false;
            LblOATitle.IsVisible = false;
            LblOAStartDate.IsVisible = false;
            LblOAEndDate.IsVisible = false;
        }
    }
    private async void BtnHome_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Terms());
    }
    private async void SaveBtn_Clicked(object sender, EventArgs e)
    {
        if (PA != null)
        {
            if (string.IsNullOrEmpty(PATitle.Text))
            {
                await DisplayAlert("No Title", "Enter a Performance Assessment title before saving", "OK");
                return;
            }
            PA.Title = Helper.Sanitize(PATitle.Text);
            PA.StartDate = PAStartDate.Date;
            PA.EndDate = PAEndDate.Date;

            await DatabaseService.UpdatePAAssessment(PA.Id, PA.Title, PA.StartDate, PA.EndDate);
            await Helper.DateAssessmentNotify(PA.Id, PA.Title, PA.StartDate, PA.EndDate);

            OnPropertyChanged(nameof(PA));
        }

        if (OA != null)
        {
            if (string.IsNullOrEmpty(OATitle.Text))
            {
                await DisplayAlert("No Title", "Enter an Objective Assessment title before saving", "OK");
                return;
            }
            OA.Title = Helper.Sanitize(OATitle.Text);
            OA.StartDate = OAStartDate.Date;
            OA.EndDate = OAEndDate.Date;

            await DatabaseService.UpdateOAAssessment(OA.Id, OA.Title, OA.StartDate, OA.EndDate);
            await Helper.DateAssessmentNotify(OA.Id, OA.Title, OA.StartDate, OA.EndDate);

            OnPropertyChanged(nameof(OA));
        }

        await Navigation.PopAsync();
    }

    private async void CancelBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}