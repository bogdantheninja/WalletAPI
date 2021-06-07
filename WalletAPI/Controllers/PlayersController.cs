using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WalletAPI.Contracts;
using WalletAPI.Dtos;
using WalletAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        //private readonly ILoggerManager _logger;
        private readonly IPlayerRepository _repository;
        private readonly IMapper _mapper;

        public PlayersController(IPlayerRepository repository, IMapper mapper) 
        { 
            //_logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        //POST api/players
        [HttpPost("register")]
        public ActionResult<PlayerReadDto> RegisterPlayer(PlayerCreateDto playerCreateDto)
        {
            try
            {
                var playerModel = _mapper.Map<Player>(playerCreateDto);
                var playerEntity = _repository.GetPlayerById(playerModel.ID);

                if (playerEntity == null)
                {
                    _repository.CreatePlayer(playerModel);
                }
                else 
                {
                    _repository.UpdatePlayer(playerModel);
                }

                playerEntity = _repository.GetPlayerById(playerModel.ID);
                var playerReadDto = _mapper.Map<PlayerReadDto>(playerEntity);
                return Ok(playerReadDto);
                
                //var playerReadDto = _mapper.Map<PlayerReadDto>(playerModel);
                //return CreatedAtRoute(nameof(GetPlayerById), new { Id = playerReadDto.ID }, playerReadDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GET api/players
        [HttpGet("getbalance/{id}")]
        public ActionResult<decimal> GetPlayersBalance(Guid id)
        {
            var playerModel = _repository.GetPlayerById(id);
            if (playerModel == null)
            {
                return NotFound();
            }
            return Ok(playerModel.Balance);
        }

        #region NotRequired

        //GET api/players
        [HttpGet]
        public ActionResult<IEnumerable<PlayerReadDto>> GetPlayers()
        {
            var players = _repository.GetPlayers();

            return Ok(_mapper.Map<IEnumerable<PlayerReadDto>>(players));
        }

        //GET api/players/{id}
        [HttpGet("{id}")]
        public ActionResult<PlayerReadDto> GetPlayerById(Guid id)
        {
            var playerModel = _repository.GetPlayerById(id);
            if (playerModel == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<PlayerReadDto>(playerModel));
        }

        #endregion
    }
}
