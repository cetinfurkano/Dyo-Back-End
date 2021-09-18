using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Core.DataAccess.MongoDB
{
    public interface IMongoDBSettings
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
    }
}
