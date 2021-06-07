using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WalletAPI.Contracts;
using WalletAPI.Dtos;
using WalletAPI.Models;
using static WalletAPI.Constants.Enums;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        //private readonly ILoggerManager _logger;
        private readonly ITransactionRepository _repository;
        private readonly IMapper _mapper;

        public TransactionsController(ITransactionRepository repository, IMapper mapper) 
        { 
            //_logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        //POST api/transactions/register
        [HttpPost("register")]
        public ActionResult<TransactionStatus> RegisterTransaction(TransactionCreateDto transactionCreateDto)
        {
            try
            {
                var transactionModel = _mapper.Map<Transaction>(transactionCreateDto);
                transactionModel.Status = _repository.CreateTransaction(transactionModel);

                return Ok(transactionModel.Status);
            }
            catch (Exception ex)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //GET api/transactions/GetUserTransactions/{id}
        [HttpGet("GetUserTransactions/{id}")]
        public ActionResult<IEnumerable<TransactionReadDto>> GetUserTransactions(Guid id)
        {
            var transactions = _repository.GetTransactions(id);

            return Ok(_mapper.Map<IEnumerable<TransactionReadDto>>(transactions));
        }

        #region NotRequired

        //GET api/transactions/GetTransactionById/{id}
        [HttpGet("GetTransactionById/{id}")]
        public ActionResult<TransactionReadDto> GetTransactionById(Guid id)
        {
            var transactionModel = _repository.GetTransactionById(id);
            if (transactionModel == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<TransactionReadDto>(transactionModel));
        }

        #endregion
    }
}
