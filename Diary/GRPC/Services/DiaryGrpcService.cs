using Diary.Core.Domain.Diary;
using Diary.DataAccess.Abstractions;
using Grpc.Core;
using Magazine.Message;
 using Grpc.Core;
using GrpcDiary;
namespace Diary.GRPC.Services
{
    public class DiaryGrpcService(IHabitDiaryRepository _diaryRepository, IHabitDiaryLineRepository _diaryLineRepository) : GrpcDiary.DiaryGrpcService.DiaryGrpcServiceBase
    {

        public override async Task<Empty> CreateDiaryLineFromMagazine(MagazineLineMessageGrpc request, ServerCallContext context)
        {
            var diary = await _diaryRepository.GetFirstWhere(r => r.RoomId == Guid.Parse(request.RoomId) && r.DiaryOwnerId == Guid.Parse(request.UserId));

            var diaryLine = new HabitDiaryLine()
            {
                DiaryId = diary.Id,
                EntityId = Guid.Parse(request.RewardId),
                EventDescription = request.EventDescription,
                CreatedDate = DateTime.Parse(request.CreatedDate),
                ModifiedDate = DateTime.Parse(request.ModifiedDate),
                Cost = (decimal)-request.Cost,
            };

            await _diaryLineRepository.AddAsync(diaryLine, context.CancellationToken);
            await _diaryLineRepository.SaveChangesAsync(context.CancellationToken);

            return new Empty();
        }
    }
}
