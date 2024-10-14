﻿using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Interfaces
{
    public interface IAccountService
    {
        Account GetAccount(string accountNumber);
        void UpdateAccount(Account account);
    }
}