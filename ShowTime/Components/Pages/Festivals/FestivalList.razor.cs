using ShowTime.Entities;

namespace ShowTime.Components.Pages.Festivals
{
    public partial class FestivalList
    {
        private List<Festival>? Festivals;

        protected override async Task OnInitializedAsync()
        {
            await LoadFestivals();
        }

        private async Task LoadFestivals()
        {
            try
            {
                Festivals = (await FestivalService.GetAllIncludingAsync(f => f.BandFestivals,
                                                                        f => (f.BandFestivals as BandFestival).Band,
                                                                        f => f.Bookings)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading festivals: {ex.Message}");
            }

        }

        private void NavigateToFestivalDetails(int festivalId)
        {
            NavigationManager.NavigateTo($"/FestivalDetails/{festivalId}");
        }
    }
}