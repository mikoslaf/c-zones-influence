using CitizenFX.Core;
using System.Dynamic;
using System;
using CitizenFX.FiveM.Native;

namespace c_zones_influence
{
    public class Client : BaseScript
    {
        private string gang = "none";
        private bool loop = false;
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

            Events.RegisterEventHandler("c-zones-influence:setgang", Func.Create<string>(setgang), Binding.All);
            //setgang("Vagos"); //test

            Natives.RegisterNuiCallbackType("c_influence");
            EventHandlers["__cfx_nui:c_influence"] += Func.Create<ExpandoObject>(influence);
            EventHandlers["QBCore:Client:OnGangUpdate"] += Func.Create<string>(setgang);

            Natives.RegisterCommand("test", new Action(test), false);
        }

        private void setgang(string gangName)
        {
            gang = gangName;
            if (gang != "none")
            {
                if (!loop) 
                {
                    Debug.WriteLine("dziala");
                    _ = occupation();
                    loop = true;
                }
            }
            else 
            {
                loop = false;
            }
        }

        private async Coroutine occupation() 
        {
            while (loop) 
            {
                await Wait(1000 * 10);
                Vector3 coords = Natives.GetEntityCoords(Natives.PlayerPedId(), false);
                Natives.SendNuiMessage("{\"action\":\"check\",\"x\":" + coords[0].ToString() + ", \"y\":" + coords[1].ToString() + "}");
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
