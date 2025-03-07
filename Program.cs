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

            EducationEndpoint.RegisterEndpoints(app);


            // Job 
            app.MapGet("/job", async (ResumeDBContext context) =>
            {
                var jobs = await context.Jobs.ToListAsync();
                return Results.Ok(jobs);
            });

            app.MapGet("/job/{id}", async (ResumeDBContext context, int id) =>
            {
                var existingJob = await context.Jobs.FirstOrDefaultAsync(j => j.ID == id);
                if (existingJob == null)
                {
                    return Results.NotFound("Job not found");
                }
                return Results.Ok(existingJob);
            });

            app.MapPost("/job", async (ResumeDBContext context, Job job) =>
            {
                context.Jobs.Add(job);
                await context.SaveChangesAsync();
                return Results.Ok(job);
            });

            app.MapPut("/job/{id}", async (ResumeDBContext context, int id, Job job) =>
            {
                var existingJob = await context.Jobs.FirstOrDefaultAsync(j => j.ID == id);
                if (existingJob == null)
                {
                    return Results.NotFound("Education not found");
                }

                existingJob.Company = job.Company;
                existingJob.Title = job.Title;
                existingJob.Description = job.Description;
                existingJob.StartDate = job.StartDate;
                existingJob.EndDate = job.EndDate;
                await context.SaveChangesAsync();

                return Results.Ok(job);
            });

            app.MapDelete("job/{id}", async (ResumeDBContext context, int id) =>
            {
                var existingJob = await context.Jobs.FirstOrDefaultAsync(j => j.ID == id);
                if (existingJob == null)
                {
                    return Results.NotFound("Education not found");
                }
                context.Remove(existingJob);
                await context.SaveChangesAsync();
                return Results.Ok(existingJob);
            });

            app.Run();
        }
    }
}
