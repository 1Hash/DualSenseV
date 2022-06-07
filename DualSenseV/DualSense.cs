using System;
using System.Text;
using GTA;
using GTA.Math;
using System.Windows.Forms;
using GTA.Native;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace DualSenseV
{
    public class DualSense : Script
    {
        static UdpClient client;
        static IPEndPoint endPoint;

        int cont = 0;
        int contP = 0;
        int contP2 = 60;
        int contC = 0;

        bool control = true;
        bool controlN = true;
        bool condP = false;

        bool ccorT = true;
        bool ccorF = true;
        bool ccorM = true;
        bool ccorO = true;

        static void Connect()
        {
            client = new UdpClient();
            var portNumber = File.ReadAllText(@"C:\Temp\DualSenseX\DualSenseX_PortNumber.txt");
            endPoint = new IPEndPoint(Triggers.localhost, Convert.ToInt32(portNumber));
        }

        static void Send(Packet data)
        {
            var RequestData = Encoding.ASCII.GetBytes(Triggers.PacketToJson(data));
            client.Send(RequestData, RequestData.Length, endPoint);
        }

        public DualSense()
        {
            this.Tick += onTick;
            this.KeyUp += onKeyUp;
            this.KeyDown += onKeyDown;

        }

        private void onTick(object sender, EventArgs e)
        {
            if (cont == 0)
            {
                Connect();
                UI.ShowHelpMessage("Conexão com o DSX feita!");
                cont++;
            }

            if (Game.Player.Character.Weapons.Current.AmmoInClip == 0 || Game.Player.Character.IsReloading == true || Game.Player.Character.IsJumping == true || Game.Player.Character.IsGettingUp == true || Game.Player.Character.IsInMeleeCombat == true || Game.Player.Character.IsInjured == true || Game.Player.Character.IsInVehicle() == true || Game.Player.Character.IsSwimming == true || Game.Player.Character.IsSwimmingUnderWater == true || Game.Player.Character.IsProne == true || Game.Player.Character.IsJacking == true || Game.Player.Character.IsRagdoll == true || Game.Player.Character.IsJumpingOutOfVehicle == true || Game.Player.Character.IsFalling == true || Game.Player.Character.IsClimbing == true || Game.Player.Character.IsDiving == true || Game.Player.Character.IsDead == true || Game.Player.Character.IsCuffed == true || Game.Player.Character.IsBeingStunned == true || Game.Player.Character.IsGettingIntoAVehicle == true || Game.Player.Character.Weapons.Current.Hash == WeaponHash.Unarmed)
            {
                if(controlN)
                {
                    controlN = false;
                    control = true;
                    UI.ShowHelpMessage("~r~Não atirando");
                }
            }
            else if(Game.Player.Character.Weapons.Current.AmmoInClip > 0 && Game.Player.Character.IsReloading == false && Game.Player.Character.IsShooting == true)
            {
                if(control)
                {
                    control = false;
                    controlN = true;
                    UI.ShowHelpMessage("~g~Atirando!");
                }               
            }

            corControle();

            if (Game.Player.WantedLevel > 0)
            {
                condP = true;

                if (contP == 50 && contC < 11)
                {
                    contP = 0;
                    contC++;
                    Packet p = new Packet();

                    int controllerIndex = 0;

                    p.instructions = new Instruction[6];

                    p.instructions[2].type = InstructionType.RGBUpdate;
                    p.instructions[2].parameters = new object[] { controllerIndex, 255, 0, 0 }; //Vermelho

                    Send(p);
                }
                else if (contP == 25 && contC < 11)
                {
                    contC++;
                    Packet p = new Packet();

                    int controllerIndex = 0;

                    p.instructions = new Instruction[6];

                    p.instructions[2].type = InstructionType.RGBUpdate;
                    p.instructions[2].parameters = new object[] { controllerIndex, 0, 0, 255 }; //Azul

                    Send(p);
                }

                if (contC > 10 && contC < 16)
                {
                    if (contP2 == 120)
                    {
                        contP2 = 0;
                        contC++;
                        Packet p = new Packet();

                        int controllerIndex = 0;

                        p.instructions = new Instruction[6];

                        p.instructions[2].type = InstructionType.RGBUpdate;
                        p.instructions[2].parameters = new object[] { controllerIndex, 255, 0, 0 }; //Vermelho

                        Send(p);

                    }
                    else if (contP2 == 60)
                    {
                        contC++;
                        Packet p = new Packet();

                        int controllerIndex = 0;

                        p.instructions = new Instruction[6];

                        p.instructions[2].type = InstructionType.RGBUpdate;
                        p.instructions[2].parameters = new object[] { controllerIndex, 0, 0, 255 }; //Azul

                        Send(p);
                    }
                    contP2++;
                }
                else if (contC > 15)
                {
                    contP = 0;
                    contP2 = 60;
                    contC = 0;
                }

                contP++;
            }
            else if (condP == true)
            {
                condP = false;
                contP = 0;
                contP2 = 60;
                contC = 0;
                ccorT = true;
                ccorM = true;
                ccorF = true;
                ccorO = true;
                corControle();
            }
        }

        private void onKeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.NumPad1)
            {
                var npc = World.CreatePed(PedHash.Abigail, Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 5, 0)));
                npc.Weapons.Give(WeaponHash.BattleAxe, 1, true, true);
                npc.Task.FightAgainst(Game.Player.Character);
            }

            if(e.KeyCode == Keys.NumPad3)
            {
                World.AddExplosion(Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 20, 0)), ExplosionType.Tanker, 10, 1);
            }

            if (e.KeyCode == Keys.NumPad7)
            {
                if(Game.Player.IsInvincible == false)
                {
                    Game.Player.IsInvincible = true;
                    //UI.ShowSubtitle("~w~Modo invencível:~g~Ativado");
                    //UI.Notify("Modo invencível ativado!");
                    UI.ShowHelpMessage("Modo invencível ~g~ativado!");
                }
                else
                {                  
                    Game.Player.IsInvincible = false;
                    //UI.ShowSubtitle("~w~Modo invencível:~r~Desativado");
                    //UI.Notify("Modo invencível desativado!");
                    UI.ShowHelpMessage("Modo invencível ~r~desativado!");
                }
                
            }

            if (e.KeyCode == Keys.NumPad9)
            {
                if(Game.Player.Character.IsInVehicle() == true)
                {              
                    Packet p = new Packet();

                    int controllerIndex = 0;

                    p.instructions = new Instruction[6];

                    // Left Trigger
                    p.instructions[0].type = InstructionType.TriggerUpdate;
                    p.instructions[0].parameters = new object[] { controllerIndex, Trigger.Left, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Rigid, 70, 5, 0, 0, 0, 0, 0 };

                    // Right Trigger
                    p.instructions[1].type = InstructionType.TriggerUpdate;
                    p.instructions[1].parameters = new object[] { controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Rigid, 110, 2, 0, 0, 0, 0, 0 };



                    //p.instructions[1].type = InstructionType.TriggerThreshold;
                    //p.instructions[1].parameters = new object[] { controllerIndex, Trigger.Left, 0 };

                    //p.instructions[1].type = InstructionType.TriggerThreshold;
                    //p.instructions[1].parameters = new object[] { controllerIndex, Trigger.Right, 0 };



                    p.instructions[2].type = InstructionType.RGBUpdate;
                    p.instructions[2].parameters = new object[] { controllerIndex, 255, 100, 0 };



                    // PLAYER LED 1-5 true/false state
                    //p.instructions[3].type = InstructionType.PlayerLED;
                    //p.instructions[3].parameters = new object[] { controllerIndex, false, false, false, false, false };

                    // Player LED for new revision controllers
                    //p.instructions[4].type = InstructionType.PlayerLEDNewRevision;
                    //p.instructions[4].parameters = new object[] { controllerIndex, PlayerLEDNewRevision.Five };



                    // Mic LED Three modes: On, Pulse, or Off
                    //p.instructions[5].type = InstructionType.MicLED;
                    //p.instructions[5].parameters = new object[] { controllerIndex, MicLEDMode.Pulse };

                    Send(p);

                    //Process[] process = Process.GetProcessesByName("DSX");




                    //byte[] bytesReceivedFromServer = client.Receive(ref endPoint);
                    //ServerResponse ServerResponseJson = JsonConvert.DeserializeObject<ServerResponse>($"{Encoding.ASCII.GetString(bytesReceivedFromServer, 0, bytesReceivedFromServer.Length)}");
                    //DateTime CurrentTime = DateTime.Now;
                    //TimeSpan Timespan = CurrentTime - TimeSent;


                    UI.ShowHelpMessage("~g~Funcionou!!!!");
                }

                if(Game.Player.Character.Weapons.Current.Hash == WeaponHash.Pistol && Game.Player.Character.IsInVehicle() == false)
                {
                    Packet p = new Packet();

                    int controllerIndex = 0;

                    p.instructions = new Instruction[6];

                    // Left Trigger
                    p.instructions[0].type = InstructionType.TriggerUpdate;
                    p.instructions[0].parameters = new object[] { controllerIndex, Trigger.Left, TriggerMode.Resistance, 0, 1 };

                    // Right Trigger
                    p.instructions[1].type = InstructionType.TriggerUpdate;
                    p.instructions[1].parameters = new object[] { controllerIndex, Trigger.Right, TriggerMode.Soft };

                    p.instructions[2].type = InstructionType.RGBUpdate;
                    p.instructions[2].parameters = new object[] { controllerIndex, 255, 255, 255 };

                    Send(p);

                    UI.ShowHelpMessage("~g~Ta com uma pistola na mão e fora de um carro!!!");
                }
            }

            if (e.KeyCode == Keys.Multiply)
            {
                Packet p = new Packet();

                int controllerIndex = 0;

                p.instructions = new Instruction[6];

                // Left Trigger
                p.instructions[0].type = InstructionType.TriggerUpdate;
                p.instructions[0].parameters = new object[] { controllerIndex, Trigger.Left, TriggerMode.Normal };

                // Right Trigger
                p.instructions[1].type = InstructionType.TriggerUpdate;
                p.instructions[1].parameters = new object[] { controllerIndex, Trigger.Right, TriggerMode.Normal };

                p.instructions[2].type = InstructionType.RGBUpdate;
                p.instructions[2].parameters = new object[] { controllerIndex, 100, 255, 0 };

                Send(p);

                UI.ShowHelpMessage("~g~Os gatilhos foram definidos para o normal!");
            }
        }

        private void corControle()
        {
            if (Game.Player.Character.Model.Hash == -1686040670 && ccorT == true) //Trevor
            {
                ccorT = false;
                ccorF = true;
                ccorM = true;
                ccorO = true;

                Packet p = new Packet();

                int controllerIndex = 0;

                p.instructions = new Instruction[6];

                p.instructions[2].type = InstructionType.RGBUpdate;
                p.instructions[2].parameters = new object[] { controllerIndex, 255, 100, 0 }; //Laranja

                Send(p);
            }

            if (Game.Player.Character.Model.Hash == -1692214353 && ccorF == true) //Franklin
            {
                ccorT = true;
                ccorF = false;
                ccorM = true;
                ccorO = true;

                Packet p = new Packet();

                int controllerIndex = 0;

                p.instructions = new Instruction[6];

                p.instructions[2].type = InstructionType.RGBUpdate;
                p.instructions[2].parameters = new object[] { controllerIndex, 50, 255, 0 }; //Verde

                Send(p);

            }

            if (Game.Player.Character.Model.Hash == 225514697 && ccorM == true) //Michael
            {
                ccorT = true;
                ccorF = true;
                ccorM = false;
                ccorO = true;

                Packet p = new Packet();

                int controllerIndex = 0;

                p.instructions = new Instruction[6];

                p.instructions[2].type = InstructionType.RGBUpdate;
                p.instructions[2].parameters = new object[] { controllerIndex, 0, 50, 255 }; //Azul

                Send(p);
            }

            if (Game.Player.Character.Model.Hash != 225514697 && Game.Player.Character.Model.Hash != -1692214353 && Game.Player.Character.Model.Hash != -1686040670 && ccorO == true) //Outro
            {
                ccorT = true;
                ccorF = true;
                ccorM = true;
                ccorO = false;

                Packet p = new Packet();

                int controllerIndex = 0;

                p.instructions = new Instruction[6];

                p.instructions[2].type = InstructionType.RGBUpdate;
                p.instructions[2].parameters = new object[] { controllerIndex, 255, 0, 255 }; //Roxo

                Send(p);
            }
        }
    }
}
