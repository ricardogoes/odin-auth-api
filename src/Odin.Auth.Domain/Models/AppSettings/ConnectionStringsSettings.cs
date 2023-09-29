namespace Odin.Auth.Domain.Models.AppSettings
{
    public class ConnectionStringsSettings
    {
        public string OdinMasterDbConnection { get; private set; }

        public ConnectionStringsSettings(string odinMasterDbConnection)
        {
            OdinMasterDbConnection = odinMasterDbConnection;
        }
    }
}
