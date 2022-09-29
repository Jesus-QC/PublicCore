﻿using System.Collections.Generic;
using Core.Features.Data.Enums;
using UnityEngine;

namespace Core.Modules.Subclasses.Features.Subclasses.Chaos;

public class ChaosJuggernautSubclass : Subclass
{
    public override string Name { get; set; } = "juggernaut";
    public override string Color { get; set; } = "#99ffb8";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Legendary;
    public override List<RoleType> AffectedRoles { get; set; } = new () { RoleType.ChaosConscript, RoleType.ChaosMarauder, RoleType.ChaosRepressor, RoleType.ChaosRifleman };
    public override RoleType SpawnAs { get; set; } = RoleType.None;
    public override Team Team { get; set; } = Team.CHI;
    public override float Health { get; set; } = 200;
    public override float Ahp { get; set; } = 100;

    public override List<ItemType> SpawnInventory { get; set; } = new ()
    {
        ItemType.GunLogicer, ItemType.ArmorCombat, ItemType.KeycardChaosInsurgency,
        ItemType.GrenadeHE, ItemType.GrenadeFlash
    };

    public override Dictionary<ItemType, ushort> SpawnAmmo { get; set; } = new Dictionary<ItemType, ushort>()
    {
        [ItemType.Ammo9x19] = 30, [ItemType.Ammo556x45] = 20, [ItemType.Ammo762x39] = 200, [ItemType.Ammo12gauge] = 0,
        [ItemType.Ammo44cal] = 20
    };
    
    public override Vector3 Scale { get; set; } = Vector3.one * 1.1f;
}