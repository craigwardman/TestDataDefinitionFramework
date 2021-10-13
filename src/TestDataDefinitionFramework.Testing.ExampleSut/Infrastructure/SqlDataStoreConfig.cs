﻿using Microsoft.Extensions.Configuration;
using TestDataDefinitionFramework.Testing.ExampleSut.Sql;

namespace TestDataDefinitionFramework.Testing.ExampleSut.Infrastructure
{
    public class SqlDataStoreConfig : ISqlDataStoreConfig
    {
        public SqlDataStoreConfig(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("SqlServer");
        }
        
        public string ConnectionString { get; set; }
    }
}