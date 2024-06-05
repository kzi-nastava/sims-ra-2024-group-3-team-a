using BookingApp.DTO;
using BookingApp.Model.Enums;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Collections.Generic;
using System.Linq;

public class RequestReportService
{
    public void GenerateTourRequestReport(List<OrdinaryTourRequestDTO> tourRequests, string filePath)
    {
        using (var writer = new PdfWriter(filePath))
        {
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            var title = new Paragraph("Izvještaj o zahtjevima za ture")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(18)
                .SetBold()
                .SetMarginBottom(20);
            document.Add(title);

            var acceptedTitle = new Paragraph("Prihvaceni zahtjevi")
                .SetFontSize(14)
                .SetBold()
                .SetMarginBottom(10);
            document.Add(acceptedTitle);

            foreach (var request in tourRequests.Where(r => r.Status == TourRequestStatus.Accepted))
            {
                var acceptedRequest = new Paragraph($"Opis: {request.Description}, Lokacija: {request.LocationDTO}, Jezik: {request.Language}, Broj turista: {request.NumberOfTourists}, Datum: {request.BeginDate.ToShortDateString()}")
                    .SetMarginBottom(5);
                document.Add(acceptedRequest);
            }

            var rejectedTitle = new Paragraph("Odbijeni zahtjevi")
                .SetFontSize(14)
                .SetBold()
                .SetMarginBottom(10);
            document.Add(rejectedTitle);

            foreach (var request in tourRequests.Where(r => r.Status == TourRequestStatus.Rejected))
            {
                var rejectedRequest = new Paragraph($"Opis: {request.Description}, Lokacija: {request.LocationDTO}, Jezik: {request.Language}, Broj turista: {request.NumberOfTourists}, Datum: {request.BeginDate.ToShortDateString()}")
                    .SetMarginBottom(5);
                document.Add(rejectedRequest);
            }

            document.Close();
        }
    }
}
