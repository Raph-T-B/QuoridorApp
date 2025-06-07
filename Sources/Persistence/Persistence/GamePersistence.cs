using System.Runtime.Serialization;
using QuoridorLib.Interfaces;
using QuoridorLib.Models;

namespace Persistence.Persistence
{
    internal class GamePersistence : IGamesPersistence
    {
        public List<Game> LoadGames(string path)
        {
            if (!File.Exists(path))
                return [];

            var serializer = new DataContractSerializer(typeof(List<Game>));

            using var stream = File.OpenRead(path);
            
            var readedobject = serializer.ReadObject(stream);
            
            if (readedobject == null) 
                return [];     
            
            return (List<Game>)readedobject;
        }

        public void SaveGames(List<Game> games, string path)
        {
            var serializer = new DataContractSerializer(typeof(List<Game>));
            using var stream = File.Create(path);
            serializer.WriteObject(stream, games);
        } 
    }
}
