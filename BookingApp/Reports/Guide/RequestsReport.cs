using BookingApp.DTO;
using BookingApp.Model.Enums;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
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

            // Title
            document.Add(new Paragraph("Izvještaj o zahtjevima za ture").SetFontSize(18).SetBold());

            // Accepted Requests
            document.Add(new Paragraph("Prihvaceni zahtjevi").SetFontSize(14).SetBold());
            foreach (var request in tourRequests.Where(r => r.Status == TourRequestStatus.Accepted))
            {
                document.Add(new Paragraph($"Details: {request.Description}, Location: {request.LocationDTO}, Language: {request.Language}, Tourists: {request.NumberOfTourists}, Date: {request.BeginDate.ToShortDateString()} - {request.EndDate.ToShortDateString()}"));
            }

            // Rejected Requests
            document.Add(new Paragraph("Odbijeni zahtjevi").SetFontSize(14).SetBold());
            foreach (var request in tourRequests.Where(r => r.Status == TourRequestStatus.Rejected))
            {
                document.Add(new Paragraph($"Details: {request.Description}, Location: {request.LocationDTO}, Language: {request.Language}, Tourists: {request.NumberOfTourists}, Date: {request.BeginDate.ToShortDateString()} - {request.EndDate.ToShortDateString()}"));
            }


            document.Close();
        }
    }
}
