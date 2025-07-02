using ShowTime.Entities;

namespace ShowTime.Components.Pages.Festivals
{
    public partial class FestivalList
    {
        private List<Festival>? Festivals;

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
    }
}