using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuoridorLib.Interfaces;
using QuoridorLib.Models;

namespace QuoridorLib.Managers
{
    public class StubSaveManager : ISaveManager
    {
        public void SaveGame(Game game)
        {
            // Cette méthode est intentionnellement vide car c'est un stub utilisé uniquement pour les tests
            // En production, cette méthode devrait sauvegarder l'état du jeu
            throw new NotSupportedException("SaveGame n'est pas implémenté dans le stub");
        }

        public void SaveGameState(GameState gameState)
        {
            // Cette méthode est intentionnellement vide car c'est un stub utilisé uniquement pour les tests
            // En production, cette méthode devrait sauvegarder l'état complet du jeu
            throw new NotSupportedException("SaveGameState n'est pas implémenté dans le stub");
        }
    }
}
