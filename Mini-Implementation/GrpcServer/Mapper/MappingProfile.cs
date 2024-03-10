using AutoMapper;
using GrpcServer.Model;
using TodoGrpc;

namespace GrpcServer.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile() {
        CreateMap<CreateToDoRequest, ToDoItem>();
    }
}
