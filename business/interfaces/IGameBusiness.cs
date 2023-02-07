using chessAPI.dataAccess.models;
using chessAPI.models.game;

namespace chessAPI.business.interfaces;

public interface IGameBusiness<TI>
    where TI : struct, IEquatable<TI>
{
    //Funcionalidad Incremental (tarea 1)
    Task<clsGame<TI>> addGame(clsNewGame newGame);
    Task<clsGame<TI>> getGame(clsGame<TI> game);
    Task<IEnumerable<clsGame<TI>>> getAllGames();
    Task<bool> updateGame(clsGame<TI> game);

    //Repository Pattern (tarea 2)
    Task<clsGame<TI>> startGame(clsNewGame newGame);
    Task<bool> CompleteGame(clsGame<TI> game);


}