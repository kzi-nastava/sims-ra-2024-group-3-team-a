using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model.Enums
{
    public enum MessageType
    {
        NotRatedNotification,
        NewReviewNotification,
        AccommodationChangeRequest,
        AccommodationReservationCanceled,
        ForumOpened,
        GoodLocationReccomendation,
        BadLocationReccomendation,

        TourAttendance,
        AcceptedChangeRequest,
        NewCreatedTour,
        RejectedChangeRequest,
        AcceptedTourRequest
    }
}
