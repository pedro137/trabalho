using System;
using System.IO;
using System.Windows.Forms;

namespace agua
{
    public partial class Inicio : Form
    {
        public Inicio()
        {
            InitializeComponent();
            AdicionarNomes1();
        }

        private void AdicionarNomes1()
        {
            string nomeArquivoNomes = "..\\..\\..\\nomes.txt";

            if (File.Exists(nomeArquivoNomes))
            {
                string[] linhas = File.ReadAllLines(nomeArquivoNomes);

                foreach (string linha in linhas)
                {
                    string[] partes = linha.Split(',');
                    if (partes.Length >= 2)
                    {
                        string nome = partes[1].Trim(';');
                        AdicionarNomeAoListBox(nome);
                    }
                }
            }
        }

        private void AdicionarNomeAoListBox(string nome)
        {
            if (!string.IsNullOrEmpty(nome))
            {
                char primeiraLetra = nome[0];

                // Se a letra atual for diferente da primeira letra do nome, adiciona um separador
                if (listBox1.Items.Count == 0 || !listBox1.Items[listBox1.Items.Count - 1].ToString().StartsWith("---") ||
                    char.ToUpper(listBox1.Items[listBox1.Items.Count - 1].ToString()[4]) != char.ToUpper(primeiraLetra))
                {
                    listBox1.Items.Add($"--- {char.ToUpper(primeiraLetra)} ---");
                }

                // Adiciona o nome ao ListBox
                listBox1.Items.Add(nome);
            }
        }


        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new MostrarContato().Show();
        }

        private void Inicio_Load(object sender, EventArgs e)
        {

        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            new Form1().Show();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
