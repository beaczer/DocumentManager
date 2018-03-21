using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastChance.Models.PdfSharp
{
    public class LayoutHelper
    {

        private readonly PdfDocument _document;
        private readonly XUnit _topPosition;
        private readonly XUnit _bottomMargin;
        private XUnit _currentPosition;
        XFont font = new XFont("Verdana", 20, XFontStyle.Bold);

        public LayoutHelper(PdfDocument document, XUnit topPosition, XUnit bottomMargin)
        {
            _document = document;
            _topPosition = topPosition;
            _bottomMargin = bottomMargin;
            watermark = "DocumentManager";
            // Set a value outside the page - a new page will be created on the first request.
            _currentPosition = bottomMargin + 10000;
        }

        public XUnit GetLinePosition(XUnit requestedHeight)
        {
            return GetLinePosition(requestedHeight, -1f);
        }

        public XUnit GetLinePosition(XUnit requestedHeight, XUnit requiredHeight)
        {
            XUnit required = requiredHeight == -1f ? requestedHeight : requiredHeight;
            if (_currentPosition + required > _bottomMargin)
            {
                CreatePage();
                Page = CreateWaterMark(Page, watermark);
            }
            XUnit result = _currentPosition;
            _currentPosition += requestedHeight;
            return result;
        }

        public XGraphics Gfx { get; private set; }
        public PdfPage Page { get; private set; }

        void CreatePage()
        {
            Page = _document.AddPage();
            Page.Size = PageSize.A4;
            Gfx = XGraphics.FromPdfPage(Page);
            _currentPosition = _topPosition;
        }
        public string watermark { get; set; }
        public void SetWatermark(string watermark)
        {
            this.watermark = watermark;
        }
            
            PdfPage CreateWaterMark(PdfPage page, string watermark)
        {
            // Variation 1: Draw watermark as text string
            Gfx.Dispose();
            // Get an XGraphics object for drawing beneath the existing content
            XGraphics gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);
          
            // Get the size (in point) of the text
            XSize size = gfx.MeasureString(watermark, font);

            // Define a rotation transformation at the center of the page
            gfx.TranslateTransform(10, -400);

            // Create a string format
            XStringFormat format = new XStringFormat();
            format.Alignment = XStringAlignment.Near;
            format.LineAlignment = XLineAlignment.Near;

            // Create a dimmed red brush
            XBrush brush = new XSolidBrush(XColor.FromArgb(128, 255, 0, 0));

            // Draw the string
            gfx.DrawString(watermark, font, brush,
              new XPoint((page.Width - size.Width) / 2, (page.Height - size.Height) / 2),
              format);
            gfx.Dispose();
            Gfx = XGraphics.FromPdfPage(Page);
            
            return page;
        }



    }
}
