﻿using System;
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
            
            string username = Session["username"]?.ToString();

            //If string is null
            if (username == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                QuestPDF.Settings.License = LicenseType.Community;
                ClearTempFolder();
                GeneratePDF("preview");
            }
        }
        protected void ClearTempFolder()
        {
            string tempFolderPath = Server.MapPath("~/Resources/TempInvoicePDFs");

            if (!Directory.Exists(tempFolderPath))
            {
                Directory.CreateDirectory(tempFolderPath);
            }

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

        private void GeneratePDF(string request) // Generates a PDF for display and potential download
        {
            //Initialise variables
            string jobId = Session["Job_ID"]?.ToString();
            string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

            lblTitle.Text = "View Invoice " + jobId;
            
            string invoiceNumber = string.Empty;
            string jobDescription = string.Empty;
            decimal jobBudget = 0;
            string invoiceComments = string.Empty;

            string resourceName = string.Empty;
            decimal wage = 0;
            int hoursWorked = 0;

            string senderName = "Pietas Staffing Solutions and Training";
            string senderAddress = "Civic Centre, 12 Horz Street, Cape Town";
            string senderPhone = "0860 103 089";
            string senderEmail = "PSST@outlook.co.za";

            string clientName = string.Empty;
            string clientAddress = string.Empty;
            string clientPhone = string.Empty;
            string clientEmail = string.Empty;

            DateTime issueDate = DateTime.Now;
            DateTime dueDate = issueDate.AddDays(31);

            try // Try fetching all neccessary information for creating an invoice
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    //Fetch and set all information for invoice
                    connection.Open();

                    string invoiceQuery = @"
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

                    using (MySqlCommand command = new MySqlCommand(invoiceQuery, connection))
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

            if (expectedPay > jobBudget) // Add comment onto invoice if the pay is above the jobs budget
                invoiceComments += "NB: Expected pay exceeds job budget.";

            string fileName = $"invoice_{Guid.NewGuid()}.pdf";
            string filePath = Path.Combine(Server.MapPath("~/Resources/TempInvoicePDFs"), fileName);

            // Ensure the directory exists
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Create))  // Attempt to create the invoice PDF document with questPDF
            {
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(11));

                        page.Header().Column(column => // Set header content
                        {
                            column.Item().Text($"Invoice #{invoiceNumber}")
                                .SemiBold().FontSize(24).FontColor(Colors.Black);
                            column.Item().Text($"Issue Date: {issueDate:MMMM dd, yyyy}");
                            column.Item().Text($"Due Date: {dueDate:MMMM dd, yyyy}");
                        });

                        page.Content().PaddingVertical(20).Column(column => // Set content for invoice
                        {
                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Column(innerColumn =>
                                {
                                    innerColumn.Item().Text("From:").Bold();
                                    innerColumn.Item().Text(senderName + ",");
                                    innerColumn.Item().Text(senderAddress + ",");
                                    innerColumn.Item().Text(senderPhone + ",");
                                    innerColumn.Item().Text(senderEmail);
                                });

                                row.RelativeItem().Column(innerColumn =>
                                {
                                    innerColumn.Item().Text("To:").Bold();
                                    innerColumn.Item().Text(clientName +",");
                                    innerColumn.Item().Text(clientAddress + ",");
                                    innerColumn.Item().Text(clientPhone + ",");
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

            if (request == "preview") // Display the invoice PDF
            {
                pdfPreview.Src = $"/Resources/TempInvoicePDFs/{fileName}";
                pdfPreview.Attributes["style"] = "display:block;";
            }
            else if (request == "download") // Download the invoice PDF
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

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Jobs.aspx");
            return;
        }
    }
}
