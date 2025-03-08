using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ResumeAPI.Data;
using ResumeAPI.Endpoints;
using ResumeAPI.Models;
using ResumeAPI.Services;

namespace ResumeAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddAuthorization();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();

			//builder.Services.AddSwaggerGen();
			builder.Services.AddSwaggerGen(options =>
			{
				options.MapType<DateOnly>(() => new OpenApiSchema
				{
					Type = "string",
					Format = "date"
				});
			});

			builder.Services.AddDbContext<ResumeDBContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

			builder.Services.AddScoped<PersonService>();
			builder.Services.AddScoped<EducationService>();
			builder.Services.AddScoped<JobService>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			PersonEndpoints.RegisterEndpoints(app);
			EducationEndpoints.RegisterEndpoints(app);
			JobEndpoints.RegisterEndpoints(app);

			app.Run();
		}
	}
}
