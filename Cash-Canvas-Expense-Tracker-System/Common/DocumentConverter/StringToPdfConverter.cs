using CashCanvas.Common.ConstantHandler;
using SelectPdf;

namespace CashCanvas.Common.DocumentConverter;

public class StringToPdfConverter
{
    public static byte[] CreatePdf(string partialView)
    {
        try
        {
            // Convert the HTML string to a PDF
            HtmlToPdf converter = new();
            PdfDocument pdf = converter.ConvertHtmlString(partialView);

            using MemoryStream stream = new();
            pdf.Save(stream);
            pdf.Close();

            return stream.ToArray();
        }
        catch
        {
            throw new Exception(Constants.EXPORT_FILE_GENERATION_ERROR);
        }
    }
}
