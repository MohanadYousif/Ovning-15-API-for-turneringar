using TournamentAPI.Core.Entities;

namespace TournamentAPI.Data.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(TournamentApiContext context)
        {
            if (!context.Tournament.Any())
            {
                context.Tournament.AddRange(
                    new Tournament
                    {
                        Title = "Spring Championship",
                        StartDate = new DateTime(2024, 6, 1),
                        Games = new[]
                        {
                            new Game
                            {
                                Title = "Match 1",
                                Time = new DateTime(2024, 6, 1, 10, 0, 0)
                            },
                            new Game
                            {
                                Title = "Match 2",
                                Time = new DateTime(2024, 6, 1, 14, 0, 0)
                            }
                        }
                    },
                    new Tournament
                    {
                        Title = "Summer Cup",
                        StartDate = new DateTime(2024, 7, 1),
                        Games = new[]
                        {
                            new Game
                            {
                                Title = "Match 1",
                                Time = new DateTime(2024, 7, 1, 10, 0, 0)
                            },
                            new Game
                            {
                                Title = "Match 2",
                                Time = new DateTime(2024, 7, 1, 14, 0, 0)
                            }
                        }
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
