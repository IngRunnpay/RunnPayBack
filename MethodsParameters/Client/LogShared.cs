using Entities.Input;
using Interfaces.DataAccess.Repository;

namespace MethodsParameters.Client
{
    public static class LogShared {

        public static ILogRepository? _logRepository = null;

        public static async Task LogDataDetail(string message1, string message2, string message3, string message4 = "BePayClient")
        {
            try
            {
                if (_logRepository != null)
                {
                    await _logRepository.Logger(new LogIn(message1, message2, message3, message4));
                }
            }
            catch { }
        }
    }
}
