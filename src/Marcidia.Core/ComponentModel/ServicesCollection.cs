using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.ComponentModel
{
    public class ServiceCollection : IServiceProvider
    {
        Dictionary<Type, object> services;

        public ServiceCollection()
        {
            services = new Dictionary<Type, object>();
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException("serviceType", "serviceType is null.");

            if (services.ContainsKey(serviceType))
                return services[serviceType];

            return null;
        }

        public void AddService(Type serviceType, object service)
        {
            if (serviceType == null)
                throw new ArgumentNullException("serviceType", "serviceType is null.");
            if (service == null)
                throw new ArgumentNullException("service", "service is null.");

            if (services.ContainsKey(serviceType))
                throw new InvalidOperationException(
                    string.Format("Service of type {0} already exists", serviceType));

            services.Add(serviceType, service);
        }

        public T GetService<T>() where T : class
        {
            return (T)GetService(typeof(T));
        }

        public void AddService<T>(T service) where T : class
        {
            AddService(typeof(T), service);
        }
    }
}
