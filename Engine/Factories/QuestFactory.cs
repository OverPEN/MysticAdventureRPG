using CommonClasses.Enums;
using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Factories
{
    internal static class QuestFactory
    {
        private static readonly List<Quest> _quests = new List<Quest>();

        static QuestFactory()
        {
            // Declare the items need to complete the quest, and its reward items
            List<Item> itemsToComplete = new List<Item>();
            List<Item> rewardItems = new List<Item>();

            itemsToComplete.Add(ItemFactory.GetItemByID(1).Clone(5));
            rewardItems.Add(ItemFactory.GetItemByID(1001).Clone());

            // Create the quest
            _quests.Add(new Quest(1, "Ripulisci il Prato", "Sconfiggi i serpenti nel prato", itemsToComplete, 25, 10, rewardItems, QuestStatus.Nuova));
        }

        internal static Quest GetQuestByID(int id)
        {
            return _quests.FirstOrDefault(quest => quest.QuestID == id);
        }
    }
}
