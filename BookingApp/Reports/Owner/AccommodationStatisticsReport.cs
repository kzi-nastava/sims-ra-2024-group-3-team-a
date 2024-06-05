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

                //slika
                if (!string.IsNullOrEmpty(accommodation.Images[0]))
                {
                    string imagePath = accommodation.Images[0];
                    if (Path.GetExtension(imagePath).ToLower() == ".png")
                    {
                        try
                        {
                            string jpgImagePath = ConvertPngToJpg(imagePath);
                            if (!string.IsNullOrEmpty(jpgImagePath))
                            {
                                float scale = GetScale(jpgImagePath);
                                Image image = new Image(jpgImagePath, 0, 100, scale);
                                firstPage.Elements.Add(image);
                            }
                        }
                        catch (Exception ex)
                        {
                            Label errorLabel = new Label("Error loading image: " + ex.Message, 0, 100, 504, 30, Font.Helvetica, 12, TextAlign.Left);
                            firstPage.Elements.Add(errorLabel);
                        }
                    }
                    else
                    {
                        try
                        {
                            float scale = GetScale(imagePath);
                            Image image = new Image(imagePath, 0, 100, scale);
                            firstPage.Elements.Add(image);
                        }
                        catch (Exception ex)
                        {
                            Label errorLabel = new Label("Error loading image: " + ex.Message, 0, 100, 504, 30, Font.Helvetica, 12, TextAlign.Left);
                            firstPage.Elements.Add(errorLabel);
                        }
                    }
                }
                else
                {
                    Label errorLabel = new Label("Image file not found.", 0, 100, 504, 30, Font.Helvetica, 12, TextAlign.Left);
                    firstPage.Elements.Add(errorLabel);
                }

                //grafik1
                Chart yearlyReservations = new Chart(-30, 380, 600, 300);
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
                Page secondPage = new Page();
                document.Pages.Add(secondPage);

                Chart reservationCancellations = new Chart(-30, 0, 600, 300);
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
                secondPage.Elements.Add(reservationCancellations);

                //grafik3
                Chart reservationChangeRequests = new Chart(0, 380, 500, 300);
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
                Page thirdPage = new Page();
                document.Pages.Add(thirdPage);

                Chart renovationRecommendations = new Chart(-30, 0, 500, 300);
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
                thirdPage.Elements.Add(renovationRecommendations);

                //tabela
                Table2 table = new Table2(-30, 380, 504, 800); 
                table.CellDefault.Border.Color = RgbColor.LightBlue;
                table.CellDefault.Border.LineStyle = LineStyle.Solid;
                table.CellDefault.Padding.Value = 5.0f; 

                for (int i = 0; i < 5; i++)
                {
                    table.Columns.Add((float)100.8); 
                }
 
                Row2 tableHeader = table.Rows.Add(25, Font.HelveticaBold, 16, RgbColor.White, RgbColor.DarkBlue);
                tableHeader.CellDefault.Align = TextAlign.Center;
                tableHeader.CellDefault.VAlign = VAlign.Center;
                tableHeader.Cells.Add("Yearly Accommodation Statistics for " + accommodation.Name + " (2022-2025)").ColumnSpan = 5;

                Row2 headerContent = table.Rows.Add(20, Font.HelveticaBoldOblique, 12, RgbColor.White, RgbColor.Gray);
                headerContent.CellDefault.Align = TextAlign.Center;
                headerContent.Cells.Add("Year");
                headerContent.Cells.Add("Reservations");
                headerContent.Cells.Add("Cancellations");
                headerContent.Cells.Add("Date Change Requests");
                headerContent.Cells.Add("Renovation Recommendations");

                for (int year = 2022; year <= 2025; year++)
                {
                    AccommodationStatisticsDTO accommodationStatistics = _accommodationStatisticsDTO.TryGetValue(year, out AccommodationStatisticsDTO item) ? item : new AccommodationStatisticsDTO();

                    Row2 tableContent = table.Rows.Add(15, Font.Helvetica, 12, RgbColor.Black, year % 2 == 0 ? RgbColor.LightSlateGray : RgbColor.White);
                    tableContent.Cells.Add(year.ToString());
                    tableContent.Cells.Add(accommodationStatistics.Reservations.ToString());
                    tableContent.Cells.Add(accommodationStatistics.Cancellations.ToString());
                    tableContent.Cells.Add(accommodationStatistics.AccommodationReservationChanges.ToString());
                    tableContent.Cells.Add(accommodationStatistics.AccommodationRenovationRecommendations.ToString());
                }

                thirdPage.Elements.Add(table);


                string reportPath = "../../../../Reports/Owner/Yearly-Report-for-" + accommodation.Name.Replace(" ", "-") + "-" + DateTime.Now.ToString("dd.MM.yyyy.HH.mm") + ".pdf";
                document.Draw(reportPath);
                Process.Start("cmd", "/c start " + reportPath);
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

                //slika
                if (!string.IsNullOrEmpty(accommodation.Images[0]))
                {
                    string imagePath = accommodation.Images[0];
                    if (Path.GetExtension(imagePath).ToLower() == ".png")
                    {
                        try
                        {
                            string jpgImagePath = ConvertPngToJpg(imagePath);
                            if (!string.IsNullOrEmpty(jpgImagePath))
                            {
                                float scale = GetScale(jpgImagePath);
                                Image image = new Image(jpgImagePath, 0, 100, scale);
                                firstPage.Elements.Add(image);
                            }
                        }
                        catch (Exception ex)
                        {
                            Label errorLabel = new Label("Error loading image: " + ex.Message, 0, 100, 504, 30, Font.Helvetica, 12, TextAlign.Left);
                            firstPage.Elements.Add(errorLabel);
                        }
                    }
                    else
                    {
                        try
                        {
                            float scale = GetScale(imagePath);
                            Image image = new Image(imagePath, 0, 100, scale);
                            firstPage.Elements.Add(image);
                        }
                        catch (Exception ex)
                        {
                            Label errorLabel = new Label("Error loading image: " + ex.Message, 0, 100, 504, 30, Font.Helvetica, 12, TextAlign.Left);
                            firstPage.Elements.Add(errorLabel);
                        }
                    }
                }
                else
                {
                    Label errorLabel = new Label("Image file not found.", 0, 100, 504, 30, Font.Helvetica, 12, TextAlign.Left);
                    firstPage.Elements.Add(errorLabel);
                }

                //grafik1
                Chart monthlyReservations = new Chart(-30, 380, 600, 300);
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
                Page secondPage = new Page();
                document.Pages.Add(secondPage);

                Chart reservationCancellations1 = new Chart(-30, 0, 600, 300);
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
                secondPage.Elements.Add(reservationCancellations1);

                //grafik3
                Chart reservationChangeRequests = new Chart(0, 380, 500, 300);
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
                Page thirdPage = new Page();
                document.Pages.Add(thirdPage);

                Chart renovationRecommendations = new Chart(-30, 0, 500, 300);
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
                thirdPage.Elements.Add(renovationRecommendations);

                // Table
                Page forthPage = new Page();
                document.Pages.Add(forthPage);

                Table2 table = new Table2(-30, 0, 504, 800);
                table.CellDefault.Border.Color = RgbColor.LightBlue;
                table.CellDefault.Border.LineStyle = LineStyle.Solid;
                table.CellDefault.Padding.Value = 5.0f;

                for (int i = 0; i < 5; i++)
                {
                    table.Columns.Add((float)100.8);
                }

                Row2 tableHeader = table.Rows.Add(25, Font.HelveticaBold, 16, RgbColor.White, RgbColor.DarkBlue);
                tableHeader.CellDefault.Align = TextAlign.Center;
                tableHeader.CellDefault.VAlign = VAlign.Center;
                tableHeader.Cells.Add("Monthly accommodation statistics in table for " + accommodation.Name + " from year: " + yearForMonthReport.ToString()).ColumnSpan = 5;

                Row2 headerContent = table.Rows.Add(20, Font.HelveticaBoldOblique, 12, RgbColor.White, RgbColor.Gray);
                headerContent.CellDefault.Align = TextAlign.Center;
                headerContent.Cells.Add("Month");
                headerContent.Cells.Add("Reservations");
                headerContent.Cells.Add("Cancellations");
                headerContent.Cells.Add("Date Change Requests");
                headerContent.Cells.Add("Renovation Recommendations");

                for (int month = 1; month <= 12; month++)
                {
                    AccommodationStatisticsDTO accommodationStatistics = _accommodationStatisticsDTO.TryGetValue(month, out AccommodationStatisticsDTO item) ? item : new AccommodationStatisticsDTO();

                    Row2 tableContent = table.Rows.Add(15, Font.Helvetica, 12, RgbColor.Black, month % 2 == 0 ? RgbColor.LightSlateGray : RgbColor.White);
                    tableContent.Cells.Add(new DateTime(yearForMonthReport, month, 1).ToString("MMM"));
                    tableContent.Cells.Add(accommodationStatistics.Reservations.ToString());
                    tableContent.Cells.Add(accommodationStatistics.Cancellations.ToString());
                    tableContent.Cells.Add(accommodationStatistics.AccommodationReservationChanges.ToString());
                    tableContent.Cells.Add(accommodationStatistics.AccommodationRenovationRecommendations.ToString());
                }

                forthPage.Elements.Add(table);


                string reportPath = "../../../../Reports/Owner/Monthly-Report-for-" + accommodation.Name.Replace(" ", "-") + "-" + yearForMonthReport.ToString() + "-" + DateTime.Now.ToString("dd.MM.yyyy.HH.mm") + ".pdf";
                document.Draw(reportPath);
                Process.Start("cmd", "/c start " + reportPath);
            }
        }

        private string ConvertPngToJpg(string pngFilePath)
        {
            try
            {
                using (Bitmap bmp = new Bitmap(pngFilePath))
                {
                    string jpgFilePath = Path.Combine(Path.GetDirectoryName(pngFilePath), Path.GetFileNameWithoutExtension(pngFilePath) + ".jpg");
                    bmp.Save(jpgFilePath, ImageFormat.Jpeg);
                    return jpgFilePath;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error converting PNG to JPG: " + ex.Message);
                return null;
            }
        }

        private float GetScale(string jpgImagePath)
        {
            float desiredWidth = 400;
            float desiredHeight = 320;

            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(jpgImagePath);
            float originalWidth = originalImage.Width;
            float originalHeight = originalImage.Height;

            float scaleX = desiredWidth / originalWidth;
            float scaleY = desiredHeight / originalHeight;

            float scale = Math.Min(scaleX, scaleY);

            return scale;
        }
    }
}
