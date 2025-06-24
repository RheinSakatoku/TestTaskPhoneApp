using Newtonsoft.Json;
using PhoneApp.Domain;
using PhoneApp.Domain.Attributes;
using PhoneApp.Domain.DTO;
using PhoneApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesLoaderPlugin
{ 

  [Author(Name = "Ivan Petrov")]
  public class Plugin : IPluggable
  {
    private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public IEnumerable<DataTransferObject> Run(IEnumerable<DataTransferObject> args)
        {
            logger.Info("Loading employees");

            // Загружаем новых сотрудников из ресурсов
            var loadedEmployees = JsonConvert.DeserializeObject<List<EmployeesDTO>>(EmployeesLoaderPlugin.Properties.Resources.EmployeesJson);

            foreach (var emp in loadedEmployees)
            {
                if (!string.IsNullOrWhiteSpace(emp.Phone) && emp.Phone != "-")
                {
                    emp.AddPhone(emp.Phone);
                }
            }

            logger.Info($"Loaded {loadedEmployees.Count} employees");

            // Объединяем с уже существующими
            var all = args.Cast<EmployeesDTO>().ToList();
            all.AddRange(loadedEmployees);

            return all.Cast<DataTransferObject>();
        }
    }
}
