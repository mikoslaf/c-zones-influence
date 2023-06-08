using CitizenFX.Core;
using CitizenFX.Server.Native;

namespace Server
{
    public class Server: BaseScript
    {
        public Server() 
        {
            EventHandlers["onClientResourceStart"] += Func.Create<string>(OnClientResourceStart);
        }
        private void OnClientResourceStart(string resource) 
        {
            if (resource != Natives.GetCurrentResourceName())
            {
                return;
            }

            Debug.WriteLine(resource);
        }
    }
}