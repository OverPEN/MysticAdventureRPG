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
            List<GroupedItem> itemsToComplete = new List<GroupedItem>();
            List<GroupedItem> itemsToComplete2 = new List<GroupedItem>();
            List<GroupedItem> rewardItems = new List<GroupedItem>();

            itemsToComplete.Add(ItemFactory.ObtainItem(1,2));
            itemsToComplete2.Add(ItemFactory.ObtainItem(1, 5));
            rewardItems.Add(ItemFactory.ObtainItem(1005,2));

            // Create the quest
            _quests.Add(new Quest(1, "Ripulisci il Prato", "Sconfiggi i serpenti nel prato", itemsToComplete, 25, 10, rewardItems, QuestStatus.Nuova));
            _quests.Add(new Quest(2, "Quest 2", "Quest 2", itemsToComplete2, 25, 10, rewardItems, QuestStatus.Nuova));
        }

        internal static Quest GetQuestByID(int id)
        {
            return _quests.FirstOrDefault(quest => quest.QuestID == id);
        }
    }
}
