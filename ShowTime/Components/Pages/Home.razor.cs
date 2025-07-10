using Microsoft.AspNetCore.Components;
using ShowTime.Entities;
using ShowTime.Services.Interfaces;

namespace ShowTime.Components.Pages
{
    public partial class Home
    {
        private List<Festival>? FeaturedFestivals;
        private List<Band>? PopularBands;
        private int ActiveFestivals = 0;
        private int UpcomingFestivals = 0;
        private int TotalBands = 0;
        private bool IsLoading = true;

        protected override async Task OnInitializedAsync()
        {
            await LoadHomeData();
        }

        private async Task LoadHomeData()
        {
            try
            {
                IsLoading = true;

                // Load featured festivals (take first 3)
                var allFestivals = await FestivalService.GetAllIncludingAsync(f => f.BandFestivals, f => (f.BandFestivals as BandFestival).Band);
                FeaturedFestivals = allFestivals.Take(3).ToList();
                ActiveFestivals = allFestivals.Count(f => f.StartDate <= DateTime.Now && f.EndDate >= DateTime.Now);
                UpcomingFestivals = allFestivals.Count(f => f.StartDate > DateTime.Now);

                // Load popular bands (take first 6)
                var allBands = await BandService.GetAllIncludingAsync(b => b.BandFestivals);
                PopularBands = allBands.Take(6).ToList();
                TotalBands = allBands.Count();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading home data: {ex.Message}");
                FeaturedFestivals = new List<Festival>();
                PopularBands = new List<Band>();
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }

        private void NavigateToFestivals()
        {
            NavigationManager.NavigateTo("/FestivalList");
        }

        private void NavigateToBands()
        {
            NavigationManager.NavigateTo("/BandList");
        }

        private void NavigateToFestivalDetails(int festivalId)
        {
            NavigationManager.NavigateTo($"/FestivalDetails/{festivalId}");
        }

        private void NavigateToCreateFestival()
        {
            NavigationManager.NavigateTo("/CreateFestival");
        }

        private void NavigateToCreateBand()
        {
            NavigationManager.NavigateTo("/CreateBand");
        }

        private string GetFestivalImageUrl(Festival festival)
        {
            if (festival.Photo != null && festival.Photo.Length > 0)
            {
                return $"data:image/jpeg;base64,{Convert.ToBase64String(festival.Photo)}";
            }
            return "/images/default-festival.jpg"; // Fallback image
        }

        private string GetBandImageUrl(Band band)
        {
            if (band.Photo != null && band.Photo.Length > 0)
            {
                return $"data:image/jpeg;base64,{Convert.ToBase64String(band.Photo)}";
            }
            return "/images/default-band.jpg"; // Fallback image
        }

        private string GetFestivalStatus(Festival festival)
        {
            var now = DateTime.Now;
            if (festival.StartDate > now)
                return "Upcoming";
            else if (festival.EndDate < now)
                return "Past";
            else
                return "Live Now";
        }

        private Blazorise.Color GetStatusColor(Festival festival)
        {
            var status = GetFestivalStatus(festival);
            return status switch
            {
                "Upcoming" => Blazorise.Color.Primary,
                "Live Now" => Blazorise.Color.Success,
                "Past" => Blazorise.Color.Secondary,
                _ => Blazorise.Color.Secondary
            };
        }
    }
}
