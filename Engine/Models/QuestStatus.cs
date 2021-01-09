using CommonClasses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class QuestStatus
    {
        public Quest Quest { get; set; }
        public QuestStatusEnum Status { get; set; }

        public QuestStatus(Quest quest, QuestStatusEnum status)
        {
            Quest = quest;
            Status = status;
        }
    }
}
