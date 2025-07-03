using Microsoft.AspNetCore.Components;
using ShowTime.Entities;
using ShowTime.Services.Implementations;

namespace ShowTime.Components.Pages.Festivals
{
    public partial class FestivalDetails
    {
        [Parameter]
        public int FestivalId { get; set; }
        private Festival? Festival;

        protected override async Task OnInitializedAsync()
        {
            await LoadFestival();
        }

        private async Task LoadFestival()
        {
            try
            {
                Festival = (await FestivalService.GetByIdIncludingAsync(FestivalId, f => f.Bands,
                                                                                     f => f.Bookings));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading festival with id {FestivalId} - {ex.Message}");
            }
        }
    }
}