using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace makeITeasy.AzureDevops.Services
{
    public static class ServiceAssembly
    {
        public static Assembly Get => typeof(ServiceAssembly).Assembly;
    }
}
