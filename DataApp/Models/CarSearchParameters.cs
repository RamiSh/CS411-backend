using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Text;

namespace DataApp.Models
{
    public class CarSearchParameters
    {
        public string manufacturer { get; set; }
        public string model { get; set; }
        public string condition { get; set; }

        public CarSearchParameters()
        {
            manufacturer = model = condition = string.Empty;
        }
        public string AsSearchQuery()
        {
            var filter = new StringBuilder();
            filter.Append(!string.IsNullOrEmpty(manufacturer) ? (" lower(manufacturer) like @manufacturer") : "");

            if (!string.IsNullOrEmpty(model))
            {
                if (filter.Length > 0)
                {
                    filter.Append(" and ");
                }
                filter.Append(" lower(model) like @model ");
            }

            if (!string.IsNullOrEmpty(condition))
            {
                if (filter.Length > 0)
                {
                    filter.Append(" and ");
                }
                filter.Append(" lower(`condition`) like @condition ");
            }

            return filter.ToString();
        }

        public dynamic AsDynamicObject()
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(this.GetType()))
                expando.Add(property.Name, "%" + property.GetValue(this) + "%");

            return expando as ExpandoObject;
        }
        public override string ToString()
        {
            return $"manufacturer: {manufacturer}. model: {model}. condition: {condition}";
        }
    }
}
