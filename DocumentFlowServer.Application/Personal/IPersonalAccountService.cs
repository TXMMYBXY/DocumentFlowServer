using DocumentFlowServer.Application.Personal.Dtos;

namespace DocumentFlowServer.Application.Personal;

public interface IPersonalAccountService
{
    Task<PersonDto> GetPersonalInfoAsync(int personId);
    Task ChangePasswordAsync(int personId, ChangePasswordDto changePasswordDto);
    Task<List<LoginTimeDto>> GetLoginTimesAsync(int userId);
    Task AddNewLoginHistoryAsync(AuthRecordDto authRecordDto);
}