using Microsoft.AspNetCore.Components;
using ShowTime.Entities;
using ShowTime.Services.Interfaces;

namespace ShowTime.Components.Pages.Festivals
{
    public partial class FestivalDetails : ComponentBase
    {
        [Parameter] public int FestivalId { get; set; }
       
        private Festival? Festival;
        private bool isLoading = true;
        private string errorMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await LoadFestival();
        }

        private async Task LoadFestival()
        {
            isLoading = true;
            errorMessage = string.Empty;

            try
            {
                Festival = await FestivalService.GetByIdIncludingAsync(FestivalId, f => f.Bands);
                
                if (Festival == null)
                {
                    errorMessage = "Festival not found.";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error loading festival details: {ex.Message}";
                Console.WriteLine($"Error loading festival with id {FestivalId} - {ex.Message}");
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private void NavigateToFestivalList()
        {
            NavigationManager.NavigateTo("/FestivalCrudTest");
        }

        private void NavigateToEditFestival()
        {
            NavigationManager.NavigateTo($"/EditFestival/{FestivalId}");
        }

        private void NavigateToBandDetails(int bandId)
        {
            NavigationManager.NavigateTo($"/BandDetails/{bandId}");
        }

        private int GetFestivalDuration()
        {
            if (Festival == null) return 0;
            return (Festival.EndDate - Festival.StartDate).Days + 1;
        }

        private string GetFestivalStatus()
        {
            if (Festival == null) return "Unknown";

            var today = DateTime.Today;
            
            if (today < Festival.StartDate.Date)
                return "Upcoming";
            else if (today > Festival.EndDate.Date)
                return "Completed";
            else
                return "Ongoing";
        }

        private Blazorise.Color GetStatusColor()
        {
            return GetFestivalStatus() switch
            {
                "Upcoming" => Blazorise.Color.Info,
                "Ongoing" => Blazorise.Color.Success,
                "Completed" => Blazorise.Color.Secondary,
                _ => Blazorise.Color.Light
            };
        }

        private int GetDaysUntilStart()
        {
            if (Festival == null) return 0;
            var days = (Festival.StartDate - DateTime.Today).Days;
            return days > 0 ? days : 0;
        }

        private int GetFestivalProgress()
        {
            if (Festival == null) return 0;
            
            var totalDays = (Festival.EndDate - Festival.StartDate).Days;
            if (totalDays <= 0) return 100;
            
            var daysPassed = (DateTime.Today - Festival.StartDate).Days;
            var progress = (double)daysPassed / totalDays * 100;
            
            return Math.Max(0, Math.Min(100, (int)progress));
        }
        private string GetStatusBootstrapColor()
        {
            return GetFestivalStatus() switch
            {
                "Upcoming" => "info",
                "Ongoing" => "success", 
                "Completed" => "secondary",
                _ => "light"
            };
        }
    }
}