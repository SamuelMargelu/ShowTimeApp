using ShowTime.Entities;
using ShowTime.Services.Implementations;

namespace ShowTime.Components.Pages
{
    public partial class BookingCrudTest
    {
        private List<Booking>? Bookings;

        private int NewFestivalId;
        private string NewBookingEmail = string.Empty;
        private Festival? NewBookingFestival;
        private DateTime NewBookingDate = DateTime.Now;

        private int UpdateBookingId;
        private int UpdateBookingFestivalId;
        private string UpdateBookingEmail = string.Empty;
        private Festival? UpdateBookingFestival = null;

        private int DeleteBookingId;

        protected override async Task OnInitializedAsync()
        {
            await LoadBookings();
            
        }

        private async Task LoadBookings()
        {
            try
            {
                Bookings = (await BookingService.GetAllIncludingAsync(b => b.Festival)).ToList();
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
                    //Email = NewBookingEmail,
                    FestivalId = NewFestivalId,
                    Festival = NewBookingFestival,
                    BookingDate = NewBookingDate,
                };

                await BookingService.AddAsync(Booking);
                await LoadBookings();

                NewFestivalId = 0;
                NewBookingEmail = string.Empty;
                NewBookingDate = DateTime.Now;
            }
        }

        private async Task UpdateBooking()
        {
            var bookingToUpdate = await BookingService.GetByIdAsync(UpdateBookingId);

            if (bookingToUpdate != null)
            {
                UpdateBookingFestival = await FestivalSerivce.GetByIdAsync(UpdateBookingFestivalId);

                if (UpdateBookingFestival != null)
                {
                    var newBooking = new Booking
                    {
                        //Email = UpdateBookingEmail,
                        FestivalId = UpdateBookingFestivalId,
                        Festival = UpdateBookingFestival,
                        BookingDate = DateTime.Now,
                    };

                    await BookingService.UpdateAsync(newBooking);
                    await LoadBookings();
                    
                    UpdateBookingId = 0;
                    UpdateBookingFestivalId = 0;
                    UpdateBookingEmail = string.Empty;
                    UpdateBookingFestival = null;
                }
            }
        }

        private async Task DeleteBooking()
        {
            var bookingToDelete = await BookingService.GetByIdAsync(DeleteBookingId);

            if (bookingToDelete != null )
            {
                await BookingService.DeleteAsync(bookingToDelete);
                await LoadBookings();

                UpdateBookingId = 0;
            }
        }
    }
}