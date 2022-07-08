using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.Configuration;

namespace WebApplication2{


    public class DropViewTemplate : ITemplate { 

    string _columnName;
        Label lbl;

    public DropViewTemplate(string colname) { 
        _columnName = colname; 
    } 

  
    void ITemplate.InstantiateIn(System.Web.UI.Control container) { 
                    lbl = new Label();
            lbl.ID = "lbl_"+_columnName;
                     lbl.DataBinding += new EventHandler(tb1_DataBinding);    
                container.Controls.Add(lbl);                         
    } 

    void tb1_DataBinding(object sender, EventArgs e){

       Label txtdata = (Label)sender; 
        GridViewRow container = (GridViewRow)txtdata.NamingContainer; 
        object dataValue = DataBinder.Eval(container.DataItem, _columnName); 

        if (dataValue != DBNull.Value) {  txtdata.Text = dataValue.ToString(); } 

    } 

}

    public class DropEditTemplate : ITemplate{

        string _columnName, _colID, _capt;
        DataView _DataSource;
        DropDownList lbl;

        public DropEditTemplate(string colname, string colID, string capt, DataView DataSource){

            _columnName = colname;
           _colID = colID;
            _capt = capt;
            _DataSource = DataSource;
        }



        void ITemplate.InstantiateIn(Control container){
            lbl = new DropDownList();
            lbl.DataBinding += new EventHandler(tb1_DataBinding);
            lbl.ID = "DD"+_columnName;
            container.Controls.Add(lbl);
            
        }

        void tb1_DataBinding(object sender, EventArgs e){

            DropDownList txtdata = (DropDownList)sender;
            GridViewRow container = (GridViewRow)txtdata.NamingContainer;

            object dataValue = DataBinder.Eval(container.DataItem, _columnName);
            
                txtdata.DataSource = _DataSource;
                txtdata.DataValueField = _colID;
                txtdata.DataTextField = _capt;
               
            txtdata.ViewStateMode= ViewStateMode.Inherit;
            if (dataValue != DBNull.Value){



                for (int i = 0; i < _DataSource.Table.Rows.Count; i++) {
                    string str;
                    if (_DataSource.Table.Columns[_capt].DataType.Name == "DateTime") {
                        str = _DataSource.Table.Rows[i].Field<DateTime>(_capt).ToString();
                    } else if (_DataSource.Table.Columns[_capt].DataType.Name == "Int32") { str = _DataSource.Table.Rows[i].Field<Int32>(_capt).ToString(); }
                    else { str = _DataSource.Table.Rows[i].Field<string>(_capt); }


                    if (str == dataValue.ToString()){txtdata.SelectedIndex = i; break;}
                }  
                
            }
        }
    }


    public partial class _Default : Page {
        DataView[] DopDW;
        DataView MyDataView;
        FbConnection connect;      //З'єднання
        DataSet ds; //Набір даних
        FbDataAdapter da;
        FbCommandBuilder cb;
        DataTable temp;

      static string []PlanHeaderTexts = {
         "ID", "№ Завдання", "Тема", "Ісходні дані", "ID Цеху", "Цех", 
         "ID Відповідального від цеха", "Відповідальний від цеху", "ID Напряму", "Напрям",
         "ID Виконавця","Виконавець", "Дата регистрації", "Затверджено", "Час на виконаня",
         "Кореспонденція", "№ Проекту", "Заплановано з", "Заплановано по",
         "Фактично з", "Фактично по", "ID Плану","План", "Кінець плану", "Виконнання" };//plany 0
                                                                                        //plany 0

        static string[] PlanDropLists = {"Відповідальний від цеху","Напрям","Виконавець","План"};
        //static string[] PlanDropID = {"ID_OTVETSTVENOGO_OT_CEHA","ID_NAPRAVLENIA","ID_ISPOLNITELA","ID_PLANA"};
        static string[] DropCapt = { "FIO", "NAIMENOVANIE", "FIO", "C" };
        static int[] DropTID = { 1, 2, 3, 0 };
        


        //{"ID", " "}
        readonly string []modes = {"Plan","Zadania", "Zadania_Dla_Spec" };
        


        string cnStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        string SelStr;

        protected void Locol() {
            
            switch (ViewState["mode"].ToString()) {
                case "Plan": {
                        for(int i = 0; MyDataView.Table.Columns.Count>i; i++){

                         MyDataView.Table.Columns[i].Caption= PlanHeaderTexts[i];
                        }
                        
                            }break;
                }
        }

        protected void LAP(){
            FbCommand myCommand;
            myCommand = connect.CreateCommand();


            // SelStr = ViewState["ASelStR"].ToString();

            string FilterStr = ViewState["AFilterStr"].ToString();

            cb = new FbCommandBuilder(da);

            int t = 0;

            DopDW = new DataView[4];
            myCommand.CommandText = "Select * from plani ;";
            FbDataReader dr = myCommand.ExecuteReader();
            da = new FbDataAdapter(myCommand.CommandText, connect);
            temp = new DataTable();
            da.Fill(temp);
            ds.Tables.Add(temp);

            DopDW[t] = new DataView(ds.Tables[t]);
            DropDownList1.DataSource = DopDW[t];
            DropDownList1.DataTextField = "C";
            DropDownList1.DataValueField = "ID";
            if (!IsPostBack)
            {
                DateTime dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                DropDownList1.SelectedValue = ds.Tables[0].Select("C = '" + dt.ToShortDateString() + "' ")[0]["ID"].ToString();
                DropDownList1.DataBind();
                ViewState.Add("ADDSV", DropDownList1.SelectedValue);

            }
            t++;

            myCommand.CommandText = "Select * from OTVETSTVENIE_OT_CEHA ;";
            dr = myCommand.ExecuteReader();
            da = new FbDataAdapter(myCommand.CommandText, connect);
            temp = new DataTable();
            da.Fill(temp);
            ds.Tables.Add(temp);

            DopDW[t] = new DataView(ds.Tables[t]);

            t++;

            myCommand.CommandText = "Select * from NAPRAVLENIA ;";
            dr = myCommand.ExecuteReader();
            da = new FbDataAdapter(myCommand.CommandText, connect);
            temp = new DataTable();
            da.Fill(temp);
            ds.Tables.Add(temp);

            DopDW[t] = new DataView(ds.Tables[t]);

            t++;

            myCommand.CommandText = "Select * from SOTRUDNIKI where DOLJNOST ='6';";
            dr = myCommand.ExecuteReader();
            da = new FbDataAdapter(myCommand.CommandText, connect);
            temp = new DataTable();
            da.Fill(temp);
            ds.Tables.Add(temp);

            DopDW[t] = new DataView(ds.Tables[t]);

            t++;

            myCommand.CommandText = "Select * from VW_ZADANIA_DLA_PKB where ID_PLANA = '" + DropDownList1.SelectedValue + "' " + FilterStr + " ;";
            dr = myCommand.ExecuteReader();
            da = new FbDataAdapter(myCommand.CommandText, connect);
            temp = new DataTable();
            da.Fill(temp);
            ds.Tables.Add(temp);

            MyDataView = new DataView(ds.Tables[t]);

        }

            protected void BDR(object sender, EventArgs e){
            connect = new FbConnection(cnStr);
            ds = new DataSet("ds");
            try{
                connect.Open();
                if (connect.State == ConnectionState.Open){
                    // DataTable table = connect.GetSchema("Tables");
                    
                    switch (ViewState["mode"].ToString()) {
                        case "Plan": {LAP();} break;
                    
                    }


                    Locol();
                    GridView1.DataSource = MyDataView;
                }



            }
            catch (Exception) { if (connect.State != ConnectionState.Open) ; }
            finally
            {
                if (connect.State == ConnectionState.Open) { connect.Close(); }   // Метод Close() закриває з'єднання
            }
        }

        protected void GenAOE(){ 
        int s = 0; int t = 0;
                for (int i = 0; i < MyDataView.Table.Columns.Count; i++){
                    if (MyDataView.Table.Columns[i].ColumnName.StartsWith("ID_")) continue;
                    Table2.Rows.Add(new TableRow());
                    Table2.Rows[t].Cells.Add(new TableCell());
                    Table2.Rows[t].Cells.Add(new TableCell());
                    Label TPMPLB = new Label();
                    TPMPLB.Text = MyDataView.Table.Columns[i].Caption;

                    Table2.Rows[t].Cells[0].Controls.Add(TPMPLB);
                    if (PlanDropLists.Length > s && MyDataView.Table.Columns[i].Caption == PlanDropLists[s]){
                        DropDownList lbl = new DropDownList();
                        lbl.ID = "DD_" + MyDataView.Table.Columns[i].ColumnName;
                        lbl.DataSource = DopDW[DropTID[s]];
                        lbl.DataValueField = "ID";
                        lbl.DataTextField = DropCapt[s];
                        Table2.Rows[t].Cells[1].Controls.Add(lbl);
                        lbl.DataBind();
                        s++;
                    }else{
                        TextBox TPMPTB = new TextBox();
                        TPMPTB.ID = "TB_" + MyDataView.Table.Columns[i].ColumnName;
                        Table2.Rows[t].Cells[1].Controls.Add(TPMPTB);
                    }
                    t++;
        }
        }


        protected void Page_Load(object sender, EventArgs e){
            if (Context.Session == null || Request.Cookies.Get(".ASPXAUTH") == null){
                Response.Redirect("Login.aspx"); return;
            }


            if (!IsPostBack){

                string mode = Request.QueryString.Get("mode");
               
                if (mode == null) {
                    ViewState.Add("mode", "Plan");
                }else {
                    for (int i = 0; i < modes.Length; i++) {
                        if (modes[i]== mode) {ViewState.Add("mode", mode);  break; }
                        if(i== modes.Length-1) ViewState.Add("mode", "Plan");
                    }
                    
                }
                

              //  SelStr = "SELECT * FROM SOTRUDNIKI_VW ";




              //  ViewState.Add("ASelStR", SelStr);
                ViewState.Add("AFilterStr", " ");
                ViewState.Add("ASortDir", "");
                ViewState.Add("ASortF", "");
                ViewState.Add("isAdding", "t");

                BDR(sender, e);
                GridView1.DataBind();
         
     }       
               
                


            // MyDataView = (DataView)ViewState["ADataView"];

            if (ViewState["ADDSV"] != null && DropDownList1.SelectedValue != ViewState["ADDSV"].ToString()) {
                ViewState["ADDSV"] = DropDownList1.SelectedValue;
                GridView1.Columns.Clear();
                BDR(sender, e);
                GridView1.DataBind();
            }

            BDR(sender, e);
            GenAOE();

            if (!IsPostBack){


                for (int i = 0; i < MyDataView.Table.Columns.Count; i++){
                    if (MyDataView.Table.Columns[i].ColumnName.StartsWith("ID_")) continue;
                    DropDownList2.Items.Add(new ListItem(MyDataView.Table.Columns[i].Caption, MyDataView.Table.Columns[i].ColumnName));
                }

                

            }
        }

        

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e){
            e.Row.Controls.ToString();
            //GridView1.Controls.Add(e.Row.Controls[0]);
        }

        protected void Button2_Click(object sender, EventArgs e){
            //BDR(sender, e);
            //DropDownList2.SelectedValue +
            if (TextBox1.Text.Trim() != ""){
                if (ViewState["mode"].ToString() != "Plan"){
                    ViewState["AFilterStr"] = " where " + DropDownList2.SelectedValue + " LIKE '%" + TextBox1.Text + "%' ";
                }else {
                    ViewState["AFilterStr"] = " and " + DropDownList2.SelectedValue + " LIKE '%" + TextBox1.Text + "%' ";
                }
                
            }else{
                ViewState["AFilterStr"] = "";
            }
            GridView1.Columns.Clear();
            BDR(sender, e);
            GridView1.DataBind();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e){
           


        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e){
            // ASC  DESC
            //Session.Add("ASortStr", ss);
            GridView1.Columns.Clear();
            string dir = ViewState["ASortDir"].ToString();
            string Fild = ViewState["ASortF"].ToString();
            if (Fild == e.SortExpression)
            {
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
            BDR(sender, e);
            Locol();

            MyDataView.Sort = " " + e.SortExpression + dir;

;

            
            GridView1.DataSource = MyDataView;
            GridView1.DataBind();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e){
            BDR(sender, e);
            GridView1.EditIndex = -1;

            GridView1.DataBind();
        }

       

    

        protected void GridView1_DataBinding(object sender, EventArgs e){
            int s = 0;
 for (int i = 0; i < MyDataView.Table.Columns.Count; i++) {
           if (MyDataView.Table.Columns[i].ColumnName.StartsWith("ID_")) continue;
               
                BoundField temp = new BoundField();
                temp. DataField = MyDataView.Table.Columns[i].ColumnName;
                temp.HeaderText = MyDataView.Table.Columns[i].Caption;
                temp.SortExpression = MyDataView.Table.Columns[i].ColumnName;
                GridView1.Columns.Add(temp);

              
            
            }
        }

        protected void Button1_Click(object sender, EventArgs e){

        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e){}

        protected void SaveBtn_Click(object sender, EventArgs e){
            
         //   BDR(sender, e);
          //  GenAOE();
            connect = new FbConnection(cnStr);
            try
            {
                connect.Open();
                if (connect.State == ConnectionState.Open)
                {

                    FbCommand myCommand;
                    myCommand = connect.CreateCommand();
                   


                    //DropDownList dd = (DropDownList)GridView1.SelectedRow.FindControl("DDOTVETSTVENI_OT_CEHA");


                    switch (ViewState["mode"].ToString()){
                        case "Plan":{
                                // Table2.Rows[2].Cells[1].Controls[0]
                                if (ViewState["isAdding"].ToString() == "f")
                                {
                                    string cmd = "update ZADANIA_DLA_PKB set   N_ZADANIA = '" + (Table2.Rows[1].Cells[1].Controls[0] as TextBox).Text +
                                                        "', TEMA = '" + (Table2.Rows[2].Cells[1].Controls[0] as TextBox).Text +
                                                         "', ISHODNIE_DANIE = '" + (Table2.Rows[3].Cells[1].Controls[0] as TextBox).Text;

                                    DropDownList tedl = (Table2.Rows[5].Cells[1].Controls[0] as DropDownList);

                                    cmd += "', OTVETSTVENI_OT_CEHA = '" + tedl.Items[tedl.SelectedIndex].Value;
                                    tedl = (Table2.Rows[6].Cells[1].Controls[0] as DropDownList);
                                    cmd += "', NAPRAVLENIE = '" + tedl.Items[tedl.SelectedIndex].Value;
                                    tedl = (Table2.Rows[7].Cells[1].Controls[0] as DropDownList);
                                    cmd += "', ISPOLNITEL = '" + tedl.Items[tedl.SelectedIndex].Value +

                                                         "', YTVERJDENIE = '" + (Table2.Rows[9].Cells[1].Controls[0] as TextBox).Text +
                                                 "', VREMA = '" + (Table2.Rows[10].Cells[1].Controls[0] as TextBox).Text +
                                                         "', KORESPONDENTCIA = '" + (Table2.Rows[11].Cells[1].Controls[0] as TextBox).Text +
                                                         "', N_PROEKTA = '" + (Table2.Rows[12].Cells[1].Controls[0] as TextBox).Text;
                                    string tempstr = (Table2.Rows[13].Cells[1].Controls[0] as TextBox).Text;
                                    cmd += "', C_PLAN = '" + (tempstr.Length > 0 ? tempstr.Substring(0, 10):"01.01.2000") ;
                                    tempstr = (Table2.Rows[14].Cells[1].Controls[0] as TextBox).Text;
                                    cmd += "', DO_PLAN = '" + (tempstr.Length > 0 ? tempstr.Substring(0, 10) : "01.01.2000");
                                    tempstr = (Table2.Rows[15].Cells[1].Controls[0] as TextBox).Text;
                                    cmd += "', C_REAL = '" + (tempstr.Length > 0 ? tempstr.Substring(0, 10) : "01.01.2000");
                                    tempstr = (Table2.Rows[16].Cells[1].Controls[0] as TextBox).Text;
                                    cmd += "', DO_REAL = '" + (tempstr.Length > 0 ? tempstr.Substring(0, 10) : "01.01.2000");
                                    tedl = (Table2.Rows[17].Cells[1].Controls[0] as DropDownList);
                                    cmd += "', ID_PLANA = '" + tedl.Items[tedl.SelectedIndex].Value +
                                                         "', VIPOLNENO = '" + (Table2.Rows[19].Cells[1].Controls[0] as TextBox).Text +
                                                         "' where(ID = '" + (Table2.Rows[0].Cells[1].Controls[0] as TextBox).Text + "'); ";
                                myCommand.CommandText = cmd;
                                }else if (ViewState["isAdding"].ToString() == "t") {
                                     string cmd = "insert into ZADANIA_DLA_PKB (N_ZADANIA, TEMA, ISHODNIE_DANIE, OTVETSTVENI_OT_CEHA, KORESPONDENTCIA, ID_PLANA) values ( '" +
                                                              (Table2.Rows[1].Cells[1].Controls[0] as TextBox).Text + "' , '" +
                                                               (Table2.Rows[2].Cells[1].Controls[0] as TextBox).Text + "' , '" +
                                                               (Table2.Rows[3].Cells[1].Controls[0] as TextBox).Text + "' , '" +
                                                              (Table2.Rows[5].Cells[1].Controls[0] as DropDownList).SelectedValue + "' , '" +
                                                               (Table2.Rows[11].Cells[1].Controls[0] as TextBox).Text + "' , '" +
                                                               (Table2.Rows[17].Cells[1].Controls[0] as DropDownList).SelectedValue+ "'); ";

                                    myCommand.CommandText = cmd;

                                }

                            }
                            break;
                    }


                    //MessageBox.Show(myCommand.CommandText);
                    //Message.Text = myCommand.CommandText;
                    if (myCommand.ExecuteNonQuery() <= 0) { Message.Visible = true; } else { Message.Visible = false; }
                    Tit0.Text = "Додавання";
                    // Response.Redirect(Request.RawUrl);
                    //GridView1.DataBind();
                }


            }
            catch (Exception) { if (connect.State != ConnectionState.Open) ; Message.Visible = true; }
            finally
            {
                if (connect.State == ConnectionState.Open) { connect.Close(); }   // Метод Close() закриває з'єднання
            }
            GridView1.Columns.Clear();
            ViewState["isAdding"] = "t";
            Tit0.Text = "Додавання";
            BDR(sender, e);
            //GridView1.EditIndex = -1;
            GridView1.DataBind();

        }

        protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e){
            ViewState["isAdding"] = "f";
            Tit0.Text = "Редагування";
            GridView1.Columns.Clear();
            BDR(sender, e);
            GridView1.DataBind();
            //GenAOE();
            string id = GridView1.Rows[e.NewSelectedIndex].Cells[1].Text;
            DataRow[] DR = MyDataView.Table.Select("ID = " + id);


            for (int i = 0; Table2.Rows.Count > i; i++)
            {
                Control tc = Table2.Rows[i].Cells[1].Controls[0];
                if (tc.ID.StartsWith("DD_"))
                {
                    DropDownList dl = (DropDownList)tc;
                    DataView dv = (DataView)dl.DataSource;
                    //  dl.DataTextField
                    string dataValue;


                    for (int j = 0; j < dv.Table.Rows.Count; j++)
                    {
                        string str;
                        if (dv.Table.Columns[dl.DataTextField].DataType.Name == "DateTime")
                        {
                            str = dv.Table.Rows[j].Field<DateTime>(dl.DataTextField).ToString();
                            dataValue = DR[0].Field<DateTime>(tc.ID.Substring(3)).ToString();
                        }
                        else if (dv.Table.Columns[dl.DataTextField].DataType.Name == "Int32")
                        {
                            str = dv.Table.Rows[j].Field<Int32>(dl.DataTextField).ToString();
                            dataValue = DR[0].Field<Int32>(tc.ID.Substring(3)).ToString();
                        }
                        else
                        {
                            str = dv.Table.Rows[j].Field<string>(dl.DataTextField);
                            dataValue = DR[0].Field<string>(tc.ID.Substring(3));
                        }

                        if (str == dataValue) { dl.SelectedIndex = j; break; }
                    }


                }else{
                    string dataValue;
                    TextBox dl = (TextBox)tc;
                    if (MyDataView.Table.Columns[dl.ID.Substring(3)].DataType.Name == "DateTime")
                    {
                        if (DR[0].IsNull(tc.ID.Substring(3))) { dl.Text = "01.01.2000";  continue; }
                        dl.Text = DR[0].Field<DateTime>(tc.ID.Substring(3)).ToString();
                    }
                    else if (MyDataView.Table.Columns[dl.ID.Substring(3)].DataType.Name == "Int32")
                    {
                        if (DR[0].IsNull(tc.ID.Substring(3))) { dl.Text = "0"; continue; }
                        dl.Text = DR[0].Field<Int32>(tc.ID.Substring(3)).ToString();
                    }
                    else
                    {
                        dl.Text = DR[0].Field<string>(tc.ID.Substring(3));
                    }

                }


            }
            e.Cancel = true;
           // GridView1.EditIndex = -1;


        }

        protected void Button1_Click1(object sender, EventArgs e){
            ViewState["isAdding"] = "t";
            Tit0.Text = "Додавання";
        }
    }
}