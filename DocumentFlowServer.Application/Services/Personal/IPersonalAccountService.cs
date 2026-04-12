
using DocumentFlowServer.Application.Services.Personal.Dto;

namespace DocumentFlowServer.Application.Services.Personal;

public interface IPersonalAccountService
{
    Task<GetPersonDto> GetPersonalInfoAsync(int personId);
    Task ChangePasswordAsync(int personId, ChangePasswordDto changePasswordDto);
    Task<List<GetLoginTimesDto>> GetLoginTimesAsync(int userId);
    Task AddNewLoginHistoryAsync(NewAuthRecordDto newAuthRecordDto);
}