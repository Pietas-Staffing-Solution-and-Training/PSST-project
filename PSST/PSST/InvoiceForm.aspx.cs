using System;
using System.IO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace invoices_last_run
{
    public partial class invoiceForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Ensure user is logged in
            //Get session value - returns null if doesn't exist
            /*
            string username = Session["username"]?.ToString();

            //If string is null
            if (username == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            /*/
            QuestPDF.Settings.License = LicenseType.Community;

            if (!IsPostBack)
            {
                ClearTempFolder();
                GeneratePDF("preview");
            }
        }
        protected void ClearTempFolder()
        {
            string tempFolderPath = Server.MapPath("~/Resources/TempInvoicePDFs");

            if (Directory.Exists(tempFolderPath))
            {
                try
                {
                    var files = Directory.GetFiles(tempFolderPath);

                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error clearing temp folder: {ex.Message}");
                }
            }
        }

        private void GeneratePDF(string action)
        {
            //Initialise variables
            int jobId = 1; // Example Job_ID
            string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString; 

            string invoiceNumber = string.Empty;
            string jobDescription = string.Empty;
            decimal jobBudget = 0;
            string invoiceComments = string.Empty;

            string resourceName = string.Empty;
            decimal wage = 0;
            int hoursWorked = 0;

            string senderName = "Pietas Staffing Solutions and Training";//string.Empty;
            string senderAddress = "Civic Centre, 12 Horz Street, Cape Town";//string.Empty;
            string senderPhone = "0860 103 089";//string.Empty;
            string senderEmail = "PSST@outlook.co.za";//string.Empty;

            string clientName = string.Empty;
            string clientAddress = string.Empty;
            string clientPhone = string.Empty;
            string clientEmail = string.Empty;

            DateTime issueDate = DateTime.Now;
            DateTime dueDate = issueDate.AddDays(31);

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    //Fetch and set all information for invoice
                    connection.Open();

                    string query = @"
                        SELECT 
                            J.Description,
                            J.Budget,
                            CONCAT(R.FName, ' ', R.LName) AS ResourceName,
                            R.Wage,
                            I.Hours_Worked,
                            I.Invoice_Num,
                            CONCAT(C.FName, ' ', C.LName) AS ClientName,
                            C.Address,
                            C.Phone_Num,
                            C.Email
                        FROM JOB J
                        JOIN RESOURCE R ON J.Resource_ID = R.Resource_ID
                        JOIN CLIENT C ON J.Client_ID = C.Client_ID
                        JOIN INVOICE I ON J.Job_ID = I.Job_ID
                        WHERE J.Job_ID = @JobID";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@JobID", jobId);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                invoiceNumber = reader["Invoice_Num"].ToString();
                                jobDescription = reader["Description"].ToString();
                                jobBudget = Convert.ToDecimal(reader["Budget"]);

                                resourceName = reader["ResourceName"].ToString();
                                wage = Convert.ToDecimal(reader["Wage"]);
                                hoursWorked = Convert.ToInt32(reader["Hours_Worked"]);

                                clientName = reader["ClientName"].ToString();
                                clientAddress = reader["Address"].ToString();
                                clientPhone = reader["Phone_Num"].ToString();
                                clientEmail = reader["Email"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex);}

            decimal expectedPay = wage * hoursWorked;

            if (expectedPay > jobBudget)
                invoiceComments += "NB: Expected pay exceeds job budget.";

            string fileName = $"invoice_{Guid.NewGuid()}.pdf";
            string filePath = Server.MapPath($"~/Resources/TempInvoicePDFs/{fileName}");

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(11));

                        page.Header().Column(column =>
                        {
                            column.Item().Text($"Invoice #{invoiceNumber}")
                                .SemiBold().FontSize(24).FontColor(Colors.Black);
                            column.Item().Text($"Issue Date: {issueDate:MMMM dd, yyyy}");
                            column.Item().Text($"Due Date: {dueDate:MMMM dd, yyyy}");
                        });

                        page.Content().PaddingVertical(20).Column(column =>
                        {
                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Column(innerColumn =>
                                {
                                    innerColumn.Item().Text("From:").Bold();
                                    innerColumn.Item().Text(senderName);
                                    innerColumn.Item().Text(senderAddress);
                                    innerColumn.Item().Text(senderPhone);
                                    innerColumn.Item().Text(senderEmail);
                                });

                                row.RelativeItem().Column(innerColumn =>
                                {
                                    innerColumn.Item().Text("To:").Bold();
                                    innerColumn.Item().Text(clientName);
                                    innerColumn.Item().Text(clientAddress);
                                    innerColumn.Item().Text(clientPhone);
                                    innerColumn.Item().Text(clientEmail);
                                });
                            });

                            column.Item().PaddingVertical(20);

                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(x => x.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2))
                                        .AlignCenter()
                                        .Text("Resource");
                                    header.Cell().Element(x => x.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2))
                                        .AlignCenter()
                                        .Text("Hourly Rate");
                                    header.Cell().Element(x => x.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2))
                                        .AlignCenter()
                                        .Text("Hours Worked");
                                    header.Cell().Element(x => x.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2))
                                        .AlignCenter()
                                        .Text("Total Pay");
                                });

                                table.Cell().Element(x => x.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2))
                                    .AlignLeft()
                                    .Text(resourceName);
                                table.Cell().Element(x => x.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2))
                                    .AlignRight()
                                    .Text($"{wage:C}");
                                table.Cell().Element(x => x.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2))
                                    .AlignCenter()
                                    .Text(hoursWorked.ToString());
                                table.Cell().Element(x => x.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2))
                                    .AlignRight()
                                    .Text($"{expectedPay:C}");

                                column.Item().PaddingVertical(150);
                                column.Item().Background(Colors.Grey.Lighten3).Padding(10).Column(commentColumn =>
                                {
                                    commentColumn.Item().Text("Comments").Bold();
                                    commentColumn.Item().PaddingTop(5).Text(invoiceComments);
                                });
                            });
                        });

                        page.Footer().AlignCenter().Text(x =>
                        {
                            x.Span("---END OF REPORT---");
                        });
                    });
                })
                .GeneratePdf(fs);
            }

            if (action == "preview")
            {
                pdfPreview.Src = $"/Resources/TempInvoicePDFs/{fileName}";
                pdfPreview.Attributes["style"] = "display:block;";
            }
            else if (action == "download")
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", $"attachment; filename=invoice.pdf");
                Response.TransmitFile(filePath);
                Response.End();
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            GeneratePDF("download");
        }
    }
}
