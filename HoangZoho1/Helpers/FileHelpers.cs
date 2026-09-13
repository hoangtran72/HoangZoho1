using System;
using System.IO;

namespace HoangZoho1.Helpers
{

    public class FileHelpers
    {

        public static void SaveBase64PdfToFile(string base64Pdf, string outputPath)
        {

            // Remove any data URI prefix if present
            if (base64Pdf.StartsWith("data:application/pdf;base64,"))
            {
                base64Pdf = base64Pdf.Substring("data:application/pdf;base64,".Length);
            }

            // Convert base64 string to bytes
            byte[] pdfBytes = Convert.FromBase64String(base64Pdf);

            // Write to file
            File.WriteAllBytes(outputPath, pdfBytes);

        }

    }

}
