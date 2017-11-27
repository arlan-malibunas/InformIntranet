<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="InformIntranet.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
	    <asp:Panel runat="server" ID="pnlCallStats">
            <strong>My Call Statistics</strong><br />
            Extension: <asp:Literal ID="litExtension" runat="server"></asp:Literal>
            <br />
            Calls made: <asp:Literal ID="litCallsMade" runat="server"></asp:Literal>
            <br />
            Connected Minutes: <asp:Literal ID="litConnectedMinutes" runat="server"></asp:Literal>
            <br />
            First Call: <asp:Literal ID="litFirstCall" runat="server"></asp:Literal>
		</asp:Panel>
		<asp:Panel runat="server" ID="pnlNonTM" Visible="False">
			<strong>No Call Stats Available</strong>
		</asp:Panel>
    </form>
</body>
</html>
