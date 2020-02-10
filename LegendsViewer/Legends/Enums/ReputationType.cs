using System.ComponentModel;

namespace LegendsViewer.Legends.Enums
{
    public enum ReputationType
    {
        Unknown,
        [Description("Enemy Fighter")]
        EnemyFighter,
        [Description("Trade Partner")]
        TradePartner,
        Killer,
        Poet,
        Bard,
        Storyteller,
        Dancer,
        Friendly,
        Buddy,
        Grudge,
        Bonded,
        Quarreler,
        Psychopath,
        [Description("Loyal Soldier")]
        LoyalSoldier,
        Bully,
        Hero,
        Hunter,
        [Description("Information Source")]
        InformationSource,
        [Description("Treasure Hunter")]
        TreasureHunter,
        [Description("Knowledge Preserver")]
        KnowledgePreserver,
        [Description("Protector Of The Weak")]
        ProtectorOfWeak,
        Love,
        Respect,
        Trust,
        Loyalty,
        Fear
    }
}
