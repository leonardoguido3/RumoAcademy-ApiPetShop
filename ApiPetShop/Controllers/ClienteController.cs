using ApiPetShop.Domain.Models;
using ApiPetShop.Filters.Exceptions;
using ApiPetShop.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiPetShop.Controllers
{
    [ApiController]
    public class ClienteController : ControllerBase
    {
        // injeção da dependencia de serviços
        private readonly ClienteService _service;

        // construtor
        public ClienteController()
        {
            _service = new ClienteService();
        }
        // metodo para inserir dados
        [HttpPost("Cliente")]
        public IActionResult Insert([FromBody] ClienteModel model)
        {
            try
            {
                _service.Insert(model);
                // retornando sucesso
                return StatusCode(201);
            }
            catch (ValidationException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        // metodo para atualizar dados
        [HttpPut("Cliente")]
        public IActionResult Update([FromBody] ClienteModel model)
        {
            try
            {
                _service.Update(model);
                // retornando sucesso
                return StatusCode(201);
            }
            catch (ValidationException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        // metodo para buscar todos os dados
        [HttpGet("Cliente")]
        public IActionResult GetAll([FromQuery] string? nome)
        {
            // retornando sucesso
            return StatusCode(200, _service.GetAll(nome));
        }

        // metodo para deletar dados
        [HttpDelete("Cliente/{CpfCliente}")]
        public IActionResult Delete([FromRoute] string CpfCliente)
        {
            _service.Delete(CpfCliente);
            // retornando sucesso
            return StatusCode(200);
        }

        // metodo para obter um cliente
        [HttpGet("Cliente/{CpfCliente}")]
        public IActionResult GetSpecific([FromRoute] string CpfCliente)
        {
            // retornando sucesso
            return StatusCode(200, _service.GetSpecific(CpfCliente));
        }
    }
}
