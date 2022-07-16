using System;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;

namespace my_example_1
{
    public partial class Form1 : Form
    {
        DataTable table = new DataTable("table");
        string itemName;
        int NoOfUnits;
        int UPrice;
        double OPrice,MPrice,PPrice;
        double total;
        int index;

        public string conString = "Data Source=MUNSIF;Initial Catalog=drink;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string q = "SELECT [Qty]FROM [dbo].[Table1] WHERE ID IN (1,2,3) ";

            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            var arrlist = new ArrayList();
            while (reader.Read())
            {
                arrlist.Add(reader["Qty"].ToString());

            }
            AuO.Text = Convert.ToString(arrlist[0]);
            AuM.Text = Convert.ToString(arrlist[1]);
            AuP.Text = Convert.ToString(arrlist[2]);
            reader.Close();
            con.Close();
        }
        
        
        
    private void Form1_Load(object sender, EventArgs e)
        {
            table.Columns.Add("item Name", Type.GetType("System.String"));
            table.Columns.Add("UnitPrice", Type.GetType("System.String"));
            table.Columns.Add("No Of Units", Type.GetType("System.Int32"));
            table.Columns.Add("Price", Type.GetType("System.Double"));
            dt.DataSource = table;

            cbmf.Text = "0";
            cbpf.Text = "0";
            cborange.Text = "0";
        }

        

        

        private void dltbtn_Click(object sender, EventArgs e)
        {
            index = dt.CurrentCell.RowIndex; //getting the index of the row that is selected
            /*the below code has been used to set the no of units needed to zero of the 
             * above selected item after deleting*/
            if (dt.Rows[index].Cells[0].Value.ToString() == "COCO")
            {
                cborange.Text = "0";

            }
            else if(dt.Rows[index].Cells[0].Value.ToString() == "KING COCO")
            {
                cbmf.Text = "0";
            }
            else
            {
                cbpf.Text = "0";
            }
            dt.Rows.RemoveAt(index); //removes the selected row 
            


        }

        private void cnfmbtn_Click(object sender, EventArgs e) // calculating the total sum of the orders
        {
            lbltotal.Text = "0"; //Setting the total to zero
            for (int i=0; i < dt.RowCount; i++) 
            {
                lbltotal.Text = Convert.ToString(double.Parse(lbltotal.Text)+
                    double.Parse(dt.Rows[i].Cells[3].Value.ToString()));
                
            }
            total = Convert.ToInt32(lbltotal.Text);
        }

       
        private void blncbtn_Click(object sender, EventArgs e) // calculating the balance
        {
            //calculates the difference between the cash recieved and the total            
            int bal = Convert.ToInt32(crtxt.Text) - Convert.ToInt32(total);  
            bltxt.Text = Convert.ToString(bal); //assign it to the balance label
        }

        private void prcdbtn_Click(object sender, EventArgs e)
        {
            //checking whether the cash recieved is sufficient to proceed the order
            if(Convert.ToDouble(crtxt.Text)< total)// if the cash recieved is less than total
            {
                MessageBox.Show("insufficient Money"); //if not sufficient pop-up a messagebox
            }
            else //if the cash recieved is greater than total
            {
                MessageBox.Show("Your Order has confirmed");
                SqlConnection con = new SqlConnection(conString);
                con.Open();
                string q = "UPDATE [dbo].[Table1] SET [Qty] = '" + (Convert.ToInt32(AuO.Text) - 
                    Convert.ToInt32(cborange.Text)) + "' WHERE ID =1";
                string q2 = "UPDATE [dbo].[Table1] SET [Qty] = '" + (Convert.ToInt32(AuM.Text) - 
                    Convert.ToInt32(cbmf.Text)) + "' WHERE ID =2";
                string q3 = "UPDATE [dbo].[Table1] SET [Qty] = '" + (Convert.ToInt32(AuP.Text) - 
                    Convert.ToInt32(cbpf.Text)) + "' WHERE ID =3";
                SqlCommand cmd = new SqlCommand(q, con);
                SqlCommand cmd2 = new SqlCommand(q2, con);
                SqlCommand cmd3 = new SqlCommand(q3, con);
                cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                cmd3.ExecuteNonQuery();
                
                string q4 = "SELECT [Qty]FROM [dbo].[Table1] WHERE ID IN (1,2,3) ";
                SqlCommand cmd4 = new SqlCommand(q4, con);
                SqlDataReader reader;
                reader = cmd4.ExecuteReader();
                var arrlist = new ArrayList();
                while (reader.Read())
                {
                    arrlist.Add(reader["Qty"].ToString());
                }
                AuO.Text = Convert.ToString(arrlist[0]);
                AuM.Text = Convert.ToString(arrlist[1]);
                AuP.Text = Convert.ToString(arrlist[2]);
                reader.Close();
                con.Close();

                //reseting all the values
                cborange.Text = "0";
                cbmf.Text = "0";
                cbpf.Text = "0";
                int i = Convert.ToInt32(dt.Rows.Count.ToString());
                while (i != 0)
                {
                    dt.Rows.RemoveAt(i - 1);
                    i = i - 1;
                }
                crtxt.Text = "0";
                lbltotal.Text = "0";
                bltxt.Text = "0";
            }
            
            
        }
        // ADD buttons
        private void button1_Click(object sender, EventArgs e)
        {

            itemName = "COCO";
            NoOfUnits = Convert.ToInt32(cborange.Text); 
            UPrice = 90;
            OPrice = UPrice * NoOfUnits;
            table.Rows.Add(itemName, UPrice + "/=", NoOfUnits, OPrice);
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            itemName = "KING COCO";
            NoOfUnits = Convert.ToInt32(cbmf.Text);
            UPrice = 110;
            MPrice = UPrice * NoOfUnits;
            table.Rows.Add(itemName, UPrice+"/=", NoOfUnits, MPrice);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            itemName = "YOUNG COCO";
            NoOfUnits = Convert.ToInt32(cbpf.Text);
            UPrice = 100;
            PPrice = UPrice * NoOfUnits;
            table.Rows.Add(itemName, UPrice + "/=", NoOfUnits, PPrice);
        }

        private void cnclbtn_Click(object sender, EventArgs e)
        {
            cborange.Text = "0";
            cbmf.Text = "0";
            cbpf.Text = "0";
            crtxt.Text = "0";
            lbltotal.Text = "0";
            bltxt.Text = "0";
            //clearing the datagrid
            int i = Convert.ToInt32(dt.Rows.Count.ToString());
            
            while(i != 0)
            {
                dt.Rows.RemoveAt(i-1);
                i = i - 1;
            }
        }



    }
}
