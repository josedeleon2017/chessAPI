using chessAPI.business.interfaces;
using chessAPI.dataAccess.queries.postgreSQL;
using chessAPI.dataAccess.repositores;
using chessAPI.models.game;
using chessAPI.models.player;

namespace chessAPI.business.impl;

public sealed class clsGameBusiness<TI, TC> : IGameBusiness<TI>
where TI : struct, IEquatable<TI>
where TC : struct
{
    internal readonly IGameRepository<TI, TC> gameRepository;

    public clsGameBusiness(IGameRepository<TI, TC> gameRepository)
    {
        this.gameRepository = gameRepository;
    }

    public async Task<clsGame<TI>> addGame(clsNewGame newGame)
    {
        var x = await gameRepository.addGame(newGame).ConfigureAwait(false);
        return new clsGame<TI>(x, newGame.started, newGame.whites, newGame.blacks, newGame.turn, newGame.winner);
    }

    public Task<bool> CompleteGame(clsGame<TI> game)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<clsGame<TI>>> getAllGames()
    {
        var x = await gameRepository.getAllGames().ConfigureAwait(false);
        var games = new List<clsGame<TI>>();
        x.ToList().ForEach(x => games.Add(new clsGame<TI>(x.id, x.started, x.whites, x.blacks, x.turn, x.winner)));
        return games;
    }

    public async Task<clsGame<TI>> getGame(clsGame<TI> game)
    {
        var x = await gameRepository.getGame(game.id).ConfigureAwait(false);
        return new clsGame<TI>(x.id, x.started, x.whites, x.blacks, x.turn, x.winner);
    }

    public async Task<clsGame<TI>> startGame(clsNewGame newGame)
    {
        newGame.started = DateTime.Now;
        var x = await gameRepository.startGame(newGame).ConfigureAwait(false);
        return new clsGame<TI>(x, newGame.started, newGame.whites, newGame.blacks, newGame.turn, newGame.winner);
    }

    public async Task<bool> updateGame(clsGame<TI> game)
    {
        try
        {
            var x = await gameRepository.updateGame(game).ConfigureAwait(false);
            return x;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
