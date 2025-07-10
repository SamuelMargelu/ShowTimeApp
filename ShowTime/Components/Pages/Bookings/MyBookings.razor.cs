using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ShowTime.Entities;

namespace ShowTime.Components.Pages.Bookings
{
    public partial class MyBookings
    {
        private List<Booking>? Bookings = new List<Booking>();
        private ApplicationUser? CurrentUser;
        private AuthenticationStateProvider AuthenticationStateProvider;

        public MyBookings(AuthenticationStateProvider authenticationStateProvider)
        {
            this.AuthenticationStateProvider = authenticationStateProvider;
        }

        protected override async Task OnInitializedAsync()
        {
            if (HttpContextAccessor.HttpContext is not null)
            {
                CurrentUser = await UserAccessor.GetRequiredUserAsync(HttpContextAccessor.HttpContext);
            }
            await LoadBookings();
        }

        private async Task LoadBookings()
        {
            // Get all the bookings for the current user
            if (CurrentUser != null)
            {
                Bookings = (await BookingService.GetBookingsByUserIdAsync(CurrentUser.Id)).ToList();
            }
            else
            {
                Bookings = new List<Booking>();
            }

        }

        private void NavigateToFestivalDetails(int festivalId)
        {
            NavigationManager.NavigateTo($"/FestivalDetails/{festivalId}");
        }
    }
}