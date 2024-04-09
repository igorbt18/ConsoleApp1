using System;
using System.Collections.Generic;

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
    public Usuario Dono { get; set; } // Referência ao objeto Usuario

    public Conta(int numero, string tipo, Usuario dono)
    {
        Numero = numero;
        Tipo = tipo;
        Saldo = 0;
        Dono = dono;
    }

    public void Depositar(decimal valor)
    {
        Saldo += valor;
        Console.WriteLine($"Depósito de R${valor:C} realizado com sucesso. Novo saldo: R${Saldo:C}");
    }

    public void Sacar(decimal valor)
    {
        if (valor > Saldo)
        {
            Console.WriteLine("Saldo insuficiente para realizar o saque.");
            return;
        }

        Saldo -= valor;
        Console.WriteLine($"Saque de R${valor:C} realizado com sucesso. Novo saldo: R${Saldo:C}");
    }

    public void Transferencia(decimal valor, Conta destino)
    {
        if (valor > Saldo)
        {
            Console.WriteLine("Saldo insuficiente para a transferência.");
            return;
        }

        Saldo -= valor;
        destino.Depositar(valor);
        Console.WriteLine($"Transferência de R${valor:C} para a conta {destino.Numero} realizada com sucesso. Novo saldo: R${Saldo:C}");
    }
}

class Banco
{
    private List<Conta> contas;

    public Banco()
    {
        contas = new List<Conta>();
    }

    public void AdicionarConta(Conta conta)
    {
        contas.Add(conta);
    }

    public Conta AcessarContaPorCPF(string cpf)
    {
        foreach (var conta in contas)
        {
            if (conta.Dono.CPF == cpf)
            {
                return conta;
            }
        }
        return null;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Banco banco = new Banco();

        Usuario JamesRodrigues = new Usuario("James Rodrigues", "400.289.221.23");
        Usuario LucasMoura = new Usuario("Lucas Moura", "580.239.271.28");

        Conta contaJames = new Conta(4002, "PF", JamesRodrigues);
        contaJames.Depositar(20000);

        Conta contaLucas = new Conta(8002, "PF", LucasMoura);
        contaLucas.Depositar(400);

        banco.AdicionarConta(contaJames);
        banco.AdicionarConta(contaLucas);

        // Função para acessar a conta por CPF
        Console.Write("Digite o CPF para acessar a conta: ");
        string cpf = Console.ReadLine();
        Conta conta = banco.AcessarContaPorCPF(cpf);

        if (conta != null)
        {
            Console.WriteLine($"Conta encontrada: {conta.Tipo}, CPF: {cpf}, Nome: {conta.Dono.Nome}, Saldo: {conta.Saldo:C}");
        }
        else
        {
            Console.WriteLine("Conta não encontrada.");
        }

        Console.ReadLine(); // Mantém o terminal aberto
    }
}

