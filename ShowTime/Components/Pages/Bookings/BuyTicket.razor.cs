using Microsoft.AspNetCore.Components;
using ShowTime.Entities;

namespace ShowTime.Components.Pages.Bookings
{
    public partial class BuyTicket
    {
        [Parameter]
        public int FestivalId { get; set; }

        private bool ShowPurchaseCompleted = false;

        private string NewBookingEmail = string.Empty;
        private Festival? NewBookingFestival;
        private DateTime NewBookingDate = DateTime.Now;

        protected override async Task OnInitializedAsync()
        {
            NewBookingFestival = await FestivalService.GetByIdAsync(FestivalId);

        }

        private async Task AddBooking()
        {

            if (NewBookingFestival != null)
            {
                var newBoking = new Booking
                {
                    Email = NewBookingEmail,
                    FestivalId = FestivalId,
                    Festival = NewBookingFestival,
                    BookingDate = NewBookingDate,
                };

                await BookingService.AddAsync(newBoking);


                ShowPurchaseCompleted = true;
                StateHasChanged();

                await Task.Delay(2000);

                NewBookingEmail = string.Empty;
                NewBookingDate = DateTime.Now;

                NavigateToFestivalList();
            }
        }

        private void NavigateToFestivalList()
        {
            NavigationManager.NavigateTo("/FestivalList");
        }

        private void NavigateToFestivalDetails()
        {
            NavigationManager.NavigateTo($"/FestivalDetails/{FestivalId}");
        }
    }
}