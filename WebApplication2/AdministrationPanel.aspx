<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdministrationPanel.aspx.cs" Inherits="WebApplication2.AdministrationPanel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
   
     <asp:Table ID="Table1" runat="server" Width="100%">
        <asp:TableRow >
<asp:TableCell><asp:CheckBox ID="CheckBox1" runat="server" OnCheckedChanged="CheckBox1_CheckedChanged" Text="Відображати хешовані поролі" style="margin-left:10px" AutoPostBack="True"/></asp:TableCell>
            <asp:TableCell >
    <div align="right">
  <asp:Label ID="Label2" runat="server" Text="Фільтрація"></asp:Label>
    <asp:DropDownList ID="DropDownList2" runat="server"></asp:DropDownList>
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
<asp:Button ID="Button2" runat="server" Text="Button" OnClick="Button2_Click" />
   </div></asp:TableCell>

        </asp:TableRow>

    </asp:Table>

    <asp:GridView ID="GridView1" runat="server"  AutoGenerateColumns="False" AllowSorting="True" AutoGenerateEditButton="True" style="margin-left:10px" OnRowDataBound="GridView1_RowDataBound" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowUpdating="GridView1_RowUpdating" OnSorting="GridView1_Sorting" OnRowEditing="GridView1_RowEditing" >
        <Columns>

            <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
            <asp:BoundField DataField="FIO" HeaderText="ПІБ" SortExpression="FIO" />
            <asp:BoundField DataField="LOGIN" HeaderText="Login" SortExpression="LOGIN" />
            <asp:BoundField DataField="PAROL" HeaderText="Пороль" ReadOnly="True" SortExpression="PAROL" Visible="False" />
            <asp:BoundField DataField="AKTIVEN" HeaderText="Доступ" SortExpression="AKTIVEN" />
            <asp:BoundField DataField="ID_DOLJNOSTI" HeaderText="ID_DOLJNOSTI" SortExpression="ID_DOLJNOSTI" Visible="False" />
            <asp:TemplateField HeaderText="Посада" SortExpression="Doljnost">
                <EditItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("ID_DOLJNOSTI") %>' Visible=" false"></asp:Label>
                    <asp:DropDownList ID="ListBox1" runat="server"  DataSourceID="SqlDataSource2" DataTextField="NAIMENOVANIE" DataValueField ="ID" >  </asp:DropDownList>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Doljnost") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="DATA_POSLEDNEGO_IZMENENIA" HeaderText="Остання зміна запису" ReadOnly="True" SortExpression="DATA_POSLEDNEGO_IZMENENIA" />
        </Columns>
        
        <SelectedRowStyle BackColor="#3289C4" />
        
        <SortedAscendingCellStyle Font-Overline="False" />
        <SortedAscendingHeaderStyle Font-Overline="True" />
        <SortedDescendingHeaderStyle Font-Underline="True" />
        
</asp:GridView>
   
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT &quot;ID&quot;, &quot;NAIMENOVANIE&quot; FROM &quot;DOLJNOSTI&quot;"></asp:SqlDataSource>
   
    <h3 style="text-indent:10px"> Регистрація</h3>
    <p style="text-indent:10px">
       ПІБ:
        <asp:TextBox ID="FIO" runat="server" Width="197px"></asp:TextBox>
    </p>
    <p style="text-indent:10px">
       Посада:
        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource1" DataTextField="NAIMENOVANIE" DataValueField="ID">
        </asp:DropDownList>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT &quot;ID&quot;, &quot;NAIMENOVANIE&quot; FROM &quot;DOLJNOSTI&quot;"></asp:SqlDataSource>
    </p>
    
    <p style="text-indent:10px">
       Логін:
        <asp:TextBox ID="Login" runat="server"></asp:TextBox>
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
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Региструвати" /></p>
    <p>
    </p>
</asp:Content>
