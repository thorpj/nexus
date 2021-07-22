using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Nexus.Shared.Models.Autotask
{
    public abstract class BaseEntity
    {
        public static string DisplayList(IList list)
        {
            var listString = new StringBuilder();

            foreach (var item in list)
            {
                listString.Append(item.ToString());
                if (item != list[^1])
                {
                    listString.Append(", ");
                }
            }

            return listString.ToString();
        }
        
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder
                .Append($"{this.GetType().Name} ({ImportantFieldsMessage()})")
                .Append(Environment.NewLine)
                .Append('{')
                .Append(Environment.NewLine);
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
            {
                var name = descriptor.Name;
                var value = descriptor.GetValue(this);
                stringBuilder
                    .Append('\t')
                    .Append(value is IList ? $"{name}: [{DisplayList((IList) value)}]" : $"{name}: {value}")
                    .Append(Environment.NewLine);
            }

            stringBuilder.Append('}');
            return stringBuilder.ToString();
        }

        protected abstract string ImportantFieldsMessage();

    }

    public class UserDefinedField
    {
        public string name;
        public string value;

        public override string ToString() => $"{name}: {value}";
    }
}