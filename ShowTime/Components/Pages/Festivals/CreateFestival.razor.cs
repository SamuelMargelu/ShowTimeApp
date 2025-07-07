using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShowTime.Entities;
using ShowTime.Enum;
using static ShowTime.Components.Pages.Festivals.CreateFestival;

namespace ShowTime.Components.Pages.Festivals
{
    public partial class CreateFestival
    {
        [Parameter]
        public int? FestivalIdQueryParameter { get; set; } = -1;
        private Festival? FestivalToUpdate { get; set; } = null;
        private string NewFestivalName = string.Empty;
        private string NewFestivalLocation = string.Empty;
        private DateTime NewFestivalStartDate = DateTime.Now;
        private DateTime NewFestivalEndDate = DateTime.Now.AddDays(1);
        private List<BandFestival>? NewBandFestival = null;
        private DropContainer<DropBand> drop_Container;

        private byte[]? NewFestivalPhoto;

        private List<Band>? AllBands;
        private HashSet<int> SelectedBandIds = new();

        private string errorMessage = string.Empty;



        protected override async Task OnInitializedAsync()
        {
            AllBands = (await BandService.GetAllAsync()).ToList();


            DropBands = AllBands.Select(b => new DropBand
            {
                Band = b,
                Group = "AllBands" // toate în zona 1 inițial
            }).ToList();


            await FillFields();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                drop_Container.Refresh();
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
                BandFestivals = NewBandFestival,
                Photo = NewFestivalPhoto
            };

            await FestivalService.AddAsync(newFestival);

            NewFestivalName = string.Empty;
            NewFestivalLocation = string.Empty;
            NewFestivalStartDate = DateTime.Now;
            NewFestivalEndDate = DateTime.Now.AddDays(1);
            SelectedBandIds.Clear();
            NewBandFestival = null;
            NewFestivalPhoto = null;

            NavigateToFestivalList();
        }

        private async Task UpdateFestival()
        {
            if (FestivalToUpdate == null)
            {
                errorMessage = "Festival to update is not set.";
                return;
            }
            var selectedBands = AllBands?.Where(b => SelectedBandIds.Contains(b.Id))
              .Select(b => new BandFestival
              {
                  BandsId = b.Id,
                  Band = b,
              }).ToList() ?? new List<BandFestival>();

            FestivalToUpdate.Name = NewFestivalName;
            FestivalToUpdate.Location = NewFestivalLocation;
            FestivalToUpdate.StartDate = NewFestivalStartDate;
            FestivalToUpdate.EndDate = NewFestivalEndDate;
            FestivalToUpdate.BandFestivals = selectedBands;
            FestivalToUpdate.Photo = NewFestivalPhoto;

            await FestivalService.UpdateAsync(FestivalToUpdate);

            NewFestivalName = string.Empty;
            NewFestivalLocation = string.Empty;
            NewFestivalStartDate = DateTime.Now;
            NewFestivalEndDate = DateTime.Now.AddDays(1);
            SelectedBandIds.Clear();
            NewFestivalPhoto = null;

            NavigateToFestivalList();
        }

        private async Task FillFields()
        {
            if (FestivalIdQueryParameter != null)
            {
                int FestivalId = FestivalIdQueryParameter.Value;
                errorMessage = string.Empty;
                try
                {
                    FestivalToUpdate = await FestivalService.GetByIdIncludingAsync(FestivalId, f => f.BandFestivals,
                                                                                       f => (f.BandFestivals as BandFestival).Band);

                    if (FestivalToUpdate == null)
                    {
                        errorMessage = "Festival not found.";
                    } else
                    {
                        NewFestivalName = FestivalToUpdate.Name;
                        NewFestivalLocation = FestivalToUpdate.Location;
                        NewFestivalStartDate = FestivalToUpdate.StartDate;
                        NewFestivalEndDate = FestivalToUpdate.EndDate;
                        NewFestivalPhoto = FestivalToUpdate.Photo;
                        SelectedBandIds = new HashSet<int>(FestivalToUpdate.BandFestivals.Select(bf => bf.BandsId));
                    }

                }
                catch (Exception ex)
                {
                    errorMessage = $"Error loading festival details: {ex.Message}";
                    Console.WriteLine($"Error loading festival with id {FestivalId} - {ex.Message}");
                }
            }
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
        private List<DropBand> DropBands = new();

        private void HandleOnDeleted(Band deletedBand)
        {
            DropBands = DropBands.Where(b => b.Band?.Id != deletedBand.Id).ToList();
        }

        private Task BandDropped(DraggableDroppedEventArgs<DropBand> dropBand)
        {
            dropBand.Item.Group = dropBand.DropZoneName;

            return Task.CompletedTask;
        }

        private string reorderStatus = string.Empty;
        private Task Reordered(DropZoneOrder<DropBand> order)
        {
            reorderStatus = $"Order in dropzone {order.DestinationDropZoneName}: {string.Join(", ", order.OrderedItems.OrderBy(x => x.Order).Select(x => x.Item.Band.Name))}";
            if (order.DestinationDropZoneName == "SelectedBands")
            {
                NewBandFestival = order.OrderedItems
                    .OrderBy(x => x.Order)
                    .Where(x => x.Item.Band != null)
                    .Select((x, idx) => new BandFestival
                    {
                        BandsId = x.Item.Band.Id,
                        Band = x.Item.Band,
                        BandOrder = idx + 1
                    })
                    .ToList();
            }

            return Task.CompletedTask;
        }


        private void NavigateToFestivalList()
        {
            NavigationManager.NavigateTo("/FestivalList");
        }
    }
}