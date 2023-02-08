using chessAPI.dataAccess.common;
using chessAPI.dataAccess.interfaces;
using chessAPI.dataAccess.models;
using chessAPI.models.game;
using Dapper;
using static Dapper.SqlMapper;

namespace chessAPI.dataAccess.repositores;

public sealed class clsGameRepository<TI, TC> : clsDataAccess<clsGameEntityModel<TI, TC>, TI, TC>, IGameRepository<TI, TC>
        where TI : struct, IEquatable<TI>
        where TC : struct
{
    public clsGameRepository(IRelationalContext<TC> rkm,
                               ISQLData queries,
                               ILogger<clsGameRepository<TI, TC>> logger) : base(rkm, queries, logger)
    {
    }

    public async Task<TI> addGame(clsNewGame game)
    {
        var p = new DynamicParameters();
        p.Add("STARTED", game.started);     
        p.Add("WHITES", game.whites);
        p.Add("BLACKS", game.blacks);
        p.Add("TURN", game.turn);
        p.Add("WINNER", game.winner);
        return await add<TI>(p).ConfigureAwait(false);
    }

    public Task deleteGame(TI id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<clsGameEntityModel<TI, TC>>> getAllGames()
    {
        var p = new DynamicParameters();
        return await getALL(p).ConfigureAwait(false);
    }

    public async Task<clsGameEntityModel<TI, TC>> getGame(TI id)
    {
        return await getEntity(id);
    }

    public async Task<TI> startGame(clsNewGame game)
    {
        var p = new DynamicParameters();
        p.Add("STARTED", game.started);
        p.Add("WHITES", game.whites);
        p.Add("BLACKS", null);
        p.Add("TURN", null);
        p.Add("WINNER", null);

        //existTeam
        return await add<TI>(p).ConfigureAwait(false);
    }

    public async Task<bool> updateGame(clsGame<TI> updatedGame)
    {
        var upd = new DynamicParameters();
        upd.Add("STARTED", updatedGame.started);
        upd.Add("WHITES", updatedGame.whites);
        upd.Add("BLACKS", updatedGame.blacks);
        upd.Add("TURN", updatedGame.turn);
        upd.Add("WINNER", updatedGame.winner);
        upd.Add("ID", updatedGame.id);
        return await update(upd, null).ConfigureAwait(false);
    }

    protected override DynamicParameters fieldsAsParams(clsGameEntityModel<TI, TC> entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        var p = new DynamicParameters();
        p.Add("ID", entity.id);
        p.Add("STARTED", entity.started);
        p.Add("WHITES", entity.whites);
        p.Add("BLACKS", entity.blacks);
        p.Add("TURN", entity.turn);
        p.Add("WINNER", entity.winner);        
        return p;
    }

    protected override DynamicParameters keyAsParams(TI key)
    {
        var p = new DynamicParameters();
        p.Add("ID", key);
        return p;
    }

}