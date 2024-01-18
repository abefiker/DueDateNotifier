﻿using DueDateNotifier.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DueDateNotifier.Services
{
    public class DueDateServices
    {
        private readonly IMongoCollection<Users> _userCollection;
        private readonly IMongoCollection<TaskModel> _taskCollection;
        public DueDateServices(IOptions<DueDateNotifierSettings> dueDateNotifierSettings){
        var client = new MongoClient(dueDateNotifierSettings.Value.ConntectionString);
        var database = client.GetDatabase(dueDateNotifierSettings.Value.DatabaseName);
        var _userCollection = database.GetCollection<Users>(dueDateNotifierSettings.Value.UsersCollectionName); // Replace User with your actual user model
        var _taskCollection = database.GetCollection<TaskModel>(dueDateNotifierSettings.Value.TasksCollectionName); // Replace TaskModel with your actual task model
        }
        public async Task<List<TaskModel>> GetAsync() =>
        await _taskCollection.Find(_ => true).ToListAsync();

        public async Task<TaskModel?> GetAsync(string id) =>
            await _taskCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(TaskModel newTask) =>
            await _taskCollection.InsertOneAsync(newTask);

        public async Task UpdateAsync(string id, TaskModel updatedTask) =>
            await _taskCollection.ReplaceOneAsync(x => x.Id == id, updatedTask);

        public async Task RemoveAsync(string id) =>
            await _taskCollection.DeleteOneAsync(x => x.Id == id);
    }
}