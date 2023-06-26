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
        private string[] notes = new string[10];
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
            Events.RegisterEventHandler("c-zones-influence:influence", Func.Create<double, string>(start_influence), Binding.All);
            Events.RegisterEventHandler("c-zones-influence:nui_open", Func.Create<string>(nui_open), Binding.All);
            //setgang("Vagos"); //test

            Natives.RegisterNuiCallbackType("c_influence");
            EventHandlers["__cfx_nui:c_influence"] += Func.Create<ExpandoObject>(influence);

            Natives.RegisterNuiCallbackType("c_close");
            EventHandlers["__cfx_nui:c_close"] += Func.Create(close);

            EventHandlers["QBCore:Client:OnGangUpdate"] += Func.Create<string>(setgang);

            Natives.RegisterCommand("test-old", new Action(test), false);
            Natives.RegisterCommand("test", new Action(test2), false);
        }
        private void addnote(string zone, string note) 
        {
            bool found = true;
            for (int i = 0; i < notes.Length; i+=2) 
            {
                if (notes[i] == null)
                {
                    notes[i] = zone;
                    notes[i+1] = note;
                    found = false;
                    break;
                }
            }

            if (found) 
            {
                for (int i = 0; i < notes.Length-2; i += 2)
                {
                    notes[i] = notes[i+2];
                    notes[i + 1] = notes[i + 3];
                }
                notes[notes.Length - 2] = zone;
                notes[notes.Length - 1] = note;
            }

        }
        private void setgang(string gangName)
        {
            gang = gangName;
            Events.TriggerServerEvent("c-zones-influence:addplayer", gang);
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
        private void test2() 
        {
            Events.TriggerServerEvent("c-zones-influence:server:nui_open");
        }
        private void test()
        {
            Debug.WriteLine(Natives.GetPlayerServerId(Natives.PlayerId()).ToString());
            setgang("vagos");
            Vector3 coords = Natives.GetEntityCoords(Natives.PlayerPedId(), false);
            Natives.SetNuiFocus(true, true);
            //Natives.SendNuiMessage("{\"action\":\"start\",\"x\":"+ coords[0].ToString() + ", \"y\":" + coords[1].ToString() + ", \"gang\":\""+ gang + "\"}");
            Events.TriggerServerEvent("c-zones-influence:getzones", new Action<object>((arg) => {
                Debug.WriteLine("working 10da");
                Debug.WriteLine(arg.ToString());
            }));
            
            Natives.SendNuiMessage("{\"action\":\"map\", \"gang\":\"" + gang + "\"}");
        }

        private void nui_open(string zones) 
        {
            setgang("vagos");
            Natives.SetNuiFocus(true, true);
            Natives.SendNuiMessage("{\"action\":\"map\", \"zones\":\"" + zones + "\", \"gang\":\"" + gang + "\"}");
        }

        private void start_influence(double val = 0.001, string note = "")
        {
            Vector3 coords = Natives.GetEntityCoords(Natives.PlayerPedId(), false);

            if (note == "")
            {
                Natives.SendNuiMessage("{\"action\":\"check\",\"x\":" + coords[0].ToString() + ", \"y\":" + coords[1].ToString() + ", \"v\":" + val.ToString() + "}");
            }
            else 
            {
                Natives.SendNuiMessage("{\"action\":\"check\",\"x\":" + coords[0].ToString() + ", \"y\":" + coords[1].ToString() + ", \"v\":" + val.ToString() + ", \"n\": "+ note +"}");
            }
         }

        private void influence(dynamic data) 
        {
            if (gang != "none")
            {
                //byte zone = Convert.ToByte(data.zone);
                int zone = (int)data.zone;
                double val = data.val;
                if (data.note == "") 
                {
                    Events.TriggerServerEvent("c-zones-influence:server:influence", zone, val, gang, "", "");
                }
                else 
                {
                    Events.TriggerServerEvent("c-zones-influence:server:influence", zone, val, gang, data.note, data.name);
                }
            }
        }

        private void close()
        {
            Natives.SetNuiFocus(false, false);
        }
    }
}
