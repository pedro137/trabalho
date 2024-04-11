﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace agua
{
    public partial class MostrarContato : Form
    {

        private List<Contato> listaDeContatos;
        private Contato contatoAux = new Contato();
        private Inicio inicioAux;

        public MostrarContato(Contato contato, Inicio inicio)
        {
            InitializeComponent();
            InitializeDataGridView();
            InitializeDataGridView2();
            inicioAux = inicio;
            contatoAux = contato;

            listaDeContatos = CarregarContatos();

            txtNome.Text = contato.Nome;
            txtEmail.Text = contato.Email != null ? contato.Email : "";


            foreach (var telefone in contato.Celulares)
            {
                dataGridView1.Rows.Add(telefone);
            }
            // Popula o dataGridView2 com os números de celular do contato
            foreach (var celular in contato.Telefones)
            {
                dataGridView2.Rows.Add(celular);
            }

            txtNome.Enabled = false;
            txtEmail.Enabled = false;
            dataGridView1.Enabled = false;
            dataGridView2.Enabled = false;
        }

        public MostrarContato()
        {
            InitializeComponent();
            InitializeDataGridView();
            InitializeDataGridView2();

            listaDeContatos = CarregarContatos();
        }

        private void InitializeDataGridView2()
        {
            dataGridView2.ColumnCount = 1;
            dataGridView2.Columns[0].Name = "Número de Telefone";


            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.EditingControlShowing += DataGridView2_EditingControlShowing;

            DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn();

            deleteButtonColumn.HeaderText = "Excluir";
            deleteButtonColumn.Text = "Excluir";
            deleteButtonColumn.UseColumnTextForButtonValue = true;
            dataGridView2.Columns.Add(deleteButtonColumn);

            dataGridView2.CellContentClick += DataGridView2_CellContentClick;
        }

        private void DataGridView2_EditingControlShowing(object? sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox textBox = e.Control as TextBox;
            if (textBox != null)
            {
                textBox.TextChanged += TextBox2_TextChanged;
            }
        }

        private void TextBox2_TextChanged(object? sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                string Celular = textBox.Text;

                if (Celular.Length >= 18)
                {
                    return;
                }

                string numeroLimpo = new string(Array.FindAll(Celular.ToCharArray(), Char.IsDigit));

                if (!numeroLimpo.StartsWith("55"))
                {
                    numeroLimpo = "55" + numeroLimpo;
                }

                if (numeroLimpo.Length > 2)
                {
                    string codigoPais = numeroLimpo.Substring(0, 2);
                    string restoNumero = numeroLimpo.Substring(2);

                    string numeroFormatado = string.Format("{0:(00)0000-0000}", Convert.ToInt64(restoNumero));
                    if (numeroFormatado.Length <= 13)
                    {
                        textBox.Text = codigoPais + numeroFormatado;
                    }
                    else
                    {
                        textBox.Text = codigoPais + numeroFormatado.Remove(numeroFormatado.Length - 1);
                    }
                }
                else
                {
                    textBox.Text = numeroLimpo;
                }

                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private void SalvarDados()
        {
            string nome = txtNome.Text.Trim();
            // Se ambos os dados forem válidos, salva-os
            List<string> listCelular = new List<string>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    string celular = Convert.ToString(row.Cells[0].Value);
                    listCelular.Add(celular);
                }
            }

            List<string> listTelefone = new List<string>();
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (!row.IsNewRow)
                {
                    string telefone = Convert.ToString(row.Cells[0].Value);
                    listTelefone.Add(telefone);
                }
            }

            string emailx = txtEmail.Text.Trim().Equals("") ? null : txtEmail.Text;
            Contato c = new Contato(nome, emailx, listTelefone, listCelular);

            // Adiciona o novo contato à lista de contatos
            listaDeContatos.Add(c);

            // Serializa a lista atualizada de contatos de volta para JSON
            string path = "..\\..\\..\\contatos.json";
            string json = JsonConvert.SerializeObject(listaDeContatos, Formatting.Indented);

            // Salva o JSON no arquivo
            File.WriteAllText(path, json);

            MessageBox.Show("Dados salvos com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }




        private void InitializeDataGridView()
        {
            dataGridView1.ColumnCount = 1;
            dataGridView1.Columns[0].Name = "Número de Celular";


            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.EditingControlShowing += DataGridView1_EditingControlShowing;

            DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn();

            deleteButtonColumn.HeaderText = "Excluir";
            deleteButtonColumn.Text = "Excluir";
            deleteButtonColumn.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(deleteButtonColumn);

            dataGridView1.CellContentClick += DataGridView1_CellContentClick;
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn();

            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
        dataGridView1.Columns[e.ColumnIndex].HeaderText == "Excluir" && !(e.RowIndex == dataGridView1.RowCount - 1))
            {
                dataGridView1.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void DataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox textBox = e.Control as TextBox;
            if (textBox != null)
            {
                textBox.TextChanged += TextBox_TextChanged;
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                string Celular = textBox.Text;

                if (Celular.Length >= 18)
                {
                    return;
                }

                string numeroLimpo = new string(Array.FindAll(Celular.ToCharArray(), Char.IsDigit));

                if (!numeroLimpo.StartsWith("55"))
                {
                    numeroLimpo = "55" + numeroLimpo;
                }

                if (numeroLimpo.Length > 2)
                {
                    string codigoPais = numeroLimpo.Substring(0, 2);
                    string restoNumero = numeroLimpo.Substring(2);

                    string numeroFormatado = string.Format("{0:(00)00000-0000}", Convert.ToInt64(restoNumero));
                    if (numeroFormatado.Length <= 14)
                    {
                        textBox.Text = codigoPais + numeroFormatado;
                    }
                    else
                    {
                        textBox.Text = codigoPais + numeroFormatado.Remove(numeroFormatado.Length - 1);
                    }
                }
                else
                {
                    textBox.Text = numeroLimpo;
                }

                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private bool VerificarNumeroCelular()
        {
            bool valido = true;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {


                if (!row.IsNewRow)
                {
                    string celular = Convert.ToString(row.Cells[0].Value);
                    if (celular.IndexOf("(00)") != -1)
                    {

                        valido = false;
                        MessageBox.Show($"{celular} não é um número válido.");
                        break;

                    }
                }
            }

            return valido;
        }

        private bool VerificarNumeroTelefone()
        {

            bool valido = true;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {


                if (!row.IsNewRow)
                {
                    string telefone = Convert.ToString(row.Cells[0].Value);
                    if (telefone.IndexOf("(00)") != -1)
                    {

                        valido = false;
                        MessageBox.Show($"{telefone} não é um número válido.");
                        break;
                    }
                }
            }

            return valido;
        }
        private bool VerificarEmail(string Celular)
        {
            int index = Celular.IndexOf(".com");
            int index2 = Celular.IndexOf("@");
            int index3 = Celular.IndexOf(" ");
            int index4 = Celular.IndexOf(" ");
            if (index != -1 && index2 != -1 && index3 == -1 && index4 == -1)
            {
                return true;
            }
            return false;
        }

        private bool ValidaNome(string nome)
        {
            bool valido = true;


            if (nome.Contains(" ") || nome.Trim().Equals(""))
            {
                valido = false;
            }
            else if (!char.IsLetter(nome[0]))
            {
                valido = false;
            }

            return valido;
        }



        private void DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn();

            if (e.RowIndex >= 0 && dataGridView2.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
        dataGridView2.Columns[e.ColumnIndex].HeaderText == "Excluir" && !(e.RowIndex == dataGridView2.RowCount - 1))
            {
                dataGridView2.Rows.RemoveAt(e.RowIndex);
            }
        }




        private bool NomeJaExiste(List<Contato> contatos, string nome)
        {

            return contatos.Any(c => c.Nome != null && c.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
        }


        private bool EmailJaExiste(List<Contato> contatos, string email)
        {
            return contatos.Any(c => c.Email != null && c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        private bool TelefoneJaExiste(string telefone)
        {
            return listaDeContatos.Any(c => c.Telefones.Contains(telefone));
        }

        private bool CelularJaExiste(string celular)
        {
            return listaDeContatos.Any(c => c.Celulares.Contains(celular));
        }



        private List<Contato> CarregarContatos()
        {
            string path = "..\\..\\..\\contatos.json";
            List<Contato> contatos = new List<Contato>();

            // Verifica se o arquivo existe
            if (File.Exists(path))
            {
                // Lê o conteúdo do arquivo
                string json = File.ReadAllText(path);

                // Desserializa o JSON para obter a lista de contatos
                contatos = JsonConvert.DeserializeObject<List<Contato>>(json);
            }

            return contatos;
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSalvar_Click_1(object sender, EventArgs e)
        {
            string novoNome = txtNome.Text.Trim();
            string novoEmail = txtEmail.Text.Trim();
            bool nomeValido = true;

            int quantCelulares = dataGridView1.RowCount - 1;
            int quantTelefones = dataGridView2.RowCount - 1;


            if (!ValidaNome(novoNome))
            {
                MessageBox.Show("Digite um nome válido");
                return;
            }



            if (quantTelefones < 1 && quantCelulares < 1)
            {
                MessageBox.Show("Digite pelo menos 1 celular ou 1 telefone", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (!VerificarNumeroCelular())
            {

                return;
            }
            if (!VerificarNumeroTelefone())
            {

                return;
            }



            if (NomeJaExiste(listaDeContatos, novoNome))
            {
                if (contatoAux.Nome != novoNome)
                {
                    MessageBox.Show("Já existe um contato com esse nome.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(novoEmail) && EmailJaExiste(listaDeContatos, novoEmail))
            {
                if (contatoAux.Nome != novoNome)
                {
                    MessageBox.Show("Já existe um contato com esse email.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {

                    string celular = Convert.ToString(row.Cells[0].Value);
                    if (!contatoAux.Celulares.Contains(celular))
                    {
                        if (CelularJaExiste(celular))
                        {
                            MessageBox.Show("Já existe um contato com esse celular.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }



            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (!row.IsNewRow)
                {

                    string telefone = Convert.ToString(row.Cells[0].Value);
                    if (!contatoAux.Telefones.Contains(telefone))
                    {
                        if (TelefoneJaExiste(telefone))
                        {
                            MessageBox.Show("Já existe um contato com esse telefone.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }

            // Se não houver duplicatas, você pode salvar os dados


            Regex regex = new Regex(@"^[A-Za-z0-9_@.-]+$");

            char caractere = '@';

            int count = novoEmail.Count(c => c == caractere);



            if ((!regex.IsMatch(novoEmail) || count > 1 || !
                VerificarEmail(novoEmail)) && !txtEmail.Text.Trim().Equals(""))
            {
                MessageBox.Show("Digite um e-mail valido agua");
                return;
            }




            ApagarContatoPorNome(contatoAux.Nome);
            //arrumar isso aqui
            SalvarDados();

            inicioAux.AtualizarContatos();











        }

        private void btnEditar_Click(object sender, EventArgs e)
        {


            txtNome.Enabled = !txtNome.Enabled;
            txtEmail.Enabled = !txtEmail.Enabled;
            dataGridView1.Enabled = !dataGridView1.Enabled;
            dataGridView2.Enabled = !dataGridView2.Enabled;
            btnEditar.Text = txtNome.Enabled ? "Cancelar edição" : "Editar dados";
        }


        private void ApagarContatoPorNome(string nome)
        {
            // Carrega os contatos do arquivo JSON


            // Remove o contato com o nome especificado
            listaDeContatos.RemoveAll(c => c.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));




            // Serializa a lista atualizada de contatos de volta para JSON
            string path = "..\\..\\..\\contatos.json";
            string json = JsonConvert.SerializeObject(listaDeContatos, Formatting.Indented);


            // Salva o JSON no arquivo
            File.WriteAllText(path, json);


        }

        private void btnApagar_Click(object sender, EventArgs e)
        {
            ApagarContatoPorNome(contatoAux.Nome);
            inicioAux.AtualizarContatos();
            MessageBox.Show($"{contatoAux.Nome} foi deletado com sucesso");
            this.Close();
        }
    }

}