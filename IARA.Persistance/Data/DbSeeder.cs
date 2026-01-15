using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IARADbContext context)
    {
        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Seed Recreational Ticket Types if none exist
        if (!await context.RecreationalTicketTypes.AnyAsync())
        {
            var ticketTypes = new List<RecreationalTicketType>
            {
                new RecreationalTicketType
                {
                    TypeName = "Daily Ticket",
                    ValidityDays = 1,
                    PriceAdult = 5.00m,
                    PriceUnder14 = 2.50m,
                    PricePensioner = 3.00m,
                    PriceDisabled = 0.00m
                },
                new RecreationalTicketType
                {
                    TypeName = "Weekly Ticket",
                    ValidityDays = 7,
                    PriceAdult = 20.00m,
                    PriceUnder14 = 10.00m,
                    PricePensioner = 12.00m,
                    PriceDisabled = 0.00m
                },
                new RecreationalTicketType
                {
                    TypeName = "Monthly Ticket",
                    ValidityDays = 30,
                    PriceAdult = 60.00m,
                    PriceUnder14 = 30.00m,
                    PricePensioner = 36.00m,
                    PriceDisabled = 0.00m
                },
                new RecreationalTicketType
                {
                    TypeName = "Annual Ticket",
                    ValidityDays = 365,
                    PriceAdult = 200.00m,
                    PriceUnder14 = 100.00m,
                    PricePensioner = 120.00m,
                    PriceDisabled = 0.00m
                }
            };

            context.RecreationalTicketTypes.AddRange(ticketTypes);
            await context.SaveChangesAsync();
        }

        // Create RecreationalFisherman records for any Person without one
        var personsWithoutFishermanRecord = await context.Persons
            .Where(p => !context.RecreationalFishermen.Any(rf => rf.PersonId == p.PersonId))
            .Where(p => !context.Administrators.Any(a => a.PersonId == p.PersonId))
            .Where(p => !context.Inspectors.Any(i => i.PersonId == p.PersonId))
            .Where(p => !context.ShipOwners.Any(so => so.PersonId == p.PersonId))
            .ToListAsync();

        if (personsWithoutFishermanRecord.Any())
        {
            var fishermanRecords = personsWithoutFishermanRecord.Select(p => new RecreationalFisherman
            {
                PersonId = p.PersonId,
                IsDisabled = false
            }).ToList();

            context.RecreationalFishermen.AddRange(fishermanRecords);
            await context.SaveChangesAsync();
        }
    }
}
