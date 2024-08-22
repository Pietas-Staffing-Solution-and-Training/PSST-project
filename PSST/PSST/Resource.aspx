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
           <div class="resource-contain">
               <asp:Label ID="lblWelcome" runat="server" Text="Manage Resources" Font-Size="X-Large" Font-Bold="True" ForeColor="#003479"></asp:Label>
               <div ID="divError" class="error-label" runat="server">
                    <asp:Label ID="lblError" runat="server" Text="Error" ></asp:Label>
                    <asp:ImageButton ID="btnExitErr" runat="server" ImageUrl="~/Resources/Icons/close - pixelperfect.png" AlternateText="Exit Error" CssClass="error-button" OnClick="btnExitErr_Click"/> 
                </div>
               <div class="content-container">
                   <div class="add-btn-div">
                       <asp:Button ID="btnAddResource" runat="server" Text="Add Resource" CssClass="waves-effect waves-light btn" style="left: 0px; top: 0px; height: 36px" PostBackUrl="~/Resource.aspx" />
                   </div>
                   <div class="search-div">
                       <p style="color: #003479; font-weight: bold">Search:</p>
                       <asp:TextBox ID="txtSearch" runat="server" CssClass="search-box" BorderColor="#A6B7CA" ForeColor="Gray" ToolTip="Search" AutoPostBack="True" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
                   </div>
               </div>
               <div class="scrollable-gridview">
                <asp:GridView ID="ResourceData" runat="server" DataKeyNames="Resource_ID" 
                              CellPadding="4" ForeColor="#333333" GridLines="None" 
                              OnSelectedIndexChanged="ResourceData_SelectedIndexChanged"   
                              OnRowDeleting="ResourceData_RowDeleting" OnRowEditing="ResourceData_RowEditing" 
                              OnRowCancelingEdit="ResourceData_RowCancelingEdit" OnRowUpdating="ResourceData_RowUpdating" 
                              Width="100%">
                    <Columns>
                   
                        <asp:CommandField ShowSelectButton="True" ButtonType="Image" SelectImageUrl="~/Resources/Icons/invoice - thoseicons.png" >
                        <ControlStyle Height="20px" />
                        </asp:CommandField>
                        <asp:CommandField ButtonType="Image" EditImageUrl="~/Resources/Icons/edit - pixelperfect.png" ShowEditButton="True" CancelImageUrl="~/Resources/Icons/cancel - gregorcresnar.png" UpdateImageUrl="~/Resources/Icons/confirm - roundicons.png" >
                        <ControlStyle Height="20px" />
                        </asp:CommandField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <!--throw confirmation before an item is deleted-->
                                <asp:ImageButton ID="DeleteButton" runat="server" ImageUrl="~/Resources/Icons/bin - freepik.png" 
                                                 CommandName="Delete" 
                                                 OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                            </ItemTemplate>
                          <ControlStyle Height="20px" />
                        </asp:TemplateField>
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
                    
           </div>
        </div>
       
        <!-- Footer -->
           
        <uc:footer runat="server" id="footer" />
    </form>
</body>
</html>