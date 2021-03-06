﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="GXIssueTrackingBot._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Genexus Issue Tracking Bot</title>
	<link rel="stylesheet" type="text/css" href="default.css" />
</head>
<body style="font-family: 'Segoe UI'">
	<form id="form1" runat="server">
		<div>
			<h1>Genexus Issue Tracking Bot</h1>
			<p>This is a 'private' bot used by the Genexus issue tracking system.</p>
		</div>
		<div id="footer">
			<asp:Label ID="lblVersion" runat="server" Font-Names="Calibri" Font-Size="Small" ForeColor="#666666"></asp:Label>
			<br />
			<asp:Label ID="devBy" runat="server" Font-Names="Calibri" Font-Size="Small" ForeColor="#666666">Developed by <a href="http://twitter.com/sebagomez">@sebagomez</a> </asp:Label>
		</div>
	</form>
	<a href="https://github.com/sebagomez/RochaIssuesBot">
		<img style="position: absolute; top: 0; right: 0; border: 0;" src="https://camo.githubusercontent.com/365986a132ccd6a44c23a9169022c0b5c890c387/68747470733a2f2f73332e616d617a6f6e6177732e636f6d2f6769746875622f726962626f6e732f666f726b6d655f72696768745f7265645f6161303030302e706e67" alt="Fork me on GitHub" data-canonical-src="https://s3.amazonaws.com/github/ribbons/forkme_right_red_aa0000.png">
	</a>
</body>
</html>
