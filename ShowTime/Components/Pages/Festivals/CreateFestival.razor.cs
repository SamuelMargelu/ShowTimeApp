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

        public ApplicationUser applicationUser { get; set; } = new ApplicationUser();

        private Festival? FestivalToUpdate { get; set; } = null;
        private string NewFestivalName = string.Empty;
        private string NewFestivalLocation = string.Empty;
        private DateTime NewFestivalStartDate = DateTime.Now;
        private DateTime NewFestivalEndDate = DateTime.Now.AddDays(1);
        private List<BandFestival>? NewBandFestival = null;
        private DropContainer<DropBand> drop_Container;

        private byte[]? NewFestivalPhoto;

        private List<Band>? AllBands;

        private string errorMessage = string.Empty;

        private bool needsDropContainerRefresh = false;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            AllBands = (await BandService.GetAllAsync()).ToList();

            DropBands = AllBands.Select(b => new DropBand
            {
                Band = b,
                Group = "AllBands"
            }).ToList();

            await FillFields();

            needsDropContainerRefresh = true;
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (needsDropContainerRefresh && drop_Container is not null)
            {
                drop_Container.Refresh();
                needsDropContainerRefresh = false;
            }

            if (firstRender)
            {
                await JS.InvokeVoidAsync("scrollToTop");
            }

        }
        private async Task AddFestival()
        {
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

            // 1. Get the current BandFestivals from the DB
            var currentBandFestivals = (await FestivalService.GetByIdIncludingAsync(
                FestivalToUpdate.Id, f => f.BandFestivals)).BandFestivals?.ToList() ?? new List<BandFestival>();

            // 2. Find BandFestivals to remove
            var toRemove = currentBandFestivals
                .Where(bf => NewBandFestival == null || !NewBandFestival.Any(nbf => nbf.BandsId == bf.BandsId))
                .ToList();

            // 3. Remove them from the DB (you may need a BandFestivalService or similar)
            foreach (var bf in toRemove)
            {
               await BandFestivalsService.DeleteByBandAndFestivalIdAsync(bf);
            }

            FestivalToUpdate.Name = NewFestivalName;
            FestivalToUpdate.Location = NewFestivalLocation;
            FestivalToUpdate.StartDate = NewFestivalStartDate;
            FestivalToUpdate.EndDate = NewFestivalEndDate;
            FestivalToUpdate.BandFestivals = NewBandFestival;
            FestivalToUpdate.Photo = NewFestivalPhoto;

            await FestivalService.UpdateAsync(FestivalToUpdate);

            NewFestivalName = string.Empty;
            NewFestivalLocation = string.Empty;
            NewFestivalStartDate = DateTime.Now;
            NewFestivalEndDate = DateTime.Now.AddDays(1);
            NewBandFestival = null;
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

                        // Set up NewBandFestival for ordering
                        NewBandFestival = FestivalToUpdate.BandFestivals?
                            .OrderBy(bf => bf.BandOrder)
                            .Select(bf => new BandFestival
                            {
                                BandsId = bf.BandsId,
                                Band = bf.Band,
                                BandOrder = bf.BandOrder
                            }).ToList() ?? new List<BandFestival>();

                        // Update DropBands group and order
                        foreach (var bandFestival in NewBandFestival)
                        {
                            var dropBand = DropBands.FirstOrDefault(db => db.Band?.Id == bandFestival.BandsId);
                            if (dropBand != null)
                            {
                                dropBand.Group = "SelectedBands";
                            }
                        }

                        // Order DropBands so that selected bands are in the correct order
                        DropBands.Sort((a, b) =>
                        {
                            int orderA = NewBandFestival.FirstOrDefault(bf => bf.BandsId == a.Band?.Id)?.BandOrder ?? int.MaxValue;
                            int orderB = NewBandFestival.FirstOrDefault(bf => bf.BandsId == b.Band?.Id)?.BandOrder ?? int.MaxValue;
                            return orderA.CompareTo(orderB);
                        });
                    }

                }
                catch (Exception ex)
                {
                    errorMessage = $"Error loading festival details: {ex.Message}";
                    Console.WriteLine($"Error loading festival with id {FestivalId} - {ex.Message}");
                }
            }

            needsDropContainerRefresh = true;

        }

        private async void OnFileUpload(FileUploadEventArgs e)
        {
            var file = e.File;
            using var stream = file.OpenReadStream();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            NewFestivalPhoto = memoryStream.ToArray();
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

            StateHasChanged();

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