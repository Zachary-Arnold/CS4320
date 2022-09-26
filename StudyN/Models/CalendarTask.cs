﻿using System.Collections.ObjectModel;

namespace StudyN.Models
{
    public class CalendarTask
    {

        //public int Id { get; set; }
        //public DateTime StartTime { get; set; }
        //public DateTime EndTime { get; set; }
        //public string Subject { get; set; }
        //public int LabelId { get; set; }
        //public string Location { get; set; }

        public CalendarTask(string Text)
        {
            this.Name = Text;
        }

        public bool Completed { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime DueDate { get; set; }
        public int TimeNeeded { get; set; }
        public string Location { get; set; }

        public CalendarTasksData Parent { get; set; }
    }

    public class CalendarTasksData
    {
        void GenerateCalendarTaskss()
        {
            CalendarTasks.Add(
                new CalendarTask("HW: Pitch your Application Idea")
                {
                    Parent = this,
                    Completed = false,
                    Id = 1,
                    Description = "Pitch your appilcation idea...",
                    StartTime = DateTime.Now,
                    DueDate = DateTime.Today,
                    TimeNeeded = 3,
                    Location = "test location 0"
                }
            ); ;
            CalendarTasks.Add(
                new CalendarTask("HW: Technology Proof of Concept")
                {
                    Parent = this,
                    Completed = false,
                    Id = 2,
                    Description = "Prove your technology works...",
                    StartTime = DateTime.Now,
                    DueDate = DateTime.Today,
                    TimeNeeded = 7,
                    Location = "test location 1"
                }
            );
            CalendarTasks.Add(
                new CalendarTask("HW: Prototype of Key Features")
                {
                    Parent = this,
                    Completed = false,
                    Id = 3,
                    Description = "Build a prototype of the feature...",
                    StartTime = DateTime.Now,
                    DueDate = DateTime.Today.AddHours(24),
                    TimeNeeded = 5,
                    Location = "test location 2"
                }
            );
            CalendarTasks.Add(
                new CalendarTask("Test")
                {
                    Parent = this,
                    Completed = false,
                    Id = 4,
                    Description = "this is a test",
                    StartTime = DateTime.Now,
                    DueDate = DateTime.Today.AddHours(24),
                    TimeNeeded = 5,
                    Location = "test location 3"
                }
            );
        }

        public void TaskComplete(CalendarTask task)
        {
            task.Completed = !task.Completed;
            CompletedTasks.Add(task);
            CalendarTasks.Remove(task);
        }

        public ObservableCollection<CalendarTask> CalendarTasks { get; private set; }
        private ObservableCollection<CalendarTask> CompletedTasks { get; set; }

        public CalendarTasksData()
        {
            CalendarTasks = new ObservableCollection<CalendarTask>();
            CompletedTasks = new ObservableCollection<CalendarTask>();
            GenerateCalendarTaskss();
        }
    }
}
