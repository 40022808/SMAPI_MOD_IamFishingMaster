using StardewModdingAPI;
using Microsoft.Xna.Framework;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;
using GenericModConfigMenu;
using Microsoft.Xna.Framework.Graphics;
using static StardewValley.Minigames.CraneGame;

namespace IamFishingMaster
{


    public class ModEntry : Mod
    {
        private ModConfig? Config; //获取配置


        public override void Entry(IModHelper helper)
        {
            this.Config = this.Helper.ReadConfig<ModConfig>();
            int fishMultiplier = this.Config.fishMultiplier; //基础倍率
            int staminaMultiplier = this.Config.staminaMultiplier; //消耗倍率
            float realitystaminaMultiplier = (fishMultiplier * staminaMultiplier) * 1f;
            helper.Events.GameLoop.OneSecondUpdateTicked += OnFishingCheck;
            helper.Events.Player.InventoryChanged += OnInventoryChanged;
            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;

        }

        private void OnFishingCheck(object sender, OneSecondUpdateTickedEventArgs e)
        {
            if (Game1.player.CurrentTool is FishingRod rod && rod.isFishing)
            {
                int fishMultiplier = this.Config.fishMultiplier;
                int staminaMultiplier = this.Config.staminaMultiplier;
                float realitystaminaMultiplier = (fishMultiplier * staminaMultiplier) * 1f;
                if (Game1.player.Stamina >= Math.Max(1 * realitystaminaMultiplier, 1))
                {
                    string text1 = $"-{Math.Max(1 * realitystaminaMultiplier, 1)} 体力";
                    SpriteFont font = Game1.dialogueFont; // 使用游戏内的字体
                    Vector2 textSize = font.MeasureString(text1);
                    float textWidth = 0 - (textSize.X / 10); // 文本宽度（像素）
                    float textHeight = textSize.Y + 70; // 文本高度（像素）

                    Game1.player.Stamina -= Math.Max(1 * realitystaminaMultiplier, 1);
                    TemporaryAnimatedSprite mySprite = new TemporaryAnimatedSprite(6, Game1.player.Position + new Vector2(textWidth, -textHeight), Color.LimeGreen, 90, false, 1, 0, 0)
                    {
                        text = text1,
                        motion = new Vector2(0, -0.06f),
                        timeBasedMotion = true,
                        layerDepth = 1f, // 确保它显示在前景
                        alphaFade = 0.0005f // 控制透明度逐渐消失的速率
                    };
                    Game1.currentLocation.TemporarySprites.Add(mySprite);
                    


                }
            }
        }



        private void OnInventoryChanged(object sender, InventoryChangedEventArgs e)
        {
            foreach (var item in e.Added)
            {
                if (item.Category == StardewValley.Object.FishCategory)
                {
                    int fishMultiplier = this.Config.fishMultiplier;
                    int staminaMultiplier = this.Config.staminaMultiplier;
                    float realitystaminaMultiplier = (fishMultiplier * staminaMultiplier) * 1f;
                    if (Game1.player.Stamina >= Math.Max(1 * realitystaminaMultiplier, 1))
                    {
                        for (int i = 1; i < fishMultiplier; i++)
                        {
                            Game1.player.addItemByMenuIfNecessary(item.getOne());
                        }
                    }
                }
            }
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            // get Generic Mod Config Menu's API (if it's installed)
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
                return;

            // register mod
            configMenu.Register(
                mod: this.ModManifest,
                reset: () => this.Config = new ModConfig(),
                save: () => this.Helper.WriteConfig(this.Config)
            );

            // add some config options
            configMenu.AddSectionTitle(
                mod: this.ModManifest,
                text: () => "我是钓鱼大师"
            );
            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => "基础倍率",
                tooltip: () => "影响鱼的数量和消耗的体力, 体力不够的时候收获的数量恢复正常",
                getValue: () => this.Config.fishMultiplier,
                setValue: value => this.Config.fishMultiplier = value,
                min: 1,
                max: 100
            );

            configMenu.SetTitleScreenOnlyForNextOptions(
                mod: this.ModManifest,
                titleScreenOnly: true
            );

            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => "消耗倍率",
                tooltip: () => "影响消耗的倍率，比如基础倍率=3 消耗倍率=2 3*2=6(消耗6倍的体力)",
                getValue: () => this.Config.staminaMultiplier,
                setValue: value => this.Config.staminaMultiplier = value,
                min: 1,
                max: 100
            );

        }
    }
}
