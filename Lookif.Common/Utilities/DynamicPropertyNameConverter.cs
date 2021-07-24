using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ESS.Lookif.Library.Common.Utilities
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class JsonPropertyNameByTypeAttribute : Attribute
    {
        public string PropertyName { get; set; }
        public Type ObjectType { get; set; }

        public JsonPropertyNameByTypeAttribute(string propertyName, Type objectType)
        {
            PropertyName = propertyName;
            ObjectType = objectType;
        }
    }

    public class DynamicPropertyNameConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Type type = value.GetType();
            JObject jo = new JObject();

            foreach (PropertyInfo prop in type.GetProperties().Where(p => p.CanRead))
            {
                string propName = prop.Name;
                object propValue = prop.GetValue(value, null);
                JToken token = (propValue != null) ? JToken.FromObject(propValue, serializer) : JValue.CreateNull();

                if (propValue != null && prop.PropertyType == typeof(object))
                {
                    JsonPropertyNameByTypeAttribute att = prop.GetCustomAttributes<JsonPropertyNameByTypeAttribute>()
                        .FirstOrDefault(a => a.ObjectType.IsAssignableFrom(propValue.GetType()));

                    if (att != null)
                        propName = att.PropertyName;
                }

                jo.Add(propName, token);
            }

            jo.WriteTo(writer);
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // ReadJson is not called if CanRead returns false.
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            // CanConvert is not called if a [JsonConverter] attribute is used
            return false;
        }
    }
}
