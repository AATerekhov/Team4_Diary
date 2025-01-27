﻿using AutoMapper;
using Diary.BusinessLogic.Models.DiaryOwner;
using Diary.BusinessLogic.Models.HabitDiary;
using Diary.BusinessLogic.Models.JournalOwner;
using Diary.BusinessLogic.Models.UserJournal;
using Diary.Core.Domain.Administration;
using Diary.Core.Domain.Diary;
using Diary.DataAccess.Models;
using Diary.Models.Request;
using Diary.Models.Response;

namespace Diary.Mapping
{
    public class HabitDiaryMappingsProfile : Profile
    {
        public HabitDiaryMappingsProfile()
        {
            CreateMap<HabitDiary, HabitDiaryResponse>();
            CreateMap<HabitDiary, HabitDiaryShortResponse>();
            CreateMap<HabitDiaryFilterRequest, HabitDiaryFilterDto>();
            CreateMap<HabitDiaryFilterDto, HabitDiaryFilterModel>();
            CreateMap<CreateHabitDiaryRequest, CreateHabitDiaryDto>();
            CreateMap<EditHabitDiaryRequest, EditHabitDiaryDto>();
            CreateMap<CreateHabitDiaryDto, HabitDiary>()
              .ForMember(j => j.Lines, opt => opt.Ignore())
              .ForMember(j => j.DiaryOwner, opt => opt.Ignore())
              .ForMember(j => j.Id, opt => opt.Ignore())
              .ForMember(j => j.Habits, opt => opt.Ignore());
        }
    }
}
