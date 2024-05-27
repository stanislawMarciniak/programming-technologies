using System;
using System.Collections.Generic;
using System.Text;

namespace StoreService.Data
{
    public interface IDataGenerator
    {
        DataContext GenerateData();
    }
}