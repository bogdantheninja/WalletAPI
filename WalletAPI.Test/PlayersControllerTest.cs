using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using WalletAPI.Contracts;
using WalletAPI.Controllers;
using WalletAPI.Data;
using WalletAPI.Dtos;
using WalletAPI.Profiles;
using WalletAPI.Repository;
using Xunit;

namespace WalletAPI.Test
{
    public class PlayersControllerTest
    {
        PlayersController _controller;
        IPlayerRepository _repository;
        IMapper _mapper;
        public PlayersControllerTest()
        {
            var services = new ServiceCollection();
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
            services.AddSingleton<SharedMemory>();
            services.AddScoped<IPlayerRepository, MockupPlayersRepository>();
            var serviceProvider = services.BuildServiceProvider();

            _repository = serviceProvider.GetService<IPlayerRepository>();
            _controller = new PlayersController(_repository, _mapper);
        }


        [Fact]
        public void RegisterPlayer_ReturnsNewUserCreatedResponse()
        {
            //Arange
            var playerCreateDto = new PlayerCreateDto()
            {
                ID = Guid.NewGuid(),
                UserName = "Bogdanovic"
            };
            decimal balance = 0;

            //Act
            var okResult = _controller.RegisterPlayer(playerCreateDto).Result as OkObjectResult;
            var okResult2 = _controller.GetPlayerById(playerCreateDto.ID).Result as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.Equal(playerCreateDto.ID, (okResult2.Value as PlayerReadDto).ID);
            Assert.Equal(playerCreateDto.UserName, (okResult2.Value as PlayerReadDto).UserName);
            Assert.Equal(balance, (okResult2.Value as PlayerReadDto).Balance);
        }

        [Fact]
        public void RegisterPlayer_ReturnsUpdatedExistingUser()
        {
            //Arange
            var playerCreateDto = new PlayerCreateDto()
            {
                ID = new Guid("122bdd09-f8a3-4619-ac75-82939878d23a"),
                UserName = "JordanAIR"
            };

            //Act
            var okResult = _controller.RegisterPlayer(playerCreateDto).Result as OkObjectResult;
            var okResult2 = _controller.GetPlayerById(playerCreateDto.ID).Result as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.Equal(playerCreateDto.ID, (okResult2.Value as PlayerReadDto).ID);
            Assert.Equal(playerCreateDto.UserName, (okResult2.Value as PlayerReadDto).UserName);
        }

        [Fact]
        public void GetPlayerById_ReturnsCreatedResponse()
        {
            //Arange
            var id = new Guid("122bdd09-f8a3-4619-ac75-82939878d23a");

            //Act
            var okResult = _controller.GetPlayerById(id);

            //Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetPlayersBalance_ReturnsSuccessfully()
        {
            //Arange
            var id = new Guid("122bdd09-f8a3-4619-ac75-82939878d23a");
            decimal balance = 70;

            //Act
            var okResult = _controller.GetPlayersBalance(id).Result as OkObjectResult;

            //Assert
            Assert.IsType<decimal>(okResult.Value);
            Assert.Equal(balance, okResult.Value);
        }
    }
}
