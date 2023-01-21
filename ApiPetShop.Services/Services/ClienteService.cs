using ApiPetShop.Domain.Models;
using ApiPetShop.Filters.Exceptions;
using ApiPetShop.Filters.Dealings;
using ApiPetShop.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPetShop.Services.Services
{
    public class ClienteService
    {
        // injetando dependencia da solução cliente repositorio
        private readonly ClienteRepository _repository;

        // inicio o construtor da dependencia
        public ClienteService()
        {
            _repository = new ClienteRepository();
        }

        // função service para inserção de cliente, passando um cliente model
        public void Insert(ClienteModel model)
        {
            // entro com o model dentro de uma tratativa antes da execução da função
            try
            {
                // faço uma validação dos valores recebidos
                ValidationModelCliente(model, false);

                // caso valido realizo a abertura da conexão com o servidor
                _repository.OpenConnection();

                // insiro o modelo
                _repository.Insert(model);
            }
            finally
            {
                // finalizo fechando a conexão com o servidor
                _repository.CloseConnection();
            }
        }

        // função service para atualização de cliente, passando um cliente model
        public void Update(ClienteModel model)
        {
            try
            {
                ValidationModelCliente(model, true);
                _repository.OpenConnection();
                _repository.Update(model);
            }
            finally
            {
                _repository.CloseConnection();
            }

        }

        // função service para listar clientes, podendo passar ou não um nome
        public List<ClienteModel> GetAll(string? nome)
        {
            try
            {
                _repository.OpenConnection();
                return _repository.GetAll(nome);
            }
            finally
            {
                _repository.CloseConnection();
            }
        }

        // função service para deletar clientes, passando cpf
        public void Delete(string cpfCliente)
        {
            try
            {
                _repository.OpenConnection();
                _repository.Delete(cpfCliente);
            }
            finally
            {
                _repository.CloseConnection();
            }
        }

        // função service para capturar um cliente especifico, passando um cpf
        public ClienteModel GetSpecific(string cpfCliente)
        {
            try
            {
                _repository.OpenConnection();
                return _repository.GetSpecific(cpfCliente);
            }
            finally
            {
                _repository.CloseConnection();
            }
        }

        // função service para validação do modelo de dados
        private void ValidationModelCliente(ClienteModel model, bool isUpdate)
        {
            // valido se minha modelo veio vazia, caso venha retorno um erro.
            if (model is null)
                throw new ValidationException("O json está mal formatado ou foi enviado vazio, por favor tente novamente!");

            // verifico se é um cliente update, caso seja, o mesmo passará por uma validação de CPF
            if (!isUpdate)
            {
                // valido se o campo do CPF veio nulo ou com espaço em branco
                if (string.IsNullOrWhiteSpace(model.CpfCliente))
                    throw new ValidationException("O cpfCliente é obrigatório!");

                // envio para minha classe de filtros para validação do CPF
                if (!Validation.CpfValidation(model.CpfCliente))
                    throw new ValidationException("O cpfCliente é inválido!");
            }

            // valido se o campo Nome veio nulo ou com espaço em branco
            if (string.IsNullOrWhiteSpace(model.Nome))
                throw new ValidationException("O nome é obrigatório!");

            // valido se o campo do CPF veio nulo ou com espaço em branco
            if (!Validation.NameValidation(model.Nome))
                throw new ValidationException("O nome precisa ter entre 3 e 255 caracteres!");

            // valido se o campo do Nascimento
            if (Validation.GetAge(model.Nascimento) < 18)
                throw new ValidationException("Somente maiores de 18 podem realizar o cadastro!");

            // valido se o campo do Telefone veio nulo ou com espaço em branco
            if (!Validation.PhoneValidation(model.Telefone))
                throw new ValidationException($"O telefone precisa ter 11 digitos!");
        }
    }
}
