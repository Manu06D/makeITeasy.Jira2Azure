using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace makeITeasy.AzureDevops.Infrastructure
{
    public class InfrastructureAssembly
    {
        public static Assembly GetAssembly => typeof(InfrastructureAssembly).Assembly;
    }
}
