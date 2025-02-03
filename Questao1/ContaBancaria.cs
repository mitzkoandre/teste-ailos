using System;
using System.Globalization;

namespace Questao1
{
    class ContaBancaria
    {
        private readonly double TAXA_SAQUE = 3.50;

        public ContaBancaria(int conta, string titular)
        {
            Conta = conta;
            Titular = titular;
        }

        public ContaBancaria(int conta, string titular, double depositoAbertura)
        {
            Conta = conta;
            Titular = titular;
            Saldo = depositoAbertura;
        }

        public int Conta { get; private set; }
        public string Titular { get; private set; }
        public double Saldo { get; private set; }

        public void AtualizarTitular(string titular)
        {
            Titular = titular;
        }

        public void Saque(double quantia)
        {
            if (quantia <= 0)
                throw new ArgumentException("O valor do saque deve ser maior que 0 (zero).");

            Saldo -= quantia + TAXA_SAQUE;
        }

        public void Deposito(double quantia)
        {
            if (quantia <= 0)
                throw new ArgumentException("O valor do deposito deve ser maior que 0 (zero).");

            Saldo += quantia;
        }
    }
}
