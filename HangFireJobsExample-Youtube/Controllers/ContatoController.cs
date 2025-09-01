using HangFireJobsExample_Youtube.Domain;
using HangFireJobsExample_Youtube.Repositorio_NH;
using Microsoft.AspNetCore.Mvc;

namespace HangFireJobsExample_Youtube.Controllers
{
    [ApiController]
    [Route("api/contatos")]
    public class ContatoController : ControllerBase
    {

        private readonly ContatoRepository _contatoRepository;

        public ContatoController(ContatoRepository contatoRepository)
        {
            _contatoRepository = contatoRepository;         
        }

        [HttpGet]
        public IActionResult Get()
        {
            return new OkObjectResult("ContatoController is working!");
        }

        [HttpGet]
        [Route("buscar-lista-ids")]
        public async Task<IActionResult> BuscarListaIds()
        {
            var ids = await _contatoRepository.BuscarListaIds();
            
            return Ok(ids);
            
        }

        [HttpGet]
        [Route("buscar-detalhado")]
        public async Task<IActionResult> BuscarDetalhado()
        {
            var ids = await _contatoRepository.BuscarListaIds();
            List<ContatoInput> listaContatos = [];

            for (var i =0; i < ids.Count; i++)
            {
                var contato = await _contatoRepository.BuscarDetalhePorId(ids[i]);
                listaContatos.Add(contato);
                Console.WriteLine(contato);

            }
            return Ok(listaContatos);
        }

        [HttpGet]
        [Route("buscar-agrupado-nome")]
        public async Task<IActionResult> BuscarAgrupadoNome()
        {
            var ids = await _contatoRepository.BuscarAgrupadoPorNomeContato();

            return Ok(ids);
        }
    }
}
