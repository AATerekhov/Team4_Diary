﻿using AutoMapper;
using Diary.BusinessLogic.Models.HabitDiaryLine;
using Diary.BusinessLogic.Models.UserJournal;
using Diary.BusinessLogic.Services;
using Diary.Cache;
using Diary.Core.Domain.Habits;
using Diary.Models.Request;
using Diary.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Diary.Controllers
{
    /// <summary>
    /// Diary Lines
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class HabitDiaryLineController(IHabitDiaryLineService _service,
                                          IMapper                _mapper,
                                          IDistributedCache      _distributedCache) : ControllerBase
    {
        
        /// <summary>
        /// Получение списка строк дневников
        /// </summary>
        /// <param name="request"><DiaryLineFilterRequest/param>
        /// <returns></returns>
        [HttpPost("list")]
        public async Task<ActionResult<HabitDiaryLineResponse>> GetDiaryLinePagedAsync(HabitDiaryLineFilterRequest request)
        {
            var filterModel = _mapper.Map<HabitDiaryLineFilterRequest, HabitDiaryLineFilterDto>(request);
            var response = _mapper.Map<List<HabitDiaryLineResponse>>(await _service.GetPagedAsync(filterModel, HttpContext.RequestAborted));
            return Ok(response);
        }

        /// <summary>
        /// Получение строки дневника через гуид
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetDiaryLine/{id}")]
        public async Task<ActionResult<HabitDiaryLineResponse>> GetDiaryLineAsync(Guid id)
        {
            string? serialized = await _distributedCache.GetStringAsync(KeyForCache.DiaryLineKey(id), HttpContext.RequestAborted);

            if (serialized is not null)
            {
                var cachResult = JsonSerializer.Deserialize<IEnumerable<HabitDiaryLineResponse>>(serialized);

                if (cachResult is not null)
                {
                    return Ok(cachResult);
                }
            }

            var response = _mapper.Map<HabitDiaryLineResponse>(await _service.GetByIdAsync(id, HttpContext.RequestAborted));

            await _distributedCache.SetStringAsync(
                key: KeyForCache.DiaryLineKey(id),
                value: JsonSerializer.Serialize(response),
                options: new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromHours(1)
                });


            return Ok(response);


        }

        /// <summary>
        /// Получение всех строк дневника
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetDiaryLinesByDiaryId/{id}")]
        public async Task<ActionResult<HabitDiaryLineResponse>> GetDiaryLinesByDiaryIdAsync(Guid id)
        {
            string? serialized = await _distributedCache.GetStringAsync(KeyForCache.DiaryLinesByDiaryIdKey(id), HttpContext.RequestAborted);

            if (serialized is not null)
            {
                var cachResult = JsonSerializer.Deserialize<IEnumerable<HabitDiaryLineResponse>>(serialized);

                if (cachResult is not null)
                {
                    return Ok(cachResult);
                }

            }
            var lines = await _service.GetAllByDiaryIdAsync(id, HttpContext.RequestAborted);
            var response = _mapper.Map<List<HabitDiaryLineResponse>>(lines);

            await _distributedCache.SetStringAsync(
                key: KeyForCache.DiaryLinesByDiaryIdKey(id),
                value: JsonSerializer.Serialize(response),
                options: new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromHours(1)
                });

         
            return Ok(response);
        }

        /// <summary>
        /// Получение всех строк дневников
        /// </summary>
        /// <returns></returns>
        [HttpGet("AllDiaryLines")]
        public async Task<ActionResult<HabitDiaryLineResponse>> GetAllAsync()
        {
            var lines = await _service.GetAllAsync(HttpContext.RequestAborted);
            var response = _mapper.Map<List<HabitDiaryLineResponse>>(lines);
            return Ok(response);
        }

        /// <summary>
        /// Создание строки дневника
        /// </summary>
        /// <param name="request">CreateOrEditHabitDiaryLineRequest</param>
        /// <returns></returns>
        [HttpPost("CreateDiaryLine")]
        public async Task<ActionResult<HabitDiaryLineResponse>> CreateDiaryLineAsync(CreateHabitDiaryLineRequest request)
        {
            var diaryLine = await _service.CreateAsync(_mapper.Map<CreateHabitDiaryLineDto>(request), HttpContext.RequestAborted);
            await _distributedCache.RemoveAsync(KeyForCache.DiaryLinesByDiaryIdKey(diaryLine.DiaryId));
            return Ok(_mapper.Map<HabitDiaryLineResponse>(diaryLine));
        }

        /// <summary>
        /// Изменение строки дневника по гуиду
        /// </summary>
        /// <param name="id">Guid</param>
        /// <param name="request">EditHabitDiaryLineRequest</param>
        /// <returns></returns>

        [HttpPut("UpdateDiaryLine/{id}")]
        public async Task<ActionResult<HabitDiaryLineResponse>> EditDiaryLineAsync(Guid id, EditHabitDiaryLineRequest request)
        {
            var diaryLine = await _service.UpdateAsync(id, _mapper.Map<EditHabitDiaryLineRequest, EditHabitDiaryLineDto>(request), HttpContext.RequestAborted);
            await _distributedCache.RemoveAsync(KeyForCache.DiaryLineKey(id));
            await _distributedCache.RemoveAsync(KeyForCache.DiaryLinesByDiaryIdKey(diaryLine.DiaryId));
            return Ok(_mapper.Map<HabitDiaryLineResponse>(diaryLine));
        }

        /// <summary>
        /// Удаление журнала по гуиду
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteDiaryLine/{id}")]
        public async Task<IActionResult> DeleteDiaryLine(Guid id)
        {
            await _service.DeleteAsync(id, HttpContext.RequestAborted);
            await _distributedCache.RemoveAsync(KeyForCache.DiaryLineKey(id));
            return Ok($"Строка дневника с id {id} удален");
        }
    }
}
