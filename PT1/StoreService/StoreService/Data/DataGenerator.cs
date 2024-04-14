using System;
using System.Collections.Generic;
using System.Text;

namespace StoreService.Data
{
    public interface IContentGenerator
    {
        DataContext GenerateContent();
    }
}