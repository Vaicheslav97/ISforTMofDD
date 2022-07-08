<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication2._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:Label ID="Tit" runat="server" Text="Завдання" Font-Bold="True" Font-Size="X-Large"></asp:Label>
    <asp:Table ID="Table1" runat="server" Width="100%">
        <asp:TableRow>
<asp:TableCell>
    <asp:Label ID="Label1" runat="server" Text="План:"></asp:Label>
    <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True"></asp:DropDownList></asp:TableCell>
            <asp:TableCell ><div align="right">
    <asp:Label ID="Label2" runat="server" Text="Фільтрація:"></asp:Label>
    <asp:DropDownList ID="DropDownList2" runat="server"></asp:DropDownList>
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
<asp:Button ID="Button2" runat="server" Text="Фільтрувати" OnClick="Button2_Click" />
   </div></asp:TableCell>

        </asp:TableRow>

    </asp:Table>
   
    <asp:GridView ID="GridView1" runat="server"  AutoGenerateColumns="False" AllowSorting="True" style="margin-left:10px" OnRowDataBound="GridView1_RowDataBound" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowUpdating="GridView1_RowUpdating" OnSorting="GridView1_Sorting" OnRowEditing="GridView1_RowEditing" OnDataBinding="GridView1_DataBinding" EnableModelValidation="False" ViewStateMode="Enabled" AutoGenerateSelectButton="True" OnSelectedIndexChanging="GridView1_SelectedIndexChanging" >    
</asp:GridView>
   
     
    <asp:Label ID="Tit0" runat="server" Text="Додавання" Font-Bold="True" Font-Size="X-Large"></asp:Label>
       
     
      </br>
    <asp:Panel ID="Panel1" runat="server" ViewStateMode="Enabled" >
        <asp:Table ID="Table2" runat="server">
        </asp:Table>

    </asp:Panel>
    <p style="text-indent:10px">
        <asp:Label ID="Message" runat="server" ForeColor="Red" Text="Запит  не пройшов. Перевірте данні."
            Visible="False"></asp:Label> </p>
    <asp:Button ID="SaveBtn" runat="server" Text="Зберегти" OnClick="SaveBtn_Click" /><asp:Button ID="Button1" runat="server" Text="Відминити" OnClick="Button1_Click1" />
    <p style="text-indent:10px">
    </p>

</asp:Content>
