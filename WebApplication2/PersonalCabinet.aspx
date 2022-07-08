<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PersonalCabinet.aspx.cs" Inherits="WebApplication2.PersonalCabinet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <h3 style="text-indent:10px"> Особисті данні</h3>
    <p style="text-indent:10px">
        <asp:Label ID="Login" runat="server" Text="Your username"></asp:Label> </p>
    <p style="text-indent:10px">
        <asp:Label ID="FIO" runat="server" Text="Your username"></asp:Label> </p>
    <p style="text-indent:10px">
        <asp:Label ID="DOLJ" runat="server" Text="Your username"></asp:Label> </p>
     <p style="text-indent:10px">
        <asp:Label ID="LCange" runat="server" Text="Your username"></asp:Label> </p>
    <h3 style="text-indent:10px"> Зміна поролю</h3>
    <p style="text-indent:10px">
        Старий пороль:
        <asp:TextBox ID="OldPass" runat="server" TextMode="Password"></asp:TextBox>
    </p>
    <p style="text-indent:10px">
        Пороль:
        <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
    </p>
   
    <p style="text-indent:10px">
        Повтор:
        <asp:TextBox ID="Password2" runat="server" TextMode="Password"></asp:TextBox>
    </p>
     <p style="text-indent:10px">
    <asp:Label ID="Message" runat="server" ForeColor="Red" Text="Your username or password is invalid. Please try again."
            Visible="False"></asp:Label> </p>
    <p style="text-indent:10px">
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Змінити" /></p>
    <p>
</asp:Content>
