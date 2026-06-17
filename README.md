- If you enjoy the mod, please consider giving the repository a star. This will help spread the mod to more people! 
- If you have any ideas or encounter any issues, please let me know by opening an issue.

# How to install
* Change the game version to `1.5.78.11833`
<img width="2184" height="1259" alt="steambetas2" src="https://github.com/user-attachments/assets/a65bee64-24ca-4576-9aae-1a113f4c4915" />

* Download and install Lumafly, you can get it from here: https://themulhima.github.io/Lumafly/
* Launch `Lumafly`, choose `Mods`, search `Charm Rebalanced`, and install the mod
<img width="1919" height="259" alt="image" src="https://github.com/user-attachments/assets/3d3d6d78-71af-4336-9981-87b2854eb6aa" />

* At `Info`, choose `Launch Modded Game`, and done
* Next time, you only need to launch the game as usual.

# Changes
## Stalwart Shell
- Notch cost: 2 => 1
- Invincibility duration: 1.75s => 2s
- While invincible, all damage dealt is reduced to 25%
- During invincibility, player cannot Focus.

***The core concept is great, and I want to retain it. The main issue is that players can take advantage of this charm for facetanking bosses, which makes the fight not healthy. Because of that, I have to add a damage penalty to prevent brainless spamming.***
## Soul Catcher
- Notch cost: 2 => 1
- Soul gain for Main Vessel: 3 => 2

***This charm certainly needs a buff to help it compete with other top-tier Soul charms like Spell Twister and Grubsong***
## Shaman Stone
- Vengeful Sprit and Shade Soul damage increase: 33% => 25% 
- **OTHER SPELL DAMAGE** increase: 30%
  
***Don't worry. This charm is still extremely powerful.***
## Soul Eater
- Notch cost: 4 => 3.
- Soul gain for Main Vessel: 8 => 4
- Soul gain for reserve Soul Vessels: 6 => 3
- Hitting enemies with spells now inflicts "Eater Curse" debuff on them for 3 seconds. 
- Striking a cursed enemy with the Nail removes the effect and Knight yields double the total Soul of that hit

***The original 4-notch cost is extremely high and I don't really find the benefits worth it. This rework also makes the charm unique, rather than a more expensive version of Soul Catcher***
## Dashmaster
- Dash cooldown: 0.4s => 0.3s
- Shadow Dash cooldown: reduced by 20%
- Players can now Down Dash at any time
- Executing a Down Dash now fully recharges Dash cooldown immediately, even in mid-air

***This charm has been overshadowed too much by Sharp Shadow. It certainly needs a buff to be relevant in the late-game, making it a highly competitive 2-notch charm.***
## Sprintmaster
- Run speed increase: 20% => 25%
- Speed increase also APPLIES WHILE IN MID-AIR.
- Increases swim speed by 100%

***I have always felt so slow when jumping with this charm equipped. The aerial speed buff will completely fix that issue.***
## Grubsong
- Soul gain now SCALES with the amount of damage taken.
- Soul gain when taking damage: 15 => 12.
- Synergy with Grubberfly's Elegy:
  - Soul gain: 25 => 20.

***Grubsong is a little too good in the early game, so a slight nerf is necessary. However, by introducing damage scaling, it remains viable in later stages, especially against bosses that deal 2 or more damage***
## Grubberfly's Elegy
- Now works properly in Pantheons with Shell Binding
- No longer loses its effect when not at full masks.
- Instead, the higher your current health is, the higher the beam damage is.
- The damage formula:    $Beam Damage = (\frac{currentMasks - 1}{maximumMasks + 1})^2 * nailDamage$
- In Pantheon with Shell Binding, the damage is multiplied by 1.5x.
- Beams no longer stagger bosses
- Doesn't fire beam when the damage is lower than 1.
- 4 maximum masks (with Pure Nail and Shell Binding)

| Health  | 4    |  3  |
| ------- | ---- |---- |
| Damage  | 12   |  6  |

- 9 maximum masks (with Pure Nail)

| Health  | 9   |  8  |  7  | 6   |  5 |  4 |
| ------- |---- |---- |---- |---- |----|----|
| Damage  | 14  | 11  |  8  |  6  |  4 |  2  |

- 12 maximum masks (with Pure Nail)

| Health  | 12   |  11  |  10  | 9   |  8 |  7 |
| ------- |----- | ---- |----- |---- |----|----|
| Damage  | 16   | 13   |  11  |  8  |  7 |  5 |

***I really love the idea of firing beam with your nail. But I just hate when it becomes completely useless the moment I take a single hit. This rework incentivizes players to maintain their health as high as possible while not punishing casual players too much.***
## Heart
- Masks bonus: 34% of player's maximum masks (rounded down)
- This means that you will gain 3 extra masks when only having 9 maximum masks

***I do think this charm needs a buff to keep up with other charms in the late game.***
## Greed
- Geo drop bonus: (20% - 100%) => (40% => 100%)
- Because Geo drop calculation is a little weird, the result is a roughly 50% average increase
- Equipping Greed in the Colosseum Of Fools now increases Trials' Geo reward by 33%

***The original increase felt so insignificant that many players didn't bother using it. The use in Colosseum Of Fools is really a fun addition - it encourages a "high risk, high return" playstyle where you sacrifice 2 notch slots for a better reward.***
## Strength
- Damage increase: 50% => 30%.

***This charm undeniably needs a nerf. A flat 50% increase in damage is purely evil.***
## Heavy Blow
- No longer increases knockback
- Increases Nail Art damage by 20%
- Hits from Great Slash and Dash Slash now count as 3 hits to stagger a boss

***The increased knockback is completely counterproductive when you want to strike your enemies as rapidly as possible. This charm certainly needs a rework.***
## Quickslash
- Attack speed increase: 46% => 33%

***This charm is incredibly flexible when it can be used to both increase damage output and Soul gain. It definitely needs a slight nerf to bring it into line with other charms!***
## Longnail & Mark Of Pride
- Now applies to Nail Art

***Because... why not?***
## Fury Of The Fallen
- Notch cost: 2 => 3
- No longer can only be activated at a single mask.
- Instead, the lower your current health is, the higher the damage is.
- The damage increase formula: $Damage Increase = (\frac{currentMasks - maximumMasks}{maximumMasks + 1})^2$
- In Pantheon with Shell Binding, the increase is multiplied by 1.5.

- 4 maximum masks (with Shell Binding)

| Health    | 1     |  2   |
| -------   | ----  |---   |
| Increase  | 54%   |  24% |

- 9 maximum masks

| Health    | 1    |  2  |  3     | 4     |  5   |  6 |
| -------   |----  |---- |----    |----   |----  |-----|
| Increase  | 64%  | 49%  |  36%  |  25%  |  16% |  9% |

- 12 maximum masks

| Health    | 1     |  2    |  3    | 4     |  5   |  6   |
| -------   |-----  | ----  |-----  |----   |  ----|----  |
| Increase  | 71%   | 59%   |  48%  |  38%  |  29% |  21% |

***This charm suffers from the exact same issue as Grubberfly's Elegy, it only activates in such an extreme condition. Very few people actually use this charm because it punishes casual players too much. Because of that, I want to still retain its original concept while making it more accessible.***
## Thorns Of Agony
- No longer inhibits movement when players take a hit.

***I think we can all agree that the freeze is that terrible***
## Baldur Shell
- Restores 1 blocker hit after every 2 successful Focus
- Synergy with Deep Focus:
    - Restores 1 blocker hit after every 1 successful Focus

***This change encourages a new playstyle without making it overpowered. With this regeneration mechanic, focusing in Radiant fights is no longer useless.***
## Flukenest
- Notch cost: 3 => 2
- Multihits bug has been fixed in the latest version of the game, but the current Modding API-supportable version hasn't yet. However, I somehow successfully fixed it. 
- Synergy with Defender's Crest:
  - Vengeful Spirit: 50 (2.2 seconds)
  - Shade Soul: 100 (2.2 seconds)
  - Can be boosted by Shaman Stone

***I love how this charm makes spells so much stronger and fun to use since it basically turns Vengeful Spirit and Shade Soul into a shotgun. It definitely deserves better!***
## Defender's Crest
- Enemies in cloud suffer 10% extra damage from all sources (except cloud, spore damage).
- Debuff lasts: 2 seconds
- Enemies with debuff will have dark purple particles around them

***This charm is far weaker than other 1-notch charms like Grubsong, Nailmaster's Glory, and its synergies are terrible as well.***
## Glowing Womb
- Damage (both normal and Dung version): 66% of Nail damage.
- Maximum Hatchlings: 4 => 6
- Summons 3 Hatchlings after a successful Focus
- Synergy with Deep Focus:
  - Maximum hatchlings: 8
  - Summons 4 Hatchlings after a successful Focus.

***This change makes the charm more interactive and fun to use now, while still retaining its original concept of using Soul to summon minions***
## Deep Focus
- Notch cost: 4 => 3.
- Focus time increase: 65% => 50%.

***It's a cool charm, but I don't see why it costs 4 notch slots. I have also slightly lowered its focus time increase, and now when combining it with Quick Focus, the result is the normal Focus time***
## Lifeblood Heart
- Notch cost: 2 => 1.

***Why not? I have never thought that 2 Lifeblood Masks is worth 2 notch slots***
## Lifeblood Core
- Notch cost: 3 => 2
- After player rest at the bench, grants him 4 => 1 additional Lifeblood Mask
- In addition, Knight gains 1 Lifeblood Mask for every 5 damage taken

***The high cost makes it hard to fit well into various builds. With the new mechanic, it can fairly compete with other healing charms like Heart, Joni's Blessing, rather than a simple upgrade to Lifeblood Heart.***
## Joni's Blessing
- Notch cost: 4 => 2
- Max health increase: 40% => 50%

***At this point, I really wonder why all Lifeblood-related charms are so weird***
## Hiveblood
- Notch cost: 4 => 2.
- Regeneration time: 10 seconds => 12 seconds
- Focus no longer interrupts regeneration

***Despite being one of my favorite charms, I would say its original cost makes many people hesitant to use it. In addition, the quality-of-life change makes it far less annoying to use this charm.***
## Spore Shroom
- Spore damage: 3x Nail Damage
- Its damage can now be boosted by Shaman Stone
- Synergy with Defender's Crest:
   - Cloud retains the same damage, but inflicts weakness debuff on enemies

***This rework literally turns Spore Shroom into a spell now***
## Sharp Shadow
- Shadow Dash damage: 1x => 2x Nail damage
- Synergy with Dashmaster:
   - Shadow Dash damage: 1.5x => 2.5x Nail damage

***Many people only equipped this charm due to its Shadow Dash speed increase, instead of the dash's damage. And I think this is such a shame because it is a really cool effect.***
## Weaversong
- Each Weaverling now deals: 25% of Nail damage (rounded down).
- Unable to deal damage to staggered bosses
- No longer wake up staggered bosses
- Each successful hit generates 2 Soul by default.
- Synergy with Sprintmaster:
  - Weaverling's speed increase: 50% => 25%

***Weaversong is a charm that is not good on its own, but for its synergy with Grubsong. I believe making it generate Soul by default will solve this problem, and make it unique compared to other summoning charm***
## Dreamshield
- Notch cost: 3 => 2
- Now summons 2 shields at a time.
- Each shield deals: 1x => 0.5x Nail damage

***I have to lower the shield's damage, otherwise it would be extremely overpowered!***
## Grimmchild
- Now fires 3 fireballs simultaneously like Primal Aspid's pattern.
- Unable to deal damage to staggered bosses
- Its projectiles no longer wake up staggered bosses
- Damage per ball:
  - Level 2: 5  => 3
  - Level 3: 8  => 6
  - Level 4: 11 => 9

***Despite this charm's unlimited power, I still buffed it to make it even more powerful.***
## Carefree Melody
- No longer relies on RNG-mechanic to block hits.
- Cooldown: 15 seconds
- While the charm is on cooldown, Knight takes double damage from all sources
- ***Doesn't work in Radiant fights***

***To put it simple, I hate RNG-based mechanics***
## Kingsoul:
- Notch cost: 5 => 2
- Can now be equipped even after player obtains Voidheart

***5 notch slots is crazy, not gonna lie***

## Voidheart
- New mechanic: Dark Overflow (Unlocked upon obtaining Voidheart)
- Successful Focus now grants the "Dark Overflow" for 4 seconds, increasing damage dealt (including Nail, Nail Art, Spell, Shadow Dash) by 50%
- If player overheals, the damage bonus increases to 70%  
- With Deep Focus equipped, the total damage bonus is multiplied by x1.25
- Taking any damage instantly clears the effect

***I made this change to somewhat compensate for the Soul and time spent in Focus. I hope it will encourage more aggressive Focus usage in the late-game***
