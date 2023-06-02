﻿using System.ComponentModel;

namespace Randomizer.Shared
{
    /// <summary>
    /// Specifies the ID's of item locations.
    /// Locations should be named after the room they're in when they're the
    /// only one in the room. Otherwise, they should be named after the room
    /// and the part of the room they're in. The vanilla item should not
    /// generally be part of the name, except when the room is named after it.
    /// </summary>
    public enum LocationId
    {
        // Metroid

        CrateriaPowerBomb = 0,
        WestOceanFloodedCavern = 1,
        WestOceanSky = 2,
        WestOceanMorphBallMaze = 3,
        TheMoat = 4,
        GauntletEnergyTank = 5,
        Pit = 6,
        BombTorizo = 7,
        Terminator = 8,
        GauntletShaftRight = 9,
        GauntletShaftLeft = 10,
        CrateriaSuper = 11,
        TheFinalMissile = 12,
        GreenBrinstarMainShaft = 13,
        SporeSpawnSuper = 14,
        EarlySupersBottom = 15,
        EarlySupersTop = 16,
        BrinstarReserveTankChozo = 17,
        BrinstarReserveTankHidden = 18,
        BrinstarReserveTankVisible = 19,
        PinkShaftTop = 21,
        PinkShaftBottom = 22,
        PinkShaftChozo = 23,
        PinkBrinstarPowerBomb = 24,
        GreenHillZone = 25,
        MorphBallRight = 26,
        MorphBallLeft = 27,
        FirstMissile = 28,
        BlueBrinstarEnergyTankCeiling = 29,
        EtecoonEnergyTank = 30,
        EtecoonSuper = 31,
        WaterwayEnergyTank = 33,
        BlueBrinstarEnergyTankRight = 34,
        PinkBrinstarHoptank = 35,
        BlueBrinstarDoubleMissileVisible = 36,
        BlueBrinstarDoubleMissileHidden = 37,
        XRayScope = 38,
        BetaPowerBomb = 39,
        AlphaPowerBombRight = 40,
        AlphaPowerBombLeft = 41,
        Spazer = 42,
        WarehouseEnergyTank = 43,
        WarehouseKihunter = 44,
        VariaSuit = 48,
        Cathedral = 49,
        IceBeam = 50,
        CrumbleShaft = 51,
        Crocomire = 52,
        HiJumpBoots = 53,
        CrocomireEscape = 54,
        HiJumpEnergyTankLeft = 55,
        HiJumpEnergyTankRight = 56,
        PostCrocomirePowerBomb = 57,
        PostCrocomireMissile = 58,
        PostCrocomireJump = 59,
        GrappleBeam = 60,
        NorfairReserveTankChozo = 61,
        NorfairReserveTankHidden = 62,
        GreenBubblesMissile = 63,
        BubbleMountain = 64,
        SpeedBoosterHall = 65,
        SpeedBooster = 66,
        DoubleChamber = 67,
        WaveBeam = 68,
        GoldenTorizoVisible = 70,
        GoldenTorizoHidden = 71,
        MickeyMouse = 73,
        SpringBallMaze = 74,
        EscapePowerBomb = 75,
        Wasteland = 76,
        ThreeMusketeers = 77,
        RidleyTank = 78,
        ScrewAttack = 79,
        LowerNorfairFireflea = 80,
        WreckedShipMainShaft = 128,
        BowlingAlleyTop = 129,
        BowlingAlleyBottom = 130,
        AssemblyLine = 131,
        WreckedShipEnergyTank = 132,
        WreckedShipWestSuper = 133,
        WreckedShipEastSuper = 134,
        GravitySuit = 135,
        MainStreetBottom = 136,
        MainStreetTop = 137,
        MamaTurtleVisible = 138,
        MamaTurtleHidden = 139,
        WateringHoleLeft = 140,
        WateringHoleRight = 141,
        PseudoPlasmaSpark = 142,
        Plasma = 143,
        WestSandHoleLeft = 144,
        WestSandHoleRight = 145,
        EastSandHoleLeft = 146,
        EastSandHoleRight = 147,
        AqueductLeft = 148,
        AqueductRight = 149,
        SpringBall = 150,
        ThePreciousRoom = 151,
        Botwoon = 152,
        SpaceJump = 154,

        // Zelda

        EtherTablet = 256 + 0,
        SpectacleRock = 256 + 1,
        SpectacleRockCave = 256 + 2,
        OldMan = 256 + 3,
        FloatingIsland = 256 + 4,
        SpiralCave = 256 + 5,
        ParadoxCaveUpperLeft = 256 + 6,
        ParadoxCaveUpperRight = 256 + 7,
        ParadoxCaveLowerFarLeft = 256 + 8,
        ParadoxCaveLowerLeft = 256 + 9,
        ParadoxCaveLowerMiddle = 256 + 10,
        ParadoxCaveLowerRight = 256 + 11,
        ParadoxCaveLowerFarRight = 256 + 12,
        MimicCave = 256 + 13,
        MasterSwordPedestal = 256 + 14,
        Mushroom = 256 + 15,
        LostWoodsHideout = 256 + 16,
        LumberjackTree = 256 + 17,
        PegasusRocks = 256 + 18,
        GraveyardLedge = 256 + 19,
        KingsTomb = 256 + 20,
        KakarikoWellTop = 256 + 21,
        KakarikoWellLeft = 256 + 22,
        KakarikoWellMiddle = 256 + 23,
        KakarikoWellRight = 256 + 24,
        KakarikoWellBottom = 256 + 25,
        BlindsHideoutTop = 256 + 26,
        BlindsHideoutFarLeft = 256 + 27,
        BlindsHideoutLeft = 256 + 28,
        BlindsHideoutRight = 256 + 29,
        BlindsHideoutFarRight = 256 + 30,
        BottleMerchant = 256 + 31,
        SickKid = 256 + 33,
        KakarikoTavern = 256 + 34,
        MagicBat = 256 + 35,
        KingZora = 256 + 36,
        ZorasLedge = 256 + 37,
        WaterfallFairyRight = 256 + 39,
        PotionShop = 256 + 40,
        SahasrahlasHutLeft = 256 + 41,
        SahasrahlasHutMiddle = 256 + 42,
        SahasrahlasHutRight = 256 + 43,
        Sahasrahla = 256 + 44,
        MazeRace = 256 + 45,
        MiniMoldormCaveFarLeft = 256 + 51,
        MiniMoldormCaveLeft = 256 + 52,
        MiniMoldormCaveNpc = 256 + 53,
        MiniMoldormCaveRight = 256 + 54,
        BombosTablet = 256 + 58,
        FloodgateChest = 256 + 59,
        SunkenTreasure = 256 + 60,
        LakeHyliaIsland = 256 + 61,
        Hobo = 256 + 62,
        IceRodCave = 256 + 63,
        SpikeCave = 256 + 64,
        HookshotCaveTopRight = 256 + 65,
        HookshotCaveTopLeft = 256 + 66,
        HookshotCaveBottomLeft = 256 + 67,
        HookshotCaveBottomRight = 256 + 68,
        SuperbunnyCaveTop = 256 + 69,
        SuperbunnyCaveBottom = 256 + 70,
        BumperCave = 256 + 71,
        ChestGame = 256 + 72,
        CShapedHouse = 256 + 73,
        Brewery = 256 + 74,
        HammerPegs = 256 + 75,
        Blacksmith = 256 + 76,
        PurpleChest = 256 + 77,
        Catfish = 256 + 78,
        Pyramid = 256 + 79,
        PyramidFairyLeft = 256 + 80,
        PyramidFairyRight = 256 + 81,
        DiggingGame = 256 + 82,
        Stumpy = 256 + 83,
        HypeCaveTop = 256 + 84,
        HypeCaveMiddleRight = 256 + 85,
        HypeCaveMiddleLeft = 256 + 86,
        HypeCaveBottom = 256 + 87,
        HypeCaveNpc = 256 + 88,
        MireShedLeft = 256 + 89,
        MireShedRight = 256 + 90,
        Sanctuary = 256 + 91,
        SewersSecretRoomLeft = 256 + 92,
        SewersSecretRoomMiddle = 256 + 93,
        SewersSecretRoomRight = 256 + 94,
        SewersDarkCross = 256 + 95,
        HyruleCastleMapChest = 256 + 96,
        HyruleCastleBoomerangChest = 256 + 97,
        HyruleCastleZeldasCell = 256 + 98,
        LinksUncle = 256 + 99,
        SecretPassage = 256 + 100,
        CastleTowerFoyer = 256 + 101,
        CastleTowerDarkMaze = 256 + 102,
        EasternPalaceCannonballChest = 256 + 103,
        EasternPalaceMapChest = 256 + 104,
        EasternPalaceCompassChest = 256 + 105,
        EasternPalaceBigChest = 256 + 106,
        EasternPalaceBigKeyChest = 256 + 107,
        EasternPalaceArmosKnights = 256 + 108,
        DesertPalaceBigChest = 256 + 109,
        DesertPalaceTorch = 256 + 110,
        DesertPalaceMapChest = 256 + 111,
        DesertPalaceBigKeyChest = 256 + 112,
        DesertPalaceCompassChest = 256 + 113,
        DesertPalaceLanmolas = 256 + 114,
        TowerOfHeraBasementCage = 256 + 115,
        TowerOfHeraMapChest = 256 + 116,
        TowerOfHeraBigKeyChest = 256 + 117,
        TowerOfHeraCompassChest = 256 + 118,
        TowerOfHeraBigChest = 256 + 119,
        TowerOfHeraMoldorm = 256 + 120,
        PalaceOfDarknessShooterRoom = 256 + 121,
        PalaceOfDarknessBigKeyChest = 256 + 122,
        PalaceOfDarknessStalfosBasement = 256 + 123,
        PalaceOfDarknessTheArenaBridge = 256 + 124,
        PalaceOfDarknessTheArenaLedge = 256 + 125,
        PalaceOfDarknessMapChest = 256 + 126,
        PalaceOfDarknessCompassChest = 256 + 127,
        PalaceOfDarknessHarmlessHellway = 256 + 128,
        PalaceOfDarknessDarkBasementLeft = 256 + 129,
        PalaceOfDarknessDarkBasementRight = 256 + 130,
        PalaceOfDarknessDarkMazeTop = 256 + 131,
        PalaceOfDarknessDarkMazeBottom = 256 + 132,
        PalaceOfDarknessBigChest = 256 + 133,
        PalaceOfDarknessHelmasaurKing = 256 + 134,
        SwampPalaceEntrance = 256 + 135,
        SwampPalaceMapChest = 256 + 136,
        SwampPalaceBigChest = 256 + 137,
        SwampPalaceCompassChest = 256 + 138,
        SwampPalaceWestChest = 256 + 139,
        SwampPalaceBigKeyChest = 256 + 140,
        SwampPalaceFloodedRoomLeft = 256 + 141,
        SwampPalaceFloodedRoomRight = 256 + 142,
        SwampPalaceWaterfallRoom = 256 + 143,
        SwampPalaceArrghus = 256 + 144,
        SkullWoodsPotPrison = 256 + 145,
        SkullWoodsCompassChest = 256 + 146,
        SkullWoodsBigChest = 256 + 147,
        SkullWoodsMapChest = 256 + 148,
        SkullWoodsPinballRoom = 256 + 149,
        SkullWoodsBigKeyChest = 256 + 150,
        SkullWoodsBridgeRoom = 256 + 151,
        SkullWoodsMothula = 256 + 152,
        ThievesTownMapChest = 256 + 153,
        ThievesTownAmbushChest = 256 + 154,
        ThievesTownCompassChest = 256 + 155,
        ThievesTownBigKeyChest = 256 + 156,
        ThievesTownAttic = 256 + 157,
        ThievesTownBlindsCell = 256 + 158,
        ThievesTownBigChest = 256 + 159,
        ThievesTownBlind = 256 + 160,
        IcePalaceCompassChest = 256 + 161,
        IcePalaceSpikeRoom = 256 + 162,
        IcePalaceMapChest = 256 + 163,
        IcePalaceBigKeyChest = 256 + 164,
        IcePalaceIcedTRoom = 256 + 165,
        IcePalaceFreezorChest = 256 + 166,
        IcePalaceBigChest = 256 + 167,
        IcePalaceKholdstare = 256 + 168,
        MiseryMireMainLobby = 256 + 169,
        MiseryMireMapChest = 256 + 170,
        MiseryMireBridgeChest = 256 + 171,
        MiseryMireSpikeChest = 256 + 172,
        MiseryMireCompassChest = 256 + 173,
        MiseryMireBigKeyChest = 256 + 174,
        MiseryMireBigChest = 256 + 175,
        MiseryMireVitreous = 256 + 176,
        TurtleRockCompassChest = 256 + 177,
        TurtleRockRollerRoomLeft = 256 + 178,
        TurtleRockRollerRoomRight = 256 + 179,
        TurtleRockChainChomps = 256 + 180,
        TurtleRockBigKeyChest = 256 + 181,
        TurtleRockBigChest = 256 + 182,
        TurtleRockCrystarollerRoom = 256 + 183,
        TurtleRockEyeBridgeTopRight = 256 + 184,
        TurtleRockEyeBridgeTopLeft = 256 + 185,
        TurtleRockEyeBridgeBottomRight = 256 + 186,
        TurtleRockEyeBridgeBottomLeft = 256 + 187,
        TurtleRockTrinexx = 256 + 188,
        GanonsTowerBobsTorch = 256 + 189,
        GanonsTowerDMsRoomTopLeft = 256 + 190,
        GanonsTowerDMsRoomTopRight = 256 + 191,
        GanonsTowerDMsRoomBottomLeft = 256 + 192,
        GanonsTowerDMsRoomBottomRight = 256 + 193,
        GanonsTowerMapChest = 256 + 194,
        GanonsTowerFiresnakeRoom = 256 + 195,
        GanonsTowerRandomizerRoomTopLeft = 256 + 196, // 230 in main
        GanonsTowerRandomizerRoomTopRight = 256 + 197, // 231 in main
        GanonsTowerRandomizerRoomBottomLeft = 256 + 198, // 232 in main
        GanonsTowerRandomizerRoomBottomRight = 256 + 199, // 233 in main
        GanonsTowerHopeRoomLeft = 256 + 200, // 234 in main
        GanonsTowerHopeRoomRight = 256 + 201, // 235 in main
        GanonsTowerTileRoom = 256 + 202,
        GanonsTowerCompassRoomTopLeft = 256 + 203,
        GanonsTowerCompassRoomTopRight = 256 + 204,
        GanonsTowerCompassRoomBottomLeft = 256 + 205,
        GanonsTowerCompassRoomBottomRight = 256 + 206,
        GanonsTowerBobsChest = 256 + 207,
        GanonsTowerBigChest = 256 + 208,
        GanonsTowerBigKeyChest = 256 + 209,
        GanonsTowerBigKeyRoomLeft = 256 + 210,
        GanonsTowerBigKeyRoomRight = 256 + 211,
        GanonsTowerMiniHelmasaurRoomLeft = 256 + 212,
        GanonsTowerMiniHelmasaurRoomRight = 256 + 213,
        GanonsTowerPreMoldormChest = 256 + 214,
        GanonsTowerMoldormChest = 256 + 215,
        Library = 256 + 240,
        FluteSpot = 256 + 241,
        SouthOfGrove = 256 + 242,
        LinksHouse = 256 + 243,
        AginahsCave = 256 + 244,
        ChickenHouse = 256 + 250,
        MiniMoldormCaveFarRight = 256 + 251,
        DesertLedge = 256 + 252,
        CheckerboardCave = 256 + 253,
        WaterfallFairyLeft = 256 + 254,
    }
}
