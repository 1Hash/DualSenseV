using System;
using System.Text;
using GTA;
using GTA.UI;
using GTA.Math;
using System.Windows.Forms;
using GTA.Native;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Media;
using System.Reflection;
using LemonUI;
using LemonUI.Menus;


namespace DualSenseV
{
    public class DualSense : Script
    {
        ObjectPool pool;
        //NativeItem basicItem;
        NativeMenu menu;
        NativeMenu subMenuGatilhos;
        NativeMenu subMenuPistolas;
        //NativeCheckboxItem checkboxItem;
        NativeListItem<string> ListItemsG;
        //NativeListItem ListItems;
        //NativeSliderItem SliderItem;

        static UdpClient client;
        static IPEndPoint endPoint;

        int contP = 0;
        int contP2 = 60;
        int contC = 0;

        int contDS = 0;
        int contRifles = 0;

        bool control = true;
        bool controlN = true;
        bool condP = false;
        bool controlDS = true;

        bool controlGatilhosPistolas = true;
        bool controlRifles = true;
        bool controlRifles2 = false;

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
            Connect();
            LoadUI();
            this.Tick += OnTick;
            this.KeyUp += OnKeyUp;
            this.KeyDown += OnKeyDown;

            ListItemsG.ItemChanged += MudaPistola;
        }

        private void MudaPistola(object sender, ItemChangedEventArgs<string> e)
        {
            GTA.UI.Screen.ShowSubtitle(ListItemsG.SelectedIndex.ToString());

            if(ListItemsG.SelectedIndex == 0)
            {
                controlGatilhosPistolas = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            } else if(ListItemsG.SelectedIndex == 1) 
            {
                controlGatilhosPistolas = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void LoadUI()
        {
            pool = new ObjectPool();
            menu = new NativeMenu("DualSenseV", "Escolha uma opção");
            pool.Add(menu);

            //basicItem = new NativeItem("Item basico", "Descrição do item basico");
            //menu.Add(basicItem);

            //checkboxItem = new NativeCheckboxItem("Teste checkbox", "Descrição do item da checkbox", true);
            //menu.Add(checkboxItem);

            subMenuGatilhos = new NativeMenu("Gatilhos", "Ajustes dos gatilhos");
            menu.AddSubMenu(subMenuGatilhos);
            pool.Add(subMenuGatilhos);

            NativeMenu subMenuArmas = new NativeMenu("Armas", "Ajustes das armas");
            subMenuGatilhos.AddSubMenu(subMenuArmas);
            pool.Add(subMenuArmas);

            subMenuPistolas = new NativeMenu("Pistolas", "Pistolas");
            subMenuArmas.AddSubMenu(subMenuPistolas);
            ListItemsG = new NativeListItem<string>("Pistola clássica", "Altera o modo dos gatilhos da pistola clássica", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuPistolas.Add(ListItemsG);
            pool.Add(subMenuPistolas);           

            NativeMenu subMenuRifles = new NativeMenu("Rifles", "Rifles");
            subMenuArmas.AddSubMenu(subMenuRifles);
            NativeListItem ListItemsR = new NativeListItem<string>("Carabina", "Altera o modo dos gatilhos da carabina", "Apenas o gatilho", "Gatilho com coice");
            subMenuRifles.Add(ListItemsR);
            pool.Add(subMenuRifles);

            NativeMenu subMenuVeiculos = new NativeMenu("Veículos", "Ajustes dos veículos");
            subMenuGatilhos.AddSubMenu(subMenuVeiculos);
            pool.Add(subMenuVeiculos);


            //ListItems = new NativeListItem<string>("Gatilhos pistolas", "Altera o modo dos gatilhos da pistola", "Apenas o gatilho", "Gatilho com coice");
            //itemSubmenu.Menu.Add(ListItems);
            //menu.Add(ListItems);

            //SliderItem = new NativeSliderItem("Teste slider", "Descrição do item slider");
            //menu.Add(SliderItem);
          
            //menu.RotateCamera = true;
        }

        private void OnTick(object sender, EventArgs e)
        {
            pool.Process();

            CorControle();

            if ((Game.Player.Character.Weapons.Current.AmmoInClip == 0 && Game.Player.Character.IsReloading == true) || Game.Player.Character.IsReloading == true || Game.Player.Character.IsJumping == true || Game.Player.Character.IsGettingUp == true || Game.Player.Character.IsInMeleeCombat == true || Game.Player.Character.IsInjured == true || Game.Player.Character.IsInVehicle() == true || Game.Player.Character.IsSwimming == true || Game.Player.Character.IsSwimmingUnderWater == true || Game.Player.Character.IsProne == true || Game.Player.Character.IsJacking == true || Game.Player.Character.IsRagdoll == true || Game.Player.Character.IsJumpingOutOfVehicle == true || Game.Player.Character.IsFalling == true || Game.Player.Character.IsClimbing == true || Game.Player.Character.IsDiving == true || Game.Player.Character.IsDead == true || Game.Player.Character.IsCuffed == true || Game.Player.Character.IsBeingStunned == true || Game.Player.Character.IsGettingIntoVehicle == true || Game.Player.Character.Weapons.Current.Hash == WeaponHash.Unarmed)
            {
                if (controlN)
                {
                    controlN = false;
                    control = true;

                    Packet p = new Packet();
                    p.instructions = new Instruction[6];

                    p.instructions[0].type = InstructionType.TriggerUpdate;
                    p.instructions[0].parameters = new object[] { 0, Trigger.Left, TriggerMode.Normal };

                    p.instructions[1].type = InstructionType.TriggerUpdate;
                    p.instructions[1].parameters = new object[] { 0, Trigger.Right, TriggerMode.Normal };

                    Send(p);
                }
            }
            else if (Game.Player.Character.Weapons.Current.AmmoInClip >= 0 && Game.Player.Character.IsReloading == false/* && Game.Player.Character.IsShooting == true*/)
            {
                if (Game.Player.Character.Weapons.Current.Hash == WeaponHash.Pistol)
                {
                    GatilhoPistola(controlGatilhosPistolas, 5);
                }
                else if (Game.Player.Character.Weapons.Current.Hash == WeaponHash.CarbineRifle || Game.Player.Character.Weapons.Current.Hash == WeaponHash.AssaultRifle || Game.Player.Character.Weapons.Current.Hash == WeaponHash.BullpupRifle || Game.Player.Character.Weapons.Current.Hash == WeaponHash.AdvancedRifle)
                {
                    GatilhoRifle(9);
                }
                else if (Game.Player.Character.Weapons.Current.Hash == WeaponHash.CompactRifle)
                {
                    GatilhoRifle(8);
                }
                else if (Game.Player.Character.Weapons.Current.Hash == WeaponHash.SpecialCarbine)
                {
                    GatilhoRifle(10);
                }
            }

            CorProcurado();
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F10)
            {
                if(!menu.Visible)
                {
                    menu.Visible = true;
                } else
                {
                    menu.Visible = false;
                }
            }

            if(e.KeyCode == Keys.NumPad1)
            {
                var npc = World.CreatePed(PedHash.Abigail, Game.Player.Character.GetOffsetPosition(new Vector3(0, 5, 0)));
                npc.Weapons.Give(WeaponHash.BattleAxe, 1, true, true);
                npc.Task.FightAgainst(Game.Player.Character);
            }

            if(e.KeyCode == Keys.NumPad3)
            {
                //World.AddExplosion(Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 20, 0)), ExplosionType.Tanker, 10, 1);

                foreach (string linha in File.ReadLines(@"C:\Temp\DualSenseX\DualSenseX_SaveFile_INI.ini"))
                {
                    if (linha.Contains("SaveFile_TouchpadLEDColor"))
                    {
                        string[] cor = linha.Split('=');

                        var rgba = System.Drawing.ColorTranslator.FromHtml(cor[1]);

                        Packet p = new Packet();

                        int controllerIndex = 0;

                        p.instructions = new Instruction[6];

                        p.instructions[2].type = InstructionType.RGBUpdate;
                        p.instructions[2].parameters = new object[] { controllerIndex, rgba.R, rgba.G, rgba.B, rgba.A };

                        Send(p);

                        //UI.ShowHelpMessage(rgba.R.ToString() + " - " + rgba.G.ToString() + " - " + rgba.B.ToString() + " - " + rgba.A.ToString());
                        GTA.UI.Screen.ShowHelpText(rgba.R.ToString() + " - " + rgba.G.ToString() + " - " + rgba.B.ToString() + " - " + rgba.A.ToString());

                    }
                }
            }

            if (e.KeyCode == Keys.NumPad7)
            {
                if(Game.Player.IsInvincible == false)
                {
                    Game.Player.IsInvincible = true;
                    //UI.ShowSubtitle("~w~Modo invencível:~g~Ativado");
                    //UI.Notify("Modo invencível ativado!");
                    //UI.ShowHelpMessage("Modo invencível ~g~ativado!");
                    GTA.UI.Screen.ShowHelpText("Modo invencível ~g~ativado!");
                }
                else
                {                  
                    Game.Player.IsInvincible = false;
                    //UI.ShowSubtitle("~w~Modo invencível:~r~Desativado");
                    //UI.Notify("Modo invencível desativado!");
                    //UI.ShowHelpMessage("Modo invencível ~r~desativado!");
                    GTA.UI.Screen.ShowHelpText("Modo invencível ~r~desativado!");
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


                    //UI.ShowHelpMessage("~g~Funcionou!!!!");
                    GTA.UI.Screen.ShowHelpText("~g~Funcionou!!!!");
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

                    //UI.ShowHelpMessage("~g~Ta com uma pistola na mão e fora de um carro!!!");
                    GTA.UI.Screen.ShowHelpText("~g~Ta com uma pistola na mão e fora de um carro!!!");
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

                //UI.ShowHelpMessage("~g~Os gatilhos foram definidos para o normal!");
                GTA.UI.Screen.ShowHelpText("~g~Os gatilhos foram definidos para o normal!");
            }
        }

        private void ExitGame(object sender, EventArgs e)
        {
            foreach (string linha in File.ReadLines(@"C:\Temp\DualSenseX\DualSenseX_SaveFile_INI.ini"))
            {
                if (linha.Contains("SaveFile_TouchpadLEDColor"))
                {
                    string[] cor = linha.Split('=');

                    var rgba = System.Drawing.ColorTranslator.FromHtml(cor[1]);

                    Packet p = new Packet();
                    p.instructions = new Instruction[6];

                    p.instructions[2].type = InstructionType.RGBUpdate;
                    p.instructions[2].parameters = new object[] { 0, rgba.R, rgba.G, rgba.B, rgba.A };

                    Send(p);

                    System.Diagnostics.Process.Start("notepad.exe");
                }
            }
        }

        private void CorProcurado()
        {
            if (Game.Player.WantedLevel > 0)
            {
                condP = true;

                if (contP == 50 && contC < 11)
                {
                    contP = 0;
                    contC++;

                    Packet p = new Packet();
                    p.instructions = new Instruction[6];

                    p.instructions[2].type = InstructionType.RGBUpdate;
                    p.instructions[2].parameters = new object[] { 0, 255, 0, 0 }; //Vermelho

                    Send(p);
                }
                else if (contP == 25 && contC < 11)
                {
                    contC++;

                    Packet p = new Packet();
                    p.instructions = new Instruction[6];

                    p.instructions[2].type = InstructionType.RGBUpdate;
                    p.instructions[2].parameters = new object[] { 0, 0, 0, 255 }; //Azul

                    Send(p);
                }

                if (contC > 10 && contC < 16)
                {
                    if (contP2 == 120)
                    {
                        contP2 = 0;
                        contC++;

                        Packet p = new Packet();
                        p.instructions = new Instruction[6];

                        p.instructions[2].type = InstructionType.RGBUpdate;
                        p.instructions[2].parameters = new object[] { 0, 255, 0, 0 }; //Vermelho

                        Send(p);

                    }
                    else if (contP2 == 60)
                    {
                        contC++;

                        Packet p = new Packet();
                        p.instructions = new Instruction[6];

                        p.instructions[2].type = InstructionType.RGBUpdate;
                        p.instructions[2].parameters = new object[] { 0, 0, 0, 255 }; //Azul

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
                CorControle();
            }
        }

        private void CorControle()
        {
            if (Game.Player.Character.Model.Hash == -1686040670 && ccorT == true) //Trevor
            {
                ccorT = false;
                ccorF = true;
                ccorM = true;
                ccorO = true;

                Packet p = new Packet();
                p.instructions = new Instruction[6];

                p.instructions[2].type = InstructionType.RGBUpdate;
                p.instructions[2].parameters = new object[] { 0, 255, 100, 0 }; //Laranja

                Send(p);
            }

            if (Game.Player.Character.Model.Hash == -1692214353 && ccorF == true) //Franklin
            {
                ccorT = true;
                ccorF = false;
                ccorM = true;
                ccorO = true;

                Packet p = new Packet();
                p.instructions = new Instruction[6];

                p.instructions[2].type = InstructionType.RGBUpdate;
                p.instructions[2].parameters = new object[] { 0, 50, 255, 0 }; //Verde

                Send(p);

            }

            if (Game.Player.Character.Model.Hash == 225514697 && ccorM == true) //Michael
            {
                ccorT = true;
                ccorF = true;
                ccorM = false;
                ccorO = true;

                Packet p = new Packet();
                p.instructions = new Instruction[6];

                p.instructions[2].type = InstructionType.RGBUpdate;
                p.instructions[2].parameters = new object[] { 0, 0, 50, 255 }; //Azul

                Send(p);
            }

            if (Game.Player.Character.Model.Hash != 225514697 && Game.Player.Character.Model.Hash != -1692214353 && Game.Player.Character.Model.Hash != -1686040670 && ccorO == true) //Outro
            {
                ccorT = true;
                ccorF = true;
                ccorM = true;
                ccorO = false;

                Packet p = new Packet();
                p.instructions = new Instruction[6];

                p.instructions[2].type = InstructionType.RGBUpdate;
                p.instructions[2].parameters = new object[] { 0, 255, 0, 255 }; //Roxo

                Send(p);
            }
        }

        private void GatilhoPistola(bool controlGatilhos, int limiteDS)
        {
            if (Game.Player.Character.IsShooting == true && controlDS == true && controlGatilhos == true)
            {
                controlDS = false;

                Packet p = new Packet();
                p.instructions = new Instruction[6];

                p.instructions[1].type = InstructionType.TriggerUpdate;
                p.instructions[1].parameters = new object[] { 0, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.PulseB, 3, 255, 140, 0, 0, 0, 0 };

                Send(p);

            }
            else if (!controlDS)
            {
                if (contDS >= limiteDS)
                {
                    controlDS = true;
                    control = true;
                    contDS = 0;

                }
                else
                {
                    contDS++;
                }
            }
            else if (control)
            {
                control = false;
                controlN = true;


                Packet p = new Packet();
                p.instructions = new Instruction[6];

                p.instructions[0].type = InstructionType.TriggerUpdate;
                p.instructions[0].parameters = new object[] { 0, Trigger.Left, TriggerMode.Resistance, 0, 1 };

                p.instructions[1].type = InstructionType.TriggerUpdate;
                p.instructions[1].parameters = new object[] { 0, Trigger.Right, TriggerMode.Soft };

                Send(p);
            }
        }

        private void GatilhoRifle(int cadenciaTiros)
        {
            if (control && Game.Player.Character.IsShooting == true)
            {
                control = false;
                controlN = true;
                controlRifles = true;
                controlRifles2 = true;
                contRifles = 0;


                Packet p = new Packet();
                p.instructions = new Instruction[6];

                p.instructions[0].type = InstructionType.TriggerUpdate;
                p.instructions[0].parameters = new object[] { 0, Trigger.Left, TriggerMode.Resistance, 0, 1 };

                p.instructions[1].type = InstructionType.TriggerUpdate;
                p.instructions[1].parameters = new object[] { 0, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.PulseB, cadenciaTiros, 255, 50, 0, 0, 0, 0 };

                Send(p);

            } else if (control && controlRifles)
            {
                controlRifles = false;
                controlRifles2 = true;

                Packet p = new Packet();
                p.instructions = new Instruction[6];

                p.instructions[0].type = InstructionType.TriggerUpdate;
                p.instructions[0].parameters = new object[] { 0, Trigger.Left, TriggerMode.Resistance, 0, 1 };

                p.instructions[1].type = InstructionType.TriggerUpdate;
                p.instructions[1].parameters = new object[] { 0, Trigger.Right, TriggerMode.Soft };

                Send(p);

            } else if (Game.Player.Character.IsShooting == false && controlRifles2 && !control)
            {
                if(contRifles >= 10)
                {
                    controlRifles2 = false;
                    contRifles = 0;
                    control = true;

                    Packet p = new Packet();
                    p.instructions = new Instruction[6];

                    p.instructions[1].type = InstructionType.TriggerUpdate;
                    p.instructions[1].parameters = new object[] { 0, Trigger.Right, TriggerMode.Soft };

                    Send(p);

                } else
                {
                    contRifles++;
                }
            } else if (Game.Player.Character.IsShooting == true) 
            {
                contRifles = 0;
                controlRifles2 = true;
            }
        }
    }
}
