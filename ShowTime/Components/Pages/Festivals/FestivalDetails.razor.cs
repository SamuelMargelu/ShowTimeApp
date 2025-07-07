using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using ShowTime.Entities;
using ShowTime.Services.Interfaces;

namespace ShowTime.Components.Pages.Festivals
{
    public partial class FestivalDetails : ComponentBase
    {
        [Parameter] public int FestivalId { get; set; }

        public Festival? Festival;
        public bool isLoading = true;
        public string errorMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await LoadFestival();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await JS.InvokeVoidAsync("scrollToTop");
            }
        }

        private async Task LoadFestival()
        {
            isLoading = true;
            errorMessage = string.Empty;

            try
            {
                Festival = await FestivalService.GetByIdIncludingAsync(FestivalId, f => f.BandFestivals,
                                                                                   f => (f.BandFestivals as BandFestival).Band);

                if (Festival != null)
                {
                    Festival.BandFestivals?.OrderBy(bf => bf.BandOrder).ToList();
                }
                else
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

        public void NavigateToFestivalList()
        {
            NavigationManager.NavigateTo("/FestivalList");
        }

        public void NavigateToEditFestival()
        {
            NavigationManager.NavigateTo("/FestivalCrudTest");
        }

        public void NavigateToBandDetails(int bandId)
        {
            NavigationManager.NavigateTo($"/BandDetails/{bandId}");
        }

        public int GetFestivalDuration()
        {
            if (Festival == null) return 0;
            return (Festival.EndDate - Festival.StartDate).Days + 1;
        }

        public string GetFestivalStatus()
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

        public Blazorise.Color GetStatusColor()
        {
            return GetFestivalStatus() switch
            {
                "Upcoming" => Blazorise.Color.Info,
                "Ongoing" => Blazorise.Color.Success,
                "Completed" => Blazorise.Color.Secondary,
                _ => Blazorise.Color.Light
            };
        }

        public string GetStatusBootstrapColor()
        {
            return GetFestivalStatus() switch
            {
                "Upcoming" => "info",
                "Ongoing" => "dark", 
                "Completed" => "secondary",
                _ => "light"
            };
        }
        public int GetDaysUntilStart()
        {
            if (Festival == null) return 0;
            var days = (Festival.StartDate - DateTime.Today).Days;
            return days > 0 ? days : 0;
        }

        public int GetFestivalProgress()
        {
            if (Festival == null) return 0;
            
            var totalDays = (Festival.EndDate - Festival.StartDate).Days;
            if (totalDays <= 0) return 100;
            
            var daysPassed = (DateTime.Today - Festival.StartDate).Days;
            var progress = (double)daysPassed / totalDays * 100;
            
            return Math.Max(0, Math.Min(100, (int)progress));
        }
    }
}