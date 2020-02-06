using System.ComponentModel;

namespace LegendsViewer.Legends.Enums
{
    public enum HfSimpleBattleType : byte
    {
        Unknown,
        [Description("2nd HF Lost After Giving Wounds")]
        Hf2LostAfterGivingWounds,
        [Description("2nd HF Lost After Mutual Wounds")]
        Hf2LostAfterMutualWounds,
        [Description("2nd HF Lost After Recieving Wounds")]
        Hf2LostAfterReceivingWounds,
        Attacked,
        Scuffle,
        Confronted,
        Ambushed,
        [Description("Happened Upon")]
        HappenedUpon,
        Cornered,
        Surprised,
        GotIntoABrawl,
        Subdued
    }
}