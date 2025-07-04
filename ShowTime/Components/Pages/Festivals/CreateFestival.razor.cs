using Blazorise;
using Microsoft.AspNetCore.Components;
using ShowTime.Entities;
using ShowTime.Services.Implementations;

namespace ShowTime.Components.Pages.Festivals
{
    public partial class CreateFestival
    {

        private string NewFestivalName = string.Empty;
        private string NewFestivalLocation = string.Empty;
        private DateTime NewFestivalStartDate = DateTime.Now;
        private DateTime NewFestivalEndDate = DateTime.Now.AddDays(1);

        private byte[]? NewFestivalPhoto;

        private List<Band>? AllBands;
        private HashSet<int> SelectedBandIds = new();



        protected override async Task OnInitializedAsync()
        {
            AllBands = (await BandService.GetAllAsync()).ToList();
        }
        private async Task AddFestival()
        {
            var selectedBands = AllBands?.Where(b => SelectedBandIds.Contains(b.Id)).ToList() ?? new List<Band>();

            var newFestival = new Festival
            {
                Name = NewFestivalName,

                Location = NewFestivalLocation,
                StartDate = NewFestivalStartDate,
                EndDate = NewFestivalEndDate,
                Bands = selectedBands,
                Photo = NewFestivalPhoto
            };

            await FestivalService.AddAsync(newFestival);

            NewFestivalName = string.Empty;
            NewFestivalLocation = string.Empty;
            NewFestivalStartDate = DateTime.Now;
            NewFestivalEndDate = DateTime.Now.AddDays(1);
            SelectedBandIds.Clear();
            NewFestivalPhoto = null;

            NavigateToFestivalList();
        }

        private async void OnFileUpload(FileUploadEventArgs e)
        {
            var file = e.File;
            using var stream = file.OpenReadStream();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            NewFestivalPhoto = memoryStream.ToArray();
        }

        private void ToggleBandSelection(int bandId)
        {
            if (!SelectedBandIds.Add(bandId))
                SelectedBandIds.Remove(bandId);
        }

        private void NavigateToFestivalList()
        {
            NavigationManager.NavigateTo("/FestivalList");
        }
    }
}