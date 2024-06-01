using BookingApp.DTO;
using BookingApp.Model;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using ceTe.DynamicPDF.PageElements.Charting;
using ceTe.DynamicPDF.PageElements.Charting.Series;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Reports.Owner
{
    public class AccommodationStatisticsReport
    {

        public void GenerateReport(Dictionary<int, AccommodationStatisticsDTO> _accommodationStatisticsDTO, AccommodationDTO accommodation, int yearForMonthReport = 0)
        {
            if(yearForMonthReport == 0)
            {
                //naslov
                Document document = new Document();
                Page firstPage = new Page(PageSize.Letter, PageOrientation.Portrait, 54.0f);
                document.Pages.Add(firstPage);
                String header = "YEARLY REPORT OF ACCOMMODATION\n Accommodation: " + accommodation.Name + "\n";
                header += "Print date: " + DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss");
                Label label = new Label(header, 0, 0, 504, 100, Font.Helvetica, 18, TextAlign.Center);
                firstPage.Elements.Add(label);

                //grafik1
                Chart yearlyReservations = new Chart(-30, 120, 600, 300);
                PlotArea plotArea = yearlyReservations.PrimaryPlotArea;
                Title title1 = new Title("Number of reservation");
                Title title2 = new Title("From year 2022 till year 2025");
                yearlyReservations.HeaderTitles.Add(title1);
                yearlyReservations.HeaderTitles.Add(title2);

                DateTimeLineSeries lineSeries1 = new DateTimeLineSeries("Reservations number");
                lineSeries1.Width = 2;

                for (int year = 2022; year <= 2025; year++)
                {
                    if (_accommodationStatisticsDTO.TryGetValue(year, out AccommodationStatisticsDTO item))
                    {
                        lineSeries1.Values.Add(item.Reservations, new DateTime(year, 1, 1));
                    }
                    else
                    {
                        lineSeries1.Values.Add(0, new DateTime(year, 1, 1));
                    }
                }

                plotArea.Series.Add(lineSeries1);
                Title title3 = new Title("Reservations number");
                lineSeries1.YAxis.Titles.Add(title3);
                lineSeries1.YAxis.Interval = 1;
                lineSeries1.XAxis.LabelFormat = "yyyy.";
                yearlyReservations.AutoLayout = true;
                firstPage.Elements.Add(yearlyReservations);

                //grafik2
                Chart reservationCancellations = new Chart(-30, 430, 600, 300);
                PlotArea plotArea2 = reservationCancellations.PrimaryPlotArea;
                Title title12 = new Title("Number of reservations canceled");
                Title title22 = new Title("From year 2022 till year 2025");
                reservationCancellations.HeaderTitles.Add(title12);
                reservationCancellations.HeaderTitles.Add(title22);

                DateTimeLineSeries lineSeries2 = new DateTimeLineSeries("Canccelations number");
                lineSeries2.Width = 2;

                for (int year = 2022; year <= 2025; year++)
                {
                    if (_accommodationStatisticsDTO.TryGetValue(year, out AccommodationStatisticsDTO item))
                    {
                        lineSeries2.Values.Add(item.Cancellations, new DateTime(year, 1, 1));
                    }
                    else
                    {
                        lineSeries2.Values.Add(0, new DateTime(year, 1, 1));
                    }
                }

                plotArea2.Series.Add(lineSeries2);
                Title title32 = new Title("Cancellation number");
                lineSeries2.YAxis.Titles.Add(title32);
                lineSeries2.YAxis.Interval = 1;
                lineSeries2.XAxis.LabelFormat = "yyyy.";
                reservationCancellations.AutoLayout = true;
                firstPage.Elements.Add(reservationCancellations);

                //grafik3
                Page secondPage = new Page();
                document.Pages.Add(secondPage);

                Chart reservationChangeRequests = new Chart(0, 0, 500, 300);
                PlotArea plotArea3 = reservationChangeRequests.PrimaryPlotArea;
                Title title13 = new Title("Number of requests that reservaton should be moved to another date");
                Title title23 = new Title("From year 2022 till year 2025");
                reservationChangeRequests.HeaderTitles.Add(title13);
                reservationChangeRequests.HeaderTitles.Add(title23);

                DateTimeLineSeries lineSeries3 = new DateTimeLineSeries("Date change requests number");
                lineSeries3.Width = 2;

                for (int year = 2022; year <= 2025; year++)
                {
                    if (_accommodationStatisticsDTO.TryGetValue(year, out AccommodationStatisticsDTO item))
                    {
                        lineSeries3.Values.Add(item.AccommodationReservationChanges, new DateTime(year, 1, 1));
                    }
                    else
                    {
                        lineSeries3.Values.Add(0, new DateTime(year, 1, 1));
                    }
                }

                plotArea3.Series.Add(lineSeries3);
                Title title33 = new Title("Date change requests number");
                lineSeries3.YAxis.Titles.Add(title33);
                lineSeries3.YAxis.Interval = 1;
                lineSeries3.XAxis.LabelFormat = "yyyy.";
                reservationChangeRequests.AutoLayout = true;
                secondPage.Elements.Add(reservationChangeRequests);

                // Grafik4
                Chart renovationRecommendations = new Chart(0, 350, 500, 300);
                PlotArea plotArea4 = renovationRecommendations.PrimaryPlotArea;
                Title title14 = new Title("Number of recommendations for accommodation renovation");
                Title title24 = new Title("From year 2022 till year 2025");
                renovationRecommendations.HeaderTitles.Add(title14);
                renovationRecommendations.HeaderTitles.Add(title24);

                DateTimeLineSeries lineSeries4 = new DateTimeLineSeries("Renovation recommendations number");
                lineSeries4.Width = 2;

                for (int year = 2022; year <= 2025; year++)
                {
                    if (_accommodationStatisticsDTO.TryGetValue(year, out AccommodationStatisticsDTO item))
                    {
                        lineSeries4.Values.Add(item.AccommodationRenovationRecommendations, new DateTime(year, 1, 1));
                    }
                    else
                    {
                        lineSeries4.Values.Add(0, new DateTime(year, 1, 1));
                    }
                }

                plotArea4.Series.Add(lineSeries4);
                Title title34 = new Title("Renovation recommendations number");
                lineSeries4.YAxis.Titles.Add(title34);
                lineSeries4.YAxis.Interval = 1;
                lineSeries4.XAxis.LabelFormat = "yyyy.";
                renovationRecommendations.AutoLayout = true;
                secondPage.Elements.Add(renovationRecommendations);

                //tabela
                Page thirdPage = new Page();
                document.Pages.Add(thirdPage);

                Table2 table = new Table2(-30, 0, 700, 800);
                table.CellDefault.Border.Color = RgbColor.LightBlue;
                table.CellDefault.Border.LineStyle = LineStyle.Solid;
                table.CellDefault.Padding.Value = 3.0f;

                table.Columns.Add(100);
                table.Columns.Add(100);
                table.Columns.Add(100);
                table.Columns.Add(100);
                table.Columns.Add(100);

                Row2 tableHeader = table.Rows.Add(20, Font.HelveticaBold, 14, RgbColor.Black, RgbColor.Gray);
                tableHeader.CellDefault.Align = TextAlign.Center;
                tableHeader.CellDefault.VAlign = VAlign.Center;
                tableHeader.Cells.Add("Yearly accommodation statistics in table for " + accommodation.Name + " from 2022 to 2025").ColumnSpan = 5;

                Row2 headerContent = table.Rows.Add(Font.HelveticaBoldOblique, 12);
                headerContent.CellDefault.Align = TextAlign.Center;

                headerContent.Cells.Add("Year");
                headerContent.Cells.Add("Reservations");
                headerContent.Cells.Add("Cancellations");
                headerContent.Cells.Add("Date change requests");
                headerContent.Cells.Add("Renovation reccomendations");

                Row2 tableContent;

                for (int year = 2022; year <= 2025; year++)
                {
                    AccommodationStatisticsDTO accommodationStatistics = null;
                    if (_accommodationStatisticsDTO.TryGetValue(year, out AccommodationStatisticsDTO item))
                    {
                        accommodationStatistics = item;
                    }
                    else
                    {
                        accommodationStatistics = new AccommodationStatisticsDTO();
                    }

                    tableContent = table.Rows.Add(10);
                    tableContent.Cells.Add(new FormattedTextArea(year.ToString(), 0, 0, 140, 50, FontFamily.Helvetica, 12, false));
                    tableContent.Cells.Add(new FormattedTextArea(accommodationStatistics.Reservations.ToString(), 0, 0, 140, 50, FontFamily.Helvetica, 12, false));
                    tableContent.Cells.Add(new FormattedTextArea(accommodationStatistics.Cancellations.ToString(), 0, 0, 140, 50, FontFamily.Helvetica, 12, false));
                    tableContent.Cells.Add(new FormattedTextArea(accommodationStatistics.AccommodationReservationChanges.ToString(), 0, 0, 140, 50, FontFamily.Helvetica, 12, false));
                    tableContent.Cells.Add(new FormattedTextArea(accommodationStatistics.AccommodationRenovationRecommendations.ToString(), 0, 0, 140, 50, FontFamily.Helvetica, 12, false));
                }

                thirdPage.Elements.Add(table);

                string reportPath = "../../../../Reports/Owner/Yearly Report for " + accommodation.Name + " " + DateTime.Now.ToString("dd.MM.yyyy.HH.mm") + ".pdf";
                document.Draw(reportPath);
            }
            else
            {
                //naslov
                Document document = new Document();
                Page firstPage = new Page(PageSize.Letter, PageOrientation.Portrait, 54.0f);
                document.Pages.Add(firstPage);
                String header = "MONTHLY REPORT OF ACCOMMODATION\n Accommodation: " + accommodation.Name + " for year: " + yearForMonthReport.ToString() + "\n";
                header += "Print date: " + DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss");
                Label label = new Label(header, 0, 0, 504, 100, Font.Helvetica, 18, TextAlign.Center);
                firstPage.Elements.Add(label);

                //grafik1
                Chart monthlyReservations = new Chart(-30, 120, 600, 300);
                PlotArea plotArea = monthlyReservations.PrimaryPlotArea;
                Title title1 = new Title("Number of reservation");
                Title title2 = new Title("From january - december " + yearForMonthReport.ToString());
                monthlyReservations.HeaderTitles.Add(title1);
                monthlyReservations.HeaderTitles.Add(title2);

                DateTimeLineSeries lineSeries1 = new DateTimeLineSeries("Reservations number");
                lineSeries1.Width = 2;

                for (int month = 1; month <= 12; month++)
                {
                    if (_accommodationStatisticsDTO.TryGetValue(month, out AccommodationStatisticsDTO item))
                    {
                        lineSeries1.Values.Add(item.Reservations, new DateTime(yearForMonthReport, month, 1));
                    }
                    else
                    {
                        lineSeries1.Values.Add(0, new DateTime(yearForMonthReport, month, 1));
                    }
                }

                plotArea.Series.Add(lineSeries1);
                Title title3 = new Title("Reservations number");
                lineSeries1.YAxis.Titles.Add(title3);
                lineSeries1.YAxis.Interval = 1;
                lineSeries1.XAxis.LabelFormat = "MMM";
                lineSeries1.XAxis.Interval = 1;
                monthlyReservations.AutoLayout = true;
                firstPage.Elements.Add(monthlyReservations);

                //grafik2
                Chart reservationCancellations1 = new Chart(-30, 430, 600, 300);
                PlotArea plotArea2 = reservationCancellations1.PrimaryPlotArea;
                Title title12 = new Title("Number of reservations canceled");
                Title title22 = new Title("From january - december " + yearForMonthReport.ToString());
                reservationCancellations1.HeaderTitles.Add(title12);
                reservationCancellations1.HeaderTitles.Add(title22);

                DateTimeLineSeries lineSeries2 = new DateTimeLineSeries("Canccelations number");
                lineSeries2.Width = 2;

                for (int month = 1; month <= 12; month++)
                {
                    if (_accommodationStatisticsDTO.TryGetValue(month, out AccommodationStatisticsDTO item))
                    {
                        lineSeries2.Values.Add(item.Cancellations, new DateTime(yearForMonthReport, month, 1));
                    }
                    else
                    {
                        lineSeries2.Values.Add(0, new DateTime(yearForMonthReport, month, 1));
                    }
                }

                plotArea2.Series.Add(lineSeries2);
                Title title32 = new Title("Cancellation number");
                lineSeries2.YAxis.Titles.Add(title32);
                lineSeries2.YAxis.Interval = 1;
                lineSeries2.XAxis.LabelFormat = "MMM";
                lineSeries2.XAxis.Interval = 1;
                reservationCancellations1.AutoLayout = true;
                firstPage.Elements.Add(reservationCancellations1);

                //grafik3
                Page secondPage = new Page();
                document.Pages.Add(secondPage);

                Chart reservationChangeRequests = new Chart(0, 0, 500, 300);
                PlotArea plotArea3 = reservationChangeRequests.PrimaryPlotArea;
                Title title13 = new Title("Number of requests that reservaton should be moved to another date");
                Title title23 = new Title("From january - december " + yearForMonthReport.ToString());
                reservationChangeRequests.HeaderTitles.Add(title13);
                reservationChangeRequests.HeaderTitles.Add(title23);

                DateTimeLineSeries lineSeries3 = new DateTimeLineSeries("Date change requests number");
                lineSeries3.Width = 2;

                for (int month = 1; month <= 12; month++)
                {
                    if (_accommodationStatisticsDTO.TryGetValue(month, out AccommodationStatisticsDTO item))
                    {
                        lineSeries3.Values.Add(item.AccommodationReservationChanges, new DateTime(yearForMonthReport, month, 1));
                    }
                    else
                    {
                        lineSeries3.Values.Add(0, new DateTime(yearForMonthReport, month, 1));
                    }
                }

                plotArea3.Series.Add(lineSeries3);
                Title title33 = new Title("Date change requests number");
                lineSeries3.YAxis.Titles.Add(title33);
                lineSeries3.YAxis.Interval = 1;
                lineSeries3.XAxis.LabelFormat = "MMM";
                lineSeries3.XAxis.Interval = 1;
                reservationChangeRequests.AutoLayout = true;
                secondPage.Elements.Add(reservationChangeRequests);

                // Grafik4
                Chart renovationRecommendations = new Chart(0, 350, 500, 300);
                PlotArea plotArea4 = renovationRecommendations.PrimaryPlotArea;
                Title title14 = new Title("Number of recommendations for accommodation renovation");
                Title title24 = new Title("From january - december " + yearForMonthReport.ToString());
                renovationRecommendations.HeaderTitles.Add(title14);
                renovationRecommendations.HeaderTitles.Add(title24);

                DateTimeLineSeries lineSeries4 = new DateTimeLineSeries("Renovation recommendations number");
                lineSeries4.Width = 2;

                for (int month = 1; month <= 12; month++)
                {
                    if (_accommodationStatisticsDTO.TryGetValue(month, out AccommodationStatisticsDTO item))
                    {
                        lineSeries4.Values.Add(item.AccommodationRenovationRecommendations, new DateTime(yearForMonthReport, month, 1));
                    }
                    else
                    {
                        lineSeries4.Values.Add(0, new DateTime(yearForMonthReport, month, 1));
                    }
                }

                plotArea4.Series.Add(lineSeries4);
                Title title34 = new Title("Renovation recommendations number");
                lineSeries4.YAxis.Titles.Add(title34);
                lineSeries4.YAxis.Interval = 1;
                lineSeries4.XAxis.LabelFormat = "MMM";
                lineSeries4.XAxis.Interval = 1;
                renovationRecommendations.AutoLayout = true;
                secondPage.Elements.Add(renovationRecommendations);

                //tabela
                Page thirdPage = new Page();
                document.Pages.Add(thirdPage);

                Table2 table = new Table2(-30, 0, 700, 800);
                table.CellDefault.Border.Color = RgbColor.LightBlue;
                table.CellDefault.Border.LineStyle = LineStyle.Solid;
                table.CellDefault.Padding.Value = 3.0f;

                table.Columns.Add(100);
                table.Columns.Add(100);
                table.Columns.Add(100);
                table.Columns.Add(100);
                table.Columns.Add(100);

                Row2 tableHeader = table.Rows.Add(20, Font.HelveticaBold, 14, RgbColor.Black, RgbColor.Gray);
                tableHeader.CellDefault.Align = TextAlign.Center;
                tableHeader.CellDefault.VAlign = VAlign.Center;
                tableHeader.Cells.Add("Monthly accommodation statistics in table for " + accommodation.Name + " from year: " + yearForMonthReport.ToString()).ColumnSpan = 5;

                Row2 headerContent = table.Rows.Add(20, Font.HelveticaBoldOblique, 12);
                headerContent.CellDefault.Align = TextAlign.Center;

                headerContent.Cells.Add("Month");
                headerContent.Cells.Add("Reservations");
                headerContent.Cells.Add("Cancellations");
                headerContent.Cells.Add("Date change requests");
                headerContent.Cells.Add("Renovation reccomendations");

                Row2 tableContent;

                for (int month = 1; month <= 12; month++)
                {
                    AccommodationStatisticsDTO accommodationStatistics = null;
                    if (_accommodationStatisticsDTO.TryGetValue(month, out AccommodationStatisticsDTO item))
                    {
                        accommodationStatistics = item;
                    }
                    else
                    {
                        accommodationStatistics = new AccommodationStatisticsDTO();
                    }

                    tableContent = table.Rows.Add(10);
                    tableContent.Cells.Add(new FormattedTextArea(new DateTime(yearForMonthReport, month, 1).ToString("MMM"), 0, 0, 140, 50, FontFamily.Helvetica, 12, false));
                    tableContent.Cells.Add(new FormattedTextArea(accommodationStatistics.Reservations.ToString(), 0, 0, 140, 50, FontFamily.Helvetica, 12, false));
                    tableContent.Cells.Add(new FormattedTextArea(accommodationStatistics.Cancellations.ToString(), 0, 0, 140, 50, FontFamily.Helvetica, 12, false));
                    tableContent.Cells.Add(new FormattedTextArea(accommodationStatistics.AccommodationReservationChanges.ToString(), 0, 0, 140, 50, FontFamily.Helvetica, 12, false));
                    tableContent.Cells.Add(new FormattedTextArea(accommodationStatistics.AccommodationRenovationRecommendations.ToString(), 0, 0, 140, 50, FontFamily.Helvetica, 12, false));
                }

                thirdPage.Elements.Add(table);


                string reportPath = "../../../../Reports/Owner/Monthly Report for " + accommodation.Name + " " + yearForMonthReport.ToString() + " " + DateTime.Now.ToString("dd.MM.yyyy.HH.mm") + ".pdf";
                document.Draw(reportPath);
            }
        }
    }
}
