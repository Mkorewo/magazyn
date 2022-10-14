using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;
using System.Diagnostics;
using System.Drawing;

namespace magazyn
{
    public partial class Form1 : Form
    {
        SqLiteMenager menager=new SqLiteMenager();
        string idCellValue;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)//utworzenie tabeli i wczytanie ich do dataGridView podczas uruchomienia aplikacji
        {
            menager.CreateDB();
            dataGridView.ColumnCount = 6;
            dataGridView.Columns[0].Name = "id";
            dataGridView.Columns[1].Name = "name";
            dataGridView.Columns[2].Name = "description";
            dataGridView.Columns[3].Name = "category";
            dataGridView.Columns[4].Name = "amount";
            dataGridView.Columns[5].Name = "price";
            dataGridView.Columns[2].Width = 327;
            dataGridView.RowTemplate.Height = 40;
            
            read();
        }

        private void tbAmount_KeyPress(object sender, KeyPressEventArgs e) //sprawdanie czy do pola tbAmount wpisywane sa tylko liczby calkowite
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void tbPrice_KeyPress(object sender, KeyPressEventArgs e) //sprawdanie czy do pola tbPrice wpisywane sa tylko liczby calkowite lub z przecinkiem
        {

            if (tbPrice.Text.Length == 8 && e.KeyChar == ',' || tbPrice.Text.Length==0&& e.KeyChar == ',')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
            if (char.IsNumber(e.KeyChar) || e.KeyChar == ',')
            {
                if (Regex.IsMatch(
                 tbPrice.Text,
                 "^\\d*\\,\\d{2}$")) e.Handled = true;
            }
            else e.Handled = e.KeyChar != (char)Keys.Back;
        }

        private void add_Click(object sender, EventArgs e) //dodawanie rekordow do tabeli o podanych wartosciach
        {

            if (tbName.Text==string.Empty || tbDescription.Text == string.Empty || tbCategory.Text == string.Empty || tbAmount.Text == string.Empty || tbPrice.Text == string.Empty)
            {
                MessageBox.Show("Podaj wszystkie wymagane pola");
            }
            else 
            {
                string name = tbName.Text;
                string description = tbDescription.Text;
                string category = tbCategory.Text;
                int amount = Convert.ToInt32(tbAmount.Text);
                double price = Convert.ToDouble(tbPrice.Text);

                menager.WriteDB(name, description, category, amount, price);
            }
            read();
            clear();

        }

        private void show_Click(object sender, EventArgs e) //wyswietlenie rekordow tabeli
        {
            read();
        }

        private void edit_Click(object sender, EventArgs e) //edytacja danego rekordu o podane wartosci
        {
            
            if (tbName.Text == string.Empty || tbDescription.Text == string.Empty || tbCategory.Text == string.Empty || tbAmount.Text == string.Empty || tbPrice.Text == string.Empty)
            {
                MessageBox.Show("Podaj wszystkie wymagane pola");
            }
            else
            {
                string name = tbName.Text;
                string description = tbDescription.Text;
                string category = tbCategory.Text;
                int amount = Convert.ToInt32(tbAmount.Text);
                double price = Convert.ToDouble(tbPrice.Text);

                menager.EditFromDB(name, description, category, amount, price,idCellValue);
                read();
                clear();

            }

        }

        private void delete_Click(object sender, EventArgs e) //usuniecie danego rekordu
        {            
            menager.DeleteFromDB(idCellValue);

            read();

        }
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e) //wpisanie wartosci z wybranego rzedu do textboxow
        {
            dataGridView.CurrentRow.Selected = true;
            try
            {
                tbName.Text = dataGridView.Rows[e.RowIndex].Cells["Name"].FormattedValue.ToString();
                tbDescription.Text = dataGridView.Rows[e.RowIndex].Cells["Description"].FormattedValue.ToString();
                tbCategory.Text = dataGridView.Rows[e.RowIndex].Cells["Category"].FormattedValue.ToString();
                tbAmount.Text = dataGridView.Rows[e.RowIndex].Cells["Amount"].FormattedValue.ToString();
                tbPrice.Text = dataGridView.Rows[e.RowIndex].Cells["Price"].FormattedValue.ToString();
            }
            catch
            {

            }
        }

        public void read() // funkcja do wprowadzenie dancy z bazy do dataGridView
        {
            dataGridView.Rows.Clear();
            var dr = menager.ReadDB();

            if (inputCategory.Text.Length != 0)
            {
                dr = menager.ReadDB(inputCategory.Text);
            }

            while (dr.Read())
            {
                dataGridView.Rows.Insert(0, dr.GetInt16(0), dr.GetString(1), dr.GetString(2), dr.GetString(3), dr.GetInt16(4), dr.GetDouble(5));
            }
        }
        public void clear() // funkcja do czyszczenia textBoxow z podanych wartosci
        {
            tbName.Clear();
            tbDescription.Clear();
            tbCategory.Clear();
            tbAmount.Clear();
            tbPrice.Clear();
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e) //funkcja do pobrania wartosci id wybranego rekordu
        {
            if (dataGridView.SelectedCells.Count > 0)
            {
                int selectedrowindex = dataGridView.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView.Rows[selectedrowindex];
                idCellValue = Convert.ToString(selectedRow.Cells["id"].Value);
                
            }
        }
    }
}
    
