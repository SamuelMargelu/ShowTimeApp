using Microsoft.AspNetCore.Components;
using ShowTime.Entities;
using ShowTime.Enum;

namespace ShowTime.Components.Pages.Bands
{
    public partial class CreateBand
    {
        private string NewBandName = string.Empty;
        private Genre NewBandGenre;


        private async Task AddBand()
        {
            var newBand = new Band
            {
                Name = NewBandName,
                Genre = NewBandGenre,
            };

            await BandService.AddAsync(newBand);

            NewBandName = string.Empty;
            NewBandGenre = Genre.Rock;

            NavigationManager.NavigateTo("/BandList");
        }

        private void NavigateToBandList()
        {
            NavigationManager.NavigateTo("/BandList");
        }
    }
}