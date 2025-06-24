using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using PhoneApp.Domain.DTO;
using PhoneApp.Domain.Attributes;
using PhoneApp.Domain.Interfaces;

namespace ApiUserLoaderPlugin
{
    [Author(Name = "Rhein_Sakatoku")]
    public class ApiUserLoaderPlugin : IPluggable
    {
        public IEnumerable<DataTransferObject> Run(IEnumerable<DataTransferObject> data)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            Console.WriteLine(">>>>> Плагин ApiUserLoaderPlugin загружен и вызван");

            var users = new List<EmployeesDTO>();

            try
            {
                using (var wc = new WebClient())
                {
                    wc.Headers.Add("User-Agent", "PhoneAppPlugin");
                    var json = wc.DownloadString("https://dummyjson.com/users");
                    var result = JsonConvert.DeserializeObject<UserApiResult>(json);

                    foreach (var user in result.users)
                        users.Add(new EmployeesDTO { Name = $"{user.firstName} {user.lastName}", Phone = user.phone });
                }

                Console.WriteLine($"ApiUserLoaderPlugin: загружено {users.Count} пользователей");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ApiUserLoaderPlugin: ошибка при загрузке данных - " + ex.Message);
            }

            var list = new List<DataTransferObject>(data);
            list.AddRange(users);

            return list;
        }
    }

    public class UserApiResult
    {
        public List<User> users { get; set; }
    }

    public class User
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phone { get; set; }
    }
}
