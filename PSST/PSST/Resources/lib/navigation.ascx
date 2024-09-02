<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="navigation.ascx.cs" Inherits="PSST.Resources.lib.navigation" %>

<!-- Navigation -->
<div class="header">
    <asp:Panel ID="topPanel" runat="server" CssClass="top-panel">
        <nav class="white" role="navigation">
            <div class="nav-wrapper container">
              <a href="/Dashboard.aspx">
                <asp:Image ID="logo" runat="server" Height="100%" ImageUrl="~/Resources/logo_landscape.png" CssClass="logo" />
              </a>
              <ul class="right hide-on-med-and-down">
                <li><a id="nav_Dashboard" runat="server" href="/Dashboard.aspx">Dashboard</a></li>
                <li id="navclients" runat="server"><a href="/Clients.aspx">Clients</a></li>
                <li><a  id="nav_Jobs" runat="server" href="/Jobs.aspx">Jobs</a></li>
                <li><a  id="nav_Resource" runat="server" href="/Resource.aspx">Resources</a></li>
                <li><asp:LinkButton ID="logoutButton" runat="server" OnClick="Logout_Click">Logout</asp:LinkButton></li>
                <li id="helpbtn"><asp:ImageButton ID="helpButton" runat="server" OnClick="Help_Click" ImageUrl="~/Resources/Icons/question - freepik.png" Height="20px" /></li>
              </ul>
            </div>
        </nav>
    </asp:Panel>
</div>