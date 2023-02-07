using Autofac;
using Autofac.Extensions.DependencyInjection;
using chessAPI;
using chessAPI.business.interfaces;
using chessAPI.models.player;
using chessAPI.models.game;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Serilog.Events;
using chessAPI.dataAccess.queries.postgreSQL;

//Serilog logger (https://github.com/serilog/serilog-aspnetcore)
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("chessAPI starting");
    var builder = WebApplication.CreateBuilder(args);

    var connectionStrings = new connectionStrings();
    builder.Services.AddOptions();
    builder.Services.Configure<connectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
    builder.Configuration.GetSection("ConnectionStrings").Bind(connectionStrings);

    // Two-stage initialization (https://github.com/serilog/serilog-aspnetcore)
    builder.Host.UseSerilog((context, services, configuration) => configuration.ReadFrom
             .Configuration(context.Configuration)
             .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning).ReadFrom
             .Services(services).Enrich
             .FromLogContext().WriteTo
             .Console());

    // Autofac como inyección de dependencias
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new chessAPI.dependencyInjection<int, int>()));
    var app = builder.Build();
    app.UseSerilogRequestLogging();
    app.UseMiddleware(typeof(chessAPI.customMiddleware<int>));
    app.MapGet("/", () =>
    {
        return "hola mundo";
    });

    //Repo original
    app.MapPost("player", 
    [AllowAnonymous] async(IPlayerBusiness<int> bs, clsNewPlayer newPlayer) => Results.Ok(await bs.addPlayer(newPlayer)));

    //Funcionalidad incremental

    //Player
    app.MapPost("playerById",
    [AllowAnonymous] async (IPlayerBusiness<int> bs, clsPlayer<int> player) => Results.Ok(await bs.getPlayer(player)));

    app.MapGet("getAllPlayers",
    [AllowAnonymous] async (IPlayerBusiness<int> bs) => Results.Ok(await bs.getAllPlayers()));

    app.MapPut("updatePlayer",
    [AllowAnonymous] async (IPlayerBusiness<int> bs, clsPlayer<int> player) => await bs.updatePlayer(player) ? Results.Ok(await bs.updatePlayer(player)) : Results.BadRequest(await bs.updatePlayer(player)));

    //Game
    app.MapPost("addGame",
    [AllowAnonymous] async (IGameBusiness<int> bs, clsNewGame game) => Results.Ok(await bs.addGame(game)));

    app.MapPut("updateGame",
    [AllowAnonymous] async (IGameBusiness<int> bs, clsGame<int> game) => await bs.updateGame(game) ? Results.Ok(await bs.updateGame(game)) : Results.BadRequest(await bs.updateGame(game)));


    //app.MapPost("getAllGames",
    //[AllowAnonymous] async (IGameBusiness<int> bs, clsPlayer<int> player) => Results.Ok(await bs.getPlayer(player)));

    //Repository pattern
    //app.MapPost("startGame",
    //[AllowAnonymous] async (IGameBusiness<int> bs, clsPlayer<int> player) => Results.Ok(await bs.getPlayer(player)));
    //app.MapPut("completeGame",
    //[AllowAnonymous] async (IGameBusiness<int> bs, clsPlayer<int> player) => Results.Ok(await bs.getPlayer(player)));



    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "chessAPI terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
