using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class SiteMaster : MasterPage{


        protected void Page_Load(object sender, EventArgs e){

            
            if (Context.Session != null){
               
                if (Request.Cookies.Get(".ASPXAUTH") != null){
                    if (Menu2.Items.Count == 0) { 
                    Menu2.Items.Add(new  MenuItem( Session["FIO"].ToString()));
                        Menu2.Items[0].ChildItems.Add(new MenuItem("Особистий кабінет","","","~/PersonalCabinet.aspx"));
                        if(Int32.Parse(Session["DOLJNOSTID"].ToString())==1) Menu2.Items[0].ChildItems.Add(new MenuItem("Адміністрування"));
                        Menu2.Items[0].ChildItems.Add(new MenuItem("Вихід"));
                    }
                    
                }
                else{
                   if ( Request.RawUrl != "/Login") {
                        Response.Redirect("Login"); 
                    }

                }

            }


        }

        protected void Menu2_MenuItemClick(object sender, MenuEventArgs e){
            switch (e.Item.Text) {
                case "Особистий кабінет": { Response.Redirect("PersonalCabinet"); } break;
                case "Адміністрування": { Response.Redirect("AdministrationPanel.aspx"); } break;
                case "Вихід": { System.Web.Security.FormsAuthentication.SignOut(); Response.Redirect("Login"); } break;
            }
            
        }
    }
}