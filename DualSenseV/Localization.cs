using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DualSenseV
{
    public enum Languages
    {
        EN = 0,
        PTBR = 1
    }

    public class Localization
    {
        public Languages Language { get; set; }

        public List<string> BqnnerTexts { get; set; }
        public List<string> DescriptionTexts { get; set; }

        public string RecoilTriggerText { get; set; }
        public string RecoillessTriggerText { get; set; }

        public string ActiveText { get; set; }
        public string DisabledText { get; set; }

        public void CreateJsonFile()
        {
            List<Localization> localizationList = new List<Localization>();

            localizationList.Add(CreateJsonEnglish());
            localizationList.Add(CreateJsonPortuguesBrazilian());

            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + $"/{nameof(Localization)}.json", Newtonsoft.Json.JsonConvert.SerializeObject(localizationList));
        }

        private Localization CreateJsonEnglish()
        {
            Localization localization = new Localization();

            localization.Language = Languages.EN;

            localization.RecoilTriggerText = "Recoil Trigger";
            localization.RecoillessTriggerText = "Recoilless Trigger";

            localization.ActiveText = "Active";
            localization.DisabledText = "Disabled";

            List<string> bqnner = new List<string>();
            List<string> description = new List<string>();

            bqnner.Add("DualSenseV"); description.Add("Choose an option");
            bqnner.Add("Triggers"); description.Add("Trigger settings");
            bqnner.Add("Weapons"); description.Add("Weapon settings");
            bqnner.Add("Pistols"); description.Add("Pistol settings");

            bqnner.Add("Classic Pistol"); description.Add("Change trigger mode for the Classic Pistol");
            bqnner.Add("Combat Pistol"); description.Add("Change trigger mode for the Combat Pistol");
            bqnner.Add(".50 Pistol"); description.Add("Change trigger mode for the .50 Pistol");
            bqnner.Add("Fake Pistol"); description.Add("Change trigger mode for the Fake Pistol");
            bqnner.Add("Heavy Pistol"); description.Add("Change trigger mode for the Heavy Pistol");
            bqnner.Add("Vintage Pistol"); description.Add("Change trigger mode for the Vintage Pistol");
            bqnner.Add("Musket"); description.Add("Change trigger mode for the Musket");
            bqnner.Add("AP Pistol"); description.Add("Change trigger mode for the AP Pistol");
            bqnner.Add("Stun Gun"); description.Add("Change trigger mode for the Stun Gun");

            bqnner.Add("SMGs"); description.Add("SMG settings");
            bqnner.Add("Micro SMG"); description.Add("Change trigger mode for the Micro SMG");
            bqnner.Add("Machine Pistol"); description.Add("Change trigger mode for the Machine Pistol");
            bqnner.Add("Mini SMG"); description.Add("Change trigger mode for the Mini SMG");
            bqnner.Add("S.M.G."); description.Add("Change trigger mode for the Submachine Gun");
            bqnner.Add("Combat PDW"); description.Add("Change trigger mode for the Combat PDW");
            bqnner.Add("Combat MG"); description.Add("Change trigger mode for the Combat MG");
            bqnner.Add("Machine Gun"); description.Add("Change trigger mode for the Machine Gun");
            bqnner.Add("Combat Machine Gun"); description.Add("Change trigger mode for the Combat Machine Gun");
            bqnner.Add("Gatling Gun"); description.Add("Change trigger mode for the Gatling Gun");

            bqnner.Add("Rifles"); description.Add("Rifle settings");
            bqnner.Add("Carbine Rifle"); description.Add("Change trigger mode for the Carbine Rifle");
            bqnner.Add("Special Carbine Rifle"); description.Add("Change trigger mode for the Special Carbine Rifle");
            bqnner.Add("Assault Rifle"); description.Add("Change trigger mode for the Assault Rifle");
            bqnner.Add("Compact Rifle"); description.Add("Change trigger mode for the Compact Rifle");
            bqnner.Add("Bullpup Rifle"); description.Add("Change trigger mode for the Bullpup Rifle");
            bqnner.Add("Advanced Rifle"); description.Add("Change trigger mode for the Advanced Rifle");

            bqnner.Add("Snipers"); description.Add("Sniper settings");
            bqnner.Add("Sniper Rifle"); description.Add("Change trigger mode for the Sniper Rifle");
            bqnner.Add("Heavy Sniper"); description.Add("Change trigger mode for the Heavy Sniper Rifle");
            bqnner.Add("Marksman Rifle"); description.Add("Change trigger mode for the Marksman Rifle");

            bqnner.Add("Shotguns"); description.Add("Shotgun settings");
            bqnner.Add("Shotgun"); description.Add("Change trigger mode for the Shotgun");
            bqnner.Add("Sawed-Off Shotgun"); description.Add("Change trigger mode for the Sawed-Off Shotgun");
            bqnner.Add("Bullpup Shotgun"); description.Add("Change trigger mode for the Bullpup Shotgun");
            bqnner.Add("Combat Shotgun"); description.Add("Change trigger mode for the Combat Shotgun");
            bqnner.Add("Musket"); description.Add("Change trigger mode for the Musket");
            bqnner.Add("Heavy Shotgun"); description.Add("Change trigger mode for the Heavy Shotgun");
            bqnner.Add("Double-Barrel Shotgun"); description.Add("Change trigger mode for the Double-Barrel Shotgun");
            bqnner.Add("Automatic Shotgun"); description.Add("Change trigger mode for the Automatic Shotgun");

            bqnner.Add("Heavy Weapons"); description.Add("Heavy Weapons settings");
            bqnner.Add("Minigun"); description.Add("Change trigger mode for the Minigun");
            bqnner.Add("RPG"); description.Add("Change trigger mode for the RPG");
            bqnner.Add("Grenade Launcher"); description.Add("Change trigger mode for the Grenade Launcher");
            bqnner.Add("Compact Grenade Launcher"); description.Add("Change trigger mode for the Compact Grenade Launcher");
            bqnner.Add("Homing Launcher"); description.Add("Change trigger mode for the Homing Launcher");
            bqnner.Add("Railgun"); description.Add("Change trigger mode for the Railgun");

            bqnner.Add("Touchpad Colors"); description.Add("Controller Touchpad Colors");
            bqnner.Add("Character Colors"); description.Add("Change LEDs according to the character's color being played");
            bqnner.Add("Police Chase"); description.Add("Change LEDs when you are in a police chase");
            bqnner.Add("Use My Colors"); description.Add("Change LEDs to your predefined DualSenseX profile (ignores all settings above)");

            bqnner.Add("Vehicles"); description.Add("Vehicle settings");
            bqnner.Add("Vehicle Triggers"); description.Add("Change trigger mode for vehicle triggers");

            localization.BqnnerTexts = bqnner;
            localization.DescriptionTexts = description;

            return localization;
        }

        private Localization CreateJsonPortuguesBrazilian()
        {
            Localization localization = new Localization();

            localization.Language = Languages.PTBR;

            localization.RecoilTriggerText = "Gatilho com recuo";
            localization.RecoillessTriggerText = "Gatilho sem recuo";

            localization.ActiveText = "Ativado";
            localization.DisabledText = "Desativado";

            List<string> bqnner = new List<string>();
            List<string> description = new List<string>();

            bqnner.Add("DualSenseV"); description.Add("Escolha uma opção");
            bqnner.Add("Gatilhos"); description.Add("Ajustes dos gatilhos");
            bqnner.Add("Armas"); description.Add("Ajustes das armas");
            bqnner.Add("Pistolas"); description.Add("Ajustes das Pistolas");

            bqnner.Add("Pistola Clássica"); description.Add("Altera o modo dos gatilhos da Pistola Clássica");
            bqnner.Add("Pistola de Comb..."); description.Add("Altera o modo dos gatilhos da Pistola de Combate");
            bqnner.Add("Pistola .50"); description.Add("Altera o modo dos gatilhos da Pistola .50");
            bqnner.Add("Pistola Fajuta"); description.Add("Altera o modo dos gatilhos da Pistola Fajuta");
            bqnner.Add("Pistola Pesada"); description.Add("Altera o modo dos gatilhos da Pistola Pesada");
            bqnner.Add("Pistola Vintage"); description.Add("Altera o modo dos gatilhos da Pistola Vintage");
            bqnner.Add("Trabuco"); description.Add("Altera o modo dos gatilhos do Trabuco");
            bqnner.Add("Pistola AP"); description.Add("Altera o modo dos gatilhos da Pistola AP");
            bqnner.Add("Arma de Choque"); description.Add("Altera o modo dos gatilhos da Arma de Choque (Stungun)");

            bqnner.Add("SMGs"); description.Add("Ajustes das SMGs");
            bqnner.Add("Micro SMG"); description.Add("Altera o modo dos gatilhos da Micro SMG");
            bqnner.Add("Pistola Metralh..."); description.Add("Altera o modo dos gatilhos da Mini SMG");
            bqnner.Add("Mini SMG"); description.Add("Altera o modo dos gatilhos da Mini SMG");
            bqnner.Add("Submetralhadora"); description.Add("Altera o modo dos gatilhos da Submetralhadora");
            bqnner.Add("SMG de Combate"); description.Add("Altera o modo dos gatilhos da Submetralhadora de Combate");
            bqnner.Add("ADP de Combate"); description.Add("Altera o modo dos gatilhos da ADP de Combate");
            bqnner.Add("Metralhadora"); description.Add("Altera o modo dos gatilhos da Metralhadora");
            bqnner.Add("MG de Combate"); description.Add("Altera o modo dos gatilhos da Metralhadora de Combate");
            bqnner.Add("Metranca"); description.Add("Altera o modo dos gatilhos da Metranca");

            bqnner.Add("Rifles"); description.Add("Ajustes dos Rifles");
            bqnner.Add("Carabina"); description.Add("Altera o modo dos gatilhos da carabina");
            bqnner.Add("Carabina Especial"); description.Add("Altera o modo dos gatilhos da Carabina Especial");
            bqnner.Add("Fuzil"); description.Add("Altera o modo dos gatilhos do Fuzil");
            bqnner.Add("Fuzil Compacto"); description.Add("Altera o modo dos gatilhos do Fuzil Compacto");
            bqnner.Add("Fuzil Bullpup"); description.Add("Altera o modo dos gatilhos do fuzil Bullpup");
            bqnner.Add("Fuzil Avançado"); description.Add("Altera o modo dos gatilhos do fuzil Avançado");

            bqnner.Add("Snipers"); description.Add("Ajustes das Snipers");
            bqnner.Add("Rifle de Precisão"); description.Add("Altera o modo dos gatilhos do Rifle de Precisão");
            bqnner.Add("Heavy Sniper"); description.Add("Altera o modo dos gatilhos do Rifle de Precisão Pesado");
            bqnner.Add("Rifle de Elite"); description.Add("Altera o modo dos gatilhos do Rifle de Elite");

            bqnner.Add("Escopetas"); description.Add("Ajustes das Escopetas");
            bqnner.Add("Escopeta"); description.Add("Altera o modo dos gatilhos da Escopeta");
            bqnner.Add("Escopeta Serrada"); description.Add("Altera o modo dos gatilhos da Escopeta de Cano Serrado");
            bqnner.Add("Escopeta Bullpup"); description.Add("Altera o modo dos gatilhos da Escopeta Bullpup");
            bqnner.Add("Escopeta de Comb..."); description.Add("Altera o modo dos gatilhos da Escopeta de Combate");
            bqnner.Add("Mosquete"); description.Add("Altera o modo dos gatilhos do Mosquete");
            bqnner.Add("Escopeta Pesada"); description.Add("Altera o modo dos gatilhos da Escopeta Pesada");
            bqnner.Add("Escopeta C. Duplo"); description.Add("Altera o modo dos gatilhos da Escopeta Cano Duplo");
            bqnner.Add("Escopeta Automática"); description.Add("Altera o modo dos gatilhos da Escopeta Automática");

            bqnner.Add("Armas Pesadas"); description.Add("Ajustes das Armas Pesadas");
            bqnner.Add("Minigun"); description.Add("Altera o modo dos gatilhos da Minigun");
            bqnner.Add("RPG"); description.Add("Altera o modo dos gatilhos da RPG");
            bqnner.Add("Lança-granada"); description.Add("Altera o modo dos gatilhos do Lança-granada");
            bqnner.Add("Lança-granada Comp."); description.Add("Altera o modo dos gatilhos do Lança-granada Compacto");
            bqnner.Add("Lança-mísseis Tel."); description.Add("Altera o modo dos gatilhos do Lança-mísseis Teleguiado");
            bqnner.Add("Canhão-elétrico"); description.Add("Altera o modo dos gatilhos do Canhão-elétrico (Railgun)");

            bqnner.Add("Cores Touchpad"); description.Add("Cores do Touchpad do Controle");
            bqnner.Add("Cores dos personagens"); description.Add("Altera o modo dos LEDs de acordo com a cor do personagem que está sendo jogado");
            bqnner.Add("Fugindo da polícia"); description.Add("Altera o modo dos LEDs caso você esteja fugindo da polícia");
            bqnner.Add("Utilizar minhas cores"); description.Add("Altera o modo dos LEDs para o seu perfil pré-definido do DualSenseX (isso acaba ignorando todas as configurações acima)");

            bqnner.Add("Veículos"); description.Add("Ajustes dos veículos");
            bqnner.Add("Gatilhos no Veículo"); description.Add("Altera o modo dos gatilhos nos veículos");

            localization.BqnnerTexts = bqnner;
            localization.DescriptionTexts = description;

            return localization;
        }
    }
}
