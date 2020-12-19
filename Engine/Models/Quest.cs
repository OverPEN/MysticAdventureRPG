using CommonClasses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Quest
    {
        public int QuestID { get; }
        public string Name { get; }
        public string Description { get; }
        public QuestStatus Status { get; set; }
        public List<Item> ItemsToComplete { get;}
        public int RewardExperiencePoints { get; }
        public int RewardGold { get; }
        public List<Item> RewardItems { get; }

        public Quest(int id, string name, string description, List<Item> itemsToComplete, int rewardExperiencePoints, int rewardGold, List<Item> rewardItems, QuestStatus status = QuestStatus.Inattiva)

        {
            QuestID = id;
            Name = name;
            Description = description;
            ItemsToComplete = itemsToComplete;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
            RewardItems = rewardItems;
            Status = status;
        }
    }
}
