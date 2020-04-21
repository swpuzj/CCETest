using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCE.Common
{
    public sealed class DependencyInjection
    {
        private readonly IServiceProvider _serviceProvider;

        private static DependencyInjection _instance;
        public static DependencyInjection Instance
        {
            get
            {
                if (null == _instance)
                    throw new Exception("请在Startup中初始化DependencyInjection");
                return _instance;
            }
        }

        public static void Init(IServiceProvider serviceProvider)
        {
            _instance = new DependencyInjection(serviceProvider);
        }

        private DependencyInjection() { }

        private DependencyInjection(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T GetService<T>() where T : class
        {
            var service = _serviceProvider.GetService<T>();
            if (null == service)
                throw new Exception("获取注入对象失败");
            return service;
        }

        public IEnumerable<T> GetServices<T>() where T : class
        {
            var service = _serviceProvider.GetServices<T>();
            if (null == service ||
                0 == service.Count())
                throw new Exception("获取注入对象失败");
            return service;
        }
    }
}
