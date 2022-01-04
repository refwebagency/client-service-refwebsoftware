using AutoMapper;
using ClientService.Dtos;
using ClientService.Models;

namespace ClientService.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ReadClientDTO>();
            CreateMap<CreateClientDTO, Client>();
            CreateMap<UpdateClientDTO, Client>();
        }
    }
}