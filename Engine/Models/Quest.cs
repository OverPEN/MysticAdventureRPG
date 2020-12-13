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
        public int QuestID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public QuestStatus Status { get; set; }
        public List<Item> ItemsToComplete { get; set; }
        public int RewardExperiencePoints { get; set; }
        public int RewardGold { get; set; }
        public List<Item> RewardItems { get; set; }

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
