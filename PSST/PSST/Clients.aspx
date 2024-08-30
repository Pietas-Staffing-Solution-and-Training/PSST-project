<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Clients.aspx.cs" Inherits="PSST.Clients" %>

<%@ Register Src="~/resources/lib/head.ascx" TagPrefix="uc" TagName="Head" %>
<%@ Register Src="~/resources/lib/navigation.ascx" TagPrefix="uc" TagName="navigation" %>
<%@ Register Src="~/resources/lib/footer.ascx" TagPrefix="uc" TagName="footer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<uc:head runat="server" id="Head" />
<body>
    <form id="clients" runat="server">
        <uc:navigation runat="server" id="navigation" />
        <!-- Body -->
       <div class="main-div">
           <div class="resource-contain">
               <asp:Label ID="lblWelcome" runat="server" Text="Manage Clients" Font-Size="XX-Large" ForeColor="#003479"></asp:Label>
               <div ID="divError" class="error-label" runat="server">
                    <asp:Label ID="lblError" runat="server" Text="Error" ></asp:Label>
                    <asp:ImageButton ID="btnExitErr" runat="server" ImageUrl="~/Resources/Icons/close - pixelperfect.png" AlternateText="Exit Error" CssClass="error-button" OnClick="btnExitErr_Click"/> 
                </div>
               
               <div class="content-container">
                   <div class="add-btn-div">
                       <asp:Button ID="btnAddClient" runat="server" Text="Add Client" CssClass="waves-effect waves-light btn" style="left: 0px; top: 0px; height: 36px" PostBackUrl="~/Clients.aspx#scrollTarget" />
                   </div>
                   <div class="search-div">
                       <asp:TextBox ID="txtSearch" runat="server" CssClass="custom-textbox" BorderColor="#A6B7CA" ForeColor="Gray" ToolTip="Search" AutoPostBack="True" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
                       <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn" OnClick="btnSearch_Click" />
                       <asp:Button ID="btnSearchClear" runat="server" Text="Clear" CssClass="btn" OnClick="btnSearchClear_Click" />
                   </div>
               </div>
               <div class="scrollable-gridview">
                <asp:GridView ID="ClientsData" runat="server" DataKeyNames="Client_ID" 
                              CellPadding="4" ForeColor="#333333" GridLines="None" 
                              OnSelectedIndexChanged="ClientsData_SelectedIndexChanged"   
                              OnRowDeleting="ClientsData_RowDeleting" OnRowEditing="ClientsData_RowEditing" 
                              OnRowCancelingEdit="ClientsData_RowCancelingEdit" OnRowUpdating="ClientsData_RowUpdating" 
                              Width="100%" AllowSorting="True" OnSorting="ClientsData_Sorting">
                    <Columns>

                        <asp:CommandField ButtonType="Image" EditImageUrl="~/Resources/Icons/edit - pixelperfect.png" ShowEditButton="True" CancelImageUrl="~/Resources/Icons/cancel - gregorcresnar.png" UpdateImageUrl="~/Resources/Icons/confirm - roundicons.png" >
                            <ControlStyle Height="20px" />
                        </asp:CommandField>
                        <asp:TemplateField>
                            <ItemTemplate>
                            <!--throw confirmation before an item is deleted-->
                            <asp:ImageButton ID="DeleteButton" runat="server" ImageUrl="~/Resources/Icons/bin - freepik.png" 
                                                    CommandName="Delete" 
                                                    ToolTip="Delete Item" 
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
              <asp:Panel ID="adminPanel" runat="server" CssClass="admin-only">
                   <hr class="add-divider" />
                     <asp:Label ID="lblAdd" runat="server" Text="Add New Client" Font-Size="X-Large" ForeColor="#003479"></asp:Label>
                     <div class="add-container" id="scrollTarget">
                        <div class="add-row">
                            <asp:Label ID="lblID" runat="server" CssClass="add-label" Text="Client ID:" ></asp:Label>
                            <asp:TextBox ID="txtID" runat="server" CssClass="custom-textbox" BorderColor="#A6B7CA" ForeColor="Gray" ToolTip="Client ID" ReadOnly="True" ></asp:TextBox>
                            <asp:Label ID="lblPhoneNum" runat="server" CssClass="add-label" Text="Phone Number:" ></asp:Label>
                            <asp:TextBox ID="txtPhoneNum" runat="server" CssClass="custom-textbox" BorderColor="#A6B7CA" ForeColor="Gray" ToolTip="Phone Number" ></asp:TextBox>
                        </div>
                        <div class="add-row">
                            <asp:Label ID="lblFName" runat="server" CssClass="add-label" Text="First Name:" ></asp:Label>
                            <asp:TextBox ID="txtFName" runat="server" CssClass="custom-textbox" BorderColor="#A6B7CA" ForeColor="Gray" ToolTip="First Name" ></asp:TextBox>
                            <asp:Label ID="lblAddress" runat="server" CssClass="add-label" Text="Address:" ></asp:Label>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="custom-textbox" BorderColor="#A6B7CA" ForeColor="Gray" ToolTip="Wage" ></asp:TextBox>
                        </div>
                        <div class="add-row">
                            <asp:Label ID="lblLName" runat="server" CssClass="add-label" Text="Last Name:" ></asp:Label>
                            <asp:TextBox ID="txtLName" runat="server" CssClass="custom-textbox" BorderColor="#A6B7CA" ForeColor="Gray" ToolTip="Last Name" ></asp:TextBox>
                            <asp:Label ID="lblEmail" runat="server" CssClass="add-label" Text="Email:" ></asp:Label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="custom-textbox" BorderColor="#A6B7CA" ForeColor="Gray" ToolTip="Competencies" ></asp:TextBox>
                        </div>
                    </div>
                    <asp:Button ID="btnAddDB" runat="server" Text="Add" CssClass="btn" OnClick="btnAddDB_Click" />
              </asp:Panel>
                    
           </div>
        </div>
       
        <!-- Footer -->
           
        <uc:footer runat="server" id="footer" />
    </form>
</body>
</html>