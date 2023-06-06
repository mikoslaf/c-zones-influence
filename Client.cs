using CitizenFX.Core;
using CitizenFX.FiveM;
using System.Dynamic;
using System;
using CitizenFX.FiveM.Native;

namespace c_zones_influence
{
    public class Client : BaseScript
    {
        public Client()
        {
            EventHandlers["onClientResourceStart"] += Func.Create<string>(OnClientResourceStart);

        }
        private void OnClientResourceStart(string resourceName)
        {
            if (resourceName != Natives.GetCurrentResourceName())
            {
                return;
            }

            Natives.RegisterNuiCallbackType("c_influence");
            EventHandlers["__cfx_nui:c_influence"] += Func.Create<ExpandoObject>(influence);

            Natives.RegisterCommand("test", new Action(test), false);
        }

        private void test() 
        {
            Vector3 coords = Natives.GetEntityCoords(Natives.PlayerPedId(), false);
            Natives.SendNuiMessage("{\"action\":\"start\",\"x\":"+ coords[0].ToString() + ", \"y\":" + coords[1].ToString() + "}");
        }

        private void influence(dynamic data) 
        {
            Debug.WriteLine(data.ToString());   
        }
    }
}
