using System;

public class Contato
{
    string nome;
    string email;
    List<string> telefones = new List<string>();
    List<string> celulares = new List<string>();

    

    public Contato(string nome, string email, List<string> telefones, List<string> celulares)
    {

        
        this.nome = nome;
        this.email = email;
        this.telefones = telefones;
        this.celulares = celulares;
    }
}
