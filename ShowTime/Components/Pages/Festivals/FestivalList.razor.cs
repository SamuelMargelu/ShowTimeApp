using Microsoft.AspNetCore.Components.Web;
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

        private void NavigateToEditFestival(int festivalId)
        {
            NavigationManager.NavigateTo($"/CreateFestival/{festivalId}");
            
        }

        private void NavigateToCreateFestival()
        {
            NavigationManager.NavigateTo("/CreateFestival");
        }   

        private async Task DeleteFestivalById(int id)
        {
            var festivalToDelete = await FestivalService.GetByIdAsync(id);
            if (festivalToDelete != null)
            {
                await FestivalService.DeleteAsync(festivalToDelete);

                Console.WriteLine($"Deleting festival: {festivalToDelete.Name}");

                Festivals = (await FestivalService.GetAllIncludingAsync(f => f.BandFestivals,
                                                                        f => (f.BandFestivals as BandFestival).Band,
                                                                        f => f.Bookings)).ToList();
                StateHasChanged();
            }

        }
    }
}