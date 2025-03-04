using Microsoft.EntityFrameworkCore;
using ResumeAPI.Data;
using ResumeAPI.Models;

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

            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ResumeDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<PersonService>();

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

            // Education
            app.MapGet("/education", async (ResumeDBContext context) =>
            {
                var educations = await context.Educations.ToListAsync();
                return Results.Ok(educations);
            });

            app.MapGet("/education/{id}", async (ResumeDBContext context, int id) =>
            {
                var existingEducation = await context.Educations.FirstOrDefaultAsync(e => e.ID == id);
                if (existingEducation == null)
                {
                    return Results.NotFound("Person not found");
                }
                return Results.Ok(existingEducation);
            });

            app.MapPost("/education", async (ResumeDBContext context, Education education) =>
            {
                context.Educations.Add(education);
                await context.SaveChangesAsync();
                return Results.Ok(education);
            });

            app.MapPut("/education/{id}", async (ResumeDBContext context, int id, Education education) =>
            {
                var existingEducation = await context.Educations.FirstOrDefaultAsync(e => e.ID == id);
                if (existingEducation == null)
                {
                    return Results.NotFound("Education not found");
                }

                existingEducation.School = education.School;
                existingEducation.Field = education.Field;
                existingEducation.Description = education.Description;
                existingEducation.StartDate = education.StartDate;
                existingEducation.EndDate = education.EndDate;
                await context.SaveChangesAsync();

                return Results.Ok(education);
            });

            app.MapDelete("education/{id}", async (ResumeDBContext context, int id) =>
            {
                var existingEducation = await context.Educations.FirstOrDefaultAsync(e => e.ID == id);
                if (existingEducation == null)
                {
                    return Results.NotFound("Education not found");
                }
                context.Remove(existingEducation);
                await context.SaveChangesAsync();
                return Results.Ok(existingEducation);
            });



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
