using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.ObjectValues
{
    public class ErrorClass
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ValidationErrorVO
    {
        private List<ErrorClass> _errorClasses;


        public ValidationErrorVO(string msg)
        {
            try
            {
                _errorClasses = JsonConvert.DeserializeObject<List<ErrorClass>>(msg) ?? new List<ErrorClass>();
            }
            catch
            {
                _errorClasses = new List<ErrorClass>();
            }
        }

        public bool ContainsErros => _errorClasses.Count > 0;

        public string this[string name]
        {
            get
            {
                var erro = _errorClasses.FirstOrDefault(x => x.PropertyName.Equals(name));

                if (erro == null)
                    return "";

                return erro.ErrorMessage;
            }
        }
    }
}
