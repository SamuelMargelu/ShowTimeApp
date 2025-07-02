using Blazorise;
using Microsoft.AspNetCore.Components;
using ShowTime.Entities;
using ShowTime.Enum;

namespace ShowTime.Components.Pages.Bands
{
    public partial class CreateBand
    {
        private string NewBandName = string.Empty;
        private Genre NewBandGenre;
        private byte[]? NewBandPhoto;


        private async Task AddBand()
        {
            var newBand = new Band
            {
                Name = NewBandName,
                Genre = NewBandGenre,
                Photo = NewBandPhoto
            };

            await BandService.AddAsync(newBand);

            NewBandName = string.Empty;
            NewBandGenre = Genre.Rock;
            NewBandPhoto = null;

            NavigationManager.NavigateTo("/BandList");
        }

        private async Task OnFileUpload(FileUploadEventArgs e)
        {
            var file = e.File;
            using var stream = file.OpenReadStream();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            NewBandPhoto = memoryStream.ToArray();
        }

        private void NavigateToBandList()
        {
            NavigationManager.NavigateTo("/BandList");
        }
    }
}