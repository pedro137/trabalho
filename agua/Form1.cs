using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace agua
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeDataGridView();
            InitializeDataGridView2();
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

        private void SalvarDados(string nome)
        {
            string nomeArquivoNomes = "..\\..\\..\\nomes.txt";
            string nomeArquivoCelulares = "..\\..\\..\\celulares.txt";
            string nomeArquivoEmail = "..\\..\\..\\emails.txt";
            string nomeArquivoTelefones = "..\\..\\..\\telefones.txt";

            int novoId = ObterNovoId(nomeArquivoNomes);

            // Verifica se o nome já existe no arquivo de nomes
            if (File.Exists(nomeArquivoNomes))
            {
                string[] linhasNomes = File.ReadAllLines(nomeArquivoNomes);
                if (linhasNomes.Any(line => line.Split(',')[1].TrimEnd(';') == nome))
                {
                    MessageBox.Show("Nome já existe no arquivo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (File.Exists(nomeArquivoEmail))
            {
                string[] linhasEmails = File.ReadAllLines(nomeArquivoEmail);
                if (linhasEmails.Any(line => line.Split(',')[1].TrimEnd(';') == txtEmail.Text && (!txtEmail.Text.Trim().Equals(""))))
                {
                    MessageBox.Show("E-mail já existe no arquivo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Verifica se o número de telefone já existe no arquivo de celulares
            if (File.Exists(nomeArquivoCelulares))
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string telefone = Convert.ToString(row.Cells[0].Value);

                        // Verifica se o número de telefone já existe no arquivo
                        string[] linhasCelulares = File.ReadAllLines(nomeArquivoCelulares);
                        if (linhasCelulares.Any(line => line.Split(',')[1].TrimEnd(';') == telefone))
                        {
                            MessageBox.Show($"O número {telefone} já está cadastrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
            }

            // Verifica se o número de telefone já existe no arquivo de celulares
            if (File.Exists(nomeArquivoTelefones))
            {
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string telefone = Convert.ToString(row.Cells[0].Value);

                        // Verifica se o número de telefone já existe no arquivo
                        string[] linhasTelefones = File.ReadAllLines(nomeArquivoTelefones);
                        if (linhasTelefones.Any(line => line.Split(',')[1].TrimEnd(';') == telefone))
                        {
                            MessageBox.Show($"O número {telefone} já está cadastrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
            }

            // Ordena as linhas do arquivo de nomes de acordo com o nome
            List<string> linhasOrdenadas = new List<string>();
            if (File.Exists(nomeArquivoNomes))
            {
                linhasOrdenadas = File.ReadAllLines(nomeArquivoNomes).ToList();
                linhasOrdenadas.Add($"{novoId},{nome};");
                linhasOrdenadas = linhasOrdenadas.OrderBy(line => line.Split(',')[1]).ToList();
            }
            else
            {
                linhasOrdenadas.Add($"1,{nome};");
            }

            using (StreamWriter writerEmails = new StreamWriter(nomeArquivoEmail, true))
            {
                if (File.Exists(nomeArquivoEmail))
                {
                    writerEmails.WriteLine($"{novoId},{txtEmail.Text};");
                }
                else
                {
                    writerEmails.WriteLine($"1,{txtEmail.Text};");
                }
            }

            // Escreve as linhas ordenadas de volta no arquivo de nomes
            File.WriteAllLines(nomeArquivoNomes, linhasOrdenadas);

            // Se ambos os dados forem válidos, salva-os
            using (StreamWriter writerCelulares = new StreamWriter(nomeArquivoCelulares, true))
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string telefone = Convert.ToString(row.Cells[0].Value);
                        writerCelulares.WriteLine($"{novoId},{telefone};");
                    }
                }
            }

            // Se ambos os dados forem válidos, salva-os
            using (StreamWriter writerTelefones = new StreamWriter(nomeArquivoTelefones, true))
            {
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string telefone = Convert.ToString(row.Cells[0].Value);
                        writerTelefones.WriteLine($"{novoId},{telefone};");
                    }
                }
            }

            MessageBox.Show("Dados salvos com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private int ObterNovoId(string filename)
        {
            if (File.Exists(filename))
            {
                string[] lines = File.ReadAllLines(filename);
                if (lines.Length > 0)
                {
                    int maxId = lines.Max(line => int.Parse(line.Split(',')[0]));
                    return maxId + 1;
                }
            }
            return 1;
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

        private bool VerificarNumeroCelular(string Celular)
        {
            int index = Celular.IndexOf("(00)");
            if (index != -1)
            {
                return true;
            }
            return false;
        }

        private bool VerificarEmail(string Celular)
        {
            int index = Celular.IndexOf(".com");
            int index2 = Celular.IndexOf("@");
            if (index != -1&&index2!=-1)
            {
                return true;
            }
            return false;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            string novoNome = txtNome.Text.Trim();
            string novoEmail = txtEmail.Text.Trim();

            if (dataGridView1.Rows.Count <= 1 && dataGridView2.Rows.Count <= 1)
            {

                MessageBox.Show("Coloque um número de celular ou de telefone", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
           

            if (!(novoEmail.Equals(""))&& !VerificarEmail(novoEmail))
            {
                MessageBox.Show("Endereço de e-mail invalido", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        string cellValue = Convert.ToString(cell.Value);

                        if (VerificarNumeroCelular(cellValue))
                        {
                            MessageBox.Show("O número de Celular não pode conter dois zeros consecutivos entre parênteses.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }

            if (novoNome.Trim().Equals(""))
            {
                MessageBox.Show("Digite um nome");
                return;
            }

            SalvarDados(novoNome);
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {


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

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            // Verifica se a primeira linha não está selecionada

            // Remove todas as linhas, exceto a primeira


            dataGridView1.Rows.Clear(); // Remove a linha após a primeira
            dataGridView2.Rows.Clear(); // Remove a linha após a primeira
            txtEmail.Text = "";
            txtNome.Text = "";





        }
    }
}
