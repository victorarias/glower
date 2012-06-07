using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlowerFramework
{
    /// <summary>
    /// A micro IoC/DI framework
    /// </summary>
    public class Glower
    {
        private Dictionary<Type, Func<object>> providers = new Dictionary<Type, Func<object>>();

        public void Bind<TKey, TImpl>() where TImpl : TKey
        {
            providers.Add(typeof(TKey), () => Resolve<TImpl>());
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        private object Resolve(Type type)
        {
            Func<object> provider;

            if (providers.TryGetValue(type, out provider))
            {
                return provider();
            }
            else
            {
                return ResolveByGreediestConstructor(type);
            }
        }

        private object ResolveByGreediestConstructor(Type type)
        {
            var constructor = type.GetConstructors().OrderBy(a => a.GetParameters().Length).First();
            var parameters = constructor.GetParameters();

            if (parameters.Length == 0)
                return Activator.CreateInstance(type);
            else
            {
                var resolvedParameters = from p in parameters
                                         select Resolve(p.ParameterType);

                return constructor.Invoke(resolvedParameters.ToArray());
            }
        }

        public void Bind<T>(T instance)
        {
            providers[typeof(T)] = () => instance;
        }
    }
}
