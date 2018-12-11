using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AssignmentAPI.Data.Entities;

namespace AssignmentAPI.Services
{
    public class SQLAssignmentData : IAssignmentData
    {
        public bool AddMatch(MatchEntity match)
        {
            throw new NotImplementedException();
        }

        public bool AddPlayer(PlayerEntity player)
        {
            throw new NotImplementedException();
        }

        public bool AddPlayerToMatch(MatchPlayerEntity matchPlayer)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MatchEntity> GetAllMatches()
        {
            //https://stackoverflow.com/questions/38752848/paging-the-huge-data-that-is-returned-by-the-web-api
            throw new NotImplementedException();
        }

        public IEnumerable<PlayerEntity> GetAllPlayers()
        {
            throw new NotImplementedException();
        }

        public MatchEntity GetMatch(int matchId)
        {
            throw new NotImplementedException();
        }

        public PlayerEntity GetPlayer(int playerId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MatchPlayerEntity> GetPlayersInMatch(int matchId)
        {
            throw new NotImplementedException();
        }
    }
}