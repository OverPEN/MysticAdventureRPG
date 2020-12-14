using CommonClasses.BaseClasses;
using Engine.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Location
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public List<Quest> QuestsAvailableHere { get; set; } = new List<Quest>();
        public List<Enemy> EnemiesHere { get; set; } = new List<Enemy>();

        public void AddQuestToLocation(int questID)
        {
            if (QuestsAvailableHere.Exists(m => m.QuestID == questID))
            {
                return;
            }
            else
            {
                QuestsAvailableHere.Add(QuestFactory.GetQuestByID(questID));
            }
        }

        public void AddEnemyToLocation(int enemyID, int encounterRate = 0)
        {
            if (EnemiesHere.Exists(m => m.EnemyID == enemyID))
            {
                // In caso l'Enemy sia già presente nella location ne aumento l'EncounterRate.
                EnemiesHere.First(m => m.EnemyID == enemyID)
                            .EncounterRate += (encounterRate/3);
            }
            else
            {
                // Se l'Enemy non esiste lo aggiungo alla Location
                EnemiesHere.Add(EnemyFactory.GetEnemyByID(enemyID));
            }
        }

        public Enemy GetEnemy()
        {
            if (!EnemiesHere.Any())
            {
                return null;
            }

            // Genero un numero casuale tra 1 e l'EncounterRate totale.
            int randomNumber = BaseRandomNumberGenerator.NumberBetween(1, EnemiesHere.Sum(m => m.EncounterRate));

            // Eseguo un loop degli Enemies presenti nella Location, sommo ogni EncounterRate nella runningTotal
            // Quando il numero random è minore di runningTotal ritorno quell'Enemy
            int runningTotal = 0;

            foreach (Enemy enemy in EnemiesHere)
            {
                runningTotal += enemy.EncounterRate;

                if (randomNumber <= runningTotal)
                {
                    return EnemyFactory.GetEnemyByID(enemy.EnemyID);
                }
            }

            // In caso di problemi ritorno il primo Enemy della lista.
            return EnemyFactory.GetEnemyByID(EnemiesHere.First().EnemyID);
        }
    }
}
