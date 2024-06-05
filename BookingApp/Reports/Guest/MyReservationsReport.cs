using BookingApp.DTO;
using BookingApp.Model;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.Imaging;
using ceTe.DynamicPDF.PageElements;
using ceTe.DynamicPDF.PageElements.Charting;
using ceTe.DynamicPDF.PageElements.Charting.Series;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Font = ceTe.DynamicPDF.Font;
using Image = ceTe.DynamicPDF.PageElements.Image;
using System.IO;
using Path = System.IO.Path;
using FontFamily = ceTe.DynamicPDF.FontFamily;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using BookingApp.Service;
using BookingApp.Repository;
using BookingApp.InjectorNameSpace;
using BookingApp.Repository.Interfaces;

namespace BookingApp.Reports.Guest
{
    public class MyReservationsReport
    {
        
        
        public void GenerateReport(UserDTO _loggedInGuest, ObservableCollection<AccommodationReservationDTO> _myReservations)
        {
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            AccommodationService _accommodationService = new AccommodationService(accommodationRepository);
        // Create a new PDF document
        Document document = new Document();
            Page firstPage = new Page(PageSize.Letter, PageOrientation.Portrait, 54.0f);
            document.Pages.Add(firstPage);

            // Add Header
            string header = "MY ACCOMMODATION RESERVATIONS REPORT\n\n";
            header += "Print date: " + DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss");
            Label headerLabel = new Label(header, 0, 0, 504, 100, Font.HelveticaBold, 18, TextAlign.Center);
            firstPage.Elements.Add(headerLabel);

            // Add Line Separator
            Line line = new Line(0, 100, 504, 100);
            firstPage.Elements.Add(line);

            // Add Table Header
            Table2 table = new Table2(0, 120, 500, 700);
            table.Columns.Add(80);  // Accommodation Name
            table.Columns.Add(70);  // City
            table.Columns.Add(70);  // Country
            table.Columns.Add(70);  // Start Date
            table.Columns.Add(70);  // End Date
            table.Columns.Add(60);  // Status
            table.Columns.Add(60);  // Rating

            Row2 tableHeader = table.Rows.Add(20, Font.HelveticaBold, 14, RgbColor.Black, RgbColor.Gray);
            tableHeader.CellDefault.Align = TextAlign.Center;
            tableHeader.CellDefault.VAlign = VAlign.Center;
            tableHeader.Cells.Add("Accommodation Name");
            tableHeader.Cells.Add("City");
            tableHeader.Cells.Add("Country");
            tableHeader.Cells.Add("Start Date");
            tableHeader.Cells.Add("End Date");
            tableHeader.Cells.Add("Status");
            tableHeader.Cells.Add("Rating");

            // Add Reservation Data
            foreach (var reservation in _myReservations)
            {
                Row2 row = table.Rows.Add(12, Font.Helvetica, 12);
                row.CellDefault.Align = TextAlign.Center;
                row.CellDefault.VAlign = VAlign.Center;

                var accommodation = _accommodationService.GetById(reservation.AccommodationId);

                row.Cells.Add(accommodation.Name);
                row.Cells.Add(accommodation.Place.City);
                row.Cells.Add(accommodation.Place.Country);
                row.Cells.Add(reservation.BeginDate.ToString("dd.MM.yyyy"));
                row.Cells.Add(reservation.EndDate.ToString("dd.MM.yyyy"));
                row.Cells.Add(reservation.Canceled ? "Canceled" : "Active");
                row.Cells.Add(reservation.RatingDTO != null ? reservation.RatingDTO.GuestRenovationRating.ToString() : "Not Rated");
            }

            firstPage.Elements.Add(table);

            // Save the document
            string reportPath = $"../../../Reports/Guest/Reservation-Report-for-{_loggedInGuest.Username.Replace(" ", "-")}-{DateTime.Now:dd.MM.yyyy.HH.mm}.pdf";
            document.Draw(reportPath);
            Process.Start("cmd", $"/c start {reportPath}");
        }
    }
}
