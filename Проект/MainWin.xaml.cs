using Newtonsoft.Json;
using System.ComponentModel;
using System.Windows;
using System.IO;
using System.Collections.ObjectModel;

namespace Проект
{
    /// <summary>
    /// Логика взаимодействия для MainWin.xaml
    /// </summary>
    public partial class MainWin : Window
    {
        public ObservableCollection<TaskItem> tasks { get; set; } // коллекция
        private List<TaskItem> task = new List<TaskItem>(); // список
        private string jsonFilePath = "tasks.json";


        public MainWin()
        {
            InitializeComponent();
            DataContext = this; //привязка контекста данных
            tasks = new ObservableCollection<TaskItem>(); // Инициализация коллекции
            LoadTasksFromJson();
            UpdateTaskList();
           
            Closing += MainWin_Closing;
        }

        private void MainWin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveTasksToJson();
        }

        private void LoadTasksFromJson()
        {
            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                tasks = JsonConvert.DeserializeObject<ObservableCollection<TaskItem>>(json);//преобразуе строку в объект списка 
            }
        }

        private void SaveTasksToJson()
        {
            string json = JsonConvert.SerializeObject(tasks);
            File.WriteAllText(jsonFilePath, json);
        }

        private void UpdateTaskList()
        {
            taskListBox.Items.Refresh();
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string taskName = taskTextBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(taskName))
            {
                tasks.Add(new TaskItem { Name = taskName });
                SaveTasksToJson();
                UpdateTaskList();
                taskTextBox.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Пожалуйста введите имя задачи.");
            }
        }

        private void RemoveTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную задачу из списка
            TaskItem selectedTask = taskListBox.SelectedItem as TaskItem;
            if (selectedTask != null)
            {
                tasks.Remove(selectedTask);
                SaveTasksToJson();
                UpdateTaskList();
            }
            else
            {
                MessageBox.Show("Пожалуйста выберите задачу для удаления.");
            }
        }

        private void taskListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //TaskItem selectedTask = taskListBox.SelectedItem as TaskItem;
            //if (selectedTask != null)
            //{
            //    MessageBox.Show($"Выбраная задача: {selectedTask.Name}");
            //}
        }
    }

    public class TaskItem : INotifyPropertyChanged
    {
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public override string ToString()
        {
            return Name; // Возвращаем значение свойства Name
        }

        public event PropertyChangedEventHandler PropertyChanged;//событие изменения свойства

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));//вызов события
        }
    }
}

