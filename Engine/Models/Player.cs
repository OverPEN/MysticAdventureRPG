using CommonClasses.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Player : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private String _name { get; set; }
        private String _surname { get; set; }
        private PlayerClassType _class { get; set; }
        private int _hitPoints { get; set; }
        private int _baseDamage { get; set; }
        private DamageType _baseDamageType { get; set; }
        private float _speed { get; set; }
        private Byte _level { get; set; }
        private int _experience { get; set; }
        private int _gold { get; set; }

        public Player(string chosenName, string chosenSurname, PlayerClassType chosenClass)
        {
            PlayerClassDefaultValues defaultValues = new PlayerClassDefaultValues(chosenClass);

            Name = chosenName;
            Surname = chosenSurname;
            Class = chosenClass;
            Level = 1;
            Experience = 0;
            BaseDamage = defaultValues.BaseDamage;
            BaseDamageType = defaultValues.BaseDamageType;
            Speed = defaultValues.Speed;
            HitPoints = defaultValues.HitPoints;
            Gold = defaultValues.Gold;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                OnPropertyChanged("Surname");
            }
        }
        public PlayerClassType Class
        {
            get { return _class; }
            set
            {
                _class = value;
                OnPropertyChanged("Class");
            }
        }
        public byte Level
        {
            get { return _level; }
            set
            {
                _level = value;
                OnPropertyChanged("Level");
            }
        }
        public int Experience
        {
            get { return _experience; }
            set
            {
                _experience = value;
                OnPropertyChanged("Experience");
            }
        }
        public int BaseDamage
        {
            get { return _baseDamage; }
            set
            {
                _baseDamage = value;
                OnPropertyChanged("BaseDamage");
            }
        }
        public DamageType BaseDamageType
        {
            get { return _baseDamageType; }
            set
            {
                _baseDamageType = value;
                OnPropertyChanged("BaseDamageType");
            }
        }
        public float Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                OnPropertyChanged("Speed");
            }
        }
        public int HitPoints
        {
            get { return _hitPoints; }
            set
            {
                _hitPoints = value;
                OnPropertyChanged("HitPoints");
            }
        }
        public int Gold
        {
            get { return _gold; }
            set
            {
                _gold = value;
                OnPropertyChanged("Gold");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
