<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication2.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     
    <h1 style="text-indent:10px">
        Login</h1>
    <p style="text-indent:10px">
        Логін:
        <asp:TextBox ID="Login" runat="server"></asp:TextBox></p>
    <p style="text-indent:10px">
        Пороль:
        <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox></p>
    <p>
     <!--   <asp:CheckBox ID="RememberMe" runat="server" Text="Remember Me" /> </p> -->
    <p style="text-indent:10px">
        <asp:Button ID="LoginButton" runat="server" Text="Login" OnClick="LoginButton_Click" /> </p>
    <p style="text-indent:10px">
        <asp:Label ID="Message" runat="server" ForeColor="Red" Text="Ваш логін або пороль не вірні"
            Visible="False"></asp:Label> </p>

</asp:Content>
