using System;
using System.Collections.Generic;

namespace Yna.Samples
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class PlayerScore
    {
        public string Pseudonyme { get; set; }
        public long TimePlayed { get; set; }
        public List<int> Scores { get; set; }

        public PlayerScore()
        {
            Pseudonyme = "John Doe";
            TimePlayed = 0;
            Scores = new List<int>();
        }

        public PlayerScore(string pseudo, long timePlayed, int[] scores)
        {
            Pseudonyme = pseudo;
            TimePlayed = timePlayed;
            Scores = new List<int>(scores.Length);
            Scores.AddRange(scores);
        }

        /// <summary>
        /// Get the current score of the player
        /// </summary>
        /// <returns></returns>
        public int GetCurrentScore()
        {
            int size = Scores.Count;
            
            if (size > 0)
                return Scores[size - 1];

            return 0;
        }

        /// <summary>
        /// Get the best score of the player
        /// </summary>
        /// <returns></returns>
        public int GetBestScore()
        {
            int bestScore = 0;

            foreach (int score in Scores)
            {
                if (score > bestScore)
                    bestScore = score;
            }

            return bestScore;
        }

        /// <summary>
        /// Get a formated string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            int bestScore = GetBestScore();
            return String.Format("Player {0}\nTime played: {1} seconds\nBest score: {2} point{3}", new object[] { Pseudonyme, TimePlayed, bestScore, bestScore > 0 ? "s" : "" });
        }
    }
}
