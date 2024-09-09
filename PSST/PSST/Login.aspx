<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PSST.Login" %>

<%@ Register Src="~/resources/lib/head.ascx" TagPrefix="uc" TagName="head" %>
<%@ Register Src="~/resources/lib/navigation.ascx" TagPrefix="uc" TagName="navigation" %>
<%@ Register Src="~/resources/lib/footer.ascx" TagPrefix="uc" TagName="footer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<uc:head runat="server" ID="head" />
<body>

    <form id="login" runat="server" DefaultButton="btnLogin">
        <uc:navigation runat="server" ID="navigation" />

        <div class="loginBackground">
            <div class="container login-contain">

                <div class="row">
                    <div class="customColumn">

                        <div class="loginInner">
                            <div>
                                <h4>Welcome to PSST
                                </h4>
                            </div>

                            <div>
                                <div>
                                    <h6>Username
                                    </h6>
                                </div>
                                <div>
                                    <asp:TextBox ID="tbUsername" runat="server" ToolTip="Please enter your email address" AutoCompleteType="Email"></asp:TextBox>
                                </div>
                            </div>

                            <div>
                                <div>
                                    <h6>Password
                                    </h6>
                                </div>
                                <div>
                                    <asp:TextBox ID="tbPassword" runat="server" ToolTip="Please enter your password" TextMode="Password"></asp:TextBox>
                                    <div id="showPWContainer" runat="server">
                                        
                                        <label for="CBshowPassword">
                                            <asp:CheckBox ID="CBshowPassword" runat="server" OnCheckedChanged="CBshowPassword_CheckedChanged" AutoPostBack="true"/>
                                            <span>Show password</span>
                                        </label>
                                    </div>
                                </div>
                            </div>

                            <div id="failedLogin">
                                <asp:Label ID="lblLoginFailed" runat="server" Text="Login failed, please try again or contact &lt;a href=&quot;mailto:admin@psst.co.za&quot;&gt;admin&lt;/a&gt; for assistance." Visible="False"></asp:Label>
                            </div>

                            <div>
                                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn" OnClick="btnLogin_Click" />
                            </div>
                        </div>

                    </div>
                </div>

            </div>
        </div>
    </form>

    <div class="loginFooter">
        <uc:footer runat="server" ID="footer" />
    </div>
</body>
</html>
