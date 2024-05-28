using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;

namespace Проект
{
    public class AuthenticationManager
    {
        //определяем события
        public event EventHandler<string> UserAuthenticated; // Аутентификация
        public event EventHandler<string> UserRegistered; // Регистрайия
        public event EventHandler<string> AuthenticationFailed;//неудачная аутентификация

        private string _usersFilePath;

        public AuthenticationManager(string usersFilePath)
        {
            _usersFilePath = usersFilePath;
        }

        public bool RegisterUser(string login, string password)
        {
            // Загружает существующих пользователей из Json файла
            var existingUsers = LoadUsers();

            // Проверяет имя пользователя
            if (existingUsers.ContainsKey(login))
            {
                // Пользователь уже существует, вызываем событие для сбоя регистрации
                AuthenticationFailed?.Invoke(this, "Такой пользователь уже существует");
                return false;
            }

            // Добавляем нового пользователя
            existingUsers.Add(login, password);

            // Сохраненяем обновленных пользователей в JSON-файл
            SaveUsers(existingUsers);

            // Вызывает ивент для успешной решистрации
            UserRegistered?.Invoke(this, login);
            return true;
        }


        public bool AuthenticateUser(string login, string password)
        {
            // Загружает сущ пользователей из JSON file
            var existingUsers = LoadUsers();

            // проверяет сущ ли пользователь и сравнивает совпадают ли пароли
            if (existingUsers.TryGetValue(login, out string storedPassword) && storedPassword == password)
            {
                // вызывает ивент успешной авторизации
                UserAuthenticated?.Invoke(this, login);
                return true;
            }
            else
            {
                // вызывает ивент сбоя авторизации
                AuthenticationFailed?.Invoke(this, "Непртавильный логин или пароль");
                return false;
            }
        }


        private Dictionary<string, string> LoadUsers()
        {
            // Загружает пользователей из JSON file
            if (File.Exists(_usersFilePath))
            {
                string json = File.ReadAllText(_usersFilePath);
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            else
            {
                // Если файл не сущ возвращает пустоту
                return new Dictionary<string, string>();
            }
        }

        private void SaveUsers(Dictionary<string, string> users)
        {
            // Сохраняет пользователей в JSON file
            string json = JsonConvert.SerializeObject(users);
            File.WriteAllText(_usersFilePath, json);
        }
    }
}
