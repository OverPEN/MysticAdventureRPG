using CommonClasses.BaseClasses;
using CommonClasses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class QuestStatus : BaseNotifyPropertyChanged
    {
        private QuestStatusEnum _status;

        public Quest Quest { get; set; }
        public QuestStatusEnum Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public QuestStatus(Quest quest, QuestStatusEnum status)
        {
            Quest = quest;
            Status = status;
        }
    }
}
