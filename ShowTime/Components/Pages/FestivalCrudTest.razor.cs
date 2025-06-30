using ShowTime.Entities;

namespace ShowTime.Components.Pages
{
    public partial class FestivalCrudTest
    {
        private List<Festival>? Festivals;
        private List<Band>? Bands;

        private string NewFestivalName = string.Empty;
        private string NewFestivalLocation = string.Empty;
        private DateTime NewFestivalStartDate = DateTime.Now;
        private DateTime NewFestivalEndDate = DateTime.Now.AddDays(1);

        // For adding a band to a festival, we need to select the festival and the band.
        private int SelectedFestivalId;
        private int SelectedBandId;

        private int UpdateFestivalId;
        private string UpdateFestivalName = string.Empty;
        private string UpdateFestivalLocation = string.Empty;
        private DateTime UpdateFestivalStartDate = DateTime.Now;
        private DateTime UpdateFestivalEndDate = DateTime.Now.AddDays(1);

        private int DeleteFestivalId;

        protected override async Task OnInitializedAsync()
        {
            await LoadFestivals();
        }

        private async Task LoadFestivals()
        {
            try
            {
                Festivals = (await FestivalService.GetAllIncludingAsync(f => f.Bands)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading festivals: {ex.Message}");
            }
        }

        private async Task AddFestival()
        {
            var newFestival = new Festival
            {
                Name = NewFestivalName,
                Location = NewFestivalLocation,
                StartDate = NewFestivalStartDate,
                EndDate = NewFestivalEndDate
            };
            await FestivalService.AddAsync(newFestival);
            await LoadFestivals();

            NewFestivalName = string.Empty;
            NewFestivalLocation = string.Empty;
            NewFestivalStartDate = DateTime.Now;
            NewFestivalEndDate = DateTime.Now.AddDays(1);
        }

        private async Task AddBandToFestival()
        {
            try
            {
                await FestivalService.AddBandtoFestival(SelectedBandId, SelectedFestivalId);
                await LoadFestivals();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding band to festival: {ex.Message}");
            }
        }

        private async Task UpdateFestival()
        {
            var festivalToUpdate = await FestivalService.GetByIdAsync(UpdateFestivalId);
            if (festivalToUpdate != null)
            {
                festivalToUpdate.Name = UpdateFestivalName;
                festivalToUpdate.Location = UpdateFestivalLocation;
                festivalToUpdate.StartDate = UpdateFestivalStartDate;
                festivalToUpdate.EndDate = UpdateFestivalEndDate;

                await FestivalService.UpdateAsync(festivalToUpdate);
                await LoadFestivals();

                UpdateFestivalId = 0;
                UpdateFestivalName = string.Empty;
                UpdateFestivalLocation = string.Empty;
                UpdateFestivalStartDate = DateTime.Now;
                UpdateFestivalEndDate = DateTime.Now.AddDays(1);
            }
        }

        private async Task DeleteFestival()
        {
            var festivalToDelete = await FestivalService.GetByIdAsync(DeleteFestivalId);

            if (festivalToDelete != null)
            {
                await FestivalService.DeleteAsync(festivalToDelete);
                await LoadFestivals();

                DeleteFestivalId = 0;
            }
        }
    }
}