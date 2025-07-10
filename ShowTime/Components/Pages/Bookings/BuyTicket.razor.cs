using Microsoft.AspNetCore.Components;
using ShowTime.Entities;

namespace ShowTime.Components.Pages.Bookings
{
    public partial class BuyTicket
    {
        [Parameter]
        public int FestivalId { get; set; }

        private bool ShowPurchaseCompleted = false;

        private ApplicationUser? currentUser;

        private int BookingPrice = 0;
        private string NewBookingEmail = string.Empty;
        private Festival? NewBookingFestival;
        private DateTime NewBookingDate = DateTime.Now;
        private List<int> NewBookingSelectedDaysIds = new List<int>();

        private List<FestivalDay> FestivalDays = new();

        protected override async Task OnInitializedAsync()
        {
            NewBookingFestival = await FestivalService.GetByIdAsync(FestivalId);
            FestivalDays = NewBookingFestival?.FestivalDays?.OrderBy(fd => fd.Date).ToList() ?? new List<FestivalDay>();
            if (HttpContextAccessor.HttpContext is not null)
            {
                currentUser = await UserAccessor.GetRequiredUserAsync(HttpContextAccessor.HttpContext);
                // Now you can use currentUser
                NewBookingEmail = currentUser?.Email ?? string.Empty;
            }

        }

        private async Task AddBooking()
        {

            if (NewBookingFestival != null)
            {
                var newBooking = new Booking
                {
                    ApplicationUserId = currentUser?.Id,
                    ApplicationUser = currentUser,
                    FestivalId = FestivalId,
                    Festival = NewBookingFestival,
                    BookingDate = NewBookingDate,
                };

                currentUser?.Bookings.Add(newBooking);

                foreach (var dayId in NewBookingSelectedDaysIds)
                {
                    newBooking.BookingFestivalDays.Add(new BookingFestivalDays
                    {
                        FestivalDayId = dayId,
                        Booking = newBooking
                    });
                }

                await BookingService.AddAsync(newBooking);


                ShowPurchaseCompleted = true;
                StateHasChanged();

                await Task.Delay(2000);

                NewBookingEmail = string.Empty;
                NewBookingDate = DateTime.Now;

                NavigateToFestivalList();
            }
        }
        private bool IsDaySelected(int dayId)
        {
            return NewBookingSelectedDaysIds.Contains(dayId);
        }

        private void OnDayCheckedChanged(int dayId, bool isChecked)
        {
            if (isChecked)
            {
                if (!NewBookingSelectedDaysIds.Contains(dayId))
                {
                    NewBookingSelectedDaysIds.Add(dayId);
                    BookingPrice += 50;
                }
            }
            else
            {
                if (NewBookingSelectedDaysIds.Contains(dayId))
                {
                    NewBookingSelectedDaysIds.Remove(dayId);
                    BookingPrice -= 50;
                }
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