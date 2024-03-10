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

    public override async Task<GetAllResponse> ListToDo(GetAllRequest request, ServerCallContext context)
    {
        var response = new GetAllResponse();
        var toDoItems = await _dbContext.ToDoItems.ToListAsync();

        foreach (var toDo in toDoItems)
        {
            response.ToDo.Add(new ReadToDoResponse
            {
                Id = toDo.Id,
                Title = toDo.Title,
                Description = toDo.Description,
                ToDoStatus = toDo.ToDoStatus
            });
        }

        return await Task.FromResult(response);
    }

    public override async Task<UpdateToDoResponse> UpdateToDo(UpdateToDoRequest request, ServerCallContext context)
    {
        if (request.Id <= 0 || request.Title == string.Empty || request.Description == string.Empty)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "You must suppply a valid object"));

        var toDoItem = await _dbContext.ToDoItems.FirstOrDefaultAsync(t => t.Id == request.Id);

        if (toDoItem == null)
            throw new RpcException(new Status(StatusCode.NotFound, $"No Task with Id {request.Id}"));

        toDoItem.Title = request.Title;
        toDoItem.Description = request.Description;
        toDoItem.ToDoStatus = request.ToDoStatus;

        await _dbContext.SaveChangesAsync();

        return await Task.FromResult(new UpdateToDoResponse
        {
            Id = toDoItem.Id
        });
    }

    public override async Task<DeleteToDoResponse> DeleteToDo(DeleteToDoRequest request, ServerCallContext context)
    {
        if (request.Id <= 0)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "resouce index must be greater than 0"));

        var toDoItem = await _dbContext.ToDoItems.FirstOrDefaultAsync(t => t.Id == request.Id);

        if (toDoItem == null)
            throw new RpcException(new Status(StatusCode.NotFound, $"No Task with Id {request.Id}"));

        _dbContext.Remove(toDoItem);

        await _dbContext.SaveChangesAsync();

        return await Task.FromResult(new DeleteToDoResponse
        {
            Id = toDoItem.Id
        });
    }
}
