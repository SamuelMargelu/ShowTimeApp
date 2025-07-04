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

        private void NavigateToCreateBand()
        {
            NavigationManager.NavigateTo("/CreateBand");
        }
    }
}