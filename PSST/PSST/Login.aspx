<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PSST.Login" %>

<%@ Register Src="~/resources/lib/head.ascx" TagPrefix="uc" TagName="Head" %>
<%@ Register Src="~/resources/lib/navigation.ascx" TagPrefix="uc" TagName="navigation" %>
<%@ Register Src="~/resources/lib/footer.ascx" TagPrefix="uc" TagName="footer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<uc:Head runat="server" ID="Head" />
<uc:navigation runat="server" ID="navigation" />
<body>

    <form id="login" runat="server">
        <div class="container">

            <div class="row center">
                <div class="col l4 m6 s12">
                    <div class="z-depth-3">
                        <h4>Welcome to PSST login
                        </h4>
                    </div>

                    <div>
                        <div>
                            <h6>Please enter your username
                            </h6>
                        </div>
                        <div>
                            <asp:TextBox ID="tbUsername" runat="server" ToolTip="Please enter your email address" AutoCompleteType="Email"></asp:TextBox>
                            <asp:CustomValidator ID="valUsername" runat="server" ErrorMessage="Please enter a valid email address" ClientValidationFunction="ValidateInput_Client" OnServerValidate="valUsername_ServerValidate" ControlToValidate="tbUsername"></asp:CustomValidator>
                        </div>
                    </div>

                    <div>
                        <div>
                            <h6>Please enter your password
                            </h6>
                        </div>
                        <div>
                            <asp:TextBox ID="tbPassword" runat="server" ToolTip="Please enter your password"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Password incorrect" ClientValidationFunction="ValidateInput_Client" ControlToValidate="tbPassword" OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator>
                        </div>
                    </div>

                    <div>
                        <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
                    </div>


                </div>
            </div>

        </div>
    </form>

</body>

<uc:footer runat="server" ID="footer" />
</html>
