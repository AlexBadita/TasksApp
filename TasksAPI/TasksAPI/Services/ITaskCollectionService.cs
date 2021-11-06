using NotesAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TasksAPI.Models;

namespace TasksAPI.Services
{
    public interface ITaskCollectionService : ICollectionService<TaskModel>
    {
    }
}
