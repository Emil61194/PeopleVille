
using PeopleVille.Engine;
using PeopleVille.Server.Hubs;
using PeopleVille.Server.Infrastructure;
using PeopleVille.Server.Services;

namespace PeopleVille.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<GameService>();
            builder.Services.AddSingleton<GameEngine>();
            builder.Services.AddSingleton<IEventPublisher, SignalREventPublisher>();
            builder.Services.AddSingleton<EventPublisher>();

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAny", policy =>
                {
                    policy.WithOrigins(
                              "http://localhost:5173",
                              "https://localhost:50733",
                              "https://localhost:7039",
                              "http://localhost:5045")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();
            // dont accept
            app.UseDefaultFiles();
            app.MapStaticAssets();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            app.UseCors("AllowAny");

            app.UseAuthorization();

            app.MapControllers();
            
            app.MapHub<GameHub>("/hubs/game");

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
