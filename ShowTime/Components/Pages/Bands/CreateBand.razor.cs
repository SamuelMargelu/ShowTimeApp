using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShowTime.Entities;
using ShowTime.Enum;

namespace ShowTime.Components.Pages.Bands
{
    public partial class CreateBand
    {
        [Parameter]
        public int? BandIdQueryParameter { get; set; } = -1;

        private Band? BandToUpdate { get; set; } = null;

        private string NewBandName = string.Empty;
        private Genre NewBandGenre;
        private byte[]? NewBandPhoto;

        private string errorMessage = string.Empty;


        protected override async Task OnInitializedAsync()
        {
            await FillFields();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await JS.InvokeVoidAsync("scrollToTop");
            }
        }

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

        private async Task UpdateBand()
        {
            if (BandToUpdate == null)
            {
                errorMessage = "Band to update is not set.";
                return;
            }

            BandToUpdate.Name = NewBandName;
            BandToUpdate.Genre = NewBandGenre;
            BandToUpdate.Photo = NewBandPhoto;

            await BandService.UpdateAsync(BandToUpdate);

            NewBandName = string.Empty;
            NewBandGenre = Genre.Rock;
            NewBandPhoto = null;

            NavigateToBandList();
        }

        private async Task FillFields()
        {
            if (BandIdQueryParameter != null)
            {
                int BandId = BandIdQueryParameter.Value;
                errorMessage = string.Empty;
                try
                {
                    BandToUpdate = await BandService.GetByIdAsync(BandId);

                    if (BandToUpdate == null)
                    {
                        errorMessage = "Band not found.";
                    }
                    else
                    {
                        NewBandName = BandToUpdate.Name;
                        NewBandGenre = BandToUpdate.Genre;
                        NewBandPhoto = BandToUpdate.Photo;
                    }
                }
                catch (Exception ex)
                {
                    errorMessage = $"Error loading band details: {ex.Message}";
                    Console.WriteLine($"Error loading band with id {BandId} - {ex.Message}");
                }
            }

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