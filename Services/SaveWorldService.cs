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
    public static class SaveWorldService
    {
        private const string WORLD_SAVE_FILE_NAME = "MARPGWD.json";

        public static void SaveWorld(World world)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "SaveFiles");
            Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, WORLD_SAVE_FILE_NAME), JsonConvert.SerializeObject(world, Formatting.Indented));
        }

        public static World LoadLastSave()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "SaveFiles");
            //Se il file di salvataggio del player non esiste ritorno null
            if (!File.Exists(Path.Combine(path, WORLD_SAVE_FILE_NAME)))
            {
                return null;
            }

            try
            {
                JObject data = JObject.Parse(File.ReadAllText(Path.Combine(path, WORLD_SAVE_FILE_NAME)));

                // Leggo i dati del PLayer dal file
                World world = CreateWorld(data);

                return world;
            }
            catch (Exception ex)
            {
                throw new InvalidDataException($"Dati di salvataggio del mondo di gioco non riconosciuti!");
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
            UpdateWorldStatus(data, world);

            return world;
        }

        private static void UpdateWorldStatus(JObject data, World world)
        {
            string fileVersion = FileVersion(data);

            switch (fileVersion)
            {
                case "1.0.0.0":
                    foreach(JToken location in (JArray)data[nameof(World.Locations)])
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

        private static string FileVersion(JObject data)
        {
            return typeof(Engine.Models.Player).Assembly.GetName().Version.ToString();
        }
    }
}
