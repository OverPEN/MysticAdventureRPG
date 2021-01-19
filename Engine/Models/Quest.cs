using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Models
{
    public class Quest
    {
        public int QuestID { get; }
        [JsonIgnore]
        public string Name { get; }
        [JsonIgnore]
        public string Description { get; }
        [JsonIgnore]
        public List<GroupedItem> ItemsToComplete { get;}
        [JsonIgnore]
        public int RewardExperiencePoints { get; }
        [JsonIgnore]
        public int RewardGold { get; }
        [JsonIgnore]
        public List<GroupedItem> RewardItems { get; }
        [JsonIgnore]
        public string ToolTipContents =>
            Description + Environment.NewLine + Environment.NewLine +
            "Consegnare:" + Environment.NewLine +
            "===========================" + Environment.NewLine +
            string.Join(Environment.NewLine, ItemsToComplete.Select(i => i.GroupedItemDescription)) +
            Environment.NewLine + Environment.NewLine +
            "Ricompense:" + Environment.NewLine +
            "===========================" + Environment.NewLine +
            $"{RewardExperiencePoints} XP" + Environment.NewLine +
            $"{RewardGold} Oro" + Environment.NewLine +
            string.Join(Environment.NewLine, RewardItems.Select(i => i.GroupedItemDescription));

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
