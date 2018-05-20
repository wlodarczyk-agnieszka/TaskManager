using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManager
{
    class TaskModel
    {
        // requied
        public string Description { get; set; } 
        public DateTime StartDate { get; set; }

        // not required
        public DateTime? EndDate { get; set; }
        public bool IsImportant { get; set; }
        public bool IsAllday { get; set; } 

        public TaskModel(string description, DateTime startDate)
        {
            Description = description;
            StartDate = startDate;
        }

        public TaskModel(string description, DateTime startDate, DateTime? endDate, bool important, bool allday)
        {
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            IsImportant = important;
            IsAllday = allday;
        }

    }
}
