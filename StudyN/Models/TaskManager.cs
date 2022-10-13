﻿using StudyN.Utilities;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;

namespace StudyN.Models
{
    //This class manages all of our tasks and preforms actions related to them
    public class TaskDataManager
    {
        //This function will add a new task to our list of tasks
        public TaskItem AddTask(string name,
                                string description,
                                DateTime dueTime,
                                int priority,
                                int CompletionProgress,
                                int TotalTimeNeeded,
                                bool updateFile = true)
        {
            //LoadFilesIntoLists();
            
            //Creating new task with sent parameters
            TaskItem newTask  = new TaskItem(name,
                                            description,
                                            dueTime,
                                            priority,
                                            CompletionProgress,
                                            TotalTimeNeeded);

            //This will add the tasks to the list
            TaskList.Add(newTask);

            //Creating a new file to store the task with
            sendFileUpdate(FileManager.Operation.AddTask, newTask.TaskId, updateFile);


            return newTask;
        }

        //This will return a requested task using its id
        public TaskItem GetTask(Guid taskId)
        {
            
            //Checking each item in the current task list
            foreach (TaskItem task in TaskList)
            {
                //If the task is found, return the task
                if (task.TaskId == taskId)
                {
                    return task;
                }
            }

            //Checking the completed tasks list
            foreach (TaskItem task in CompletedTasks)
            {
                if(task.TaskId == taskId)
                {
                    return task;
                }
            }

            //If not found in either list, return null
            return null;
        }

        //This function will recieve data to update a task with
        //!!!This function might be bugged! It is not saving the new data!
        public bool EditTask(Guid taskId,
                                string name,
                                string description,
                                DateTime dueTime,
                                int priority,
                                int CompletionProgress,
                                int TotalTimeNeeded,
                                bool updateFile = true)
        {

            //Retrieving the task
            TaskItem task = GetTask(taskId);

            //If the task is not found, return false to end early
            if(task == null)
            {
                return false;
            }

            //Updating all of the fields in the given task
            task.Name = name;
            task.Description = description;
            task.DueTime = dueTime;
            task.Priority = priority;
            task.CompletionProgress = CompletionProgress;
            task.TotalTimeNeeded = TotalTimeNeeded;

            //Updating the tasks's file
            sendFileUpdate(FileManager.Operation.EditTask, taskId, updateFile);

            //Returning true
            return true;
        }

        //This function will move a given task to the completed task list
        public void CompleteTask(Guid taskId, bool updateFile = true)
        {
            //Searching for the task in the normal TaskList
            foreach (TaskItem task in TaskList)
            {
                //If it is found in the list
                if (task.TaskId == taskId)
                {
                    //Set the tasks to completed, add it to the completed list, and remove it from the normal one
                    task.Completed = true;
                    CompletedTasks.Add(task);
                    TaskList.Remove(task);

                    //Update the files
                    sendFileUpdate(FileManager.Operation.CompleteTask, taskId, updateFile);

                    return;
                }
            }
        }


        public void DeleteTask(Guid taskId, bool updateFile = true)
        {
            foreach (TaskItem task in TaskList)
            {
                if (task.TaskId == taskId)
                {
                    TaskList.Remove(task);

                    sendFileUpdate(FileManager.Operation.DeleteTask, taskId, updateFile);

                    return;
                }
            }
        }

        public void DeleteListOfTasks(List<Guid> taskIds, bool updateFile = true)
        {
            foreach (Guid id in taskIds)
            {
                DeleteTask(id);
            }
        }


        public void sendFileUpdate(FileManager.Operation op, Guid taskId, bool updateFile)
        {
            if (updateFile)
            {
                // Send update to Filemanager
                FileManager.FILE_OP_QUEUE.Enquue(
                    new FileManager.FileOperation(op, taskId));
            }
        }

        
        public void LoadFilesIntoLists()
        {
            //string fileName = "WeatherForecast.json";
            //string jsonString = File.ReadAllText(fileName);
            //WeatherForecast weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(jsonString)!;

            string jsonfiletext;

            string []taskfilelist = FileManager.LoadTaskFileNames();
            string[] completedfiles = FileManager.LoadCompletedFileNames();
            foreach (string file in taskfilelist)
            {
                //Console.WriteLine("WARNING CHECKING GUID COMPONENT. VOLATILE INFORMATION");
                //Console.WriteLine(file);
                jsonfiletext = File.ReadAllText(file);
                TaskItem task = JsonSerializer.Deserialize<TaskItem>(jsonfiletext)!;
                //Console.WriteLine($"TaskGUID: {task.TaskId}");
                TaskList.Add(task);
            }

            //gets completed tasks
            foreach (string file in completedfiles)
            {
                //Console.WriteLine("WARNING CHECKING GUID COMPONENT. VOLATILE INFORMATION");
                //Console.WriteLine(file);
                jsonfiletext = File.ReadAllText(file);
                TaskItem task = JsonSerializer.Deserialize<TaskItem>(jsonfiletext)!;
                //Console.WriteLine($"TaskGUID: {task.TaskId}");
                CompletedTasks.Add(task);
            }
        }

        public ObservableCollection<TaskItem> TaskList { get; private set; }
        private ObservableCollection<TaskItem> CompletedTasks { get; set; }

        public TaskDataManager()
        {
            TaskList = new ObservableCollection<TaskItem>();
            CompletedTasks = new ObservableCollection<TaskItem>();
        }
    }
}