using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.ServiceModel.Syndication;



namespace HaberCekme
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection("Data Source=KAMILEENER-DELL\\KAMILEENER;Initial Catalog=haberCek;Integrated Security=True");
        SqlCommand cmd;
        SqlTransaction islem = null;
        

       public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            comboBox1.Items.Clear();
            con.Open();
            cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT haber_isim,haber_adid FROM table_haberid";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = dr["haber_isim"].ToString();
                item.Value = Convert.ToInt32(dr["haber_adid"]);
                comboBox1.Items.Add(item);
            }

            con.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            combo();

        }

        private void combo()
        {
            if(con.State != ConnectionState.Open)
                con.Open();
            // SqlCommand cmd = new SqlCommand("Select * from table_ktg.haber_ad right join table_ktg On table_haber.kategori_id=table_ktg.kategori_id where table_ktg.kategori_ad='" + comboBox2.Text + "'";
            cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;

            ComboboxItem secili = comboBox1.SelectedItem as ComboboxItem;
            cmd.CommandText = "select table_ktg.kategori_ad,table_ktg.kategori_id,haberAlt.haber_adid from table_haber as haberAlt inner join table_ktg as table_ktg on haberAlt.kategori_id=table_ktg.kategori_id where haber_adid="+ secili.Value;

            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            comboBox2.Items.Clear();

            foreach (DataRow dr in dt.Rows)
              {
                ComboboxItem item = new ComboboxItem();
                item.Text = dr["kategori_ad"].ToString();
                item.Value = Convert.ToInt32(dr["kategori_id"]);
            
                comboBox2.Items.Add(item);
              }
              
            con.Close();

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {          
            listBox1.Items.Clear();

            listBox1.FormattingEnabled = true;

            if(con.State != ConnectionState.Open)
                con.Open();
            cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            
            ComboboxItem secili2 = comboBox2.SelectedItem as ComboboxItem;
            ComboboxItem secili1 = comboBox1.SelectedItem as ComboboxItem;
            cmd.CommandText = "select haber.haber_link from dbo.table_haber as haber " +
           "where haber.kategori_id ="+secili2.Value+" and haber.haber_adid ="+secili1.Value;

            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
           
                string link = dt.Rows[0]["haber_link"].ToString();
            
            try
            {
                //con.Open();
                // XmlReader reader = cmd.ExecuteXmlReader();
                XmlTextReader reader = new XmlTextReader(link);
                XElement rootElement = XElement.Load(link);

                List<XElement> xElements = rootElement.Elements("channel").ToList();
                List<XElement> items = xElements.FirstOrDefault().Elements("item").ToList();
               
                for(int i = 0; i < items.Count(); i++)
                {
                    string sss= items[i].Element("title").Value;
                    
                    listBox1.Items.Add(sss);
                }
                
                return;       
           
            }
            catch(Exception err)
            {
                listBox1.Items.Add(err.ToString());
            }
            con.Close(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            


            for (int i = 0; i < listBox1.Items.Count; i++) {
                         
         
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    islem = con.BeginTransaction();
                    
                    string kayit = " insert into table_icerik (haber_icerik)values(@haber_icerik) ";
                    SqlCommand cmd = new SqlCommand(kayit, con);

                    cmd.Transaction = islem;

                    cmd.Parameters.AddWithValue("@haber_icerik", listBox1.Items[i].ToString());


                    cmd.ExecuteNonQuery();
                    islem.Commit();

                }catch(SqlException err)
                {
                    islem.Rollback();
                    MessageBox.Show(err.ToString());
                }






            }            
            con.Close();
            MessageBox.Show("Saved!");

        }
    }

      
    }

