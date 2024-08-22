<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvoiceForm.aspx.cs" Inherits="invoices_last_run.invoiceForm" %>
<%@ Register Src="~/resources/lib/head.ascx" TagPrefix="uc" TagName="Head" %>
<%@ Register Src="~/resources/lib/navigation.ascx" TagPrefix="uc" TagName="navigation" %>
<%@ Register Src="~/resources/lib/footer.ascx" TagPrefix="uc" TagName="footer" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<uc:head runat="server" id="Head" />
<body>
    <form id="form1" runat="server">
        <uc:navigation runat="server" id="navigation" />
            <div class="invoice-form">
                <h4>Preview Invoice</h4>
                <div class="form-group">
                </div>
                <div class="form-group">
                    <asp:Button ID="btnDownload" runat="server" Text="Download PDF" OnClick="btnDownload_Click" CssClass="btn" />
                </div>
            </div>
            <div class="preview-container">
               <iframe id="pdfPreview" runat="server" style="display:none;"></iframe>
            </div>
      <uc:footer runat="server" id="footer" />
    </form>
</body>
</html>
