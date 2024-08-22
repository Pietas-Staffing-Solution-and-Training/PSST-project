<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="PSST.Dashboard" %>
<%@ Register Src="~/resources/lib/head.ascx" TagPrefix="uc" TagName="Head" %>
<%@ Register Src="~/resources/lib/navigation.ascx" TagPrefix="uc" TagName="navigation" %>
<%@ Register Src="~/resources/lib/footer.ascx" TagPrefix="uc" TagName="footer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<uc:head runat="server" id="Head" />
<body>
    <form id="form1" runat="server">
        <uc:navigation runat="server" id="navigation" />
        <!-- Body -->
        <div class="main-div">
            <div class="dash-contain">
                <asp:Image ID="imgLogo" runat="server" Height="200px" ImageUrl="~/Resources/logo.png" />
                <br />
                <asp:Label ID="lblWelcome" runat="server" Text="Welcome to PSST, user!" Font-Size="Large"></asp:Label>
                <br />
                <asp:Label ID="lblIntro" runat="server" Text="The following options are available to you:"></asp:Label>
                <br />
                <div class="options-div">
                    <asp:Button ID="btnClients" runat="server" Text="Clients" CssClass="waves-effect waves-light btn" style="left: 0px; top: 0px; height: 36px" PostBackUrl="~/Clients.aspx" />
                    <asp:Button ID="btnJobs" runat="server" Text="Jobs" CssClass="waves-effect waves-light btn" PostBackUrl="~/Jobs.aspx" />
                    <asp:Button ID="btnResources" runat="server" Text="Resources" CssClass="waves-effect waves-light btn" style="left: 0px; top: 0px" PostBackUrl="~/Resource.aspx" />
                </div>
                <br />
                <asp:Label ID="lblExplain" runat="server" Text="Jobs are open contracts offered by clients, who are companies seeking resources (skilled workers) to be contracted in for work."></asp:Label>
            </div>
        </div>
        <uc:footer runat="server" id="footer" />
    </form>
</body>
</html>
