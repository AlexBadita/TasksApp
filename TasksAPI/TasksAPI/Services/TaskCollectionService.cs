using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksAPI.Models;
using TasksAPI.Settings;

namespace TasksAPI.Services
{
    public class TaskCollectionService : ITaskCollectionService
    {
        private readonly IMongoCollection<Models.TaskModel> _tasks;

        public TaskCollectionService(IMongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _tasks = database.GetCollection<TaskModel>(settings.TaskCollectionName);
        }

        public async Task<bool> Create(TaskModel model)
        {
            await _tasks.InsertOneAsync(model);
            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            var result = await _tasks.DeleteOneAsync(task => task.Id == id);
            if (!result.IsAcknowledged && result.DeletedCount == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<TaskModel> Get(Guid id)
        {
            return (await _tasks.FindAsync(task => task.Id == id)).FirstOrDefault();
        }

        public async Task<List<TaskModel>> GetAll()
        {
            var result = await _tasks.FindAsync(task => true);
            return result.ToList();
        }

        public async Task<bool> Update(Guid id, TaskModel model)
        {
            model.Id = id;
            var result = await _tasks.ReplaceOneAsync(task => task.Id == id, model);
            if (!result.IsAcknowledged && result.ModifiedCount == 0)
            {
                await _tasks.InsertOneAsync(model);
                return false;
            }

            return true;
        }
    }
}
