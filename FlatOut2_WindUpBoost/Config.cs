using System.ComponentModel;
using FlatOut2.SDK.Enums;
using FlatOut2_WindUpBoost.Template.Configuration;
using Reloaded.Mod.Interfaces.Structs;

namespace FlatOut2_WindUpBoost.Configuration
{
    public class Config : Configurable<Config>
    {
        /*
            User Properties:
                - Please put all of your configurable properties here.
    
            By default, configuration saves as "Config.json" in mod user config folder.    
            Need more config files/classes? See Configuration.cs
    
            Available Attributes:
            - Category
            - DisplayName
            - Description
            - DefaultValue

            // Technically Supported but not Useful
            - Browsable
            - Localizable

            The `DefaultValue` attribute is used as part of the `Reset` button in Reloaded-Launcher.
        */

        [DisplayName("Wind Up Speed")]
        [Description("The speed you go backwards when winding up.")]
        [DefaultValue(0.75f)]
        public float WindUpSpeed { get; set; } = 0.75f;

        [DisplayName("Wind Up Gain")]
        [Description("The amount of energy you gain each tick of going backwards")]
        [DefaultValue(1.5f)]
        public float WindUpGain { get; set; } = 1.5f;

        [DisplayName("Max Wind Up")]
        [Description("The max amount of energy you can gain before you automatically shoot")]
        [DefaultValue(150.0f)]
        public float MaxWindUp { get; set; } = 150.0f;

        [DisplayName("Boost Key")]
        [Description("Allows you to remap the boost button")]
        [DefaultValue(KeyboardKeys.Control)]
        public KeyboardKeys BoostKey { get; set; } = KeyboardKeys.Control;
    }

    /// <summary>
    /// Allows you to override certain aspects of the configuration creation process (e.g. create multiple configurations).
    /// Override elements in <see cref="ConfiguratorMixinBase"/> for finer control.
    /// </summary>
    public class ConfiguratorMixin : ConfiguratorMixinBase
    {
        // 
    }
}
