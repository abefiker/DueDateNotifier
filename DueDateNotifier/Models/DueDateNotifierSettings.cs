﻿namespace DueDateNotifier.Models
{
    public class DueDateNotifierSettings
    {
        public string? ConnectionString { get; set;}
        public string? DatabaseName { get; set;}
        public string? UsersCollectionName { get; set;}
        public string? TasksCollectionName { get; set;}

    }
}
