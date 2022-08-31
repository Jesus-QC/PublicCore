﻿using System.Collections.Generic;
using Core.Features.Data.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.Chaos;

public class JammerSubclass : Subclass
{
    public override string Name { get; set; } = "jammer";
    public override string Color { get; set; } = "#3271a8";
    public override string Description { get; set; } = "You are a jammer.\nYou like to disturb signals.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Common;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.ChaosConscript, RoleType.ChaosMarauder, RoleType.ChaosRepressor, RoleType.ChaosRifleman };
    public override Team Team { get; set; } = Team.CHI;

    public override List<ItemType> SpawnInventory { get; set; } = new List<ItemType>()
    {
        ItemType.GunCrossvec,
        ItemType.KeycardChaosInsurgency,
        ItemType.Flashlight,
        ItemType.Painkillers,
        ItemType.ArmorHeavy,
        ItemType.Radio
    };
    
    public override Dictionary<ItemType, ushort> SpawnAmmo { get; set; } = new Dictionary<ItemType, ushort>()
    {
        [ItemType.Ammo9x19] = 50, [ItemType.Ammo556x45] = 50, [ItemType.Ammo762x39] = 50, [ItemType.Ammo12gauge] = 100,
        [ItemType.Ammo44cal] = 20
    };
}