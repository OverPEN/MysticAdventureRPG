﻿using CommonClasses.Enums;
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
        public List<GroupedItem> ItemsToComplete { get;}
        public int RewardExperiencePoints { get; }
        public int RewardGold { get; }
        public List<GroupedItem> RewardItems { get; }

        public Quest(int id, string name, string description, List<GroupedItem> itemsToComplete, int rewardExperiencePoints, int rewardGold, List<GroupedItem> rewardItems)

        {
            QuestID = id;
            Name = name;
            Description = description;
            ItemsToComplete = itemsToComplete;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
            RewardItems = rewardItems;
        }
    }
}
