using Microsoft.AspNetCore.Components;
using ShowTime.Entities;

namespace ShowTime.Components.Pages.Bands
{
    public partial class BandList
    {
        List<Band>? Bands = new List<Band>();

        protected override async Task OnInitializedAsync()
        {
            await LoadBands();
        }

        private async Task LoadBands()
        {
            try
            {
                Bands = (await BandService.GetAllIncludingAsync(b => b.BandFestivals,
                                                                b => (b.BandFestivals as BandFestival).Festival)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading bands: {ex.Message}");
            }
        }

        private async Task DeleteBandById(int bandId)
        {
            var bandToDelete = await BandService.GetByIdAsync(bandId);
            if (bandToDelete != null)
            {
                await BandService.DeleteAsync(bandToDelete);
                Console.WriteLine($"Deleting band: {bandToDelete.Name}");
            }

            Bands = (await BandService.GetAllIncludingAsync(b => b.BandFestivals,
                                                                b => (b.BandFestivals as BandFestival).Festival)).ToList();
            StateHasChanged();

        }

        private void NavigateToCreateBand()
        {
            NavigationManager.NavigateTo("/CreateBand");
        }

        private void NavigateToEditBand(int bandId)
        {
            NavigationManager.NavigateTo($"CreateBand/{bandId}");
        }
    }
}