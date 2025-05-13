using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuoridorLib.Interfaces;
using QuoridorLib.Models;

namespace QuoridorLib.Managers
{
    public class StubLoadManager : ILoadManager
    {
        public Game LoadGame()
        {
            // Cette méthode retourne un nouveau jeu vide car c'est un stub utilisé uniquement pour les tests
            // En production, cette méthode devrait charger un jeu existant
            throw new NotSupportedException("LoadGame n'est pas implémenté dans le stub");
        }

        public GameState LoadGameState()
        {
            // Cette méthode retourne un nouvel état vide car c'est un stub utilisé uniquement pour les tests
            // En production, cette méthode devrait charger l'état d'un jeu existant
            throw new NotSupportedException("LoadGameState n'est pas implémenté dans le stub");
        }
    }
}
