using chessAPI.business.interfaces;
using chessAPI.dataAccess.repositores;
using chessAPI.models.player;
using System.Net;

namespace chessAPI.business.impl;

public sealed class clsPlayerBusiness<TI, TC> : IPlayerBusiness<TI> 
    where TI : struct, IEquatable<TI>
    where TC : struct
{
    internal readonly IPlayerRepository<TI, TC> playerRepository;

    public clsPlayerBusiness(IPlayerRepository<TI, TC> playerRepository)
    {
        this.playerRepository = playerRepository;
    }

    public async Task<clsPlayer<TI>> addPlayer(clsNewPlayer newPlayer)
    {
        var x = await playerRepository.addPlayer(newPlayer).ConfigureAwait(false);
        return new clsPlayer<TI>(x, newPlayer.email);
    }

    public async Task<IEnumerable<clsPlayer<TI>>> getAllPlayers()
    {
        var x = await playerRepository.getAllPlayers().ConfigureAwait(false);
        var players = new List<clsPlayer<TI>>();
        x.ToList().ForEach(x => players.Add(new clsPlayer<TI>(x.id, x.email)));
        return players;
    }

    public async Task<clsPlayer<TI>> getPlayer(clsPlayer<TI> player)
    {
        var x = await playerRepository.getPlayer(player.id).ConfigureAwait(false);
        return new clsPlayer<TI>(x.id, x.email);
    }

    public async Task<bool> updatePlayer(clsPlayer<TI> player)
    {
        try
        {
            var x = await playerRepository.updatePlayer(player).ConfigureAwait(false);
            return x;
        }
        catch (Exception)
        {
            return false;
        }
    }
}