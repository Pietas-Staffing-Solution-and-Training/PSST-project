<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Help.aspx.cs" Inherits="PSST.Help" %>

<%@ Register Src="~/resources/lib/head.ascx" TagPrefix="uc" TagName="Head" %>
<%@ Register Src="~/resources/lib/navigation.ascx" TagPrefix="uc" TagName="navigation" %>
<%@ Register Src="~/resources/lib/footer.ascx" TagPrefix="uc" TagName="footer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<uc:Head runat="server" ID="Head" />
<body>
    <form id="help" runat="server">
        <uc:navigation runat="server" ID="navigation" />
        <div class="main-div">
            <div class="help-contain">
                <asp:Label ID="lblWelcome" runat="server" Text="Help & Documentation" Font-Size="XX-Large" ForeColor="#003479"></asp:Label>
                <asp:Panel ID="notlogged" runat="server"><p>If you're seeing this message you're not logged in. To gain access to the system, please <a href="/Login.aspx">login.</a></p></asp:Panel>
                <p><b>Welcome to PSST!</b> This is a short guide to using the PSST system depending on what your user level is (user or admin). You will only be able to see your relevant documentation, so please login with your credentials before proceeding.</p>
                <p>Pieta’s Staffing Solutions and Training (PSST) is a human resources company in South Africa with the functions of recruitment, staffing, training, and other human resources functions. We provide only the best service for our clients, both jobseekers and employers. For queries and concerns, please contact support@psst.com.</p>
                <asp:Panel ID="loggedUser" runat="server">
                    <h4>Information for Users</h4>
                    <h5>System requirements</h5>
                    <p>To use PSST effectively, please ensure that your system meets the following requirements:</p>
                    <ul>
                        <li><strong>Modern Web Browser:</strong>
                            <ul>
                                <li>Supported browsers: Chrome (latest version), Firefox (latest version), Microsoft Edge (latest version), Safari (latest version).</li>
                                <li>Ensure JavaScript, cookies, and pop-ups are enabled.</li>
                            </ul>
                        </li>
                        <li><strong>Internet Connection:</strong>
                            <ul>
                                <li>Minimum: 5 Mbps for standard performance.</li>
                                <li>Recommended: 10 Mbps or higher low-latency connection for optimal performance.</li>
                            </ul>
                        </li>
                        <li><strong>Operating System:</strong>
                            <ul>
                                <li>Windows 10 or later, macOS 10.14 or later, Linux distributions with a compatible browser.</li>
                            </ul>
                        </li>
                        <li><strong>Display:</strong>
                            <ul>
                                <li>Minimum screen resolution: 1280x720 (HD).</li>
                                <li>Recommended: 1920x1080 (Full HD) or higher for better user experience.</li>
                            </ul>
                        </li>
                        <li><strong>Processor & Memory:</strong>
                            <ul>
                                <li>Minimum: Dual-core processor, 4 GB RAM.</li>
                                <li>Recommended: Quad-core processor, 8 GB RAM or more for smoother multi-tasking.</li>
                            </ul>
                        </li>
                        <li><strong>Storage for Saved PDF Files:</strong>
                            <ul>
                                <li>Users should have at least 500 MB of local disk space available for storage of downloaded PDFs.</li>
                            </ul>
                        </li>
                        <li><strong>Plugins and Add-ons:</strong>
                            <ul>
                                <li>Latest version of Adobe Reader or another PDF viewer (for viewing downloaded PDF files).</li>
                            </ul>
                        </li>
                        <li><strong>Security:</strong>
                            <ul>
                                <li>Up-to-date antivirus and firewall settings.</li>
                                <li>TLS 1.2+ enabled for secure connections.</li>
                            </ul>
                        </li>
                    </ul>
                    <h5>Logging in</h5>
                    <p>Logging in enables you to gain access to the system and all functions you need.</p>
                    <p>To log in, navigate to the <a href="/Login.aspx">login page</a> and enter your details. Your username is your full name (eg. "John Doe"). If you have not received a username or password, please contact your administrator.</p>
                    <p>To logout, click on the Logout button in the top bar or simply close your browser.</p>
                    <h5>Navigation</h5>
                    <p>If you're reading this, you've already successfully navigated to the PSST system in your web browser. That's a great first step!</p>
                    <p>Now that you're logged in, you have access to all the other navigation options. This includes the following:</p>
                    <ul style="list-style-type:circle; list-style-position: inside;">
                        <li style="list-style-type:circle;">The top bar at the top of your screen.</li>
                        <li style="list-style-type:circle;">Buttons and links in some pages.</li>
                        <li style="list-style-type:circle;">The footer at the bottom of every page.</li>
                    </ul>
                    <p>The options you see in any of those places will all be open to you in some capacity.</p>
                    <h5>The Dashboard</h5>
                    <p>This is the landing page where you'll receive a quick explanation of the PSST system's purpose and foundational functionality.</p>
                    <p>Select one of the buttons to start your PSST journey.</p>
                    <h5>The Jobs Page</h5>
                    <p>This page shows the job you are currently assigned to, including ones previously completed. You can also add your time to the current active job on this page.</p>
                    <p>To <b>add the time you spent on a job</b>, click the "Edit Time" button at the top left of the table. This takes you to the section under the table where you can enter in the new time spent as the number of hours.</p>
                    <p>You cannot edit anything else on this page. Please contact an admin if you notice any erroneous information.</p>
                    <h5>The Resources Page</h5>
                    <p>You, as a Resource, are able to view your Resource information stored with PSST and change the password if required on this page.</p>
                    <p>To <b>change your password</b>, simply click the "Change Password" button at the top left of the table which will take you to the section at the bottom of the page. Then, enter a new password that has as least one capital letter, one number, and one special character in it.</p>
                    <p>You cannot edit anything else on this page. Please contact an admin if you notice any erroneous information.</p>
                </asp:Panel>
                <asp:Panel ID="loggedAdmin" runat="server">
                    <h4>Information for Admins Only</h4>
                    <h5>The Clients Page</h5>
                    <p>Only admins are able to access the Clients page. This page shows a list of all the clients and allows you to search, sort, and maintain client data.</p>
                    <p>To <b>search</b>, simply select the search textbox at the top right of the table and start typing. Click the Search button when you're ready to search. The clear button clears the search box.</p>
                    <p>To <b>update a Client</b>, simply click the <img class="help-icons" src="/Resources/Icons/edit - pixelperfect.png" /> button to start editing that row, then edit the desired field, and click the <img class="help-icons" src="/Resources/Icons/confirm - roundicons.png" /> icon to confirm or the <img class="help-icons" src="/Resources/Icons/cancel - gregorcresnar.png" /> icon to cancel editing.</p>
                    <p>To <b>delete a Client</b>, click the <img class="help-icons" src="/Resources/Icons/bin - freepik.png" /> button to delete that row's entry. This action is permanent, so please think carefully before confirming deletion.</p>
                    <p>To <b>add a new Client</b>, click the "Add Client" button at the top left of the table. This takes you to the section under the table where you can enter in the new client's details. The ID field is automatically filled with the next ID and cannot be changed. Phone numbers must start with a 0 or a +27 code and email addresses must be written correctly.</p>
                    <p> To <b>sort the table</b>, click once on any of the column headers to sort by that column, and click to again to sort in the opposite order (ascending or descending).</p>
                    <h5>The Jobs Page</h5>
                    <p>Admins have permissions to do much more on the Jobs page than users. You can view all the jobs at PSST, both active and complete, edit information, add new Jobs, and generate invoices.</p>
                    <p>To <b>add a new Job</b>, click the "Add Job" button which takes you to the section at the bottom of the page. There, you must select the Resource and Client from the dropdown and then add a descriptio text and budget number in ZAR. Press "Add" to confirm the new Job. <b>Remember that only one active job can be assigned to a Resource at one time. Clients can have multiple active jobs.</b></p>
                    <p>To <b>update a Job</b>, simply click the <img class="help-icons" src="/Resources/Icons/edit - pixelperfect.png" /> button to start editing that row, then edit the desired field (only edit Resource and Client IDs, not their names), and click the <img class="help-icons" src="/Resources/Icons/confirm - roundicons.png" /> icon to confirm or the <img class="help-icons" src="/Resources/Icons/cancel - gregorcresnar.png" /> icon to cancel editing.</p>
                    <p>To <b>mark at job as complete</b>, click the <img class="help-icons" src="/Resources/Icons/flag - iconmania.png" /> button to mark that row's entry as complete. This action is permanent, so ensure it is truly complete before clicking.</p>
                    <p> To <b>sort the table</b>, click once on any of the column headers to sort by that column, and click to again to sort in the opposite order (ascending or descending).</p>
                    <p>To <b>view and optionally download a job's invoice</b>, click the <img class="help-icons" src="/Resources/Icons/invoice - thoseicons.png" /> button which will take you to that job's invoice page. There, you can preview the invoice, select the "Download PDF" button to download it, or "Go Back" to the jobs page.</p>
                    <h5>The Resources Page</h5>
                    <p>Admins have permissions to do much more on the Resources page than users. You can view all the Resources at PSST, edit their information, delete Resources, and add new Resources.</p>
                    <p>To <b>update a Resource</b>, simply click the <img class="help-icons" src="/Resources/Icons/edit - pixelperfect.png" /> button to start editing that row, then edit the desired field, and click the <img class="help-icons" src="/Resources/Icons/confirm - roundicons.png" /> icon to confirm or the <img class="help-icons" src="/Resources/Icons/cancel - gregorcresnar.png" /> icon to cancel editing.</p>
                    <p>To <b>delete a Resource</b>, click the <img class="help-icons" src="/Resources/Icons/bin - freepik.png" /> button to delete that row's entry. This action is permanent, so please think carefully before confirming deletion.</p>
                    <p>To <b>add a new Resource</b>, click the "Add Resource" button at the top left of the table. This takes you to the section under the table where you can enter in the new resource's details. The ID field is automatically filled with the next ID and cannot be changed. Phone numbers must start with a 0 or a +27 code and wages must be numbers only.</p>
                    <p> To <b>sort the table</b>, click once on any of the column headers to sort by that column, and click to again to sort in the opposite order (ascending or descending).</p>
                </asp:Panel>
            </div>
        </div>
        <uc:footer runat="server" ID="footer" />
    </form>
</body>
</html>
