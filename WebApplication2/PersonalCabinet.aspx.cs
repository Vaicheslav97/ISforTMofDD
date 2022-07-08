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
    public partial class PersonalCabinet : System.Web.UI.Page{
        protected void Page_Load(object sender, EventArgs e){
            if (Context.Session == null|| Request.Cookies.Get(".ASPXAUTH") == null) return;
                int ID = Int32.Parse(Session["ID"].ToString());

            FbConnection connect;      //З'єднання
            DataSet ds; //Набір даних
            FbDataAdapter da;
            FbCommandBuilder cb;

            string cnStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString; ;

            connect = new FbConnection(cnStr);
            ds = new DataSet("ds");
            try{
                connect.Open();
                if (connect.State == ConnectionState.Open){
                    // DataTable table = connect.GetSchema("Tables");
                    FbCommand myCommand;
                    myCommand = connect.CreateCommand();
                    
                    myCommand.CommandText = "Select * FROM SOTRUDNIKI_VW where ID = '" + ID + "' ;";
                    //MessageBox.Show(myCommand.CommandText);
                    FbDataReader dr = myCommand.ExecuteReader();

                    da = new FbDataAdapter(myCommand.CommandText, connect);
                    cb = new FbCommandBuilder(da);
                    DataTable Temp = new DataTable();
                    da.Fill(Temp);

                    if (Temp.Rows.Count > 0){

                       Login.Text = "Логін: "+ Temp.Rows[0].Field<string>("LOGIN");
                        FIO.Text = "ПІБ: " + Temp.Rows[0].Field<string>("FIO");
                        DOLJ.Text = "Посада: " + Temp.Rows[0].Field<string>("DOLJNOST");
                        LCange.Text = "Дата останьої зміни: " + Temp.Rows[0].Field<DateTime>("DATA_POSLEDNEGO_IZMENENIA").ToString();
                    }

                }

            }
            catch (Exception) { if (connect.State != ConnectionState.Open) ;  }
            finally{
                if (connect.State == ConnectionState.Open) { connect.Close(); }   // Метод Close() закриває з'єднання
            }


        }

        protected void Button1_Click(object sender, EventArgs e){
            this.Validate();// Исполнить валидаторы на сервере
            if (!this.IsValid) { return; }
            if (OldPass.Text.Trim() == "" || Password.Text.Trim() == "" || Password2.Text.Trim() == "") {
                Message.Text = "Одне або декілька полей пусті";
                Message.Visible = true;
                return;
            }
            else
            if (Password.Text.Trim() != Password2.Text.Trim()) {
                Message.Text = "Поролі не співпадають";
                Message.Visible = true;
                return;
            } else if (OldPass.Text.Trim() == Password.Text.Trim()) {
                Message.Text = "Старий та новий пороль співпадають";
                Message.Visible = true;
                return;
            }

            if (Context.Session == null || Request.Cookies.Get(".ASPXAUTH") == null) return;
            int ID = Int32.Parse(Session["ID"].ToString());
            FbConnection connect;      //З'єднання
            DataSet ds; //Набір даних
            FbDataAdapter da;
            FbCommandBuilder cb;


            string cnStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString; ;

            connect = new FbConnection(cnStr);
            try{
                connect.Open();
                if (connect.State == ConnectionState.Open){
                    FbCommand myCommand;


                    myCommand = connect.CreateCommand();
                    string hPass = FormsAuthentication.HashPasswordForStoringInConfigFile(OldPass.Text.Trim(), "MD5");
                    string hPass2 = FormsAuthentication.HashPasswordForStoringInConfigFile(Password.Text.Trim(), "MD5");
                    //myCommand.CommandText = "Select * FROM SOTRUDNIKI where ID = '" + ID + "' and PAROL = '" + hPass + "' ;";
                    //MessageBox.Show(myCommand.CommandText);
myCommand.CommandText = "update sotrudniki set parol = '"+ hPass2 + "' where(ID = '" + ID + "' and PAROL = '" + hPass + "'); ";
                    if (myCommand.ExecuteNonQuery() == 0){
                        Message.Text = "Старий пороль не вірний";
                        Message.Visible = true;
                    }
                    Message.Visible = false;

                }

            }catch (Exception) { if (connect.State != ConnectionState.Open) ; Message.Visible = true; }
            finally{
                if (connect.State == ConnectionState.Open) { connect.Close(); }   // Метод Close() закриває з'єднання
            }
        }
    }
}