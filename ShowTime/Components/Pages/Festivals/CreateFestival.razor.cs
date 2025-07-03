using Microsoft.AspNetCore.Components;
using ShowTime.Entities;
using ShowTime.Services.Implementations;

namespace ShowTime.Components.Pages.Festivals
{
    public partial class CreateFestival
    {
        private int NewFestivalId;
        private string NewFestivalName = string.Empty;
        public string NewFestivalLocation = string.Empty;
        public DateTime NewFestivalStartDate = DateTime.Now;
        public DateTime NewFestivalEndDate = DateTime.Now.AddDays(1);

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
                Bands = selectedBands
            };

            await FestivalService.AddAsync(newFestival);

            NewFestivalName = string.Empty;
            NewFestivalLocation = string.Empty;
            NewFestivalStartDate = DateTime.Now;
            NewFestivalEndDate = DateTime.Now.AddDays(1);
            SelectedBandIds.Clear();

            NavigateToFestivalList();
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