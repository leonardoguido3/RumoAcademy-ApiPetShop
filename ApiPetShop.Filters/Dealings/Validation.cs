using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiPetShop.Filters.Dealings
{
    public class Validation
    {
        // função filtro para remover mascaramento do numero de telefone com REGEX
        public static string RemoveMaskPhone(string telefone)
        {
            return Regex.Replace(telefone, @"[^\d]", "");
        }

        // função filtro para obter a idade do cliente cadastrado e não dar erro
        public static int GetAge(DateTime nascimento)
        {
            int age = DateTime.Today.Year - nascimento.Year;

            return age;

            //var today = DateTime.Today;
            //var idade = today.Year - nascimento.Year;
            //if (nascimento > today.AddYears(-idade)) idade--;
            //return idade;
        }

        // função filtro para validação de CPF
        public static bool CpfValidation(string CpfCliente)
        {
            // recebo o cpf e faço replace, retornando o valor sem os caracteres
            string cpfValido = CpfCliente.Replace(".", "");

            cpfValido = cpfValido.Replace("-", "");

            // faço uma verificacao de quantidade de digitos e se não é um valor sequencial
            if (cpfValido == "12345678909" || cpfValido == "098765432100" || cpfValido.Length != 11)
            {
                return false;
            }
            return true;
        }

        // função filtro para validação do NOME
        public static bool NameValidation(string nome)
        {
            // Recebo o nome, removo o espaço e valido a quantidade de caracteres
            if (nome.Trim().Length >= 255 || nome.Trim().Length < 3)
            {
                return false;
            }
            return true;
        }

        // função filtro para validação de Telefone
        public static bool PhoneValidation(string phone)
        {
            if (phone is not null && (phone.Trim().Length < 11 || phone.Trim().Length > 11 || phone.Trim().Length != RemoveMaskPhone(phone).Length))
            {
                return false;
            }
            return true;
        }
    }
}
