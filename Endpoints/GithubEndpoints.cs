
using ResumeAPI.DTO;
using System.Text.Json;

namespace ResumeAPI.Endpoints
{
	class GithubEndpoints
	{
		public static void RegisterEndpoints(WebApplication app)
		{
			app.MapGet("/github/{username}", async (HttpClient client, string username) =>
			{
				client.DefaultRequestHeaders.Add("User-Agent", "MyMinimalAPI");

				var response = await client.GetAsync($"https://api.github.com/users/{username}/repos");

				if (!response.IsSuccessStatusCode)
				{
					return Results.NotFound();
				}

				var json = await response.Content.ReadAsStringAsync();
				var repositories = JsonSerializer.Deserialize<List<GithubDTO>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				if (repositories == null || repositories.Count == 0)
				{
					return Results.NoContent();
				}

				foreach (var repo in repositories)
				{
					if (string.IsNullOrEmpty(repo.Language))
					{
						repo.Language = "Unknown";
					}
				}
				return Results.Ok(repositories ?? new List<GithubDTO>());
			});
		}
	}
}
