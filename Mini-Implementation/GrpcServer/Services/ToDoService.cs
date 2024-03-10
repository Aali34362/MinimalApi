using AutoMapper;
using Dumpify;
using Grpc.Core;
using GrpcServer.Model;
using GrpcServer.Repository;
using Microsoft.EntityFrameworkCore;
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

        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                await _dbContext.AddAsync<ToDoItem>(toDoItem);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new RpcException(new Status(StatusCode.Aborted, $"Data couldnt be stored in Sqlite DB : {ex.Message}"));
            }
        }

        return await Task.FromResult(new CreateToDoResponse { Id = toDoItem.Id });
    }

    public override async Task<ReadToDoResponse> ReadToDo(ReadToDoRequest request, ServerCallContext context)
    {
        if (request.Id <= 0)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Resource index must be greater than 0"));

        var Item = await _dbContext.ToDoItems.FirstOrDefaultAsync<ToDoItem>(t => t.Id == request.Id);

        if (Item != null)
        {
            return await Task.FromResult(_mapper.Map<ToDoItem, ReadToDoResponse>(Item));
        }
        throw new RpcException(new Status(StatusCode.NotFound, $"No task with id {request.Id}"));
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
