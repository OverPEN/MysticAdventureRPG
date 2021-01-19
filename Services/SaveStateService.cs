using CommonClasses.Enums;
using Engine.Factories;
using Engine.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace Services
{
    public static class SaveStateService
    {
        public static void SaveState(SaveState saveState, string filename)
        {   
            Directory.CreateDirectory(Application.Current.Properties["SAVE_GAME_FILES_FOLDER"].ToString());
            File.WriteAllText(Path.Combine(Application.Current.Properties["SAVE_GAME_FILES_FOLDER"].ToString(), filename), JsonConvert.SerializeObject(saveState, Formatting.Indented));
        }

        public static SaveState LoadSave(string filename)
        {
            //Se il file di salvataggio non esiste ritorno null
            if (!File.Exists(Path.Combine(Application.Current.Properties["SAVE_GAME_FILES_FOLDER"].ToString(), filename)))
            {
                return null;
            }

            try
            {
                JObject data = JObject.Parse(File.ReadAllText(Path.Combine(Application.Current.Properties["SAVE_GAME_FILES_FOLDER"].ToString(), filename)));

                // Leggo i dati del PLayer e del Mondo di Gioco dal file
                JObject playerToken = (JObject)data[nameof(Player)];
                JObject locationsToken = (JObject)data[nameof(World)];

                Player player = CreatePlayer(playerToken);
                World world = CreateWorld(locationsToken);

                return new SaveState(player, world);
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

        private static World CreateWorld(JObject data)
        {
            string fileVersion = FileVersion(data);

            World world;

            switch (fileVersion)
            {
                case "1.0.0.0":
                    world = WorldFactory.CreateWorld();
                    break;
                default:
                    throw new InvalidDataException($"Versione '{fileVersion}' non riconosciuta!");
            }

            //Popolo l'Inventario del Player
            UpdateWorldQuestStatus(data, world);

            return world;
        }

        private static void UpdateWorldQuestStatus(JObject data, World world)
        {
            string fileVersion = FileVersion(data);

            switch (fileVersion)
            {
                case "1.0.0.0":
                    foreach (JToken location in (JArray)data[nameof(World.Locations)])
                    {
                        int locationID = (int)location[nameof(Location.LocationID)];

                        foreach (JToken questStatus in (JArray)location[nameof(Location.QuestsAvailableHere)])
                        {
                            int questId = (int)questStatus[nameof(QuestStatus.Quest)][nameof(QuestStatus.Quest.QuestID)];
                            QuestStatusEnum status = (QuestStatusEnum)(byte)questStatus[nameof(QuestStatus.Status)];

                            world.GetLocationByID(locationID).QuestsAvailableHere.FirstOrDefault(w => w.Quest.QuestID == questId).Status = status;
                        }
                    }
                    break;
                default:
                    throw new InvalidDataException($"Versione '{fileVersion}' non riconosciuta!");
            }
        }

        private static void UpdateWorldTraderStatus(JObject data, World world)
        {
            string fileVersion = FileVersion(data);

            switch (fileVersion)
            {
                case "1.0.0.0":
                    foreach (JToken location in (JArray)data[nameof(World.Locations)])
                    {
                        int locationID = (int)location[nameof(Location.LocationID)];
                        JToken traderStatus = (JObject)location[nameof(Location.TraderHere)];

                        int traderId = (int)traderStatus[nameof(Trader.TraderID)];
                        ObservableCollection<GroupedItem> groupedInventory = new ObservableCollection<GroupedItem>();

                        foreach (JToken groupedItem in (JArray)data[nameof(Trader.GroupedInventory)])
                        {
                            int itemId = (int)groupedItem[nameof(GroupedItem.Item)][nameof(GroupedItem.Item.ItemID)];
                            byte quantity = (byte)groupedItem[nameof(GroupedItem.Quantity)];

                            groupedInventory.Add(ItemFactory.ObtainItem(itemId, quantity));
                        }

                        world.GetLocationByID(locationID).TraderHere.GroupedInventory = groupedInventory;

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
