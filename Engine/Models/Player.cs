using CommonClasses.BaseClasses;
using CommonClasses.Enums;
using Engine.Factories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Player : LivingEntity
    {
        public event EventHandler OnLeveledUp;

        #region Private Properties
        private int _experience;
        private int _baseDamage;
        private int _worldID;
        private int _xCoordinate;
        private int _yCoordinate;
        #endregion

        #region Public Properties
        public int WorldID
        {
            get { return _worldID; }
            set
            {
                _worldID = value;
                OnPropertyChanged();
            }
        }
        public int XCoordinate
        {
            get { return _xCoordinate; }
            set
            {
                _xCoordinate = value;
                OnPropertyChanged();
            }
        }
        public int YCoordinate
        {
            get { return _yCoordinate; }
            set
            {
                _yCoordinate = value;
                OnPropertyChanged();
            }
        }
        public int Experience
        {
            get { return _experience; }
            private set
            {
                _experience = value;
                OnPropertyChanged();
                SetLevelAndParameters();
            }
        }
        public int BaseDamage
        {
            get { return _baseDamage; }
            set
            {
                _baseDamage = value;
                OnPropertyChanged();
            }
        }
        [JsonIgnore]
        public WeaponDamageTypeEnum BaseDamageType { get;} 
        public ObservableCollection<QuestStatus> Quests { get; }
        public ObservableCollection<Recipe> Recipes { get; }

        #endregion

        public Player(string name, int maxHitPoints, int currHitPoints, float speed, int gold, int worldID, int xCoord, int yCoord, PlayerClassTypeEnum chosenClass, int baseDamage, byte level = 1, int experience = 0) : base(name, maxHitPoints, currHitPoints, speed, gold, chosenClass, level )
        {
            ClassBaseValues defaultValues = new ClassBaseValues(chosenClass);

            Experience = experience;
            BaseDamage = baseDamage;
            BaseDamageType = defaultValues.BaseDamageType;
            WorldID = worldID;
            XCoordinate = xCoord;
            YCoordinate = yCoord;
            Quests = new ObservableCollection<QuestStatus>();
            Recipes = new ObservableCollection<Recipe>();

        }

        public Player(string name, PlayerClassTypeEnum chosenClass) : base(name, new ClassBaseValues(chosenClass).HitPoints, new ClassBaseValues(chosenClass).HitPoints, new ClassBaseValues(chosenClass).Speed, new ClassBaseValues(chosenClass).Gold, chosenClass)
        {
            ClassBaseValues defaultValues = new ClassBaseValues(chosenClass);
            Level = 1;
            Experience = 0;
            BaseDamage = defaultValues.BaseDamage;
            BaseDamageType = defaultValues.BaseDamageType;
            WorldID = 0;
            XCoordinate = 0;
            YCoordinate = 0;
            Quests = new ObservableCollection<QuestStatus>();
            Recipes = new ObservableCollection<Recipe>();
        }

        #region Functions
        
        public void AddExperience(int experiencePoints)
        {
            Experience += experiencePoints;
        }

        private void SetLevelAndParameters()
        {
            byte originalLevel = Level;

            byte tempLevel = (byte)(Experience / (100 + ((Level-1)*50)) + 1);

            if (tempLevel > originalLevel)
            {
                Level = tempLevel;
                ClassBaseValues classBaseValues = new ClassBaseValues(Class);
                MaximumHitPoints = (Level * classBaseValues.HitPoints) - ((Level -1) *((classBaseValues.HitPoints / 5)*4));
                BaseDamage = (Level * classBaseValues.BaseDamage) - ((Level - 1) * ((classBaseValues.BaseDamage / 5) * 4));
                Speed = (Level * classBaseValues.Speed) - ((Level - 1) * ((classBaseValues.Speed / 5) * 4));
                OnLeveledUp?.Invoke(this, System.EventArgs.Empty);
            }
        }

        public void LearnRecipe(Recipe recipe)
        {
            if (!Recipes.Any(r => r.ID == recipe.ID))
            {
                Recipes.Add(recipe);
            }
        }

        public void ObtainQuest(QuestStatus questStatus)
        {
            if (!Quests.Any(r => r.Quest.QuestID == questStatus.Quest.QuestID))
            {
                Quests.Add(questStatus);
            }
        }
        #endregion
    }
}
