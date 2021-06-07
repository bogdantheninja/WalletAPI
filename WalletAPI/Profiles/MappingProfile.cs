using AutoMapper;
using WalletAPI.Dtos;
using WalletAPI.Models;

namespace WalletAPI.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Player,PlayerReadDto>();
            CreateMap<PlayerCreateDto, Player>();
            CreateMap<Transaction, TransactionReadDto>();
            CreateMap<TransactionCreateDto, Transaction>();
        }
    }
}
