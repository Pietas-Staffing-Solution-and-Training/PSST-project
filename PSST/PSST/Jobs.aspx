<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Jobs.aspx.cs" Inherits="PSST.Jobs" %>

<%@ Register Src="~/resources/lib/head.ascx" TagPrefix="uc" TagName="Head" %>
<%@ Register Src="~/resources/lib/navigation.ascx" TagPrefix="uc" TagName="navigation" %>
<%@ Register Src="~/resources/lib/footer.ascx" TagPrefix="uc" TagName="footer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<uc:Head runat="server" ID="Head" />
<body>
    <form id="resource" runat="server">
        <uc:navigation runat="server" ID="navigation" />
        <!-- Body -->


        <div class="main-div">
            <div class="resource-contain">
                <asp:Label ID="lblWelcome" runat="server" Text="Manage Jobs" Font-Size="XX-Large" ForeColor="#003479"></asp:Label>
                <div id="divError" class="error-label" runat="server">
                    <asp:Label ID="lblError" runat="server" Text="Error"></asp:Label>
                    <asp:ImageButton ID="btnExitErr" runat="server" ImageUrl="~/Resources/Icons/close - pixelperfect.png" AlternateText="Exit Error" CssClass="error-button" OnClick="btnExitErr_Click" />
                </div>

                <div class="content-container">
                    <div class="add-btn-div">
                        <asp:Button ID="btnAddJob" runat="server" Text="Add Job" CssClass="waves-effect waves-light btn" Style="left: 0px; top: 0px; height: 36px" PostBackUrl="~/Jobs.aspx#scrollTarget" />
                    </div>
                    <div class="search-div">
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="custom-textbox" BorderColor="#A6B7CA" ForeColor="Gray" ToolTip="Search" AutoPostBack="True" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnSearchClear" runat="server" Text="Clear" CssClass="btn" OnClick="btnSearchClear_Click" />
                    </div>
                </div>
                <div class="scrollable-gridview">
                    <asp:GridView ID="JobData" runat="server" DataKeyNames="Job_ID"
                        CellPadding="4" ForeColor="#333333" GridLines="None"
                        OnSelectedIndexChanged="JobData_SelectedIndexChanged"
                        OnRowDeleting="JobData_RowDeleting" OnRowEditing="JobData_RowEditing"
                        OnRowCancelingEdit="JobData_RowCancelingEdit" OnRowUpdating="JobData_RowUpdating"
                        Width="100%" AllowSorting="True" OnSorting="JobData_Sorting">
                        <Columns>

                            <asp:CommandField ShowSelectButton="True" ButtonType="Image" SelectImageUrl="~/Resources/Icons/invoice - thoseicons.png">
                                <ControlStyle Height="20px" />
                            </asp:CommandField>
                            <asp:CommandField ButtonType="Image" EditImageUrl="~/Resources/Icons/edit - pixelperfect.png" ShowEditButton="True" CancelImageUrl="~/Resources/Icons/cancel - gregorcresnar.png" UpdateImageUrl="~/Resources/Icons/confirm - roundicons.png">
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
                    <asp:Label ID="lblAdd" runat="server" Text="Add New Job" Font-Size="X-Large" ForeColor="#003479"></asp:Label>
                    <div class="add-container" id="scrollTarget">
                        <div class="add-row">
                            <asp:Label ID="lblID" runat="server" CssClass="add-label" Text="Job ID:"></asp:Label>
                            <asp:TextBox ID="txtJobID" runat="server" CssClass="custom-textbox" BorderColor="#A6B7CA" ForeColor="Gray" ToolTip="Job ID" ReadOnly="True"></asp:TextBox>
                            <asp:Label ID="lblDescription" runat="server" CssClass="add-label" Text="Description:"></asp:Label>
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="custom-textbox" BorderColor="#A6B7CA" ForeColor="Gray" ToolTip="Phone Number"></asp:TextBox>
                        </div>
                        <div class="add-row">
                            <asp:Label ID="lblJobID" runat="server" CssClass="add-label" Text="Job ID:"></asp:Label>
                            <asp:TextBox ID="txtResourceID" runat="server" CssClass="custom-textbox" BorderColor="#A6B7CA" ForeColor="Gray" ToolTip="First Name"></asp:TextBox>
                            <asp:Label ID="lblBudget" runat="server" CssClass="add-label" Text="Budget:"></asp:Label>
                            <asp:TextBox ID="txtBudget" runat="server" CssClass="custom-textbox" BorderColor="#A6B7CA" ForeColor="Gray" ToolTip="Wage"></asp:TextBox>
                        </div>
                        <div class="add-row">
                            <asp:Label ID="lblClientID" runat="server" CssClass="add-label" Text="Client ID:"></asp:Label>
                            <asp:TextBox ID="txtClientID" runat="server" CssClass="custom-textbox" BorderColor="#A6B7CA" ForeColor="Gray" ToolTip="Last Name"></asp:TextBox>                           
                        </div>
                    </div>
                    <asp:Button ID="btnAddDB" runat="server" Text="Add" CssClass="btn" OnClick="btnAddDB_Click" />
                </asp:Panel>
                <asp:Panel ID="userPanel" runat="server" CssClass="admin-only">
                    <hr class="add-divider" />
                    <asp:Label ID="lblTime" runat="server" Text="Add Time Worked" Font-Size="X-Large" ForeColor="#003479"></asp:Label>
                    <div class="edit-container" id="scrollTarget">
                        <div>
                            <asp:Label ID="lblTimeAdd" runat="server" CssClass="add-label" Text="Time:"></asp:Label>
                            <asp:TextBox ID="txtTime" runat="server" CssClass="custom-textbox" BorderColor="#A6B7CA" ForeColor="Gray" ToolTip="Time Worked"></asp:TextBox>                           
                        </div>
                    </div>
                    <asp:Button ID="btnAddTime" runat="server" Text="Add" CssClass="btn" OnClick="btnAddTime_Click" />
                </asp:Panel>

            </div>
        </div>

        <!-- Footer -->

        <uc:footer runat="server" ID="footer" />
    </form>
</body>
</html>
