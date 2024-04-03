using System;

class Usuario
{
    public string Nome { get; set; }

    public string CPF { get; set; }

    public Usuario(string nome, string cpf)
    {
        Nome = nome;
        CPF = cpf;
    }
}

class Conta
{
    public int Numero { get; set; }
    public string Tipo { get; set; }
    public decimal Saldo { get; set; }

    public Conta(int numero, string tipo)
    {
        Numero = numero;
        Tipo = tipo;
        Saldo = 0;
    }

    public void depositar(decimal valor)
    {

        Saldo += valor;
        Console.WriteLine($"Depósito de R{valor:C} realizado com sucesso. Novo saldo: R{Saldo:C}");
        return;
    }

    public void sacar(decimal valor)
    {
        if (valor > Saldo)
        {
            Console.WriteLine("Saldo insufisiente para realizar o saque.");
            return;
        }
        else
        {

            Saldo -= valor;
            Console.WriteLine($"Depósito de R{valor:C} realizado com sucesso. Novo saldo: R{Saldo:C}");
        }

    }

    public void transferencia(decimal valor, Conta destino)
    {
        if (valor > Saldo)
        {
            Console.WriteLine("Saldo insuficiente para a transferência.");
            return;
        }

        Saldo -= valor;
        destino.depositar(valor);
        Console.WriteLine($"Depósito de R{valor:C} realizado com sucesso. Novo saldo: R{Saldo:C}");
    }
}

class AtividadeConta
{
    static void Main(string[] args)
    {
        Usuario JamesRodrigues = new Usuario("James Rodrigues", "400.289.221.23");
        Usuario LucasMoura = new Usuario("Lucas Moura", "580.239.271.28");

        Conta contaJames = new Conta(4002, "PF");
        Conta contaLucas = new Conta(8002, "PF");

        contaJames.depositar(20000);
        contaLucas.depositar(400);

        contaJames.sacar(10000);
        contaLucas.transferencia(350, contaJames);

    }
}

