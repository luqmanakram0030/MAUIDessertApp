using Desserts.Domain.Services.Interface;
//using DinkToPdf;
//using DinkToPdf.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Services.Implementation
{
    public class PrintPdfService
    {
        //private readonly IConverter _converter;

        //public PrintPdfService(IConverter converter)
        //{
        //    _converter = converter;
        //}

        //public byte[] ConvertToBytes(string htmlContent)
        //{
        //    var globalSettings = new GlobalSettings
        //    {
        //        PaperSize = PaperKind.A4,
        //        Orientation = Orientation.Portrait,
        //        Margins = new MarginSettings { Top = 10 },
        //    };

        //    var objectSettings = new ObjectSettings
        //    {
        //        PagesCount = true,
        //        HtmlContent = htmlContent,
        //        WebSettings = { DefaultEncoding = "utf-8" },
        //        HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
        //        FooterSettings = { FontSize = 9, Line = true, Center = "Footer" }
        //    };

        //    var pdf = new HtmlToPdfDocument()
        //    {
        //        GlobalSettings = globalSettings,
        //        Objects = { objectSettings }
        //    };

        //    return _converter.Convert(pdf);
        //}
    }
}
