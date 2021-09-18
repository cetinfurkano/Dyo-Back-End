using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Core.Utilities.IoC
{
   public interface ICoreModule
    {
        void Load(IServiceCollection serviceCollection);
    }
}
