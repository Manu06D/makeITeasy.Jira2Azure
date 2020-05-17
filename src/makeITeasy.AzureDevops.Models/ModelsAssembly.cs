using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace makeITeasy.AzureDevops.Models
{
    public static class ModelsAssembly
    {
        public static Assembly Get => typeof(ModelsAssembly).Assembly;
    }
}
