using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace gagFIS_Interfase
{
    public class PageEventHelper : PdfPageEventHelper
    {
        PdfContentByte cb;
        PdfTemplate template;


        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            cb = writer.DirectContent;
            template = cb.CreateTemplate(50, 50);
        }

        public override void OnEndPage(PdfWriter writer, Document doc)
        {

            BaseColor grey = new BaseColor(128, 128, 128);
            iTextSharp.text.Font font = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, grey);
            //tbl footer
            PdfPTable footerTbl = new PdfPTable(1);
            footerTbl.TotalWidth = doc.PageSize.Width;
            Phrase titulo = new Phrase();           
           
            //doc.Add(Ctte.imagenMINTELL);
            //doc.Add(Ctte.imagenDPEC);

            //Chunk chunkLeyenda = new Chunk(Vble.leyenda + " - Periodo:" + Vble.Periodo + " - Ruta: " + Vble.rutas,
            //                         FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD,
            //                         new iTextSharp.text.BaseColor(0, 102, 0)));
           
            Ctte.chunkLeyenda = new Chunk("      Ruta: " + Vble.rutas + "\n",
                                     FontFactory.GetFont("Arial", 20, iTextSharp.text.Font.BOLD,
                                     new iTextSharp.text.BaseColor(0, 102, 0)));
            titulo.Add(Ctte.chunkLeyenda);
           
            
            //doc.Add(new Paragraph("  "));
            //numero de la page
            Chunk myFooter = new Chunk("Página " + (doc.PageNumber), FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 8, grey));
            PdfPCell footer = new PdfPCell(new Phrase(myFooter));
            footer.Border = iTextSharp.text.Rectangle.NO_BORDER;
            footer.HorizontalAlignment = Element.ALIGN_CENTER;
            footerTbl.AddCell(footer);
            ///Esta linea ubica la numeración de pagina en donde se indique segun margenes que se envian como parametro
            footerTbl.WriteSelectedRows(0, -1, 0, (doc.BottomMargin + 4), writer.DirectContent);

            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, titulo, doc.PageSize.Width / 2,
                                      doc.PageSize.Height - 30, 0);



        }




        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

        }
    }
}
