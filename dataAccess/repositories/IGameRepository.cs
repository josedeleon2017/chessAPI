using chessAPI.dataAccess.models;
using chessAPI.models.game;
using chessAPI.models.player;

namespace chessAPI.dataAccess.repositores;

public interface IGameRepository<TI, TC>
        where TI : struct, IEquatable<TI>
        where TC : struct
{
    Task<TI> addGame(clsNewGame game);
    Task<TI> startGame(clsNewGame game);
    Task<IEnumerable<clsGameEntityModel<TI, TC>>> getAllGames();
    Task<bool> updateGame(clsGame<TI> updatedGame);
    Task deleteGame(TI id);
    Task<clsGameEntityModel<TI, TC>> getGame(TI id);

}