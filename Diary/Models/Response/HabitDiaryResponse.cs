﻿using Diary.Core.Domain.Administration;
using Diary.Core.Domain.Diary;
using System.ComponentModel.DataAnnotations;

namespace Diary.Models.Response
{
    public class HabitDiaryResponse
    {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid RoomId { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        public Guid DiaryOwnerId { get; set; }

        public decimal TotalCost { get; set; }

        public List<HabitDiaryLineResponse> Lines { get; set; }   
    }
}
