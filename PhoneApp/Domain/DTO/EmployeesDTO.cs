﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneApp.Domain.DTO
{
    public partial class EmployeesDTO : DataTransferObject
    {
        public string Name { get; set; }

        private string _phoneFromJson;

        public string Phone
        {
            get { return _phones.Any() ? _phones.LastOrDefault().Value : (_phoneFromJson ?? "-"); }
            set { _phoneFromJson = value; }
        }

        public void AddPhone(string phone)
        {
            if (String.IsNullOrEmpty(phone))
            {
                throw new Exception("Phone must be provided!");
            }

            _phones.Add(DateTime.Now, phone);
        }

        private Dictionary<DateTime, string> _phones = new Dictionary<DateTime, string>();
    }
}

