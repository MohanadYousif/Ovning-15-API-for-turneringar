using Microsoft.EntityFrameworkCore;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<TournamentApiContext>();
                if (context != null)
                {
                    await context.Database.MigrateAsync();
                    await SeedData.SeedAsync(context);
                }
            }
        }
    }
}
