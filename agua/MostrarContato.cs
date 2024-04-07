using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace agua
{
    public partial class MostrarContato : Form
    {
        public MostrarContato()
        {
            InitializeComponent();
            txtNome.Enabled = false;
            txtEmail.Enabled = false;
            dataGridView1.Enabled = false;
            dataGridView2.Enabled = false;


            dataGridView1.Columns.Add("ID", "ID");
            dataGridView1.Columns.Add("Nome", "Nome");

            // Adicionar algumas linhas de exemplo
            dataGridView1.Rows.Add(1, "Pedro");
            dataGridView1.Rows.Add(2, "Bob");
            dataGridView1.Rows.Add(3, "Charlie");



            dataGridView2.Columns.Add("ID", "ID");
            dataGridView2.Columns.Add("Nome", "Nome");

            // Adicionar algumas linhas de exemplo
            dataGridView2.Rows.Add(1, "agua");
            dataGridView2.Rows.Add(2, "x");
            dataGridView2.Rows.Add(3, "1");
        }

        public MostrarContato(Contato contato)
        {
            InitializeComponent();
            txtNome.Enabled = false;
            txtEmail.Enabled = false;
            dataGridView1.Enabled = false;
            dataGridView2.Enabled = false;


        }

        private void MostrarContato_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            btnEditar.Text = txtNome.Enabled ? "Editar contato" : "Cancelar edição";

            txtNome.Enabled =  !txtNome.Enabled;
            txtEmail.Enabled = !txtEmail.Enabled;
            dataGridView1.Enabled = !dataGridView1.Enabled;
            dataGridView2.Enabled = !dataGridView2.Enabled;
        }
    }
}
