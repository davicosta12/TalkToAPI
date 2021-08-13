using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalkToApi.Helpers.Constants;
using TalkToApi.V1.Models;
using TalkToApi.V1.Models.DTO;
using TalkToApi.V1.Repositories.Contracts;

namespace TalkToApi.V1.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiExplorerSettings(GroupName = "v1.0")]
    [ApiVersion("1.0")]
    [EnableCors] // Default Policy
    [ApiController]
    public class MensagemController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMensagemRepository _mensagemRepository;

        public MensagemController(IMensagemRepository mensagemRepository, IMapper mapper)
        {
            _mensagemRepository = mensagemRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("{usuarioUmId}/{usuarioDoisId}", Name = "MensagemObter")]
        public IActionResult ObterMensagem(string usuarioUmId, string usuarioDoisId, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (usuarioUmId == usuarioDoisId)
            {
                return UnprocessableEntity();
            }

            var mensagens = _mensagemRepository.ObterMensagens(usuarioUmId, usuarioDoisId);

            if (mediaType == CustomMediaType.Hateoas)
            {
                var listaMsg = _mapper.Map<List<Mensagem>, List<MensagemDTO>>(mensagens);

                var lista = new ListaDTO<MensagemDTO>() { Lista = listaMsg };
                lista.Links.Add(new LinkDTO("_self", Url.Link("MensagemObter", new { usuarioUmId = usuarioUmId, usuarioDoisId = usuarioDoisId }), "GET"));

                return Ok(lista);
            }
            else
            {
                return Ok(mensagens);
            }


        }

        [Authorize]
        [HttpPost("", Name = "MensagemCadastrar")]
        public IActionResult Cadastrar([FromBody] Mensagem mensagem, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _mensagemRepository.Cadastrar(mensagem);

                    if (mediaType == CustomMediaType.Hateoas)
                    {
                        var mensagemDTO = _mapper.Map<Mensagem, MensagemDTO>(mensagem);
                        mensagemDTO.Links.Add(new LinkDTO("_self", Url.Link("MensagemCadastrar", null), "POST"));
                        mensagemDTO.Links.Add(new LinkDTO("_atualizarParcial", Url.Link("MensagemAtualizacaoParcial", new { id = mensagem.Id }), "PATCH"));

                        return Ok(mensagemDTO);
                    }
                    else
                    {
                        return Ok(mensagem);
                    }


                }
                catch (Exception e)
                {
                    return UnprocessableEntity(e);
                }

            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }

        [Authorize]
        [HttpPatch("{id}", Name = "MensagemAtualizacaoParcial")]
        public IActionResult AtualizacaoParcial(int id, [FromBody] JsonPatchDocument<Mensagem> jsonPatch, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (jsonPatch == null)
            {
                return BadRequest();
            }

            var mensagem = _mensagemRepository.Obter(id);

            jsonPatch.ApplyTo(mensagem);
            mensagem.Atualizado = DateTime.UtcNow;
            _mensagemRepository.Atualizar(mensagem);

            if (mediaType == CustomMediaType.Hateoas)
            {
                var mensagemDTO = _mapper.Map<Mensagem, MensagemDTO>(mensagem);
                mensagemDTO.Links.Add(new LinkDTO("_self", Url.Link("MensagemAtualizacaoParcial", new { id = mensagem.Id }), "PATCH"));

                return Ok(mensagemDTO);
            }

            return Ok(mensagem);
        }
    }
}
