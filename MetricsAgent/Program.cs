using MetricsAgent.Converters;
using MetricsAgent.Services;
using MetricsAgent.Services.Impl;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NLog.Web;
using System.Data.SQLite;

namespace MetricsAgent
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            /*                         
            1. Добавьте логирование всех параметров в каждом из контроллеров в обоих проектах
            2. Добавьте репозитории для каждого типа метрик в сервис агент сбора метрик
            3. Добавьте обработчики в контроллеры в REST стиле для наполнения метриками базы данных
            4. Добавьте тесты на все контроллеры и все методы с использованием заглушек
             */

            #region Configure Repository

            builder.Services.AddScoped<ICpuMetricsRepository, CpuMetricsRepository>();
            builder.Services.AddScoped<IDotNetErrorsMetricsRepository, DotNetErrorsMetricsRepository>();
            builder.Services.AddScoped<IHddLeftMetricsRepository, HddLeftMetricsRepository>();
            builder.Services.AddScoped<INetworkMetricsRepository, NetworkMetricsRepository>();
            builder.Services.AddScoped<IRamAvailableMetricsRepository, RamAvailableMetricsRepository>();
            #endregion

            #region Configure Database

            ConfigureSqlLiteConnection(builder.Services);

            #endregion

            #region Configure logging

            builder.Host.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();

            }).UseNLog(new NLogAspNetCoreOptions() { RemoveLoggerFactoryFilter = true });

            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestQuery;
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
                logging.RequestHeaders.Add("Authorization");
                logging.RequestHeaders.Add("X-Real-IP");
                logging.RequestHeaders.Add("X-Forwarded-For");
            });

            #endregion


            builder.Services.AddControllers()
              .AddJsonOptions(options =>
                  options.JsonSerializerOptions.Converters.Add(new CustomTimeSpanConverter()));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MetricsAgent", Version = "v1" });

                // Поддержка TimeSpan
                c.MapType<TimeSpan>(() => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("00:00:00")
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.UseHttpLogging();

            app.MapControllers();



            app.Run();
        }

        private static void ConfigureSqlLiteConnection(IServiceCollection services)
        {
            const string connectionString = "Data Source = metrics.db; Version = 3; Pooling = true; Max Pool Size = 100;";
            var connection = new SQLiteConnection(connectionString);
            connection.Open();
            PrepareSchema(connection);
        }

        private static void PrepareSchema(SQLiteConnection connection)
        {
            using (var command = new SQLiteCommand(connection))
            {
                // Задаём новый текст команды для выполнения
                // Удаляем таблицу с метриками, если она есть в базе данных
                command.CommandText = "DROP TABLE IF EXISTS cpumetrics";
                command.CommandText = "DROP TABLE IF EXISTS dotneterrorsmetrics";
                command.CommandText = "DROP TABLE IF EXISTS hddleftmetrics";
                command.CommandText = "DROP TABLE IF EXISTS networksmetrics";
                command.CommandText = "DROP TABLE IF EXISTS ramavailablemetrics";
                // Отправляем запрос в базу данных
                command.ExecuteNonQuery();
                command.CommandText =
                    @"CREATE TABLE cpumetrics(id INTEGER
                    PRIMARY KEY,
                    value INT, time INT)";
                command.CommandText =
                    @"CREATE TABLE dotneterrorsmetrics(id INTEGER
                    PRIMARY KEY,
                    value INT, time INT)";
                command.CommandText =
                    @"CREATE TABLE hddleftmetrics(id INTEGER
                    PRIMARY KEY,
                    value INT, time INT)";
                command.CommandText =
                    @"CREATE TABLE networksmetrics(id INTEGER
                    PRIMARY KEY,
                    value INT, time INT)";
                command.CommandText =
                    @"CREATE TABLE ramavailablemetrics(id INTEGER
                    PRIMARY KEY,
                    value INT, time INT)";
                command.ExecuteNonQuery();
            }
        }
    }
}