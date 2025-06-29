using ShowTime.Entities;
using ShowTime.Enum;


namespace ShowTime.Components.Pages
{
    public partial class BandCrudTest
    {
        private List<Band>? Bands;
        private List<Festival>? Festivals;

        private string NewBandName = string.Empty;
        private Genre NewBandGenre;

        private int UpdateBandId;
        private string UpdateBandName = string.Empty;
        private Genre UpdateBandGenre;

        private int DeleteBandId;

        protected override async Task OnInitializedAsync()
        {
            await LoadBands();
        }

        private async Task LoadBands()
        {
            try
            {
                Bands = (await BandService.GetBandsWithFestivalsAsync()).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading bands: {ex.Message}");
            }
        }

        private async Task AddBand()
        {
            var newBand = new Band
            {
                Name = NewBandName,
                Genre = NewBandGenre
            };

            await BandService.AddAsync(newBand);
            await LoadBands();

            NewBandName = string.Empty;
            NewBandGenre = Genre.Rock;
        }

        private async Task UpdateBand()
        {
            var bandToUpdate = await BandService.GetByIdAsync(UpdateBandId);
            if (bandToUpdate != null)
            {
                bandToUpdate.Name = UpdateBandName;
                bandToUpdate.Genre = UpdateBandGenre;

                await BandService.UpdateAsync(bandToUpdate);
                await LoadBands();

                UpdateBandId = 0;
                UpdateBandName = string.Empty;
                UpdateBandGenre = Genre.Rock;
            }

        }

        private async Task DeleteBand()
        {
            var bandToDelete = await BandService.GetByIdAsync(DeleteBandId);
            if (bandToDelete != null)
            {
                await BandService.DeleteAsync(bandToDelete);
                await LoadBands();

                DeleteBandId = 0;
            }
        }
    }
}