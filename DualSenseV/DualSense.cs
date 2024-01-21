using System;
using System.Text;
using GTA;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using LemonUI;
using LemonUI.Menus;
using System.Collections.Generic;


namespace DualSenseV
{
    public class DualSense : Script
    {
        #region Variables

        public static DualSense Instance;

        //Pool
        ObjectPool pool;

        //Menus
        NativeMenu menu;
        NativeMenu subMenuGatilhos;
        NativeMenu subMenuLeds;
        NativeMenu subMenuArmas;
        NativeMenu subMenuPistolas;
        NativeMenu subMenuSMG;
        NativeMenu subMenuRifles;
        NativeMenu subMenuSnipers;
        NativeMenu subMenuEscopetas;
        NativeMenu subMenuPesadas;
        NativeMenu subMenuVeiculos;

        //Listas

        //Pistolas
        NativeListItem<string> ListItemsP1;
        NativeListItem<string> ListItemsP2;
        NativeListItem<string> ListItemsP3;
        NativeListItem<string> ListItemsP4;
        NativeListItem<string> ListItemsP5;
        NativeListItem<string> ListItemsP6;
        NativeListItem<string> ListItemsP7;
        NativeListItem<string> ListItemsP8;
        NativeListItem<string> ListItemsP9;

        //SMGs
        NativeListItem<string> ListItemsM1;
        NativeListItem<string> ListItemsM2;
        NativeListItem<string> ListItemsM3;
        NativeListItem<string> ListItemsM4;
        NativeListItem<string> ListItemsM5;
        NativeListItem<string> ListItemsM6;
        NativeListItem<string> ListItemsM7;
        NativeListItem<string> ListItemsM8;
        NativeListItem<string> ListItemsM9;

        //Rifles
        NativeListItem<string> ListItemsR1;
        NativeListItem<string> ListItemsR2;
        NativeListItem<string> ListItemsR3;
        NativeListItem<string> ListItemsR4;
        NativeListItem<string> ListItemsR5;
        NativeListItem<string> ListItemsR6;

        //Snipers
        NativeListItem<string> ListItemsS1;
        NativeListItem<string> ListItemsS2;
        NativeListItem<string> ListItemsS3;

        //Escopetas
        NativeListItem<string> ListItemsE1;
        NativeListItem<string> ListItemsE2;
        NativeListItem<string> ListItemsE3;
        NativeListItem<string> ListItemsE4;
        NativeListItem<string> ListItemsE5;
        NativeListItem<string> ListItemsE6;
        NativeListItem<string> ListItemsE7;
        NativeListItem<string> ListItemsE8;

        //Pesadas
        NativeListItem<string> ListItemsPE1;
        NativeListItem<string> ListItemsPE2;
        NativeListItem<string> ListItemsPE3;
        NativeListItem<string> ListItemsPE4;
        NativeListItem<string> ListItemsPE5;
        NativeListItem<string> ListItemsPE6;

        //LEDs
        NativeListItem<string> ListItemsLED1;
        NativeListItem<string> ListItemsLED2;
        NativeListItem<string> ListItemsLED3;

        //Veiculos
        NativeListItem<string> ListItemsV1;

        //Conexão com DSX
        static UdpClient client;
        static IPEndPoint endPoint;

        //Contadores Globais
        int contP = 0;
        int contP2 = 60;
        int contC = 0;
        int contRifles = 0;

        //Controladores globais
        bool control = true;
        bool controlN = true;
        bool controlV = true;
        bool controlV2 = true;

        bool condP = false;

        bool controlDS = true;
        bool controlDS2 = false;

        bool controlArma = true;

        bool ccorT = true;
        bool ccorF = true;
        bool ccorM = true;
        bool ccorO = true;

        //Pistolas
        bool controlGatilhoPistolaClassica = true;
        bool controlGatilhoCombatPistol = true;
        bool controlGatilhoPistol50 = true;
        bool controlGatilhoSNSPistol = true;
        bool controlGatilhoHeavyPistol = true;
        bool controlGatilhoVintagePistol = true;
        bool controlGatilhoMarksmanPistol = true;
        bool controlGatilhoPistolAP = true;
        bool controlGatilhoStunGun = true;

        //SMGs
        bool controlGatilhoMicroSMG = true;
        bool controlGatilhoMachinePistol = true;
        bool controlGatilhoMiniSMG = true;
        bool controlGatilhoSMG = true;
        bool controlGatilhoAssaltSMG = true;
        bool controlGatilhoCombatPDW = true;
        bool controlGatilhoMG = true;
        bool controlGatilhoCombatMG = true;
        bool controlGatilhoGusenberg = true;

        //Rifles
        bool controlGatilhoCarbineRifle = true;
        bool controlGatilhoAssaultRifle = true;
        bool controlGatilhoAdvancedRifle = true;
        bool controlGatilhoBullpupRifle = true;
        bool controlGatilhoCompactRifle = true;
        bool controlGatilhoSpecialCarbine = true;

        //Snipers
        bool controlGatilhoSniperRifle = true;
        bool controlGatilhoHeavySniper = true;
        bool controlGatilhoMarksmanRifle = true;

        //Escopetas
        bool controlGatilhoPumpShotgun = true;
        bool controlGatilhoOffShotgun = true;
        bool controlGatilhoBShotgun = true;
        bool controlGatilhoAssaltShotgun = true;
        bool controlGatilhoMusket = true;
        bool controlGatilhoHeavyShotgun = true;
        bool controlGatilhoDBShotgun = true;
        bool controlGatilhoSweeperShotgun = true;

        //Pesadas
        bool controlGatilhoMinigun = true;
        bool controlGatilhoRPG = true;
        bool controlGatilhoLG = true;
        bool controlGatilhoLGC = true;
        bool controlGatilhoLMT = true;
        bool controlGatilhoRailgun = true;

        //LEDs
        bool controlLEDPersonagens = true;
        bool controlLEDPolicia = true;
        bool controlLEDControle = false;
        bool controlLEDPadrao = true;

        static int tickCounter = 0;

        static Packet bk = null;

        //Veiculos
        bool controlVeiculos = true;

        #endregion

        public DualSense()
        {
            if(Instance == null) Instance = this;

            CosturaUtility.Initialize();

            Connect();
            LoadUI();

            // Events

            this.Tick += OnTick;
            this.KeyUp += OnKeyUp;
            this.KeyDown += OnKeyDown;

            subMenuPistolas.SelectedIndexChanged += SubMenuPistolas_SelectedIndexChanged;
            subMenuRifles.SelectedIndexChanged += SubMenuRifles_SelectedIndexChanged;
            subMenuSMG.SelectedIndexChanged += SubMenuSMG_SelectedIndexChanged;
            subMenuSnipers.SelectedIndexChanged += SubMenuSnipers_SelectedIndexChanged;
            subMenuEscopetas.SelectedIndexChanged += SubMenuEscopetas_SelectedIndexChanged;
            subMenuPesadas.SelectedIndexChanged += SubMenuPesadas_SelectedIndexChanged;
            subMenuLeds.SelectedIndexChanged += SubMenuLeds_SelectedIndexChanged;
            subMenuVeiculos.SelectedIndexChanged += SubMenuVeiculos_SelectedIndexChanged;

            // Pistols
            ListItemsP1.ItemChanged += ListItemsP1_ItemChanged;
            ListItemsP2.ItemChanged += ListItemsP2_ItemChanged;
            ListItemsP3.ItemChanged += ListItemsP3_ItemChanged;
            ListItemsP4.ItemChanged += ListItemsP4_ItemChanged;
            ListItemsP5.ItemChanged += ListItemsP5_ItemChanged;
            ListItemsP6.ItemChanged += ListItemsP6_ItemChanged;
            ListItemsP7.ItemChanged += ListItemsP7_ItemChanged;
            ListItemsP8.ItemChanged += ListItemsP8_ItemChanged;
            ListItemsP9.ItemChanged += ListItemsP9_ItemChanged;

            // SMGs
            ListItemsM1.ItemChanged += ListItemsM1_ItemChanged;
            ListItemsM2.ItemChanged += ListItemsM2_ItemChanged;
            ListItemsM3.ItemChanged += ListItemsM3_ItemChanged;
            ListItemsM4.ItemChanged += ListItemsM4_ItemChanged;
            ListItemsM5.ItemChanged += ListItemsM5_ItemChanged;
            ListItemsM6.ItemChanged += ListItemsM6_ItemChanged;
            ListItemsM7.ItemChanged += ListItemsM7_ItemChanged;
            ListItemsM8.ItemChanged += ListItemsM8_ItemChanged;
            ListItemsM9.ItemChanged += ListItemsM9_ItemChanged;

            // Rifles
            ListItemsR1.ItemChanged += ListItemsR1_ItemChanged;
            ListItemsR2.ItemChanged += ListItemsR2_ItemChanged;
            ListItemsR3.ItemChanged += ListItemsR3_ItemChanged;
            ListItemsR4.ItemChanged += ListItemsR4_ItemChanged;
            ListItemsR5.ItemChanged += ListItemsR5_ItemChanged;
            ListItemsR6.ItemChanged += ListItemsR6_ItemChanged;

            // Snipers
            ListItemsS1.ItemChanged += ListItemsS1_ItemChanged;
            ListItemsS2.ItemChanged += ListItemsS2_ItemChanged;
            ListItemsS3.ItemChanged += ListItemsS3_ItemChanged;

            // Shotguns
            ListItemsE1.ItemChanged += ListItemsE1_ItemChanged;
            ListItemsE2.ItemChanged += ListItemsE2_ItemChanged;
            ListItemsE3.ItemChanged += ListItemsE3_ItemChanged;
            ListItemsE4.ItemChanged += ListItemsE4_ItemChanged;
            ListItemsE5.ItemChanged += ListItemsE5_ItemChanged;
            ListItemsE6.ItemChanged += ListItemsE6_ItemChanged;
            ListItemsE7.ItemChanged += ListItemsE7_ItemChanged;
            ListItemsE8.ItemChanged += ListItemsE8_ItemChanged;

            // Heavy
            ListItemsPE1.ItemChanged += ListItemsPE1_ItemChanged;
            ListItemsPE2.ItemChanged += ListItemsPE2_ItemChanged;
            ListItemsPE3.ItemChanged += ListItemsPE3_ItemChanged;
            ListItemsPE4.ItemChanged += ListItemsPE4_ItemChanged;
            ListItemsPE5.ItemChanged += ListItemsPE5_ItemChanged;
            ListItemsPE6.ItemChanged += ListItemsPE6_ItemChanged;

            // LEDs
            ListItemsLED1.ItemChanged += ListItemsLED1_ItemChanged;
            ListItemsLED2.ItemChanged += ListItemsLED2_ItemChanged;
            ListItemsLED3.ItemChanged += ListItemsLED3_ItemChanged;

            // Vehicles
            ListItemsV1.ItemChanged += ListItemsV1_ItemChanged;
            
        }

        private void OnTick(object sender, EventArgs e)
        {
            pool.Process();

            ControlLEDs();

            if (isValidForChangeTriggers() && controlN)
            {
                controlN = false;
                control = true;
                controlDS = true;
                controlArma = true;

                ResetTriggers();
            }
            else if (Game.Player.Character.Weapons.Current.AmmoInClip >= 0 && Game.Player.Character.IsReloading == false)
            {
                var currentWeaponHash = Game.Player.Character.Weapons.Current.Hash;

                if (weaponMappings.TryGetValue(currentWeaponHash, out var weaponAction))
                {
                    weaponAction.Item1.Invoke();
                }
                else if (controlArma)
                {
                    controlArma = false;
                    controlDS = true;

                    ResetTriggers();
                }
            }

            if (controlVeiculos)
            {
                if (Game.Player.Character.IsInVehicle() && controlV && !Game.Player.Character.IsJumpingOutOfVehicle)
                {
                    controlV = false;
                    controlV2 = true;

                    ChangeTriggersVehicle();
                }
                else if ((Game.Player.Character.IsJumpingOutOfVehicle || !Game.Player.Character.IsSittingInVehicle()) && controlV2)
                {
                    controlV = true;
                    controlV2 = false;

                    ResetTriggers();
                }
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            // Nothing for now
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Open config menu

            if (e.KeyCode == Keys.F10)
            {
                if(!menu.Visible)
                {
                    menu.Visible = true;
                } else
                {
                    menu.Visible = false;
                }
            }

            #region Test

            /*if(e.KeyCode == Keys.NumPad1)
            {
                var npc = World.CreatePed(PedHash.Abigail, Game.Player.Character.GetOffsetPosition(new Vector3(0, 5, 0)));
                npc.Weapons.Give(WeaponHash.BattleAxe, 1, true, true);
                npc.Task.FightAgainst(Game.Player.Character);
            }

            if(e.KeyCode == Keys.NumPad3)
            {
                //World.AddExplosion(Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 20, 0)), ExplosionType.Tanker, 10, 1);

                foreach (string linha in File.ReadLines(@"C:\Program Files (x86)\Steam\steamapps\common\DSX\DSX_Savefile\DSX_SaveFile.ini"))
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
            }*/

            #endregion
        }

        #region Triggers functions

        private void ActivateWeaponTriggers(bool controlGatilhos, int cadenciaTiros, int modoGatilho, int ticksChange)
        {
            if (control && Game.Player.Character.IsShooting == true)
            {
                control = false;
                controlN = true;

                controlDS = true;
                controlDS2 = true;
                contRifles = 0;
                controlArma = true;               

                Packet p = new Packet();
                p.instructions = new Instruction[6];

                if (modoGatilho == 0)
                {
                    p.instructions[0].type = InstructionType.TriggerUpdate;
                    p.instructions[0].parameters = new object[] { 0, Trigger.Left, TriggerMode.Resistance, 0, 1 };
                }
                else if (modoGatilho == 1)
                {
                    p.instructions[0].type = InstructionType.TriggerUpdate;
                    p.instructions[0].parameters = new object[] { 0, Trigger.Left, TriggerMode.Resistance, 0, 3 };
                }

                if (controlGatilhos == true)
                {

                    p.instructions[1].type = InstructionType.TriggerUpdate;
                    p.instructions[1].parameters = new object[] { 0, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.PulseB, cadenciaTiros, 255, 140, 0, 0, 0, 0 };

                } 
                else
                {
                    if (modoGatilho == 0)
                    {
                        p.instructions[1].type = InstructionType.TriggerUpdate;
                        p.instructions[1].parameters = new object[] { 0, Trigger.Right, TriggerMode.Soft };
                    }
                    else if (modoGatilho == 1) 
                    {
                        p.instructions[1].type = InstructionType.TriggerUpdate;
                        p.instructions[1].parameters = new object[] { 0, Trigger.Right, TriggerMode.VeryHard };
                    }
                }

                Send(p);
            }
            else if (control && controlDS)
            {
                controlDS = false;
                controlDS2 = true;
                controlN = true;

                Packet p = new Packet();
                p.instructions = new Instruction[6];

                if (modoGatilho == 0)
                {
                    p.instructions[0].type = InstructionType.TriggerUpdate;
                    p.instructions[0].parameters = new object[] { 0, Trigger.Left, TriggerMode.Resistance, 0, 1 };
                }
                else if (modoGatilho == 1)
                {
                    p.instructions[0].type = InstructionType.TriggerUpdate;
                    p.instructions[0].parameters = new object[] { 0, Trigger.Left, TriggerMode.Resistance, 0, 3 };
                }

                if (modoGatilho == 0)
                {
                    p.instructions[1].type = InstructionType.TriggerUpdate;
                    p.instructions[1].parameters = new object[] { 0, Trigger.Right, TriggerMode.Soft };
                }
                else if (modoGatilho == 1)
                {
                    p.instructions[1].type = InstructionType.TriggerUpdate;
                    p.instructions[1].parameters = new object[] { 0, Trigger.Right, TriggerMode.VeryHard };
                }

                Send(p);

            }
            else if (Game.Player.Character.IsShooting == false && controlDS2 && !control && controlGatilhos)
            {
                if (contRifles >= ticksChange)
                {
                    controlDS2 = false;
                    contRifles = 0;
                    control = true;

                    Packet p = new Packet();
                    p.instructions = new Instruction[6];

                    p.instructions[0].type = InstructionType.TriggerUpdate;
                    p.instructions[0].parameters = new object[] { 0, Trigger.Left, TriggerMode.Resistance, 0, 1 };

                    p.instructions[1].type = InstructionType.TriggerUpdate;
                    p.instructions[1].parameters = new object[] { 0, Trigger.Right, TriggerMode.Soft };

                    Send(p);

                }
                else
                {
                    contRifles++;
                }
            }
            else if (Game.Player.Character.IsShooting == true && controlGatilhos)
            {
                contRifles = 0;
                controlDS2 = true;
            }

        }

        private void ChangeTriggersVehicle()
        {
            Packet p = new Packet();
            p.instructions = new Instruction[6];

            p.instructions[0].type = InstructionType.TriggerUpdate;
            p.instructions[0].parameters = new object[] { 0, Trigger.Left, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Rigid, 70, 5, 0, 0, 0, 0, 0 };

            p.instructions[1].type = InstructionType.TriggerUpdate;
            p.instructions[1].parameters = new object[] { 0, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Rigid, 110, 2, 0, 0, 0, 0, 0 };

            Send(p);
        }

        private void ResetTriggers()
        {
            Packet p = new Packet();
            p.instructions = new Instruction[6];

            p.instructions[0].type = InstructionType.TriggerUpdate;
            p.instructions[0].parameters = new object[] { 0, Trigger.Left, TriggerMode.Normal };

            p.instructions[1].type = InstructionType.TriggerUpdate;
            p.instructions[1].parameters = new object[] { 0, Trigger.Right, TriggerMode.Normal };

            Send(p);
        }

        private bool isValidForChangeTriggers()
        {
            if ((Game.Player.Character.Weapons.Current.AmmoInClip == 0 &&
                Game.Player.Character.IsReloading) ||
                Game.Player.Character.IsReloading ||
                Game.Player.Character.IsJumping ||
                Game.Player.Character.IsGettingUp ||
                Game.Player.Character.IsInMeleeCombat ||
                Game.Player.Character.IsInjured ||
                Game.Player.Character.IsInVehicle() ||
                Game.Player.Character.IsSwimming ||
                Game.Player.Character.IsSwimmingUnderWater ||
                Game.Player.Character.IsProne ||
                Game.Player.Character.IsJacking ||
                Game.Player.Character.IsRagdoll ||
                Game.Player.Character.IsJumpingOutOfVehicle ||
                Game.Player.Character.IsFalling ||
                Game.Player.Character.IsClimbing ||
                Game.Player.Character.IsDiving ||
                Game.Player.Character.IsDead ||
                Game.Player.Character.IsCuffed ||
                Game.Player.Character.IsBeingStunned ||
                Game.Player.Character.IsGettingIntoVehicle ||
                Game.Player.Character.Weapons.Current.Hash == WeaponHash.Unarmed) return true;
            else return false;

        }

        #endregion

        #region Color LEDs functions

        private void ControlLEDs()
        {
            tickCounter++;

            if (tickCounter > 600)
            {
                tickCounter = 0;
                if (bk != null) Send(bk);
            }

            if (!controlLEDControle && controlLEDPersonagens && controlLEDPolicia)
            {
                ColorJoystick();
                ColorJoystickWanted();
            }
            else if (!controlLEDControle && !controlLEDPersonagens && controlLEDPolicia)
            {
                DefaultColor();
                ColorJoystickWanted();
            }
            else
                DefaultColor();
        }

        private void ColorJoystick()
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
                p.instructions[2].parameters = new object[] { 0, 255, 100, 0 }; //Orange

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
                p.instructions[2].parameters = new object[] { 0, 50, 255, 0 }; //Green

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
                p.instructions[2].parameters = new object[] { 0, 0, 50, 255 }; //Blue

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
                p.instructions[2].parameters = new object[] { 0, 255, 0, 255 }; //Purple

                Send(p);
            }
        }

        private void ColorJoystickWanted()
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

                if (controlLEDPersonagens)
                {
                    ccorT = true;
                    ccorM = true;
                    ccorF = true;
                    ccorO = true;
                    ColorJoystick();
                }
                else
                {
                    controlLEDPadrao = true;
                    DefaultColor();
                }
            }
        }

        private void DefaultColor()
        {
            if (controlLEDPadrao)
            {
                controlLEDPadrao = false;

                foreach (string linha in File.ReadLines(@"C:\Program Files (x86)\Steam\steamapps\common\DSX\DSX_Savefile\DSX_SaveFile.ini"))
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
                    }
                }
            }
        }

        #endregion

        #region UI functions

        private void LoadUI()
        {
            pool = new ObjectPool();
            menu = new NativeMenu("DualSenseV", "Escolha uma opção");
            pool.Add(menu);

            subMenuGatilhos = new NativeMenu("Gatilhos", "Ajustes dos gatilhos");
            menu.AddSubMenu(subMenuGatilhos);
            pool.Add(subMenuGatilhos);

            subMenuArmas = new NativeMenu("Armas", "Ajustes das armas");
            subMenuGatilhos.AddSubMenu(subMenuArmas);
            pool.Add(subMenuArmas);

            //Lista de pistolas
            subMenuPistolas = new NativeMenu("Pistolas", "Ajustes das Pistolas");
            subMenuArmas.AddSubMenu(subMenuPistolas);
            ListItemsP1 = new NativeListItem<string>("Pistola Clássica", "Altera o modo dos gatilhos da Pistola Clássica", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuPistolas.Add(ListItemsP1);
            ListItemsP2 = new NativeListItem<string>("Pistola de Comb...", "Altera o modo dos gatilhos da Pistola de Combate", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuPistolas.Add(ListItemsP2);
            ListItemsP3 = new NativeListItem<string>("Pistola .50", "Altera o modo dos gatilhos da Pistola .50", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuPistolas.Add(ListItemsP3);
            ListItemsP4 = new NativeListItem<string>("Pistola Fajuta", "Altera o modo dos gatilhos da Pistola Fajuta", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuPistolas.Add(ListItemsP4);
            ListItemsP5 = new NativeListItem<string>("Pistola Pesada", "Altera o modo dos gatilhos da Pistola Pesada", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuPistolas.Add(ListItemsP5);
            ListItemsP6 = new NativeListItem<string>("Pistola Vintage", "Altera o modo dos gatilhos da Pistola Vintage", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuPistolas.Add(ListItemsP6);
            ListItemsP7 = new NativeListItem<string>("Trabuco", "Altera o modo dos gatilhos do Trabuco", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuPistolas.Add(ListItemsP7);
            ListItemsP8 = new NativeListItem<string>("Pistola AP", "Altera o modo dos gatilhos da Pistola AP", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuPistolas.Add(ListItemsP8);
            ListItemsP9 = new NativeListItem<string>("Arma de Choque", "Altera o modo dos gatilhos da Arma de Choque (Stungun)", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuPistolas.Add(ListItemsP9);
            pool.Add(subMenuPistolas);

            //Listas de SMGs
            subMenuSMG = new NativeMenu("SMGs", "Ajustes das SMGs");
            subMenuArmas.AddSubMenu(subMenuSMG);
            ListItemsM1 = new NativeListItem<string>("Micro SMG", "Altera o modo dos gatilhos da Micro SMG", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuSMG.Add(ListItemsM1);
            ListItemsM2 = new NativeListItem<string>("Pistola Metralh...", "Altera o modo dos gatilhos da Pistola Metralhadora", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuSMG.Add(ListItemsM2);
            ListItemsM3 = new NativeListItem<string>("Mini SMG", "Altera o modo dos gatilhos da Mini SMG", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuSMG.Add(ListItemsM3);
            ListItemsM4 = new NativeListItem<string>("Submetralhadora", "Altera o modo dos gatilhos da Submetralhadora", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuSMG.Add(ListItemsM4);
            ListItemsM5 = new NativeListItem<string>("SMG de Combate", "Altera o modo dos gatilhos da Submetralhadora de Combate", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuSMG.Add(ListItemsM5);
            ListItemsM6 = new NativeListItem<string>("ADP de Combate", "Altera o modo dos gatilhos da ADP de Combate", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuSMG.Add(ListItemsM6);
            ListItemsM7 = new NativeListItem<string>("Metralhadora", "Altera o modo dos gatilhos da Metralhadora", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuSMG.Add(ListItemsM7);
            ListItemsM8 = new NativeListItem<string>("MG de Combate", "Altera o modo dos gatilhos da Metralhadora de Combate", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuSMG.Add(ListItemsM8);
            ListItemsM9 = new NativeListItem<string>("Metranca", "Altera o modo dos gatilhos da Metranca", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuSMG.Add(ListItemsM9);
            pool.Add(subMenuSMG);

            //Listas de rifles
            subMenuRifles = new NativeMenu("Rifles", "Ajustes dos Rifles");
            subMenuArmas.AddSubMenu(subMenuRifles);
            ListItemsR1 = new NativeListItem<string>("Carabina", "Altera o modo dos gatilhos da carabina", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuRifles.Add(ListItemsR1);
            ListItemsR2 = new NativeListItem<string>("Carabina Especial", "Altera o modo dos gatilhos da Carabina Especial", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuRifles.Add(ListItemsR2);
            ListItemsR3 = new NativeListItem<string>("Fuzil", "Altera o modo dos gatilhos do Fuzil", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuRifles.Add(ListItemsR3);
            ListItemsR4 = new NativeListItem<string>("Fuzil Compacto", "Altera o modo dos gatilhos do Fuzil Compacto", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuRifles.Add(ListItemsR4);
            ListItemsR5 = new NativeListItem<string>("Fuzil Bullpup", "Altera o modo dos gatilhos do fuzil Bullpup", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuRifles.Add(ListItemsR5);
            ListItemsR6 = new NativeListItem<string>("Fuzil Avançado", "Altera o modo dos gatilhos do fuzil Avançado", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuRifles.Add(ListItemsR6);
            pool.Add(subMenuRifles);

            //Listas de snipers
            subMenuSnipers = new NativeMenu("Snipers", "Ajustes das Snipers");
            subMenuArmas.AddSubMenu(subMenuSnipers);
            ListItemsS1 = new NativeListItem<string>("Rifle de Precisão", "Altera o modo dos gatilhos do Rifle de Precisão", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuSnipers.Add(ListItemsS1);
            ListItemsS2 = new NativeListItem<string>("Heavy Sniper", "Altera o modo dos gatilhos do Rifle de Precisão Pesado", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuSnipers.Add(ListItemsS2);
            ListItemsS3 = new NativeListItem<string>("Rifle de Elite", "Altera o modo dos gatilhos do Rifle de Elite", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuSnipers.Add(ListItemsS3);
            pool.Add(subMenuSnipers);

            //Lista de escopetas
            subMenuEscopetas = new NativeMenu("Escopetas", "Ajustes das Escopetas");
            subMenuArmas.AddSubMenu(subMenuEscopetas);
            ListItemsE1 = new NativeListItem<string>("Escopeta", "Altera o modo dos gatilhos da Escopeta", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuEscopetas.Add(ListItemsE1);
            ListItemsE2 = new NativeListItem<string>("Escopeta Serrada", "Altera o modo dos gatilhos da Escopeta de Cano Serrado", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuEscopetas.Add(ListItemsE2);
            ListItemsE3 = new NativeListItem<string>("Escopeta Bullpup", "Altera o modo dos gatilhos da Escopeta Bullpup", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuEscopetas.Add(ListItemsE3);
            ListItemsE4 = new NativeListItem<string>("Escopeta de Comb...", "Altera o modo dos gatilhos da Escopeta de Combate", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuEscopetas.Add(ListItemsE4);
            ListItemsE5 = new NativeListItem<string>("Mosquete", "Altera o modo dos gatilhos do Mosquete", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuEscopetas.Add(ListItemsE5);
            ListItemsE6 = new NativeListItem<string>("Escopeta Pesada", "Altera o modo dos gatilhos da Escopeta Pesada", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuEscopetas.Add(ListItemsE6);
            ListItemsE7 = new NativeListItem<string>("Escopeta C. Duplo", "Altera o modo dos gatilhos da Escopeta Cano Duplo", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuEscopetas.Add(ListItemsE7);
            ListItemsE8 = new NativeListItem<string>("Escopeta Automática", "Altera o modo dos gatilhos da Escopeta Automática", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuEscopetas.Add(ListItemsE8);
            pool.Add(subMenuEscopetas);

            //Lista de armas pesadas
            subMenuPesadas = new NativeMenu("Armas Pesadas", "Ajustes das Armas Pesadas");
            subMenuArmas.AddSubMenu(subMenuPesadas);
            ListItemsPE1 = new NativeListItem<string>("Minigun", "Altera o modo dos gatilhos da Minigun", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuPesadas.Add(ListItemsPE1);
            ListItemsPE2 = new NativeListItem<string>("RPG", "Altera o modo dos gatilhos da RPG", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuPesadas.Add(ListItemsPE2);
            ListItemsPE3 = new NativeListItem<string>("Lança-granada", "Altera o modo dos gatilhos do Lança-granada", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuPesadas.Add(ListItemsPE3);
            ListItemsPE4 = new NativeListItem<string>("Lança-granada Comp.", "Altera o modo dos gatilhos do Lança-granada Compacto", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuPesadas.Add(ListItemsPE4);
            ListItemsPE5 = new NativeListItem<string>("Lança-mísseis Tel.", "Altera o modo dos gatilhos do Lança-mísseis Teleguiado", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuPesadas.Add(ListItemsPE5);
            ListItemsPE6 = new NativeListItem<string>("Canhão-elétrico", "Altera o modo dos gatilhos do Canhão-elétrico (Railgun)", "Gatilho com recuo", "Gatilho sem recuo");
            subMenuPesadas.Add(ListItemsPE6);
            pool.Add(subMenuPesadas);

            subMenuLeds = new NativeMenu("Cores Touchpad", "Cores do Touchpad do Controle");
            ListItemsLED1 = new NativeListItem<string>("Cores dos personagens", "Altera o modo dos LEDs de acordo com a cor do personagem que está sendo jogado", "Ativo", "Desativado");
            subMenuLeds.Add(ListItemsLED1);
            ListItemsLED2 = new NativeListItem<string>("Fugindo da polícia", "Altera o modo dos LEDs caso você esteja fugindo da polícia", "Ativo", "Desativado");
            subMenuLeds.Add(ListItemsLED2);
            ListItemsLED3 = new NativeListItem<string>("Utilizar minhas cores", "Altera o modo dos LEDs para o seu perfil pré-definido do DualSenseX (isso acaba ignorando todas as configurações acima)", "Desativado", "Ativado");
            subMenuLeds.Add(ListItemsLED3);
            menu.AddSubMenu(subMenuLeds);
            pool.Add(subMenuLeds);

            subMenuVeiculos = new NativeMenu("Veículos", "Ajustes dos veículos");
            subMenuGatilhos.AddSubMenu(subMenuVeiculos);
            ListItemsV1 = new NativeListItem<string>("Gatilhos no Veículo", "Altera o modo dos gatilhos nos veículos", "Ativado", "Desativado");
            subMenuVeiculos.Add(ListItemsV1);
            pool.Add(subMenuVeiculos);
        }

        #endregion

        #region Menu event functions

        private void ListItemsV1_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsV1.SelectedIndex == 0)
            {
                controlVeiculos = true;
                controlV = true;
                controlV2 = true;

                GTA.UI.Screen.ShowSubtitle("Gatilhos nos veículos ~g~ativado");

            }
            else if (ListItemsV1.SelectedIndex == 1)
            {
                controlVeiculos = false;

                if(Game.Player.Character.IsInVehicle())
                {
                    Packet p = new Packet();
                    p.instructions = new Instruction[6];

                    p.instructions[0].type = InstructionType.TriggerUpdate;
                    p.instructions[0].parameters = new object[] { 0, Trigger.Left, TriggerMode.Normal };

                    p.instructions[1].type = InstructionType.TriggerUpdate;
                    p.instructions[1].parameters = new object[] { 0, Trigger.Right, TriggerMode.Normal };

                    Send(p);
                }

                GTA.UI.Screen.ShowSubtitle("Gatilhos nos veículos ~r~desativado");
            }
        }

        private void ListItemsLED3_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsLED3.SelectedIndex == 0)
            {
                controlLEDControle = false;
                ccorT = true;
                ccorF = true;
                ccorM = true;
                ccorO = true;

                GTA.UI.Screen.ShowSubtitle("Cor padrão do touchpad ~r~desativada");

            }
            else if (ListItemsLED3.SelectedIndex == 1)
            {
                controlLEDControle = true;
                controlLEDPadrao = true;
                GTA.UI.Screen.ShowSubtitle("Cor padrão do touchpad ~g~ativada");
            }
        }

        private void ListItemsLED2_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsLED2.SelectedIndex == 0)
            {
                controlLEDPolicia = true;
                GTA.UI.Screen.ShowSubtitle("Cor ao fugir da polícia ~g~ativada");

            }
            else if (ListItemsLED2.SelectedIndex == 1)
            {
                controlLEDPolicia = false;
                GTA.UI.Screen.ShowSubtitle("Cor ao fugir da polícia ~r~desativada");
            }
        }

        private void ListItemsLED1_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsLED1.SelectedIndex == 0)
            {
                controlLEDPersonagens = true;
                ccorT = true;
                ccorF = true;
                ccorM = true;
                ccorO = true;

                GTA.UI.Screen.ShowSubtitle("Cores dos personagens ~g~ativado");

            }
            else if (ListItemsLED1.SelectedIndex == 1)
            {
                controlLEDPersonagens = false;
                controlLEDPadrao = true;
                GTA.UI.Screen.ShowSubtitle("Cores dos personagens ~r~desativado");
            }

        }

        private void ListItemsPE6_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsPE6.SelectedIndex == 0)
            {
                controlGatilhoRailgun = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsPE6.SelectedIndex == 1)
            {
                controlGatilhoRailgun = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsPE5_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsPE5.SelectedIndex == 0)
            {
                controlGatilhoLMT = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsPE5.SelectedIndex == 1)
            {
                controlGatilhoLMT = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsPE4_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsPE4.SelectedIndex == 0)
            {
                controlGatilhoLGC = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsPE4.SelectedIndex == 1)
            {
                controlGatilhoLGC = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsPE3_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsPE3.SelectedIndex == 0)
            {
                controlGatilhoLG = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsPE3.SelectedIndex == 1)
            {
                controlGatilhoLG = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsPE2_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsPE2.SelectedIndex == 0)
            {
                controlGatilhoRPG = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsPE2.SelectedIndex == 1)
            {
                controlGatilhoRPG = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsPE1_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsPE1.SelectedIndex == 0)
            {
                controlGatilhoMinigun = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsPE1.SelectedIndex == 1)
            {
                controlGatilhoMinigun = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsE8_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsE8.SelectedIndex == 0)
            {
                controlGatilhoSweeperShotgun = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsE8.SelectedIndex == 1)
            {
                controlGatilhoSweeperShotgun = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsE7_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsE7.SelectedIndex == 0)
            {
                controlGatilhoDBShotgun = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsE7.SelectedIndex == 1)
            {
                controlGatilhoDBShotgun = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsE6_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsE6.SelectedIndex == 0)
            {
                controlGatilhoHeavyShotgun = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsE6.SelectedIndex == 1)
            {
                controlGatilhoHeavyShotgun = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsE5_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsE5.SelectedIndex == 0)
            {
                controlGatilhoMusket = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsE5.SelectedIndex == 1)
            {
                controlGatilhoMusket = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsE4_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsE4.SelectedIndex == 0)
            {
                controlGatilhoAssaltShotgun = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsE4.SelectedIndex == 1)
            {
                controlGatilhoAssaltShotgun = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsE3_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsE3.SelectedIndex == 0)
            {
                controlGatilhoBShotgun = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsE3.SelectedIndex == 1)
            {
                controlGatilhoBShotgun = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsE2_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsE2.SelectedIndex == 0)
            {
                controlGatilhoOffShotgun = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsE2.SelectedIndex == 1)
            {
                controlGatilhoOffShotgun = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsE1_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsE1.SelectedIndex == 0)
            {
                controlGatilhoPumpShotgun = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsE1.SelectedIndex == 1)
            {
                controlGatilhoPumpShotgun = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsS3_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsS3.SelectedIndex == 0)
            {
                controlGatilhoMarksmanRifle = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsS3.SelectedIndex == 1)
            {
                controlGatilhoMarksmanRifle = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsS2_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsS2.SelectedIndex == 0)
            {
                controlGatilhoHeavySniper = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsS2.SelectedIndex == 1)
            {
                controlGatilhoHeavySniper = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsS1_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsS1.SelectedIndex == 0)
            {
                controlGatilhoSniperRifle = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsS1.SelectedIndex == 1)
            {
                controlGatilhoSniperRifle = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsM9_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsM9.SelectedIndex == 0)
            {
                controlGatilhoGusenberg = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsM9.SelectedIndex == 1)
            {
                controlGatilhoGusenberg = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsM8_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsM8.SelectedIndex == 0)
            {
                controlGatilhoCombatMG = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsM8.SelectedIndex == 1)
            {
                controlGatilhoCombatMG = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsM7_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsM7.SelectedIndex == 0)
            {
                controlGatilhoMG = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsM7.SelectedIndex == 1)
            {
                controlGatilhoMG = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsM6_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsM6.SelectedIndex == 0)
            {
                controlGatilhoCombatPDW = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsM6.SelectedIndex == 1)
            {
                controlGatilhoCombatPDW = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsM5_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsM5.SelectedIndex == 0)
            {
                controlGatilhoAssaltSMG = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsM5.SelectedIndex == 1)
            {
                controlGatilhoAssaltSMG = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsM4_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsM4.SelectedIndex == 0)
            {
                controlGatilhoSMG = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsM4.SelectedIndex == 1)
            {
                controlGatilhoSMG = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsM3_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsM3.SelectedIndex == 0)
            {
                controlGatilhoMiniSMG = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsM3.SelectedIndex == 1)
            {
                controlGatilhoMiniSMG = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsM2_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsM2.SelectedIndex == 0)
            {
                controlGatilhoMachinePistol = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsM2.SelectedIndex == 1)
            {
                controlGatilhoMachinePistol = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsM1_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsM1.SelectedIndex == 0)
            {
                controlGatilhoMicroSMG = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsM1.SelectedIndex == 1)
            {
                controlGatilhoMicroSMG = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsP9_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsP9.SelectedIndex == 0)
            {
                controlGatilhoStunGun = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsP9.SelectedIndex == 1)
            {
                controlGatilhoStunGun = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsP8_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsP8.SelectedIndex == 0)
            {
                controlGatilhoPistolAP = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsP8.SelectedIndex == 1)
            {
                controlGatilhoPistolAP = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsP7_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsP7.SelectedIndex == 0)
            {
                controlGatilhoMarksmanPistol = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsP7.SelectedIndex == 1)
            {
                controlGatilhoMarksmanPistol = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsP6_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsP6.SelectedIndex == 0)
            {
                controlGatilhoVintagePistol = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsP6.SelectedIndex == 1)
            {
                controlGatilhoVintagePistol = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsP5_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsP5.SelectedIndex == 0)
            {
                controlGatilhoHeavyPistol = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsP5.SelectedIndex == 1)
            {
                controlGatilhoHeavyPistol = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsP4_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsP4.SelectedIndex == 0)
            {
                controlGatilhoSNSPistol = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsP4.SelectedIndex == 1)
            {
                controlGatilhoSNSPistol = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsP3_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsP3.SelectedIndex == 0)
            {
                controlGatilhoPistol50 = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsP3.SelectedIndex == 1)
            {
                controlGatilhoPistol50 = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsP2_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsP2.SelectedIndex == 0)
            {
                controlGatilhoCombatPistol = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsP2.SelectedIndex == 1)
            {
                controlGatilhoCombatPistol = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsR6_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsR6.SelectedIndex == 0)
            {
                controlGatilhoAdvancedRifle = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsR6.SelectedIndex == 1)
            {
                controlGatilhoAdvancedRifle = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsR5_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsR5.SelectedIndex == 0)
            {
                controlGatilhoBullpupRifle = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsR5.SelectedIndex == 1)
            {
                controlGatilhoBullpupRifle = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsR4_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsR4.SelectedIndex == 0)
            {
                controlGatilhoCompactRifle = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsR4.SelectedIndex == 1)
            {
                controlGatilhoCompactRifle = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsR3_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsR3.SelectedIndex == 0)
            {
                controlGatilhoAssaultRifle = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsR3.SelectedIndex == 1)
            {
                controlGatilhoAssaultRifle = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsR2_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsR2.SelectedIndex == 0)
            {
                controlGatilhoSpecialCarbine = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsR2.SelectedIndex == 1)
            {
                controlGatilhoSpecialCarbine = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsR1_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            if (ListItemsR1.SelectedIndex == 0)
            {
                controlGatilhoCarbineRifle = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsR1.SelectedIndex == 1)
            {
                controlGatilhoCarbineRifle = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void ListItemsP1_ItemChanged(object sender, ItemChangedEventArgs<string> e)
        {
            GTA.UI.Screen.ShowSubtitle(ListItemsP1.SelectedIndex.ToString());

            if (ListItemsP1.SelectedIndex == 0)
            {
                controlGatilhoPistolaClassica = true;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~g~ativado");

            }
            else if (ListItemsP1.SelectedIndex == 1)
            {
                controlGatilhoPistolaClassica = false;
                GTA.UI.Screen.ShowSubtitle("Recuo no gatilho ~r~desativado");
            }
        }

        private void SubMenuVeiculos_SelectedIndexChanged(object sender, SelectedEventArgs e)
        {
            GTA.UI.Screen.ShowSubtitle("");
        }

        private void SubMenuLeds_SelectedIndexChanged(object sender, SelectedEventArgs e)
        {
            GTA.UI.Screen.ShowSubtitle("");
        }

        private void SubMenuPesadas_SelectedIndexChanged(object sender, SelectedEventArgs e)
        {
            GTA.UI.Screen.ShowSubtitle("");
        }

        private void SubMenuPistolas_SelectedIndexChanged(object sender, SelectedEventArgs e)
        {
            GTA.UI.Screen.ShowSubtitle("");
        }

        private void SubMenuRifles_SelectedIndexChanged(object sender, SelectedEventArgs e)
        {
            GTA.UI.Screen.ShowSubtitle("");
        }

        private void SubMenuEscopetas_SelectedIndexChanged(object sender, SelectedEventArgs e)
        {
            GTA.UI.Screen.ShowSubtitle("");
        }

        private void SubMenuSnipers_SelectedIndexChanged(object sender, SelectedEventArgs e)
        {
            GTA.UI.Screen.ShowSubtitle("");
        }

        private void SubMenuSMG_SelectedIndexChanged(object sender, SelectedEventArgs e)
        {
            GTA.UI.Screen.ShowSubtitle("");
        }

        #endregion

        #region Static functions

        private static Dictionary<WeaponHash, Tuple<Action>> weaponMappings = new Dictionary<WeaponHash, Tuple<Action>>
        {        
            // Pistols
            { WeaponHash.Pistol, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoPistolaClassica, 3, 0, 10))) },
            { WeaponHash.CombatPistol, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoCombatPistol, 3, 0, 10))) },
            { WeaponHash.Pistol50, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoPistol50, 2, 0, 10))) },
            { WeaponHash.SNSPistol, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoSNSPistol, 3, 0, 10))) },
            { WeaponHash.HeavyPistol, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoHeavyPistol, 2, 0, 10))) },
            { WeaponHash.VintagePistol, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoVintagePistol, 2, 0, 10))) },
            { WeaponHash.MarksmanPistol, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoMarksmanPistol, 1, 1, 10))) },
            { WeaponHash.Revolver, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(false, 0, 1, 10))) },
            { WeaponHash.APPistol, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoPistolAP, 8, 0, 10))) },
            { WeaponHash.StunGun, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoStunGun, 1, 0, 10))) },

            // SMGs
            { WeaponHash.MicroSMG, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoMicroSMG, 10, 0, 10))) },
            { WeaponHash.MachinePistol, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoMachinePistol, 9, 0, 10))) },
            { WeaponHash.MiniSMG, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoMiniSMG, 10, 0, 10))) },
            { WeaponHash.SMG, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoSMG, 10, 0, 10))) },
            { WeaponHash.AssaultSMG, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoAssaltSMG, 10, 0, 10))) },
            { WeaponHash.CombatPDW, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoCombatPDW, 8, 0, 10))) },
            { WeaponHash.MG, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoMG, 7, 0, 10))) },
            { WeaponHash.CombatMG, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoCombatMG, 8, 0, 10))) },
            { WeaponHash.Gusenberg, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoGusenberg, 10, 0, 10))) },

            // Rifles
            { WeaponHash.CarbineRifle, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoCarbineRifle, 9, 0, 10))) },
            { WeaponHash.AssaultRifle, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoAssaultRifle, 9, 0, 10))) },
            { WeaponHash.BullpupRifle, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoBullpupRifle, 9, 0, 10))) },
            { WeaponHash.AdvancedRifle, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoAdvancedRifle, 9, 0, 10))) },
            { WeaponHash.CompactRifle, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoCompactRifle, 8, 0, 10))) },
            { WeaponHash.SpecialCarbine, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoSpecialCarbine, 10, 0, 10))) },

            // Snipers
            { WeaponHash.SniperRifle, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoSniperRifle, 1, 0, 10))) },
            { WeaponHash.HeavySniper, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoHeavySniper, 1, 0, 10))) },
            { WeaponHash.MarksmanRifle, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoMarksmanRifle, 3, 0, 20))) },

            // Shotguns
            { WeaponHash.PumpShotgun, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoPumpShotgun, 1, 0, 10))) },
            { WeaponHash.SawnOffShotgun, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoOffShotgun, 1, 0, 10))) },
            { WeaponHash.BullpupShotgun, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoBShotgun, 1, 0, 10))) },
            { WeaponHash.AssaultShotgun, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoAssaltShotgun, 4, 0, 20))) },
            { WeaponHash.Musket, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoMusket, 1, 0, 10))) },
            { WeaponHash.HeavyShotgun, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoHeavyShotgun, 4, 0, 20))) },
            { WeaponHash.DoubleBarrelShotgun, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoDBShotgun, 5, 0, 10))) },
            { WeaponHash.SweeperShotgun, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoSweeperShotgun, 3, 0, 20))) },

            // Heavy
            { WeaponHash.Minigun, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoMinigun, 17, 0, 25))) },
            { WeaponHash.RPG, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoRPG, 1, 0, 10))) },
            { WeaponHash.GrenadeLauncher, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoLG, 1, 0, 10))) },
            { WeaponHash.CompactGrenadeLauncher, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoLGC, 1, 0, 10))) },
            { WeaponHash.HomingLauncher, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoLMT, 1, 0, 10))) },
            { WeaponHash.Railgun, Tuple.Create(new Action(() => Instance.ActivateWeaponTriggers(Instance.controlGatilhoRailgun, 1, 0, 10))) },
        };

        static void Connect()
        {
            client = new UdpClient();
            var portNumber = File.ReadAllText(@"C:\Program Files (x86)\Steam\steamapps\common\DSX\DualSenseX_PortNumber.txt");
            endPoint = new IPEndPoint(Triggers.localhost, Convert.ToInt32(portNumber));
        }

        static void Send(Packet data)
        {
            bk = data;
            tickCounter = 0;

            var RequestData = Encoding.ASCII.GetBytes(Triggers.PacketToJson(data));
            client.Send(RequestData, RequestData.Length, endPoint);
        }

        #endregion
    }
}
