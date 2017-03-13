using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SRBD_UWP.PackagesLoader
{
    public class ModuleContainer 
    {
        private readonly Dictionary<string, object> Properties;

        public ModuleContainer(Dictionary<string, object> properties = null)
        {
            if (properties == null)
                this.Properties = new Dictionary<string, object>();
            else
                this.Properties = properties;
        }

        public void AddPropery(string name, object value)
        {
            this.Properties.Add(name,value);
        }

        public object GetProperty(string key)
        {
            object result = this.Properties[key];
            return result;
        }

        public dynamic GetDynamicObj() => this;
    }
}