using System;

namespace QuoridorLib.Models
{
    public class BestOf
    {
        private int scoreJoueur1;
        private int scoreJoueur2;

        public BestOf(int nbParties)
        {
            scoreJoueur1 = 0;
            scoreJoueur2 = 0;
        }

        public void AjouterVictoireJoueur1()
        {
            scoreJoueur1++;
        }

        public void AjouterVictoireJoueur2()
        {
            scoreJoueur2++;
        }

        public int GetScoreJoueur1()
        {
            return scoreJoueur1;
        }

        public int GetScoreJoueur2()
        {
            return scoreJoueur2;
        }

    }
} 