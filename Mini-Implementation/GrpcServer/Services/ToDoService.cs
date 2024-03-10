using AutoMapper;
using Dumpify;
using Grpc.Core;
using GrpcServer.Model;
using GrpcServer.Repository;
using TodoGrpc;

namespace GrpcServer.Services;

public class ToDoService(AppDbContext dbContext, IMapper mapper) : TodoGrpc.TodoIt.TodoItBase
{
    public AppDbContext _dbContext => dbContext;
    public IMapper _mapper => mapper;

    public override async Task<CreateToDoResponse> CreateToDo(CreateToDoRequest request, ServerCallContext context)
    {
        if (request.Title == string.Empty || request.Description == string.Empty)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "you must supply valid object"));
        
        request.Dump("CreateToDoRequest");
        
        var toDoItem = _mapper.Map<CreateToDoRequest, ToDoItem>(request);
        
        toDoItem.Dump("ToDoItem");

        using (var transaction = _dbContext.Database.BeginTransaction())
        {
            try
            {
                await _dbContext.AddAsync(toDoItem);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new RpcException(new Status(StatusCode.Aborted, $"Data couldnt be stored in Sqlite DB : {ex.Message}"));
            }
        }

        return await Task.FromResult(new CreateToDoResponse { Id = toDoItem.Id });
    }

    public override Task<ReadToDoResponse> ReadToDo(ReadToDoRequest request, ServerCallContext context)
    {
        return base.ReadToDo(request, context);
    }

    public override Task<GetAllResponse> ListToDo(GetAllRequest request, ServerCallContext context)
    {
        return base.ListToDo(request, context);
    }

    public override Task<UpdateToDoResponse> UpdateToDo(UpdateToDoRequest request, ServerCallContext context)
    {
        return base.UpdateToDo(request, context);
    }

    public override Task<DeleteToDoResponse> DeleteToDo(DeleteToDoRequest request, ServerCallContext context)
    {
        return base.DeleteToDo(request, context);
    }
}
