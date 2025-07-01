using ShowTime.Entities;
using ShowTime.Services.Implementations;

namespace ShowTime.Components.Pages
{
    public partial class BookingCrudTest
    {
        private List<Booking>? Bookings;

        private string NewBookingEmail = string.Empty;
        private int NewFestivalId;
        private Festival? NewBookingFestival;
        private DateTime NewBookingDate = DateTime.Now;

        private int UpdateBooking;
        private string UpdateBookingEmail = string.Empty;
        private DateTime UpdateBookingDate = DateTime.Now;

        private int DeleteBookingId;

        protected override async Task OnInitializedAsync()
        {
            await LoadBookings();
        }

        private async Task LoadBookings()
        {
            try
            {
                Bookings = (await BookingService.GetAllAsync()).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading bookings: {ex.Message}");
            }
        }

        private async Task AddBooking()
        {
            NewBookingFestival = await FestivalSerivce.GetByIdAsync(NewFestivalId);

            if (NewBookingFestival != null)
            {
                var Booking = new Booking
                {
                    Email = NewBookingEmail,
                    FestivalId = NewFestivalId,
                    Festival = NewBookingFestival,
                    BookingDate = NewBookingDate,
                };

                await BookingService.AddAsync(Booking);
                await LoadBookings();

                NewBookingEmail = string.Empty;
                NewFestivalId = 0;
                Booking.Festival = null;
                NewBookingDate = DateTime.Now;
            }
        }
    }
}