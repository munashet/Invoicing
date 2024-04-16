using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Invoicing {
    public class Quotation: Document {

        const string MAIN_COLOR = "#33CCCC";
        const string FONT_FAMILY = "Calibri";
        const int FONT_SIZE = 12;
        public int ValidFor { get; set; }

        public Quotation(Order order, double vat)
            : base(order, vat) {
        }

        public override string GetTitle() {
            return "Quotation :: " + this.Order.Date.ToString("dd MMM, yyyy");
        }

        public override FlowDocument GetDocument() {
            var flowDocument = new FlowDocument();
            Table table = new Table();
            table.RowGroups.Add(new TableRowGroup());
            flowDocument.Blocks.Add(table);
            table.CellSpacing = 0;
            table.Background = Brushes.White;
            for (int x = 0; x < 4; x++)
                table.Columns.Add(new TableColumn());
            table.RowGroups[0].Rows.Add(new TableRow());

            TableRow currentRow = table.RowGroups[0].Rows[0];
            TableCell cell = new TableCell();
            cell.ColumnSpan = 4;
            currentRow.Cells.Add(cell);

            Table table1 = new Table();
            table1.CellSpacing = 0;
            table1.RowGroups.Add(new TableRowGroup());
            cell.Blocks.Add(table1);
            table1.RowGroups[0].Rows.Add(new TableRow());
            TableCell cell1 = new TableCell();
            TableCell cell2 = new TableCell();
            table1.RowGroups[0].Rows[0].Cells.Add(cell1);
            table1.RowGroups[0].Rows[0].Cells.Add(cell2);

            Run r = new Run(Order.Customer.Company.CompanyName);
            r.FontFamily = new FontFamily(FONT_FAMILY);
            // r.FontStyle = FontStyles.Italic;
            r.FontSize = 24;
            r.FontWeight = System.Windows.FontWeights.Bold;
            BrushConverter bc = new BrushConverter();
            Brush brush = (Brush)bc.ConvertFrom(MAIN_COLOR);
            brush.Freeze();
            r.Foreground = brush;
            r.BaselineAlignment = BaselineAlignment.Top;
            Paragraph p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Left;
            cell1.Blocks.Add(p);

            r = new Run(Order.Customer.Company.PhysicalAddress);
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontSize = FONT_SIZE;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Left;
            cell1.Blocks.Add(p);
            r = new Run("Phone: " + Order.Customer.Company.Phone);
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontSize = FONT_SIZE;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Left;
            cell1.Blocks.Add(p);
            r = new Run("Cell: " + Order.Customer.Company.Cell);
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontSize = FONT_SIZE;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Left;
            cell1.Blocks.Add(p);
            r = new Run("Email: " + Order.Customer.Company.Email);
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontSize = FONT_SIZE;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Left;
            cell1.Blocks.Add(p);

            r = new Run("Quotation");
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontStyle = FontStyles.Italic;
            r.FontSize = 24;
            bc = new BrushConverter();
            brush = (Brush)bc.ConvertFrom(MAIN_COLOR);
            brush.Freeze();
            r.Foreground = brush;
            r.BaselineAlignment = BaselineAlignment.Top;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Right;
            cell2.Blocks.Add(p);
            r = new Run("Date: " + this.Order.Date.ToString("dd MMM, yyyy"));
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontSize = FONT_SIZE;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Right;
            cell2.Blocks.Add(p);
            r = new Run("Quote: " + DocumentNumberString);
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontSize = FONT_SIZE;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Right;
            cell2.Blocks.Add(p);

            table.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table.RowGroups[0].Rows[1];
            cell = new TableCell();
            cell.ColumnSpan = 4;
            currentRow.Cells.Add(cell);
            r = new Run("Bill to: ");
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontSize = FONT_SIZE;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Left;
            cell.Blocks.Add(p);
            r = new Run(Order.Customer.Name);
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontSize = FONT_SIZE;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Left;
            cell.Blocks.Add(p);
            r = new Run(Order.Customer.PhysicalAddress);
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontSize = FONT_SIZE;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Left;
            cell.Blocks.Add(p);

            table.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table.RowGroups[0].Rows[2];
            cell = new TableCell();
            cell.ColumnSpan = 4;
            currentRow.Cells.Add(cell);
            table1 = new Table();
            table1.CellSpacing = 1;
            table1.RowGroups.Add(new TableRowGroup());
            cell.Blocks.Add(table1);
            table1.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table1.RowGroups[0].Rows[0];
            r = new Run("Description");
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontSize = FONT_SIZE;
            bc = new BrushConverter();
            brush = (Brush)bc.ConvertFrom(MAIN_COLOR);
            brush.Freeze();
            r.BaselineAlignment = BaselineAlignment.Top;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Center;
            cell1 = new TableCell(p);
            cell1.Background = brush;
            currentRow.Cells.Add(cell1);
            r = new Run("Quantity");
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontSize = FONT_SIZE;
            bc = new BrushConverter();
            brush = (Brush)bc.ConvertFrom(MAIN_COLOR);
            brush.Freeze();
            r.BaselineAlignment = BaselineAlignment.Top;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Center;
            cell1 = new TableCell(p);
            cell1.Background = brush;
            currentRow.Cells.Add(cell1);
            r = new Run("Unit Price");
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontSize = FONT_SIZE;
            bc = new BrushConverter();
            brush = (Brush)bc.ConvertFrom(MAIN_COLOR);
            brush.Freeze();
            r.BaselineAlignment = BaselineAlignment.Top;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Center;
            cell1 = new TableCell(p);
            cell1.Background = brush;
            currentRow.Cells.Add(cell1);
            r = new Run("Line Total");
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontSize = FONT_SIZE;
            bc = new BrushConverter();
            brush = (Brush)bc.ConvertFrom(MAIN_COLOR);
            brush.Freeze();
            r.BaselineAlignment = BaselineAlignment.Top;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Center;
            cell1 = new TableCell(p);
            cell1.Background = brush;
            currentRow.Cells.Add(cell1);
            int count = 1;
            double total = 0;
            foreach (Item item in Order.Items) {
                table1.RowGroups[0].Rows.Add(new TableRow());
                currentRow = table1.RowGroups[0].Rows[count];
                r = new Run(item.Description);
                r.FontFamily = new FontFamily(FONT_FAMILY);
                r.FontSize = FONT_SIZE;
                p = new Paragraph(r);
                p.TextAlignment = TextAlignment.Left;
                cell1 = new TableCell(p);
                currentRow.Cells.Add(cell1);

                r = new Run(item.Quantity.ToString());
                r.FontFamily = new FontFamily(FONT_FAMILY);
                r.FontSize = FONT_SIZE;
                p = new Paragraph(r);
                p.TextAlignment = TextAlignment.Center;
                cell1 = new TableCell(p);
                currentRow.Cells.Add(cell1);

                r = new Run(item.UnitPrice.ToString());
                r.FontFamily = new FontFamily(FONT_FAMILY);
                r.FontSize = FONT_SIZE;
                p = new Paragraph(r);
                p.TextAlignment = TextAlignment.Center;
                cell1 = new TableCell(p);
                currentRow.Cells.Add(cell1);

                double lineTotal = item.UnitPrice * item.Quantity;
                total += lineTotal;
                r = new Run(lineTotal.ToString("0.00"));
                r.FontFamily = new FontFamily(FONT_FAMILY);
                r.FontSize = FONT_SIZE;
                p = new Paragraph(r);
                p.TextAlignment = TextAlignment.Center;
                cell1 = new TableCell(p);
                currentRow.Cells.Add(cell1);
                ++count;
            }

            table.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table.RowGroups[0].Rows[3];
            cell = new TableCell();
            cell.ColumnSpan = 2;
            currentRow.Cells.Add(cell);
            r = new Run("Sub-Total");
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontSize = FONT_SIZE;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Right;
            cell = new TableCell(p);
            currentRow.Cells.Add(cell);
            r = new Run(total.ToString("0.00"));
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontSize = FONT_SIZE;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Center;
            cell = new TableCell(p);
            currentRow.Cells.Add(cell);

            double vat = (this.VAT / 100.0) * total;
            table.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table.RowGroups[0].Rows[4];
            cell = new TableCell();
            cell.ColumnSpan = 2;
            currentRow.Cells.Add(cell);
            r = new Run("VAT");
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontSize = FONT_SIZE;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Right;
            cell = new TableCell(p);
            currentRow.Cells.Add(cell);
            r = new Run(vat.ToString("0.00"));
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontSize = FONT_SIZE;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Center;
            cell = new TableCell(p);
            currentRow.Cells.Add(cell);

            total += vat;
            table.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table.RowGroups[0].Rows[5];
            cell = new TableCell();
            cell.ColumnSpan = 2;
            currentRow.Cells.Add(cell);
            r = new Run("Total");
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontSize = FONT_SIZE;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Right;
            cell = new TableCell(p);
            currentRow.Cells.Add(cell);
            r = new Run(total.ToString("0.00"));
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontSize = FONT_SIZE;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Center;
            cell = new TableCell(p);
            currentRow.Cells.Add(cell);

            r = new Run(this.Notes);
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontSize = FONT_SIZE;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Left;
            cell = new TableCell(p);
            table.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table.RowGroups[0].Rows[6];
            currentRow.Cells.Add(cell);

            r = new Run("Valid for: " + this.ValidFor + " days");
            r.FontFamily = new FontFamily(FONT_FAMILY);
            r.FontSize = FONT_SIZE;
            p = new Paragraph(r);
            p.TextAlignment = TextAlignment.Left;
            cell = new TableCell(p);
            table.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table.RowGroups[0].Rows[7];
            currentRow.Cells.Add(cell);

            return flowDocument;
        }

        public override string GetPrefix() {
            return "QTE";
        }
    }
}
