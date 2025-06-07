using System.Runtime.Serialization;
using QuoridorLib.Interfaces;
using QuoridorLib.Models;

namespace Persistence.Persistence
{
    internal class PlayersPersistence : IPlayersPersistence
    {
         public List<Player> LoadPlayers(string path)
        {
            if (!File.Exists(path))
                return [];

            var serializer = new DataContractSerializer(typeof(List<Player>));

            using var stream = File.OpenRead(path);

            var readedobject = serializer.ReadObject(stream);

            if (readedobject == null)
                return [];

            return (List<Player>)readedobject;
        }

        public void SavePlayers(List<Player> players, string path)
        {
            var serializer = new DataContractSerializer(typeof(List<Player>));
            using var stream = File.Create(path);
            serializer.WriteObject(stream, players);
        }
    }
}
