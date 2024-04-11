using Newtonsoft.Json;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

public class Contato
{

    [JsonProperty("nome")]
    public string Nome { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("telefones")]
    public List<string> Telefones { get; set; }

    [JsonProperty("celulares")]
    public List<string> Celulares { get; set; }

    public Contato(string nome, string email, List<string> telefones, List<string> celulares)
    {
        Nome = nome;
        Email = email;
        Telefones = telefones;
        Celulares = celulares;
    }

    public Contato()
    {

    }

}
