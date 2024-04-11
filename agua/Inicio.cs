using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace agua
{
    public partial class Inicio : Form
    {
        private List<Contato> contatos; // Lista de contatos acessível globalmente

        public Inicio()
        {
            InitializeComponent();
            CarregarContatosDoJson(); // Método para carregar contatos do JSON
            CarregaLista();
        }

        public void CarregarContatosDoJson()
        {
            string filePath = "..\\..\\..\\contatos.json";

            // Verifica se o arquivo existe
            if (File.Exists(filePath))
            {
                // Lê todo o conteúdo do arquivo
                string json = File.ReadAllText(filePath);

                // Desserializa o JSON em uma lista de objetos Contato
                contatos = JsonConvert.DeserializeObject<List<Contato>>(json);
            }
            else
            {
                MessageBox.Show("O arquivo de contatos não foi encontrado!");
            }
        }

        public void CarregaLista()
        {
            // Verifica se a lista de contatos foi carregada
            if (contatos == null || contatos.Count == 0)
            {
                MessageBox.Show("Não há contatos para carregar!");
                return;
            }

            // Agrupa os nomes por letra maiúscula
            var grupos = contatos
                .Select(c => c.Nome)
                .OrderBy(nome => nome)
                .GroupBy(nome => char.ToUpper(nome[0]));



            // Adiciona os grupos ao ListBox
            foreach (var grupo in grupos)
            {
                listBox1.Items.Add($"-- {grupo.Key} --"); // Adiciona a letra maiúscula como cabeçalho

                // Adiciona os nomes ordenados ao grupo
                foreach (var nome in grupo.OrderBy(nome => nome))
                {
                    listBox1.Items.Add(nome);

                }
            }


        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                // Obtém o nome selecionado na ListBox
                string nomeSelecionado = listBox1.SelectedItem.ToString();

                // Procura na lista de contatos pelo nome correspondente
                Contato contato = contatos.Find(c => c.Nome == nomeSelecionado);

                // Se o contato for encontrado, exibe seus dados em um MessageBox
                if (contato != null)
                {

                    new MostrarContato(contato,this).Show();

                }
                else
                {
                    MessageBox.Show("Contato não encontrado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new MostrarContato().Show();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            new Form1(this).Show();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void AtualizarContatos()
        {
            listBox1.Items.Clear();
            CarregarContatosDoJson(); // Atualiza a lista de contatos
            CarregaLista(); // Atualiza a ListBox com os novos contatos
        }
    }
}
