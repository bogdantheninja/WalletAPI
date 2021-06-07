using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletAPI.Contracts;
using WalletAPI.Controllers;
using WalletAPI.Data;
using WalletAPI.Dtos;
using WalletAPI.Profiles;
using WalletAPI.Repository;
using Xunit;
using static WalletAPI.Constants.Enums;

namespace WalletAPI.Test
{
    public class TransactionsControllerTest
    {
        PlayersController _playersController;
        TransactionsController _transactionsController;
        IPlayerRepository _playerRepository;
        ITransactionRepository _transactionRepository;
        IMapper _mapper;
        public TransactionsControllerTest()
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
            services.AddScoped<ITransactionRepository, MockupTransactionRepository>();
            var serviceProvider = services.BuildServiceProvider();

            _playerRepository = serviceProvider.GetService<IPlayerRepository>();
            _transactionRepository = serviceProvider.GetService<ITransactionRepository>();
            _playersController = new PlayersController(_playerRepository, _mapper);
            _transactionsController = new TransactionsController(_transactionRepository, _mapper);
        }

        [Fact]
        public void RegisterTransaction_ReturnsSameServerStateResponse()
        {
            //Arange
            var id = new Guid("122bdd09-f8a3-4619-ac75-82939878d23a");
            var transaction = new TransactionCreateDto()
            {
                ID = new Guid("dc93c054-90b6-4ecb-b7d5-f5d420cddc9e"),
                Amount = 500,
                Type = TransactionType.deposit,
                PlayerId = id
            };

            //Act
            var okResult = _transactionsController.RegisterTransaction(transaction).Result as OkObjectResult;
            var okResult2 = _transactionsController.RegisterTransaction(transaction).Result as OkObjectResult;

            //Assert
            Assert.IsType<TransactionStatus>(okResult.Value);
            Assert.Equal(okResult.Value, okResult2.Value);
        }

        [Fact]
        public void RegisterTransaction_ReturnsAcceptedAndBalanceUpdated()
        {
            //Arange
            var id = new Guid("122bdd09-f8a3-4619-ac75-82939878d23a");
            decimal initBalance = (decimal)(_playersController.GetPlayersBalance(id).Result as OkObjectResult).Value;
            var transaction = new TransactionCreateDto()
            {
                ID = new Guid("dc93c054-90b6-4ecb-b7d5-f5d420cddc9e"),
                Amount = 500,
                Type = TransactionType.deposit,
                PlayerId = id
            };
            var status = TransactionStatus.accepted;

            //Act
            var okResult = _transactionsController.RegisterTransaction(transaction).Result as OkObjectResult;
            decimal changedBalance = (decimal)(_playersController.GetPlayersBalance(id).Result as OkObjectResult).Value;

            //Assert
            Assert.IsType<TransactionStatus>(okResult.Value);
            Assert.Equal(status, okResult.Value);
            Assert.Equal(changedBalance, initBalance + transaction.Amount);
        }

        [Fact]
        public void RegisterTransaction_ReturnsRejectedWhenAmountLessThanStake()
        {
            //Arange
            var id = new Guid("122bdd09-f8a3-4619-ac75-82939878d23a");
            decimal initBalance = (decimal)(_playersController.GetPlayersBalance(id).Result as OkObjectResult).Value;
            var transaction = new TransactionCreateDto()
            {
                ID = Guid.NewGuid(),
                Amount = 500,
                Type = TransactionType.stake,
                PlayerId = id
            };
            var status = TransactionStatus.rejected;

            //Act
            var okResult = _transactionsController.RegisterTransaction(transaction).Result as OkObjectResult;
            decimal changedBalance = (decimal)(_playersController.GetPlayersBalance(id).Result as OkObjectResult).Value;

            //Assert
            Assert.IsType<TransactionStatus>(okResult.Value);
            Assert.Equal(status, okResult.Value);
            Assert.Equal(initBalance, changedBalance);
        }

        [Fact]
        public void MultipleRegisterTransaction_ReturnsAcceptedAndBalanceUpdated()
        {
            //Arange
            var playerCreateDto = new PlayerCreateDto()
            {
                ID = Guid.NewGuid(),
                UserName = "Jokic"
            };

            //Act
            var okResultPlayer = _playersController.RegisterPlayer(playerCreateDto);
            int numberOfCalls = 100;
            int threadCounter = 4;
            decimal amount = 100.01M;

            List<Task> listOfTasks = new List<Task>();

            Guid[] listOfTransactionIds = new Guid[numberOfCalls];

            for (int k = 0; k < numberOfCalls; k++)
            {
                listOfTransactionIds[k] = Guid.NewGuid();
            }

            List<OkObjectResult> listResults = new List<OkObjectResult>();
            for (int i = 0; i < threadCounter; i++)
            {
                int taskCounter = i;
                Task task = Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < numberOfCalls; j++)
                    {
                        var transaction = new TransactionCreateDto
                        {
                            ID = listOfTransactionIds[j],
                            Amount = amount,
                            Type = TransactionType.deposit,
                            PlayerId = playerCreateDto.ID
                        };
                        var okResult = _transactionsController.RegisterTransaction(transaction).Result as OkObjectResult;
                        listResults.Add(okResult);
                    }
                });

                listOfTasks.Add(task);
            }

            int threadCounterForLogging = threadCounter;
            Task.WaitAll(listOfTasks.ToArray());
            decimal changedBalance = (decimal)(_playersController.GetPlayersBalance(playerCreateDto.ID).Result as OkObjectResult).Value;

            //Assert
            Assert.Equal(numberOfCalls * amount, changedBalance);
        }
    }
}
