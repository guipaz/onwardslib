using System;
using System.Reflection;
using onwards.ecs;
using onwards.utils;

namespace onwards
{
    public static class Reflector
    {
        public static object Instantiate(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type == null)
            {
                Logger.Error("Reflector tried to instantiate non-existent class: " + typeName);
                return null;
            }

            return Activator.CreateInstance(type);
        }

        public static Entity InstantiateEntity(Type type)
        {
            return (Entity) Activator.CreateInstance(type);
        }

        public static T Instantiate<T>(string typeName)
        {
            return (T) Instantiate(typeName);
        }

        public static T Instantiate<T>()
        {
            return (T) Activator.CreateInstance(typeof(T));
        }

        public static Type GetEntityType(string name, Assembly assembly)
        {
            var fullname = $"{assembly.GetName().Name}.entities.{name}, {assembly.GetName().Name}";
            var type = Type.GetType(fullname);
            if (type == null)
            {
                type = Type.GetType("onwards.entities." + name + ", onwards");
                if (type == null)
                {
                    Logger.Error("No entity with classname " + name);
                    return null;
                }
            }

            return type;
        }
    }
}