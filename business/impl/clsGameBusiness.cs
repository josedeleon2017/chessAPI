using chessAPI.business.interfaces;
using chessAPI.dataAccess.repositores;
using chessAPI.models.game;

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

    public Task<IEnumerable<clsGame<TI>>> getAllGames()
    {
        throw new NotImplementedException();
    }

    public Task<clsGame<TI>> getGame(clsGame<TI> game)
    {
        throw new NotImplementedException();
    }

    public Task<clsGame<TI>> startGame(clsNewGame newGame)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> updateGame(clsGame<TI> game)
    {
        try
        {
            var x = await gameRepository.updateGame(game).ConfigureAwait(false);
            return x;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
