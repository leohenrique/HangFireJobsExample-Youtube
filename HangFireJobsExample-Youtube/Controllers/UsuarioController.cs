using HangFireJobsExample_Youtube.Domain;
using HangFireJobsExample_Youtube.Repositorio_NH;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.Graylog.Core.Transport;



namespace HangFireJobsExample_Youtube.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : ControllerBase
    {

        private readonly UsuarioRepository _usuarioRepository;
        private readonly ContatoRepository _contatoRepository;
        private readonly IMemoryCache _cache;

        public UsuarioController(
            UsuarioRepository usuarioRepo,
            ContatoRepository contatoRepo,
            IMemoryCache cache
            )
        {
            _usuarioRepository = usuarioRepo;
            _contatoRepository = contatoRepo;
            _cache = cache;

        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar(Usuario dto)
        {

            var log = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Graylog(new GraylogSinkOptions
                {
                    HostnameOrAddress = "127.0.0.1",
                    Port = 12201,
                    TransportType = TransportType.Udp
                })
                .CreateLogger();

            log.Information("Invocado o método Registrar. Objeto:", dto);



            try
            {
                var usuario = new Usuario
                {
                    //Id = Guid.NewGuid(),
                    Nome = dto.Nome,
                    Email = dto.Email,
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.SenhaHash),
                    
                };

                await _usuarioRepository.Salvar(usuario); /// Salva o usuário no banco de dados

                var contatos = dto.Contatos;
                if (contatos != null && contatos.Count > 0)
                {
                    foreach (var contato in contatos)
                    {
                        Contato contatoNew = new Contato {
                            Nome = contato.Nome, 
                            Telefone = contato.Telefone, 
                            Usuario = usuario 
                        };
                        //contato.Usuario = usuario; // Estabelece a relação
                        await _contatoRepository.Salvar(contatoNew); // Salva o contato
                    }
                }
                               

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });

            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(long id)
        {
            try
            {
                await _usuarioRepository.Remove(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var usuariosCached = _cache.GetOrCreate("ListaUsuarios2", entrada =>
                {
                    var usuarios = _usuarioRepository.ListarTodosAsync();
                    return usuarios;
                });

                return Ok(usuariosCached);                
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("invalidar-cache")]
        public IActionResult InvalidarCache()
        {
            try
            {
                _cache.Remove("ListaUsuarios2");
                return Ok(new { message = "Cache invalidado com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
