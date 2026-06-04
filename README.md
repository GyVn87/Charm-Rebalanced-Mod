- If you enjoy the mod, please consider giving the repository a star, this will help spread the mod to more people! 
- If you have any idea or encounter any issue, please let me know by opening an issue.

# How to install
* Change the game version to `1.5.78.11833`
![How to change the game version](https://media.discordapp.net/attachments/1207486627124093019/1469343521260703867/steambetas2.jpg?ex=69fd4e8a&is=69fbfd0a&hm=753c394259c15c2b3de099d0fec43cc28aebf208aa3326eb36aacb98cdd1bdb0&format=webp&width=2682&height=1546&)
* Download and install Lumafly, you can get it from here: https://themulhima.github.io/Lumafly/
* Launch `Lumafly`, choose `Mods`, search `Charm Rebalanced`, and install the mod
<img width="1919" height="259" alt="image" src="https://github.com/user-attachments/assets/3d3d6d78-71af-4336-9981-87b2854eb6aa" />

* At `Info`, choose `Launch Modded Game`, and done
* Next times, you only need to launch the game as usual.

# Changes
## Voidheart/Upgraded Focus
- After obtaining Voidheart, Focus will be upgraded
- After completing Focus, Knight now gain "Dark Overflow" effect
- For 4 seconds, successful normal Focus grants Knight 50% extra damage dealt from Nail, Nail Art, Spell, Shadow Dash 
- Successful Focus at full masks increases this number to 70%
- With Deep Focus equipped, extra damage dealt is multiplied by 1.25
- Taking hit clears the effect

***This somehow compensates for the Soul and time spending in Focus. I hope this change will encourage players to focus more in late-game stages.***
## Stalwart Shell
- Notch cost: 2 => 1
- Invincibility duration: 1.75s => 2s
- During invincibility, decreases Knight's damage dealt from all sources to 25%
- During invincibility, player can not focus.

***I rarely saw anyone using this charm, and neither did I. The concept is good, and I want to retain it. The main issue is that players can take advantage of this charm by facetanking bosses, so I have to add a damage penalty.***
## Soul Catcher
- Notch cost: 2 => 1
- Soul gain for Main Vessel: 3 => 2

***This charm certainly needs a buff to help it compete with other Soul charms such as Spell Twister, Grubsong***
## Shaman Stone
- Vengeful Sprit and Shade Soul damage increase: 33% => 30% 
- **OTHER SPELL DAMAGE** increase: 40%

***Don't worry. This charm is still extremely powerful. I might even want to nerf it to 1% increase.***
## Soul Eater
- Notch cost: 4 => 3.
- Soul gain for Main Vessel: 8 => 4
- Soul gain for reserve Soul Vessels: 6 => 3
- Spells now infect enemies with "Eater Curse" debuff. Striking affected enemies will double the soul gain.
- The debuff lasts 3 seconds, and striking enemies also clears the effect. 

***The 4-notch cost is extremely high and I don't find the benefits really worth it. And the added debuff will make the charm unique to Soul Catcher, rather than a simply upgrade.***
## Dashmaster
- Shadow Dash cooldown reduce: 20%
- Players can Dash Downward at any time
- Down Dash now fully recharges Dash cooldown, even in mid-air

***This charm has been overshadowed too much by Sharp Shadow, it certainly need a buff to be relevant in the late game and compete with other 2-notch slots charms.***
## Sprintmaster
- Run speed increase: 20% => 25%
- Speed increase also APPLIES WHEN ON AIR.
- Increases swim speed by 100%
- Synergy with Dashmaster:
  - Run speed increase: 39% => 35%

***I always feel so slow when jumping when using this charm. Now it's not anymore. This charm is already good so I have to remove the synergy with Dashmaster, otherwise it will be too overpowered.***
## Grubsong
- Soul gain now SCALES with the amount of damage taken.
- Soul gain when taking damage: 15 => 12.
- Synergy with Grubberfly's Elegy:
  - Soul gain: 25 => 20.

***Grubsong is a little too good for early game. So I think it is fine to nerf it a little bit. However, by scaling soul gain with damage, it remains viable in later stages, especially againsts bosses that deal more than 1 damage.***
## Grubberfly's Elegy
- Grubberfly's Elegy now works properly in Patheons with Shell Binding
- Grubberfly's Elegy no longer loses its effect when not at full masks.
- Instead, the less white masks you are currently at, the lower the beam damage is.
- The damage formula:    $Beam Damage = (\frac{currentMasks - 1}{maximumMasks + 1})^2 * nailDamage$
- In Patheon with Shell Binding, the damage is multiplied by 1.5.
- Beam no longer staggers bosses
- Doesn't fire beam when the damage is lower than 1.
- Its behavior with Joni's Blessing is not changed.
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

***I really like the idea of firing beam with your nail. But I just hate when it becomes completely useless when not at full masks. I think this change is nice since it still incentivizes players to keep their masks staying at high as possible while doesn't punish casual players too much.***
## Heart
- Masks increase: 34% of current maximum maks (round down)
- Means that you will gain extra 3 masks when only have 9 maximum masks

***I do think this charm need a buff to keep up with other charms in the late game.***
## Greed
- Geo drop increase: (20% - 100%) => (40% => 100%)
- Because Geo drop calculation is a little weird, on average, the increase is around 50%
- When being used in Colosseum Of Fools, Greed increases Trials' Geo reward by 33%

***The original increase is almost insignificant which many players didn't bother using it. The use in Colosseum Of Fools is really interesting to me. It encourages high risk, high return playstyle when you have to sacrifice 2 notch slots.***
## Strength
- Damage increase: 50% => 30%.

***This charm certainly needs a nerf, 50% increases in damage is purely evil.***
## Heavy Blow
- Remove increased knockback
- Increases Nail Art damage by 20%
- Hits from Great Slash and Dash Slash now count as 3 hits to stagger a boss

***Why increase the knockback when you need to hit the enemies as many as possible. I believe this charm needs some reworks.***
## Quickslash
- Attack speed increase: 46% => 33%

***This charm is really flexible when it can be used to both increase damage output and soul gain. It certainly needs a little nerf!***
## Longnail
- Now applies to Nail Art

## Mark Of Pride
- Now applies to Nail Art

***Because... why not?***
## Fury Of The Fallen
- Notch cost: 2 => 3
- No longer can only be activated at single mask.
- The less health you have, the higher the damage is.
- The damage increase formula: $Damage Increase = (\frac{currentMasks - maximumMasks}{maximumMasks + 1})^2$
- In Patheon with Shell Binding, the increase is multiplied by 1.5.

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

***This charm has the same issue with Grubberfly's Elegy, when it is only activated in such an extreme condition. There are very few people really use this charm, and it punishes casual players too much. Because of that, I want to still retain its original concept while making it more accessible to more people.***
## Thorns Of Agony
- No longer inhibits movement when taken hit.

***I think we both agree that the freeze is that bad***
## Baldur Shell
- Restores 1 blocker hit after every 2 successful Focus
- Synergy with Deep Focus:
    - Restores 1 blocker hit after every 1 successful Focus

***I feel like this change would encourage a new playstyle without making it overpowered. With this, focusing in Radiant boss fights is no longer entirely useless.***
## Flukenest
- Notch cost: 3 => 2
- Multihits bug has been fixed in the latest version of the game, but the current Modding API-supportable version haven't yet. However, I fixed it somehow. 
- Vengeful Spirit + Flukenest: 8 flukes x 5 damage (36)
- Vengeful Spirit + Flukenest + Shaman Stone: 10 flukes x 5 damage (45 => 50)
- Shade Soul + Flukenest: 16 flukes x 4 damage (64)
- Shade Soul + Flukenest + Shaman Stone: 15 flukes x 6 damage (80 => 90)
- Synergies:
  - Defender's Crest:
    - Vengeful Spirit: 50 (2.2 seconds)
    - Shade Soul: 100 (2.2 seconds)
    - Affected by Shaman Stone

***I love how this charm make spell so much stronger and fun to use since it basically makes Vengeful Spirit and Shade Soul a shotgun. I think it deserves more!***
## Defender's Crest
- Enemies in cloud suffer 10% more damage from all sources (except cloud, spore damage).
- Debuff lasts: 2 seconds
- Enemies with debuff will have dark purple particles around them

***This charm is far weaker than other 1-notch charms like Grubsong, Nailmaster's Glory, and its synergies are terrible as well.***
## Glowing Womb
- Damage (both normal and Dung version): 75% of Nail damage.
- Maximum Hatchlings: 4 => 6
- Summons 2 Hatchlings after a successful Focus
- Summons 1 Hatchling after using a Spell
- Synergy with Deep Focus:
    - Summons 3 Hatchlings after a successful Focus.

***This change makes the charm more interactive and fun to use now, while still retaining its original concept of using Soul to summon minions***
## Deep Focus
- Notch cost: 4 => 3.
- Focus time increase: 65% => 50%.

***It's a cool charm, but I don't see why it costs 4 notches. I have also lowered its focus time increases a little, and now when combining this charm with Quick Focus, the result is the normal Focus time***
## Liveblood Heart
- Notch cost: 2 => 1.

***Because... why not? I have never thought that 2 Liveblood Masks is worth 2 Notches***
## Lifeblood Core
- Notch cost: 3 => 2
- No longer grants players additional Lifeblood Masks after resting at the bench
- Instead, player gains 1 Lifeblood Mask after taking 4 damage

***The high cost really makes it not fit well in various builds. And hopefully, with the new mechanic, it can fairly compete with other healing charms like Heart, Joni's Blessin, rather than a simple upgrade to Lifeblood Heart.***
## Joni's Blessing
- Notch cost: 4 => 2
- Max health increase: 40% => 50%

***At this point, I really wonder why all Liveblood-related charms are so weird. This charm is not a exception. I do think that these changes would make the charm more fun to use.***
## Hiveblood
- Notch cost: 4 => 2.
- Regeneration duration: 10 seconds => 12 seconds
- Focus no longer interrupts regeneration

***I just hate when Focus interrupts regeneration, which is pretty annoying***
## Spore Shroom
- Spore damage: 3x Nail Damage
- Its damage can now be boosted by Shaman Stone
- Synergies:
  - Defender's Crest:
    - Cloud still deals the same damage as that of spore, but inflicts weakness debuff

***This changes makes Spore Shroom literally a spell now***
## Sharp Shadow
- Shadow Dash damage: 2x Nail damage
- Synergy with Dashmaster:
    - Shadow Dash damage: 2.5x Nail damage

***I do believe this charm is a little too overpowered. But I can't help but buffing it, everyone loves dashing, right...?***
## Weaversong
- Each weaversling now deals: 25% of Nail damage (round down).
- Each successful hit gain player 2 soul by default.
- Synergy with Grubsong:
  - Soul gain: 3
- Synergy with Sprintmaster:
  - Speed increase: 50% => 25%

***I love summoner playstyle, but this charm is really ineffective. I believe the effect that gain player soul on hit would make this charm unique to other summon charms.***
## Dreamshield
- Notch cost: 3 => 2
- Now summons 2 shields at a time.
- Shield damage: 1x => 0.5x Nail damage

***I like this change, but wonder if it makes this charm too powerful.***
## Grimmchild
- Now fires 3 fireballs at once
- Damage per ball:
  - Level 2: 5  => 3
  - Level 3: 8  => 6
  - Level 4: 11 => 9
- Grimmchild can now shoot more ***ACCURATELY***.

***As I said before, I love summoner playstyle. Despite this charm's unlimited power, I still buffed it to make it even stronger.***
## Carefree Melody
- No longer relies on luck to block attacks.
- Cooldown: 20 seconds
- While on cooldown, Knight takes double of the damage from all sources
- ***Doesn't work in Radiant fights***

***To put it simple, I hate RNG-based mechanics***
## Kingsoul:
- Notch cost: 5 => 3
- Soul gain: 4 => 5
- Can now be equipped even after player obtains Voidheart

***5 notch slots is crazy, not gonna lie***
