using ShowTime.Entities;
using ShowTime.Services.Implementations;

namespace ShowTime.Components.Pages.Bands
{
    public partial class BandDetails
    {
        private Band? Band;
        private int BandId;
        private bool isLoading = true;
        private string errorMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await LoadBand();
        }

        private async Task LoadBand()
        {
            isLoading = true;
            errorMessage = string.Empty;

            try
            {
                Band = await BandService.GetByIdIncludingAsync(BandId, b => b.Festivals);

                if (Band == null)
                {
                    errorMessage = "Band not found.";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error loading Band details: {ex.Message}";
                Console.WriteLine($"Error loading Band with id {Band} - {ex.Message}");
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }
    }
}