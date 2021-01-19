using CommonClasses.Enums;
using Engine.Factories;
using Engine.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Services
{
    public static class SavePlayerService
    {
        private const string PLAYER_SAVE_FILE_NAME = "MARPGPG.json";

        public static void SavePlayer(Player player)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "SaveFiles");
            Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, PLAYER_SAVE_FILE_NAME), JsonConvert.SerializeObject(player, Formatting.Indented));
        }

        public static Player LoadLastSave()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "SaveFiles");
            //Se il file di salvataggio del player non esiste ritorno null
            if (!File.Exists(Path.Combine(path, PLAYER_SAVE_FILE_NAME)))
            {
                return null;
            }

            try
            {
                JObject data = JObject.Parse(File.ReadAllText(Path.Combine(path, PLAYER_SAVE_FILE_NAME)));

                // Leggo i dati del PLayer dal file
                Player player = CreatePlayer(data);

                return player;
            }
            catch (Exception ex)
            {
                throw new InvalidDataException($"Dati di salvataggio del giocatore non riconosciuti!");
            }
        }

        private static Player CreatePlayer(JObject data)
        {
            string fileVersion = FileVersion(data);

            Player player;

            switch (fileVersion)
            {
                case "1.0.0.0":
                    player = new Player((string)data[nameof(Player.Name)], 
                                        (int)data[nameof(Player.MaximumHitPoints)], 
                                        (int)data[nameof(Player.CurrentHitPoints)], 
                                        (float)data[nameof(Player.Speed)], 
                                        (int)data[nameof(Player.Gold)], 
                                        (int)data[nameof(Player.WorldID)], 
                                        (int)data[nameof(Player.XCoordinate)], 
                                        (int)data[nameof(Player.YCoordinate)], 
                                        (PlayerClassTypeEnum)(byte)data[nameof(Player.Class)],
                                        (int)data[nameof(Player.BaseDamage)],
                                        (byte)data[nameof(Player.Level)], 
                                        (int)data[nameof(Player.Experience)]);

                    break;
                default:
                    throw new InvalidDataException($"Versione '{fileVersion}' non riconosciuta!");
            }

            //Popolo l'Inventario del Player
            PopulatePlayerInventory(data, player);

            //Popolo le Quest del Player
            PopulatePlayerQuests(data, player);

            //Popolo le Recipes del Player
            PopulatePlayerRecipes(data, player);

            return player;
        }

        private static void PopulatePlayerInventory(JObject data, Player player)
        {
            string fileVersion = FileVersion(data);

            switch (fileVersion)
            {
                case "1.0.0.0":
                    foreach (JToken groupedItem in (JArray)data[nameof(Player.GroupedInventory)])
                    {
                        int itemId = (int)groupedItem[nameof(GroupedItem.Item)][nameof(GroupedItem.Item.ItemID)];
                        byte quantity = (byte)groupedItem[nameof(GroupedItem.Quantity)];

                        player.AddItemToInventory(ItemFactory.ObtainItem(itemId, quantity));
                    }

                    break;
                default:
                    throw new InvalidDataException($"Versione '{fileVersion}' non riconosciuta!");
            }
        }

        private static void PopulatePlayerQuests(JObject data, Player player)
        {
            string fileVersion = FileVersion(data);

            switch (fileVersion)
            {
                case "1.0.0.0":
                    foreach (JToken questToken in (JArray)data[nameof(Player.Quests)])
                    {
                        int questId = (int)questToken[nameof(QuestStatus.Quest)][nameof(QuestStatus.Quest.QuestID)];
                        QuestStatusEnum status = (QuestStatusEnum)(byte)questToken[nameof(QuestStatus.Status)];

                        Quest quest = QuestFactory.GetQuestByID(questId);
                        QuestStatus questStatus = new QuestStatus(quest, status);
                        player.ObtainQuest(questStatus);
                    }

                    break;
                default:
                    throw new InvalidDataException($"Versione '{fileVersion}' non riconosciuta!");
            }
        }

        private static void PopulatePlayerRecipes(JObject data, Player player)
        {
            string fileVersion = FileVersion(data);

            switch (fileVersion)
            {
                case "1.0.0.0":
                    foreach (JToken recipeToken in (JArray)data[nameof(Player.Recipes)])
                    {
                        int recipeId = (int)recipeToken[nameof(Recipe.ID)];

                        Recipe recipe = RecipeFactory.RecipeByID(recipeId);
                        player.LearnRecipe(recipe);
                    }

                    break;
                default:
                    throw new InvalidDataException($"Versione '{fileVersion}' non riconosciuta!");
            }
        }

        private static string FileVersion(JObject data)
        {
            return typeof(Engine.Models.Player).Assembly.GetName().Version.ToString();
        }
    }
}
