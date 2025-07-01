using Microsoft.AspNetCore.Components;
using ShowTime.Entities;

namespace ShowTime.Components.Pages.Bands
{
    public partial class BandList
    {
        List<Band>? Bands = new List<Band>();

        protected override async Task OnInitializedAsync()
        {
            await LoadBands();
        }

        private async Task LoadBands()
        {
            try
            {
                Bands = (await BandService.GetAllIncludingAsync(b => b.Festivals)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading bands: {ex.Message}");
            }
        }

        private void NavigateToCreateBand()
        {
            NavigationManager.NavigateTo("/CreateBand");
        }

        private async void OnLocationChanged(object? sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
        {
            if (e.Location.Contains("/BandList", StringComparison.OrdinalIgnoreCase))
            {
                await LoadBands();
                StateHasChanged();
            }
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}