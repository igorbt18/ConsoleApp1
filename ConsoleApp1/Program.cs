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

class Transacao
{
    public decimal Valor { get; }
    public DateTime Data { get; }

    public Transacao(decimal valor, DateTime data)
    {
        Valor = valor;
        Data = data;
    }
}

class Conta
{
    public int Numero { get; set; }
    public string Tipo { get; set; }
    public decimal Saldo { get; set; }
    public Usuario Dono { get; set; }
    public List<Transacao> HistoricoTransacoes { get; }

    public Conta(int numero, string tipo, Usuario dono)
    {
        Numero = numero;
        Tipo = tipo;
        Saldo = 0;
        Dono = dono;
        HistoricoTransacoes = new List<Transacao>();
    }

    public void Depositar(decimal valor)
    {
        Saldo += valor;
        RegistrarTransacao(valor);
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
        RegistrarTransacao(-valor); // O valor é negativo para representar um saque
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
        RegistrarTransacao(-valor); // O valor é negativo para representar uma transferência de saída
        destino.RegistrarTransacao(valor); // O valor é positivo para representar uma transferência de entrada na conta de destino
        Console.WriteLine($"Transferência de R${valor:C} para a conta {destino.Numero} realizada com sucesso. Novo saldo: R${Saldo:C}");
    }

    private void RegistrarTransacao(decimal valor)
    {
        HistoricoTransacoes.Add(new Transacao(valor, DateTime.Now));
    }
}

interface IContaRepository
{
    Conta AcessarContaPorCPF(string cpf);
    Conta AcessarContaPorNome(string nome);
    void GerarNovaConta(Usuario usuario);
}

class ContaRepository : IContaRepository
{
    private List<Conta> contas;

    public ContaRepository()
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

    public Conta AcessarContaPorNome(string nome)
    {
        foreach (var conta in contas)
        {
            if (conta.Dono.Nome == nome)
            {
                return conta;
            }
        }
        return null;
    }

    public void GerarNovaConta(Usuario usuario)
    {
        Random random = new Random();
        int numeroConta = random.Next(1000, 9999);
        Conta novaConta = new Conta(numeroConta, "PF", usuario);
        novaConta.Depositar(1000); // Adiciona um depósito inicial de R$ 1000 para a nova conta
        novaConta.Depositar(500); // Adiciona outro depósito de R$ 500 para a nova conta
        AdicionarConta(novaConta);
        Console.WriteLine($"Nova conta gerada para {usuario.Nome} (CPF: {usuario.CPF}). Número da conta: {novaConta.Numero}");
    }
}

class ContaController
{
    private readonly IContaRepository contaRepository;

    public ContaController(IContaRepository repository)
    {
        contaRepository = repository;
    }

    public void AtivarPorCPF(string cpf)
    {
        Conta conta = contaRepository.AcessarContaPorCPF(cpf);
        if (conta != null)
        {
            Console.WriteLine($"Conta encontrada: {conta.Tipo}, CPF: {cpf}, Nome: {conta.Dono.Nome}, Saldo: {conta.Saldo:C}");
            Console.WriteLine("Histórico de Transações:");
            foreach (var transacao in conta.HistoricoTransacoes)
            {
                Console.WriteLine($"- Data: {transacao.Data}, Valor: {transacao.Valor:C}");
            }
        }
        else
        {
            Console.WriteLine("Conta não encontrada para o CPF fornecido.");
        }
    }

    public void AtivarPorNome(string nome)
    {
        Conta conta = contaRepository.AcessarContaPorNome(nome);
        if (conta != null)
        {
            Console.WriteLine($"Conta encontrada: {conta.Tipo}, CPF: {conta.Dono.CPF}, Nome: {nome}, Saldo: {conta.Saldo:C}");
            Console.WriteLine("Histórico de Transações:");
            foreach (var transacao in conta.HistoricoTransacoes)
            {
                Console.WriteLine($"- Data: {transacao.Data}, Valor: {transacao.Valor:C}");
            }
        }
        else
        {
            Console.WriteLine("Conta não encontrada para o nome fornecido.");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        IContaRepository contaRepository = new ContaRepository();
        ContaController contaController = new ContaController(contaRepository);

        // Adicionando usuários James e Lucas
        Usuario James = new Usuario("James", "12345678901");
        Usuario Lucas = new Usuario("Lucas", "98765432101");

        // Gerando contas para os usuários
        contaRepository.GerarNovaConta(James);
        contaRepository.GerarNovaConta(Lucas);

        while (true)
        {
            Console.WriteLine("Menu de Busca de Conta:");
            Console.WriteLine("1. Buscar por CPF");
            Console.WriteLine("2. Buscar por Nome");
            Console.WriteLine("3. Limpar tela");
            Console.WriteLine("4. Sair");

            Console.Write("Escolha uma opção: ");
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    Console.Write("Digite o CPF: ");
                    string cpf = Console.ReadLine();
                    contaController.AtivarPorCPF(cpf);
                    Console.WriteLine("Pressione Enter para continuar...");
                    Console.ReadLine();
                    Console.Clear();
                    break;
                case "2":
                    Console.Write("Digite o Nome: ");
                    string nome = Console.ReadLine();
                    contaController.AtivarPorNome(nome);
                    Console.WriteLine("Pressione Enter para continuar...");
                    Console.ReadLine();
                    Console.Clear();
                    break;
                case "3":
                    Console.Clear();
                    break;
                case "4":
                    Console.WriteLine("Saindo do programa.");
                    return;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
        }
    }
}
