﻿using CommonClasses.BaseClasses;
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
        public int LocationID { get; }
        public int XCoordinate { get; }
        public int YCoordinate { get; }
        public string Name { get; }
        public string Description { get; }
        public string ImageName { get; }
        public List<Quest> QuestsAvailableHere { get; } = new List<Quest>();
        public List<EnemyEncounter> EnemiesHere { get; } = new List<EnemyEncounter>();
        public Trader TraderHere { get; set; }

        public Location(int id, int xCoord, int yCoord, string name, string description, string imageName)
        {
            LocationID = id;
            XCoordinate = xCoord;
            YCoordinate = yCoord;
            Name = name;
            Description = description;
            ImageName = imageName;
        }

        #region Functions
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

        public void AddEnemyToLocation(int enemyID, int encounterRate)
        {
            if (EnemiesHere.Exists(m => m.EnemyID == enemyID))
            {
                // In caso l'Enemy sia già presente nella location ne aumento l'EncounterRate.
                EnemiesHere.First(m => m.EnemyID == enemyID).EncounterRate += (encounterRate / 3);
            }
            else
            {
                // Se l'Enemy non esiste lo aggiungo alla Location
                EnemiesHere.Add(new EnemyEncounter(enemyID, encounterRate));
            }
        }

        public Enemy GetEnemyAtLocation()
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

            foreach (EnemyEncounter enemy in EnemiesHere)
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

        public void AddTraderToLocation(int traderID)
        {
            if (TraderHere == null)
                TraderHere = TraderFactory.GetTraderByID(traderID);
            else
                throw new InvalidOperationException($"Nella Location {Name} è già presente un Trader!");

        }
        #endregion
    }
}
