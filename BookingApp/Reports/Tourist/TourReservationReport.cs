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

namespace BookingApp.Reports.Tourist
{
    public class TourReservationReport
    {

        public void GenerateReport(TourReservationDTO tourReservationDTO, TourDTO tourDTO, List<KeyPoint> keyPoints, ObservableCollection<TouristDTO> tourists, bool isVoucherUsed)
        {

            Document document = new Document();
            Page firstPage = new Page(PageSize.Letter, PageOrientation.Portrait, 54.0f);
            document.Pages.Add(firstPage);
            String header = "TOUR RESERVATION REPORT\n Accommodation: " + tourDTO.Name + "\n";
            header += "Print date: " + DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss");
            Label label = new Label(header, 0, 0, 504, 100, Font.HelveticaBold, 18, TextAlign.Center);
            firstPage.Elements.Add(label);
            //Lajna
            Line line = new Line(0, 100, 504, 100);
            firstPage.Elements.Add(line);

         

            //slika
            if (!string.IsNullOrEmpty(tourDTO.Images[0]))
            {
                string imagePath = tourDTO.Images[0];
                if (Path.GetExtension(imagePath).ToLower() == ".png")
                {
                    try
                    {
                        string jpgImagePath = ConvertPngToJpg(imagePath);
                        if (!string.IsNullOrEmpty(jpgImagePath))
                        {
                            float scale = GetScale(jpgImagePath);
                            Image image = new Image(jpgImagePath, 10, 120, scale);
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
                        Image image = new Image(imagePath, 10, 120, scale);
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

            String locationInfo = "Location:" + " " + tourDTO.LocationDTOString + "\n\n";
            locationInfo += "Date of begining:" + " " + tourDTO.BeginingTime.ToString() + "\n\n";
            locationInfo += "Language:" + " " + tourDTO.Language.ToString() + "\n\n";
            locationInfo += "Maximum number of tourists:" + " " + tourDTO.MaxTouristNumber.ToString() + "\n\n";
            locationInfo += "Current capacity:" + " " + tourDTO.CurrentCapacity.ToString() + "\n\n";
            locationInfo += "Guide:" + " " + "Neko Nekic" + "\n\n";

          

            Label labelInfo = new Label(locationInfo, 280, 120, 550, 200, Font.Helvetica, 13, TextAlign.Left);
            firstPage.Elements.Add(labelInfo);

            String description = "Description:" + "\n";
             description += tourDTO.Description;
          
            if(description!="")
            {
                Label descriptionLabel = new Label(description, 0, 350, 500, 300, Font.Helvetica, 12, TextAlign.Left);
                firstPage.Elements.Add(descriptionLabel);
            }
            else
            {
                Label descriptionLabel = new Label("There is no description for this tour.", 280, 140, 550, 200, Font.Helvetica, 13, TextAlign.Left);
                firstPage.Elements.Add(descriptionLabel);
            }
            int brojac = 0;

            String keyPointsString="KeyPoints:"+"\n";
            foreach(KeyPoint kp in keyPoints) 
            {

                keyPointsString += "•" + kp.Name.ToString() + "\n";
                brojac++;
            }
            if (brojac != 0)
            {
                Label keyPointsStringLabel = new Label(keyPointsString, 0, 500, 200, 300, Font.Helvetica, 13, TextAlign.Left);
                firstPage.Elements.Add(keyPointsStringLabel);
            }
            else
            {
                Label keyPointsStringLabel = new Label("There is no keypoints for this tour.", 280, 140, 550, 200, Font.Helvetica, 13, TextAlign.Left);
                firstPage.Elements.Add(keyPointsStringLabel);
            }

            if (!string.IsNullOrEmpty(tourDTO.Images[1]))
            {
                string imagePath = tourDTO.Images[1];
                if (Path.GetExtension(imagePath).ToLower() == ".png")
                {
                    try
                    {
                        string jpgImagePath = ConvertPngToJpg(imagePath);
                        if (!string.IsNullOrEmpty(jpgImagePath))
                        {
                            float scale = GetScale(jpgImagePath);
                            Image image = new Image(jpgImagePath, 280, 500, scale);
                            firstPage.Elements.Add(image);
                        }
                    }
                    catch (Exception ex)
                    {
                        Label errorLabel = new Label("Error loading image: " + ex.Message, 280, 500, 504, 30, Font.Helvetica, 12, TextAlign.Left);
                        firstPage.Elements.Add(errorLabel);
                    }
                }
                else
                {
                    try
                    {
                        float scale = GetScale(imagePath);
                        Image image = new Image(imagePath, 280, 500, scale);
                        firstPage.Elements.Add(image);
                    }
                    catch (Exception ex)
                    {
                        Label errorLabel = new Label("Error loading image: " + ex.Message, 280, 500, 504, 30, Font.Helvetica, 12, TextAlign.Left);
                        firstPage.Elements.Add(errorLabel);
                    }
                }
            }
            else
            {
                Label errorLabel = new Label("Image file not found.", 0, 100, 504, 30, Font.Helvetica, 12, TextAlign.Left);
                firstPage.Elements.Add(errorLabel);
            }
            Page secondPage = new Page();
            document.Pages.Add(secondPage);

            //table
            Table2 table = new Table2(0,0, 700, 800);
            table.CellDefault.Border.Color = RgbColor.LightBlue;
            table.CellDefault.Border.LineStyle = LineStyle.Solid;
            table.CellDefault.Padding.Value = 3.0f;

            table.Columns.Add(100);
            table.Columns.Add(100);
            table.Columns.Add(100);
           

            Row2 tableHeader = table.Rows.Add(20, Font.HelveticaBold, 14, RgbColor.Black, RgbColor.Gray);
            tableHeader.CellDefault.Align = TextAlign.Center;
            tableHeader.CellDefault.VAlign = VAlign.Center;
            tableHeader.Cells.Add("List of tourists for tour " + tourDTO.Name).ColumnSpan = 5;

            Row2 headerContent = table.Rows.Add(Font.HelveticaBoldOblique, 12);
            headerContent.CellDefault.Align = TextAlign.Center;

            headerContent.Cells.Add("Name");
            headerContent.Cells.Add("Surname");
            headerContent.Cells.Add("Age");
          

            Row2 tableContent;

            foreach(TouristDTO tourist in tourists)
            {
                

                tableContent = table.Rows.Add(10);
                tableContent.Cells.Add(new FormattedTextArea(tourist.Name.ToString(), 0, 0, 140, 50, FontFamily.Helvetica, 12, false));
                tableContent.Cells.Add(new FormattedTextArea(tourist.Surname.ToString(), 0, 0, 140, 50, FontFamily.Helvetica, 12, false));
                tableContent.Cells.Add(new FormattedTextArea(tourist.Age.ToString(), 0, 0, 140, 50, FontFamily.Helvetica, 12, false));
            }

            secondPage.Elements.Add(table);

            if(isVoucherUsed)
            {
                String voucher = "IsVoucher used for this tour: Yes";
                Label labelVoucher = new Label(voucher, 310, 0, 550, 200, Font.Helvetica, 13, TextAlign.Left);
                secondPage.Elements.Add(labelVoucher);
            }
            else
            {
                String voucher = "IsVoucher used for this tour: No";
                Label labelVoucher = new Label(voucher, 310, 0, 550, 200, Font.Helvetica, 13, TextAlign.Left);
                secondPage.Elements.Add(labelVoucher);
            }
            string reportPath = "../../../../Reports/Tourist/Reservation-Report-for-" + tourDTO.Name.Replace(" ", "-") + "-"  + DateTime.Now.ToString("dd.MM.yyyy.HH.mm") + ".pdf";
            document.Draw(reportPath);
            Process.Start("cmd", "/c start " + reportPath);

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
            float desiredWidth = 250;
            float desiredHeight = 200;

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
