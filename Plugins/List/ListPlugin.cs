using BasePlugin;
using BasePlugin.Interfaces;
using BasePlugin.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ListPlugin
{
    record PersistentDataStructure(List<string> List);

    public class ListPlugin : IPlugin
    {
        public static string _Id = "list";
        public string Id => _Id;

        public PluginOutput Execute(PluginInput input)
        {
            List<string> list = new();

            if (string.IsNullOrEmpty(input.PersistentData) == false)
            {
                list = JsonSerializer.Deserialize<PersistentDataStructure>(input.PersistentData).List;
            }

            string message = input.Message.ToLower(); // הפיכת הקלט לפורמט קטן

            if (message == "")
            {
                input.Callbacks.StartSession();
                return new PluginOutput("List started. Enter 'Add' to add task. Enter 'Delete <task>' to delete a specific task. Enter 'List' to view all list. Enter 'Exit' to stop.", input.PersistentData);
            }
            else if (message == "exit")
            {
                input.Callbacks.EndSession();
                return new PluginOutput("List stopped.", input.PersistentData);
            }
            else if (message.StartsWith("add"))
            {
                var str = input.Message.Substring("add".Length).Trim();
                list.Add(str);

                var data = new PersistentDataStructure(list);

                return new PluginOutput($"New task: {str}", JsonSerializer.Serialize(data));
            }
            else if (message.StartsWith("delete"))
            {
                var taskToDelete = input.Message.Substring("delete".Length).Trim(); // מקבל את המשימה למחיקה

                if (list.Contains(taskToDelete)) // אם המשימה קיימת ברשימה
                {
                    list.Remove(taskToDelete); // מסיר את המשימה הספציפית
                    var data = new PersistentDataStructure(list);
                    return new PluginOutput($"Deleted task: {taskToDelete}", JsonSerializer.Serialize(data)); // מחזירים את הרשימה המעודכנת
                }
                else
                {
                    return new PluginOutput($"Task '{taskToDelete}' not found.", input.PersistentData); // אם המשימה לא נמצאה
                }
            }
            else if (message == "list")
            {
                string listtasks = string.Join("\r\n", list);
                return new PluginOutput($"All list tasks:\r\n{listtasks}", input.PersistentData);
            }
            else
            {
                return new PluginOutput("Error! Enter 'Add' to add task. Enter 'Delete <task>' to delete a specific task. Enter 'List' to view all list. Enter 'Exit' to stop.");
            }
        }
    }
}