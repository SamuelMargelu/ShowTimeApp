using Blazorise;
using Microsoft.AspNetCore.Components;
using ShowTime.Entities;
using Microsoft.JSInterop;
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

            DropBands = AllBands.Select(b => new DropBand
            {
                Band = b,
                Group = "1"
            }).ToList();

            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await JS.InvokeVoidAsync("scrollToTop");
            }
        }
        private async Task AddFestival()
        {
            var selectedBands = AllBands?.Where(b => SelectedBandIds.Contains(b.Id))
              .Select(b => new BandFestival
              {
                  BandsId = b.Id,
                  Band = b,
              }).ToList() ?? new List<BandFestival>();



            var newFestival = new Festival
            {
                Name = NewFestivalName,

                Location = NewFestivalLocation,
                StartDate = NewFestivalStartDate,
                EndDate = NewFestivalEndDate,
                BandFestivals = selectedBands,
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

        public class DropBand
        {
            public Band? Band { get; set; }
            public string? Group { get; set; }
        }

        private List<DropBand> DropBands = new List<DropBand>();

        private void HandleOnDeleted(Band deletedBand)
        {
            DropBands = DropBands.Where(b => b.Band?.Id != deletedBand.Id).ToList();
        }

        private Task BandDropped(DraggableDroppedEventArgs<DropBand> dropBand)
        {
            dropBand.Item.Group = dropBand.DropZoneName;

            DropBands = DropBands.Select(b => new DropBand
            {
                Band = b.Band,
                Group = b.Group
            }).ToList();

            return Task.CompletedTask;
        }

        private string reorderStatus = string.Empty;
        private Task Reordered(DropZoneOrder<DropBand> order)
        {
            reorderStatus = $"Order in dropzone {order.DestinationDropZoneName}: {string.Join(", ", order.OrderedItems.OrderBy(x => x.Order).Select(x => x.Item.Band.Name))}";
            return Task.CompletedTask;
        }


        private void NavigateToFestivalList()
        {
            NavigationManager.NavigateTo("/FestivalList");
        }
    }
}