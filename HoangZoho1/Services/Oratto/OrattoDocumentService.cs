using HoangZoho1.Models.Common;
using System;
using Syncfusion.OCRProcessor;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Graphics;
using System.IO;
using Syncfusion.Pdf.Exporting;
using System.Drawing;
using System.Collections.Generic;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Presentation;
using Syncfusion.PresentationRenderer;

namespace HoangZoho1.Services.Oratto
{
    public class OrattoDocumentService : IOrattoDocumentService
    {

        public ApiResultDto<List<string>> SplitPdfFile(string filePath)
        {

            var apiResult = new ApiResultDto<List<string>>()
            {
                Code = ResultCode.BadRequest,
            };

            try
            {

                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Please ensure that the PDF file.");
                    return apiResult;
                }

                var fileList = new List<string>();

                using (FileStream inputStream = new FileStream(filePath, FileMode.Open, FileAccess.Read)) 
                using (PdfLoadedDocument loadedDocument = new PdfLoadedDocument(inputStream))
                {

                    int totalPages = loadedDocument.PageCount;
                    int pagesPerFile = 10;
                    int fileIndex = 1;

                    for (int startPage = 0; startPage < totalPages; startPage += pagesPerFile)
                    {
                        // Create a new PDF document for each segment
                        using (PdfDocument newDocument = new PdfDocument())
                        {
                            // Calculate the number of pages to add to the current document
                            int pageCount = Math.Min(pagesPerFile, totalPages - startPage);

                            // Import the pages to the new document
                            for (int pageIndex = startPage; pageIndex < startPage + pageCount; pageIndex++)
                            {
                                newDocument.ImportPage(loadedDocument, pageIndex);
                            }

                            // Define the output file path

                            string fileName = Path.GetFileNameWithoutExtension(filePath);
                            string folder = Path.GetDirectoryName(filePath);

                            string outputFilePath = $"{folder}\\{fileName}_{fileIndex}.pdf";

                            // Save the new document
                            using (FileStream outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
                            {
                                newDocument.Save(outputStream);
                                fileList.Add(outputFilePath);
                            }

                            if (fileIndex == 3)
                            {
                                break;
                            }

                            fileIndex++;
                        }
                    }
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Data = fileList;
                return apiResult;

            }
            catch (Exception ex)
            {
                return apiResult;
            }
        
        }
        
        public ApiResultDto<string> ConvertDocxToPdf(string filePath)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
            };

            // Check if the DOCX file exists.
            if (!File.Exists(filePath))
            {
                return apiResult;
            }

            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string folder = Path.GetDirectoryName(filePath);

            // Open the DOCX file using a FileStream.
            using (FileStream inputStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // Load the Word document into the WordDocument object.
                using (WordDocument wordDocument = new WordDocument(inputStream, Syncfusion.DocIO.FormatType.Docx))
                {
                    // Create an instance of the DocIORenderer class.
                    using (DocIORenderer renderer = new DocIORenderer())
                    {
                        // Convert the Word document to a PDF document.
                        using (PdfDocument pdfDocument = renderer.ConvertToPDF(wordDocument))
                        {
                            // Save the PDF document to a file.
                            string pdfFilePath = $"{folder}/{fileName}.pdf";

                            using (FileStream outputStream = new FileStream(pdfFilePath, FileMode.Create, FileAccess.Write))
                            {
                                pdfDocument.Save(outputStream);
                            }

                            apiResult.Code = ResultCode.OK;
                            apiResult.Data = pdfFilePath;

                            return apiResult;
                        }
                    }
                }
            }
        }

        public ApiResultDto<string> ConvertPptxToPdf(string filePath)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
            };

            // Check if the DOCX file exists.
            if (!File.Exists(filePath))
            {
                return apiResult;
            }

            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string folder = Path.GetDirectoryName(filePath);

            // Open the DOCX file using a FileStream.
            using (FileStream inputStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // Load the Word document into the WordDocument object.
                using (IPresentation presentation = Presentation.Open(inputStream))
                {
                    // Initialize the PresentationRenderer.
                    presentation.PresentationRenderer = new PresentationRenderer();

                    // Convert the PowerPoint presentation to a PDF document.
                    using (PdfDocument pdfDocument = PresentationToPdfConverter.Convert(presentation))
                    {
                        // Save the PDF document to a file.
                        string pdfFilePath = $"{folder}/{fileName}.pdf";
                        using (FileStream outputStream = new FileStream(pdfFilePath, FileMode.Create, FileAccess.Write))
                        {
                            pdfDocument.Save(outputStream);
                        }

                        apiResult.Code = ResultCode.OK;
                            apiResult.Data = pdfFilePath;

                            return apiResult;
                    }
                }
            }
        }

    }
}
