using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TestDataDefinitionFramework.Testing.ExampleSut.Infrastructure;
using TestDataDefinitionFramework.Testing.ExampleSut.MongoDB;
using TestDataDefinitionFramework.Testing.ExampleSut.Redis;
using TestDataDefinitionFramework.Testing.ExampleSut.Sql;

namespace TestDataDefinitionFramework.Testing.ExampleSut
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TestDataDefinitionFramework.Testing.ExampleSut", Version = "v1" });
            });

            services.AddSingleton<IMongoDataStoreConfig, MongoDataStoreConfig>();
            services.AddMongoRepository();

            services.AddSingleton<ISqlDataStoreConfig, SqlDataStoreConfig>();
            services.AddSqlRepository();

            services.AddSingleton<IRedisDataStoreConfig, RedisDataStoreConfig>();
            services.AddRedisRepository();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestDataDefinitionFramework.Testing.ExampleSut v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}