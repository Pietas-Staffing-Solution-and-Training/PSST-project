<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="footer.ascx.cs" Inherits="PSST.Resources.lib.footer" %>

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