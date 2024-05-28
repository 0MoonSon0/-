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
        public ObservableCollection<TaskItem> tasks { get; set; } // коллекция задач
        private List<TaskItem> task = new List<TaskItem>(); // список задач
        private string jsonFilePath = "tasks.json"; //путь к файлу 


        public MainWin()
        {
            InitializeComponent();
            DataContext = this; //привязка контекста данных
            tasks = new ObservableCollection<TaskItem>(); // Инициализация коллекции
            LoadTasksFromJson();
            UpdateTaskList();
            // Добавление обработчика события закрытия окна
            Closing += MainWin_Closing;
        }

        private void MainWin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveTasksToJson(); // Сохранение задач при закрытии окна
        }

        private void LoadTasksFromJson()//метод выгрузки задач
        {
            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);//читаем сожержимое файла и сохраняем
                tasks = JsonConvert.DeserializeObject<ObservableCollection<TaskItem>>(json);//преобразуе строку в объект списка 
            }
        }

        private void SaveTasksToJson()//метод сохранения задач
        {
            string json = JsonConvert.SerializeObject(tasks);
            File.WriteAllText(jsonFilePath, json);
        }

        private void UpdateTaskList()//метод обновления листа задач
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
            TaskItem selectedTask = taskListBox.SelectedItem as TaskItem;
            if (selectedTask != null)
            {
                MessageBox.Show($"Выбраная задача: {selectedTask.Name}");
                // Здесь вы можете добавить код для вывода информации о выбранной задаче в другом месте вашего приложения
            }
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

