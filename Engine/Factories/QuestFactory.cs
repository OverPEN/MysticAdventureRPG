using CommonClasses.ExtensionMethods;
using Engine.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Engine.Factories
{
    public static class QuestFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Quests.xml";
        private static readonly List<Quest> _quests = new List<Quest>();

        static QuestFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                LoadQuestsFromNodes(data.SelectNodes("/Quests/Quest"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }

        #region Functions
        private static void LoadQuestsFromNodes(XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                List<GroupedItem> itemsToComplete = new List<GroupedItem>();
                List<GroupedItem> rewardItems = new List<GroupedItem>();

                foreach (XmlNode childNode in node.SelectNodes("./ItemsToComplete/Item"))
                {
                    itemsToComplete.Add(new GroupedItem(ItemFactory.GetItemByID(childNode.GetXmlAttributeAsInt(nameof(Item.ItemID))), childNode.GetXmlAttributeAsByte(nameof(GroupedItem.Quantity))));
                }

                foreach (XmlNode childNode in node.SelectNodes("./RewardItems/Item"))
                {
                    rewardItems.Add(new GroupedItem(ItemFactory.GetItemByID(childNode.GetXmlAttributeAsInt(nameof(Item.ItemID))), childNode.GetXmlAttributeAsByte(nameof(GroupedItem.Quantity))));
                }

                _quests.Add(new Quest(node.GetXmlAttributeAsInt(nameof(Quest.QuestID)), node.SelectSingleNode($"./{nameof(Quest.Name)}")?.InnerText ?? "", node.SelectSingleNode($"./{nameof(Quest.Description)}")?.InnerText ?? "", itemsToComplete, node.GetXmlAttributeAsInt(nameof(Quest.RewardExperiencePoints)), node.GetXmlAttributeAsInt(nameof(Quest.RewardGold)), rewardItems));
            }
        }

        public static Quest GetQuestByID(int id)
        {
            return _quests.FirstOrDefault(quest => quest.QuestID == id);
        }
        #endregion
    }
}
