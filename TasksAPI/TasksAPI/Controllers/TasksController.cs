using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksAPI.Models;
using TasksAPI.Services;

namespace TasksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : ControllerBase
    {
        ITaskCollectionService _taskCollectionService;
    
        public TasksController(ITaskCollectionService taskCollectionService)
        {
            _taskCollectionService = taskCollectionService ?? throw new ArgumentNullException(nameof(taskCollectionService));
        }

        /// <summary>
        ///     Returns a list of tasks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            List<TaskModel> tasks = await _taskCollectionService.GetAll();
            return Ok(tasks);
        }

        /// <summary>
        ///     Get task by id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetTask")]
        public async Task<IActionResult> GetTasksById(Guid id)
        {
            if (id == null)
            {
                return BadRequest("Id cannot be null!");
            }

            TaskModel task = await _taskCollectionService.Get(id);
            if (task == null)
            {
                return NoContent();
            }

            return Ok(task);
        }

        /// <summary>
        ///     Create a task
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskModel task)
        {
            if (task == null)
            {
                return BadRequest("Task cannot be null!");
            }

            if (await _taskCollectionService.Create(task))
            {
                return CreatedAtRoute("GetTask", new { id = task.Id.ToString() }, task);
            }
            return NoContent();
        }

        /// <summary>
        ///     Update task by id
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] TaskModel task)
        {
            if (task == null)
            {
                return BadRequest("Task cannot be null");
            }

            if (await _taskCollectionService.Update(id, task))
            {
                return Ok(_taskCollectionService.Get(id));
            }

            return NotFound("Task not found!");
        }

        /// <summary>
        ///     Delete task by id
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            if (id == null)
            {
                return BadRequest("Id cannot be null!");
            }

            if (await _taskCollectionService.Delete(id))
            {
                return NoContent();
            }

            return NotFound("Task not found!");
        }
    }
}
