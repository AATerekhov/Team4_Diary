﻿using AutoFixture.Xunit2;
using Diary.BusinessLogic.Services.Implementation;
using Diary.Core.Domain.Diary;
using Diary.DataAccess.Abstractions;
using Diary.UnitTests.Helps;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Diary.UnitTests.Applications
{
   public class GetHabitDiary
    {
        [Theory, AutoMoqData]
        public async Task GetByIdAsync_DiaryExists_ReturnsDiary(
        Guid id,
        HabitDiary habitDiary,  
        [Frozen] Mock<IHabitDiaryRepository> habitDiaryRepositoryMock,
        HabitDiaryService habitDiaryService,
        CancellationToken token)
        {
            // Arrange
            habitDiaryRepositoryMock.Setup(repo => repo.GetByIdAsync(id, token, It.IsAny<string>()))
                .ReturnsAsync(habitDiary);

            // Act
            var result = await habitDiaryService.GetByIdAsync(id, token);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
        }

    }
}
