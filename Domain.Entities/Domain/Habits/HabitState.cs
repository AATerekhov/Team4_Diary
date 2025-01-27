﻿using Diary.Core.Domain.BaseTypes;
using Diary.Core.Domain.Diary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Core.Domain.Habits
{
    public class HabitState : BaseEntity
    {
        public Guid HabitId { get; set; }
        public int Status { get; set; }
        public int Tag { get; set; }
        public int TitleValue { get; set; }
        public bool TitleCheck { get; set; }
        public int TextPositive { get; set; }
        public string? TitleReportField { get; set; }
        public int TextNegative{ get; set; }
        public bool IsNotified { get; set; }
        public bool isRated { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Habit Habit { get; set; }
    }
}
