using Microsoft.EntityFrameworkCore;
using Pr_UP;
using Pr_UP.Models;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        // добавление таблицы в контекст ASP.NET
        builder.Services.AddDbContext<IceRinkDB>();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(); // подключение свагера 
            app.UseSwaggerUI();
        }

        app.MapEntityEndpoints<Users, IceRinkDB>("/Users");
        app.MapEntityEndpoints<Tickets, IceRinkDB>("/Tickets");
        app.MapEntityEndpoints<TicketType, IceRinkDB>("/TicketType");
        app.MapEntityEndpoints<Equipments, IceRinkDB>("/Equipments");
        app.MapEntityEndpoints<EquipmentType, IceRinkDB>("/EquipmentType");
        app.MapEntityEndpoints<Rental, IceRinkDB>("/Rental");
        app.MapEntityEndpoints<Booking, IceRinkDB>("/Booking");
        app.MapEntityEndpoints<Schedule, IceRinkDB>("/Schedule");
        app.MapEntityEndpoints<Pass, IceRinkDB>("/Pass");
        app.MapEntityEndpoints<Qualification, IceRinkDB>("/Qualification");
        app.MapEntityEndpoints<Coaches, IceRinkDB>("/Coaches");
        app.MapEntityEndpoints<Training, IceRinkDB>("/Training");
        app.Run();
    }
}

public static class EntityEndpoints
{
    public static void MapEntityEndpoints<TEntity, TDbContext>(this WebApplication app, string routePrefix)
            where TEntity : class, IEntity
            where TDbContext : DbContext
    {
        app.MapGet(routePrefix, async (TDbContext dbContext) =>
        {
            var entities = await dbContext.Set<TEntity>().ToListAsync();
            return entities;
        });

        app.MapGet($"{routePrefix}/{{ID}}", async (int id, TDbContext dbContext) =>
        {
            var entity = await dbContext.Set<TEntity>().FindAsync(id);
            if (entity == null) return Results.NotFound();
            return Results.Ok(entity);
        });

        app.MapPost(routePrefix, async (TDbContext dbContext, TEntity entity) =>
        {
            dbContext.Set<TEntity>().Add(entity);
            await dbContext.SaveChangesAsync();
            return Results.Created($"{routePrefix}/{entity.ID}", entity);
        });

        app.MapPut($"{routePrefix}/{{ID}}", async (int id, TDbContext dbContext, TEntity updatedEntity) =>
        {
            var existingEntity = await dbContext.Set<TEntity>().FindAsync(id);
            if (existingEntity == null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
            await dbContext.SaveChangesAsync();

            return Results.Ok(updatedEntity);
        });



        app.MapDelete($"{routePrefix}/{{ID}}", async (int id, TDbContext dbContext) =>
        {
            if (await dbContext.Set<TEntity>().FindAsync(id) is TEntity entity)
            {
                dbContext.Set<TEntity>().Remove(entity);
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            }
            return Results.NotFound();
        });
    }
}