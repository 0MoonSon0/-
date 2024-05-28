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
        public event EventHandler<string> UserAuthenticated; 
        public event EventHandler<string> UserRegistered; 
        public event EventHandler<string> AuthenticationFailed;

        private string _usersFilePath;

        //конструктор класса
        public AuthenticationManager(string usersFilePath)
        {
            _usersFilePath = usersFilePath;
        }

        public bool RegisterUser(string login, string password)
        {
            //загрухка пользователей
            var existingUsers = LoadUsers();
            
            if (existingUsers.ContainsKey(login))
            {
                
                AuthenticationFailed?.Invoke(this, "Такой пользователь уже существует");
                return false;
            }

            
            existingUsers.Add(login, password);
            
            SaveUsers(existingUsers);
           
            UserRegistered?.Invoke(this, login);
            return true;
        }


        public bool AuthenticateUser(string login, string password)
        {            
            var existingUsers = LoadUsers();
            
            if (existingUsers.TryGetValue(login, out string storedPassword) && storedPassword == password)
            {

                UserAuthenticated?.Invoke(this, login);
                return true;
            }
            else
            {

                AuthenticationFailed?.Invoke(this, "Непртавильный логин или пароль");
                return false;
            }
        }


        private Dictionary<string, string> LoadUsers()
        {

            if (File.Exists(_usersFilePath))
            {
                string json = File.ReadAllText(_usersFilePath);
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            else
            {
                return new Dictionary<string, string>();
            }
        }

        private void SaveUsers(Dictionary<string, string> users)
        {
            string json = JsonConvert.SerializeObject(users);
            File.WriteAllText(_usersFilePath, json);
        }
    }
}
