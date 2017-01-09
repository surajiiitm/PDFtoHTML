using ERP.Erp.Interfaces;
using ERP.ExternalServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Content;
using PdfSharp.Pdf.Advanced;
using System.Diagnostics;
using System.Web.UI;
using Castle.Core.Logging;

namespace ERP.Erp
{
    class PdfService : IPdfService
    {
        #region Fields

        private IAzureService _azureService;
        private ILogger _logger;

        #endregion

        #region Constructor

        public PdfService(IAzureService azureService)
        {
            _azureService = azureService;
            _logger = NullLogger.Instance;
        }

        #endregion

        #region Methods

        public async Task<List<string>> GetPDFasHtml(Uri uri)
        {
            PdfDocument document = PdfReader.Open(await _azureService.DownloadToStream(uri));
            var pages = new List<string>();

            try
            {
                for (int i = 0; i < document.PageCount; i++)
                {
                    StringWriter stringWriter = new StringWriter();
                    var pageText = PdfTextExtractor.GetText(document.Pages[i]);
                    pageText = pageText.Replace("\r\n","<br/>");
                    pageText = pageText.Replace("  ","<p></p>");

                    using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
                    {
                        string classValue = "pdfHtml";
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, classValue);
                        writer.RenderBeginTag(HtmlTextWriterTag.Div); // Begin #1
                        writer.Write(pageText);
                        writer.RenderEndTag(); // End #1
                    }
                    pages.Add(stringWriter.ToString());
                }

                // Return the result.
                return pages;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return pages;
            }
        }

        #endregion
    }
}
