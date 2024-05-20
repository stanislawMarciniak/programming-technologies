using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Presentation.Common
{
    public class ErrorValidator : INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> PropertyErrors = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => PropertyErrors.Any();

        public IEnumerable GetErrors(string propertyName)
        {
            return PropertyErrors.ContainsKey(propertyName) ? PropertyErrors[propertyName] : string.Empty as IEnumerable;
        }

        public void AddError(string propertyName, string errorMessage)
        {
            if (!PropertyErrors.ContainsKey(propertyName))
            {
                PropertyErrors.Add(propertyName, new List<string>());
            }

            PropertyErrors[propertyName].Add(errorMessage);
            OnErrorsChanged(nameof(propertyName));
        }

        public void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void ClearErrors(string propertyName)
        {
            if (PropertyErrors.Remove(propertyName))
            {
                OnErrorsChanged(propertyName);
            }
        }
    }
}