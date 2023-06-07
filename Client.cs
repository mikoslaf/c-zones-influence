using CitizenFX.Core;
using CitizenFX.FiveM;
using System.Dynamic;
using System;
using CitizenFX.FiveM.Native;

namespace c_zones_influence
{
    public class Client : BaseScript
    {
        private string gang = "none";
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

            Events.RegisterEventHandler("QBCore:Client:OnGangUpdate", Func.Create<ExpandoObject>(setgang), Binding.All);
            //setgang("Vagos"); //test

            Natives.RegisterNuiCallbackType("c_influence");
            EventHandlers["__cfx_nui:c_influence"] += Func.Create<ExpandoObject>(influence);
            EventHandlers["QBCore:Client:OnGangUpdate"] += Func.Create<string>(setgang);

            Natives.RegisterCommand("test", new Action(test), false);
        }

        private void setgang(dynamic data)
        {
            gang = data.name;
            if (gang != "none")
            {
                //do something
            }
        }

        private void test()
        {
            Vector3 coords = Natives.GetEntityCoords(Natives.PlayerPedId(), false);
            Natives.SendNuiMessage("{\"action\":\"start\",\"x\":"+ coords[0].ToString() + ", \"y\":" + coords[1].ToString() + "}");
        }

        private void influence(dynamic data) 
        {
            if (gang != "none")
            {
                //byte zone = Convert.ToByte(data.zone);
                uint zone = data.zone;
                double val = data.val;
                Events.TriggerServerEvent("qb-handler:c-zones-influence:influence",zone,val,gang);
            }
        }
    }
}
