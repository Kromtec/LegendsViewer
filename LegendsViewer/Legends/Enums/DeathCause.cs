using System.ComponentModel;

namespace LegendsViewer.Legends.Enums
{
    public enum DeathCause
    {
        Unknown,
        None,
        Struck,
        [Description("Old Age")]
        OldAge,
        Thirst,
        Suffocated,
        Bled,
        Cold,
        [Description("Crushed by a Bridge")]
        CrushedByABridge,
        Drowned,
        Starved,
        [Description("In a Cage")]
        InACage,
        Infection,
        [Description("Collided With an Obstacle")]
        CollidedWithAnObstacle,
        [Description("Put to Rest")]
        PutToRest,
        [Description("Starved on Quit")]
        StarvedQuit,
        Trap,
        [Description("Dragon's Fire")]
        DragonsFire,
        Burned,
        Murdered,
        Shot,
        [Description("Cave In")]
        CaveIn,
        [Description("Frozen in Water")]
        FrozenInWater,
        [Description("Executed - Generic")]
        ExecutedGeneric,
        [Description("Executed - Fed To Beasts")]
        ExecutedFedToBeasts,
        [Description("Executed - Burned Alive")]
        ExecutedBurnedAlive,
        [Description("Executed - Crucified")]
        ExecutedCrucified,
        [Description("Executed - Drowned")]
        ExecutedDrowned,
        [Description("Executed - Hacked To Pieces")]
        ExecutedHackedToPieces,
        [Description("Executed - Buried Alive")]
        ExecutedBuriedAlive,
        [Description("Executed - Beheaded")]
        ExecutedBeheaded,
        [Description("Drained of blood")]
        DrainedBlood,
        Collapsed,
        [Description("Scared to death")]
        ScaredToDeath,
        Scuttled,
        [Description("Killed by flying object")]
        FlyingObject,
        Slaughtered,
        Melted,
        Spikes,
        Heat,
        Vanish,
        [Description("Encased in cooling magma")]
        CoolingMagma,
        Vehicle,
        [Description("Suicide - Drowned")]
        SuicideDrowned,
        [Description("Suicide - Leaping")]
        SuicideLeaping,
        Chasm
    }
}