<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Resource.aspx.cs" Inherits="PSST.Resource" %>

<%@ Register Src="~/resources/lib/head.ascx" TagPrefix="uc" TagName="Head" %>
<%@ Register Src="~/resources/lib/navigation.ascx" TagPrefix="uc" TagName="navigation" %>
<%@ Register Src="~/resources/lib/footer.ascx" TagPrefix="uc" TagName="footer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<uc:head runat="server" id="Head" />
<body>
    <form id="resource" runat="server">
        <uc:navigation runat="server" id="navigation" />
        <!-- Body -->
       <div class="main-div">
            <asp:Label ID="lblWelcome" runat="server" Text="Welcome to PSST, user!" Font-Size="Large"></asp:Label>
            <asp:GridView ID="ResourceData" runat="server" DataKeyNames="Resource ID" CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="ResourceData_SelectedIndexChanged"   OnRowDeleting="ResourceData_RowDeleting" OnRowEditing="ResourceData_RowEditing" OnRowCancelingEdit="ResourceData_RowCancelingEdit" OnRowUpdating="ResourceData_RowUpdating">
                <Columns>
                   
                    <asp:CommandField ShowSelectButton="True" ButtonType="Image" SelectImageUrl="~/Resources/Icons/check - reussy.png" >
                    <ControlStyle Height="20px" />
                    </asp:CommandField>
                    <asp:CommandField ButtonType="Image" EditImageUrl="~/Resources/Icons/edit - pixelperfect.png" ShowEditButton="True" CancelImageUrl="~/Resources/Icons/cancel - gregorcresnar.png" UpdateImageUrl="~/Resources/Icons/confirm - roundicons.png" >
                    <ControlStyle Height="20px" />
                    </asp:CommandField>
                    <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Resources/Icons/bin - freepik.png" ShowDeleteButton="True">
                    <ControlStyle Height="20px" />
                    </asp:CommandField>
                </Columns>
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
        </div>
         <!--OnRowCommand="ResourceData_RowCommand" OnRowDataBound="ResourceData_RowDataBound"-->
        <!-- Footer -->
        <uc:footer runat="server" id="footer" />
    </form>
</body>
</html>