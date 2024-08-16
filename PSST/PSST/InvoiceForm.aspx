<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvoiceForm.aspx.cs" Inherits="invoices_last_run.invoiceForm" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice Form</title>
    <style>
        .invoice-form {
            max-width: 500px;
            margin: 0 auto;
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            font-family: Arial, sans-serif;
        }

        .form-group {
            margin-bottom: 20px;
        }

        label {
            font-weight: bold;
            margin-bottom: 5px;
            display: block;
        }

        .form-control {
            width: 100%;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 3px;
            font-size: 16px;
        }

        .btn {
            display: inline-block;
            padding: 10px 20px;
            background-color: #007bff;
            color: #fff;
            border: none;
            border-radius: 3px;
            font-size: 16px;
            cursor: pointer;
            margin: 5px;
        }

        .btn:hover {
            background-color: #0056b3;
        }

        h1 {
            text-align: center;
            margin-bottom: 20px;
        }

        .preview-container {
            display: flex;
            justify-content: center;
            margin-top: 20px;
        }

        #pdfPreview {
            width: 50%; 
            height: 600px;
            border: 1px solid #ccc;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="invoice-form">
            <h1>Invoice Form</h1>
            <div class="form-group">
                <label for="ddlJobs">Select a Job:</label>
                <asp:DropDownList ID="ddlJobs" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Select a job" Value="" />
                    <asp:ListItem Text="Job 1" Value="1" />
                    <asp:ListItem Text="Job 2" Value="2" />
                    <asp:ListItem Text="Job 3" Value="3" />
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Button ID="btnPreview" runat="server" Text="Preview PDF" OnClick="btnPreview_Click" CssClass="btn" />
                <asp:Button ID="btnDownload" runat="server" Text="Download PDF" OnClick="btnDownload_Click" CssClass="btn" />
            </div>
        </div>
        <div class="preview-container">
           <iframe id="pdfPreview" runat="server" style="display:none;"></iframe>
        </div>
    </form>
</body>
</html>
