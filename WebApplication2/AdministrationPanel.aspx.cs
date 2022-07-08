using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.Configuration;
using System.Web.Security;


namespace WebApplication2{
    public partial class AdministrationPanel : System.Web.UI.Page{
DataView MyDataView;
DataTable Temp;
FbConnection connect;      //З'єднання
            DataSet ds; //Набір даних
            FbDataAdapter da;
            FbCommandBuilder cb;
string cnStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        
  string SelStr;

        protected void BDR(object sender, EventArgs e){
         connect = new FbConnection(cnStr);
            ds = new DataSet("ds");
            try{
                connect.Open();
                if (connect.State == ConnectionState.Open){
                    // DataTable table = connect.GetSchema("Tables");
                    FbCommand myCommand;
                    myCommand = connect.CreateCommand();
                    SelStr = ViewState["ASelStR"].ToString();
                    string FilterStr = ViewState["AFilterStr"].ToString();
                    myCommand.CommandText = SelStr + FilterStr + " ;";
                   
                    FbDataReader dr = myCommand.ExecuteReader();

                    da = new FbDataAdapter(myCommand.CommandText, connect);
                    cb = new FbCommandBuilder(da);
                    Temp = new DataTable();
                    da.Fill(Temp);
                    MyDataView = new DataView(Temp);
                   
                    GridView1.DataSource = MyDataView;
                  
                }



            }
            catch (Exception) { if (connect.State != ConnectionState.Open) ; Message.Visible = true; }
            finally
            {
                if (connect.State == ConnectionState.Open) { connect.Close(); }   // Метод Close() закриває з'єднання
            }
        }




            protected void Page_Load(object sender, EventArgs e){
            if (Context.Session == null || Request.Cookies.Get(".ASPXAUTH") == null) { return; }
            if (Session["DOLJNOSTID"].ToString() != "1") { Response.Redirect("Default.aspx"); return; }

            if (!IsPostBack)
            {

                SelStr = "SELECT * FROM SOTRUDNIKI_VW ";
                ViewState.Add("ASelStR", SelStr);
                ViewState.Add("AFilterStr", " ");
                ViewState.Add("ASortDir", "");
                ViewState.Add("ASortF", "");

                BDR(sender, e);
                GridView1.DataBind();
            }
           // else { BDR(sender, e); }


           // MyDataView = (DataView)ViewState["ADataView"];


            if (IsPostBack) return;


            int j = 0;
            for (int i = 0; GridView1.Columns.Count > i; i++)
                if (GridView1.Columns[i].Visible) {
                    DropDownList2.Items.Add(GridView1.Columns[i].HeaderText);
                    DropDownList2.Items[j].Value =MyDataView.Table.Columns[i].ColumnName;
                        j++;
                }
  
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e){
            GridView1.Columns[3].Visible = CheckBox1.Checked;
            BDR(sender, e);
            GridView1.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e){
            this.Validate();// Исполнить валидаторы на сервере
            if (!this.IsValid) { return; }
            if (FIO.Text.Trim() == "" || Login.Text.Trim() == "" || Password.Text.Trim() == "" || Password2.Text.Trim() == "") {
                Message.Text = "Одне або декілька полей пусті";
                Message.Visible = true;
                return;
            }
            else
            if (Password.Text.Trim() != Password2.Text.Trim()) {
                Message.Text = "Поролі не співпадають";
                Message.Visible = true;
                return; }
            
            connect = new FbConnection(cnStr);
            try{
                connect.Open();
                if (connect.State == ConnectionState.Open){
                    FbCommand myCommand;
                    myCommand = connect.CreateCommand();
                    string hPass = FormsAuthentication.HashPasswordForStoringInConfigFile(Password.Text.Trim(), "MD5");
                    myCommand.CommandText = "insert into sotrudniki (fio, doljnost,login, parol, aktiven) values( '"+ FIO.Text.Trim() + "' , "+ DropDownList1.SelectedValue+" , '" + Login.Text.Trim() + "' , '" + hPass + "',  1); ";
                    //MessageBox.Show(myCommand.CommandText);
                    myCommand.ExecuteNonQuery();
                    //Response.Redirect(Request.RawUrl);
                    BDR(sender, e);
                    GridView1.DataBind();

                }

            }
            catch (Exception) { if (connect.State != ConnectionState.Open) ; Message.Visible = true; }
            finally
            {
                if (connect.State == ConnectionState.Open) { connect.Close(); }   // Метод Close() закриває з'єднання
            }

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e){
            
            DropDownList dd = (DropDownList)e.Row.FindControl("ListBox1");
              if (null != dd){
                Label ll = (Label)e.Row.FindControl("Label1");
                for (int i = 0; i<dd.Items.Count; i++) {
                    if (dd.Items[i].Value == ll.Text) { 
                        dd.SelectedIndex = i; break;
                    }
                }
                //dd.SelectedIndex = 1;
               // dd.SelectedValue = "";
                 //dd.SelectMethod = "ID = '2'";
               
                dd.DataBind();
             }
        }

        protected void Button2_Click(object sender, EventArgs e){
            //DropDownList2.SelectedValue +
               if (TextBox1.Text.Trim() != ""){
                ViewState["AFilterStr"] = " where "+ DropDownList2.SelectedValue +" LIKE '%"+ TextBox1.Text + "%' " ;
               }else{
                ViewState["AFilterStr"] = "";
                }
            BDR(sender, e);
            GridView1.DataBind();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e){
            BDR(sender, e);
            GridView1.EditIndex = e.NewEditIndex;
            GridViewRow GR = GridView1.Rows[e.NewEditIndex];

            GridView1.DataBind();
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e){
            // ASC  DESC
            //Session.Add("ASortStr", ss);
            BDR(sender, e);
            string dir = ViewState["ASortDir"].ToString();
            string Fild = ViewState["ASortF"].ToString();
            if (Fild == e.SortExpression){
                if (dir == "") { dir = " ASC "; }
                else if (dir == " ASC ")
                {
                    dir = " DESC ";
                }
                else { dir = ""; ViewState["ASortF"] = ""; }
            }
            else { dir = " ASC "; }

            ViewState["ASortF"] = e.SortExpression;
            ViewState["ASortDir"] = dir;
 
            MyDataView.Sort = " " + e.SortExpression + dir ;
            
            GridView1.DataBind();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e){
            GridView1.EditIndex = -1;
            BDR(sender, e);
            GridView1.DataBind();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e){


            connect = new FbConnection(cnStr);
            try{
                connect.Open();
                if (connect.State == ConnectionState.Open) {
                   
                    FbCommand myCommand;
                    myCommand = connect.CreateCommand();
                    GridViewRow GR = GridView1.Rows[e.RowIndex];
                    myCommand.CommandText = "update sotrudniki set FIO = '" + (GR.Cells[2].Controls[0] as TextBox).Text + "', AKTIVEN = '" + (GR.Cells[5].Controls[0] as TextBox).Text + "', DOLJNOST = '" + (GR.Cells[7].Controls[3] as DropDownList).SelectedValue + "' where(ID = '" + (GR.Cells[1].Controls[0] as TextBox).Text + "'); ";
                    //MessageBox.Show(myCommand.CommandText);
                   // Message.Text = myCommand.CommandText;

                     myCommand.ExecuteNonQuery();
                    // Response.Redirect(Request.RawUrl);
                   
                   // GridView1.DataBind();
                }


            }
            catch (Exception) { if (connect.State != ConnectionState.Open) ; }
            finally{
                if (connect.State == ConnectionState.Open) { connect.Close(); }   // Метод Close() закриває з'єднання
            }

            BDR(sender, e);
            GridView1.EditIndex = -1;
            GridView1.DataBind();

            
           // Message.Visible = true;

        }

       
    }
}