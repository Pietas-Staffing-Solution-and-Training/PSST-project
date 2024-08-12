<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="PSST.Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="styles.css"/>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Header -->
        <div class="header">
            <asp:Panel ID="topPanel" runat="server" CssClass="top-panel">
                <nav class="white" role="navigation">
                    <div class="nav-wrapper container">
                      <a href="/Dashboard.aspx">
                        <asp:Image ID="logo" runat="server" Height="100%" ImageUrl="~/Resources/logo_landscape.png" CssClass="logo" />
                      </a>
                      <ul class="right hide-on-med-and-down">
                        <li><a href="/Dashboard.aspx">Dashboard</a></li>
                        <li><a href="/Clients.aspx">Clients</a></li>
                        <li><a href="/Jobs.aspx">Jobs</a></li>
                        <li><a href="/Resources.aspx">Resources</a></li>
                        <li><a href="#">Logout</a></li>
                      </ul>
                    </div>
                </nav>
            </asp:Panel>
        </div>
        <!-- Body -->
        <div class="main-div">
            <asp:Label ID="lblWelcome" runat="server" Text="Welcome to PSST, user!" Font-Size="Large"></asp:Label>
        </div>
        <!-- Footer -->
        <footer class="page-footer teal">
            <div class="container">
              <div class="row">
                <div class="col l3 s12" style="height: 150px;">
                  <a href="/Dashboard.aspx">
                    <asp:Image ID="Image1" runat="server" Height="100%" ImageUrl="~/Resources/logo_landscape.png" CssClass="logo" />
                  </a>
                </div>
                <div class="col l6 s12">
                  <h5 class="white-text">About PSST</h5>
                  <p class="grey-text text-lighten-4">Pieta’s Staffing Solutions and Training (PSST) is a human resources company in South Africa with the functions of recruitment, staffing, training, and other human resources functions. We provide only the best service for our clients, both jobseekers and employers. For queries and concerns, please contact support@psst.com.</p>
                </div>
                <div class="col l3 s12">
                  <h5 class="white-text">Navigation</h5>
                  <ul>
                    <li><a class="white-text" href="/Clients.aspx">Clients</a></li>
                    <li><a class="white-text" href="/Jobs.aspx">Jobs</a></li>
                    <li><a class="white-text" href="/Resources.aspx">Resources</a></li>
                    <li><a class="white-text" href="#">Help/Documentation</a></li>
                  </ul>
                </div>
              </div>
            </div>
            <div class="footer-copyright">
              <div class="container">
                <p>Made by Group 1 - CMPG223</p>
              </div>
            </div>
          </footer>
    </form>
</body>
</html>
