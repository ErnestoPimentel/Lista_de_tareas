using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs.Tasks;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Interfaces.Tasks;

namespace TaskManager.Web.Controllers;

[ApiController]
[Route("api/tasks")]
public sealed class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ICurrentUser _currentUser;

    public TasksController(ITaskService taskService, ICurrentUser currentUser)
    {
        _taskService = taskService;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<TaskResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        EnsureAuthenticated();
        var tasks = await _taskService.GetAllAsync(_currentUser.UserId, cancellationToken);
        return Ok(tasks);
    }

    [HttpGet("{taskId:guid}")]
    public async Task<ActionResult<TaskResponse>> GetById(
        Guid taskId,
        CancellationToken cancellationToken)
    {
        EnsureAuthenticated();

        var task = await _taskService.GetByIdAsync(_currentUser.UserId, taskId, cancellationToken);

        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponse>> Create(
        CreateTaskRequest request,
        CancellationToken cancellationToken)
    {
        EnsureAuthenticated();
        var task = await _taskService.CreateAsync(_currentUser.UserId, request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { taskId = task.Id },
            task);
    }

    [HttpPut("{taskId:guid}")]
    public async Task<ActionResult<TaskResponse>> Update(
        Guid taskId,
        UpdateTaskRequest request,
        CancellationToken cancellationToken)
    {
        EnsureAuthenticated();

        var task = await _taskService.UpdateAsync(
            _currentUser.UserId,
            taskId,
            request,
            cancellationToken);

        if (task is null)
            return NotFound();

        return Ok(task);
    }

    [HttpDelete("{taskId:guid}")]
    public async Task<ActionResult<bool>> Delete(
        Guid taskId,
        CancellationToken cancellationToken)
    {
        EnsureAuthenticated();

        var deleted = await _taskService.DeleteAsync(
            _currentUser.UserId,
            taskId,
            cancellationToken);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    private void EnsureAuthenticated()
    {
        if(!_currentUser.IsAuthenticated)
            throw new UnauthorizedAccessException("User is not authenticated.");
    }
}
