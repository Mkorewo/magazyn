using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;
using System.Diagnostics;

namespace magazyn
{
    public partial class Form1 : Form
    {
        SqLiteMenager menager=new SqLiteMenager();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            menager.CreateDB();
            dataGridView.ColumnCount = 6;
            dataGridView.Columns[0].Name = "id";
            dataGridView.Columns[1].Name = "name";
            dataGridView.Columns[2].Name = "description";
            dataGridView.Columns[3].Name = "category";
            dataGridView.Columns[4].Name = "amount";
            dataGridView.Columns[5].Name = "price";
        }

        private void tbAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void tbPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            //TODO sprawdzanie czy . jest na koncu lub poczatku

            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
            if (char.IsNumber(e.KeyChar) || e.KeyChar == '.')
            {
                if (Regex.IsMatch(
                 tbPrice.Text,
                 "^\\d*\\.\\d{2}$")) e.Handled = true;
            }
            else e.Handled = e.KeyChar != (char)Keys.Back;
        }

        private void add_Click(object sender, EventArgs e)
        {

            //TODO sprawdzanie czy textboxy nie sa puste
            /*
            string name = tbName.Text;
            string description = tbDescription.Text;
            string category = tbCategory.Text;
            int amount = Convert.ToInt32(tbAmount.Text);
            double price = Convert.ToDouble(tbPrice.Text);
            */
            ///*
            string name = "test";
            string description = "desc";
            string category = "cate";
            int amount = 100;
            double price = 1.50;
            //*/
            menager.WriteDB(name, description, category, amount, price);





            
            
        }

        private void show_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
            var dr = menager.ReadDB();

            if (inputCategory.Text.Length!=0)
            {
                dr = menager.ReadDB(inputCategory.Text);
            }

            while (dr.Read())
            {
                dataGridView.Rows.Insert(0,dr.GetInt16(0), dr.GetString(1), dr.GetString(2),dr.GetString(3),dr.GetInt16(4),dr.GetDouble(5));
            }
        }
    }
}
    