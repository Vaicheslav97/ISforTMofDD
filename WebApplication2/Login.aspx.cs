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
using System.Web.SessionState;
namespace WebApplication2{
    public partial class WebForm1 : System.Web.UI.Page{
        protected void Page_Load(object sender, EventArgs e){

            
        }

        protected void LoginButton_Click(object sender, EventArgs e){
            this.Validate();// Исполнить валидаторы на сервере
            if (!this.IsValid) {return;}
            FbConnection connect;      //З'єднання
            DataSet ds; //Набір даних
            FbDataAdapter da;
            FbCommandBuilder cb;
            
            string cnStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString; ;

            connect = new FbConnection(cnStr);
            ds = new DataSet("ds");
            try {
                connect.Open();
                if (connect.State == ConnectionState.Open){
                    // DataTable table = connect.GetSchema("Tables");
                    FbCommand myCommand;
                    myCommand = connect.CreateCommand();
                   string hPass = FormsAuthentication.HashPasswordForStoringInConfigFile(Password.Text.Trim(), "MD5");
                    myCommand.CommandText = "Select * FROM SOTRUDNIKI where LOGIN = '" + Login.Text.Trim() + "' AND PAROL = '" + hPass + "' AND AKTIVEN = 1 ;";
                    //MessageBox.Show(myCommand.CommandText);
                    FbDataReader dr = myCommand.ExecuteReader();

                    da = new FbDataAdapter(myCommand.CommandText, connect);
                    cb = new FbCommandBuilder(da);
                    DataTable Temp = new DataTable();
                    da.Fill(Temp);
                   
                    if (Temp.Rows.Count>0) {
                        //  FormsAuthentication.RedirectFromLoginPage(Login.Text, false);  
                        FormsAuthentication.SetAuthCookie(Login.Text, false);
                    int sID = Temp.Rows[0].Field<int>("ID");
                       Session.Add("ID", sID);
                        //Context.Response.Cookies[".ASPXAUTH"].Values["ID"] = sID.ToString();
                    //Response.Cookies[".ASPXAUTH"].Values["LOGIN"] = Login.Text.Trim();
                    string sFIO = Temp.Rows[0].Field<string>("FIO");
                       Session.Add("FIO", sFIO);
                       //Context.Response.Cookies[".ASPXAUTH"].Values["FIO"] = sFIO;
                    int sDOLJNOSTID  = Temp.Rows[0].Field<int>("DOLJNOST");
                        Session.Add("DOLJNOSTID", sDOLJNOSTID);
                        //Context.Response.Cookies[".ASPXAUTH"].Values["DOLJNOSTID"] = sDOLJNOSTID.ToString();
                        //Context.Response.Redirect("Default.aspx");
                        Response.Redirect("Default.aspx");
                    } else { Message.Visible = true; }


                    //Message.Text = Request.UrlReferrer.AbsoluteUri;
                    //  Message.Visible = true;
                    // FormsAuthentication.SignOut();
                    //Response.Redirect("Default.aspx");
                    //Server.Transfer("Default.aspx", true);
                }

            }
            catch (Exception) { if (connect.State != ConnectionState.Open)  ; Message.Visible = true; }
            finally{
                if (connect.State == ConnectionState.Open) { connect.Close(); }   // Метод Close() закриває з'єднання
            }
        }


    
    }
}