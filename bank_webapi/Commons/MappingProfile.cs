using AutoMapper;
using bank_webapi.Entities;
using bank_webapi.Operations.UserOperations.Commands;
using bank_webapi.Operations.UserOperations.Queries;

namespace bank_webapi.Commons;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, GetUsersQueryModel>();
        CreateMap<User, GetDetailUserQueryModel>();
        CreateMap<CreateUserCommandModel, User>();
        CreateMap<User, GetMyInfoQueryModel>();
    }
}