# omni-plugins-pub
Omni Plugins were originally developed for Omni-2 SCP SL roleplay.

**Contact icedchqi on Discord for any issues, questions, or concerns.**

## [ColdWaterLibrary](https://github.com/icedchai/ColdWaterLibrary/releases)
[ColdWaterLibrary](https://github.com/icedchai/ColdWaterLibrary/releases) is a revamped version of another plugin that was previously part of Omni-2 plugins.

It contains one important class, *OverallRoleType*, which is critical but simple to understand.

The `RoleType` field can be one of three values: `BaseGame`, `ExiledCustom` (EXILED Custom Role), or `Uncomplicated` (UncomplicatedCustomRoles).

The `RoleId` field can be any integer, which corresponds to the ID of the role in its respective system (BaseGame, EXILED Customrole, UCR). The `BaseGameRole` ID system can be found below.
```
| Id  | RoleTypeId     |
|-----|----------------|
| -1  | None           |
| 0   | Scp173         |
| 1   | ClassD         |
| 2   | Spectator      |
| 3   | Scp106         |
| 4   | NtfSpecialist  |
| 5   | Scp049         |
| 6   | Scientist      |
| 7   | Scp079         |
| 8   | ChaosConscript |
| 9   | Scp096         |
| 10  | Scp0492        |
| 11  | NtfSergeant    |
| 12  | NtfCaptain     |
| 13  | NtfPrivate     |
| 14  | Tutorial       |
| 15  | FacilityGuard  |
| 16  | Scp939         |
| 17  | CustomRole     |
| 18  | ChaosRifleman  |
| 19  | ChaosMarauder  |
| 20  | ChaosRepressor |
| 21  | Overwatch      |
| 22  | Filmmaker      |
| 23  | Scp3114        |
| 24  | Flamingo       |
| 25  | AlphaFlamingo  |
| 26  | ZombieFlamingo |
```
An example `OverallRoleType` corresponding to the Scientist class:
```
  role_id: 6
  role_type: BaseGame
```
Another example `OverallRoleType` corresponding to a UCR role with its ID set to 22:
```
  role_id: 22
  role_type: Uncomplicated
```
If there are any questions, do not be afraid to send me a DM!


## Omni Roleplay Utilities (ABRIDGED)
Omni-Utils is a roleplay utilities plugin, adding a .nick command, the ability to create randomized name systems, a jump-stamina waster, and more. All of these features can be enabled, disabled, and configured to your wishes.

**Dependencies are RUEI and UCR.**
**Please see the README under Omni-Utils to learn more, and shoot me any questions if you need using my discord @icedchqi!**

Config
```
# Indicates plugin enabled or not
is_enabled: true
# Indicates debug mode enabled or not
debug: false
# Amount of stamina to consume when jumping. Set to 0 to disable (especially if other plugin already does this).
stamina_use_on_jump: 30
# Roleplay Height features include randomized height and a player-command to change it
use_roleplay_height: true
height_min: 0.899999976
height_max: 1.10000002
# Nickname configs, good for roleplay purposes.
nickname_config:
# Whether this feature set is enabled.
# There are several keywords to look out for, but mostly they're %4digit% (four random digits), %nick% (player's display name), %rank% (random rank from E-1 to O-10). You can also create your own lists of ranks, below.
  is_enabled: true
  # Whether to show the hint to a player telling them their nickname, and role name.
  show_intro_text: true
  # Reset nicknames after death.
  reset_names_on_mortality: true
  role_nicknames:
    ClassD: D-%nickfirst%%4digit%
    Scientist: Staff %nick%
    FacilityGuard: Agent %nick%
    NtfCaptain: '%rankhigh% %nick%'
    NtfSergeant: '%rankmid% %nick%'
    NtfPrivate: '%ranklow% %nick%'
    ChaosConscript: Recruit %nick%
    ChaosMarauder: '%ranklow% %nick%'
    ChaosRepressor: '%ranklow% %nick%'
    ChaosRifleman: '%ranklow% %nick%'
  # Make sure that when you use these, you enwrap it in two %s. These are meant to be used in nicknames, or rolenames! Note that it stays the same no matter what during one life (If a player puts %rank% in their name, and they get the rank Private, and they do it again, it stays the same.)
  rank_groups:
  -
  # This string will be able to be replaced with one of the random possible_replacements in a player's nickname, by enclosing it with two %s on each side (e.g rank would be %rank%)
    name: 'rank'
    possible_replacements:
    - 'Private'
    - 'Private First Class'
    - 'Corporal'
    - 'Specialist'
    - 'Sergeant'
    - 'Staff Sergeant'
    - 'Sergeant First Class'
    - 'Master Sergeant'
    - 'First Sergeant'
    - 'Sergeant Major'
    - 'Command Sergeant Major'
    - 'Second Lieutenant'
    - 'First Lieutenant'
    - 'Captain'
    - 'Major'
    - 'Lieutenant Colonel'
    - 'Colonel'
    - 'Brigadier General'
    - 'Major General'
    - 'Lieutenant General'
    - 'General'
  -
  # This string will be able to be replaced with one of the random possible_replacements in a player's nickname, by enclosing it with two %s on each side (e.g rank would be %rank%)
    name: 'ranklow'
    possible_replacements:
    - 'Private'
    - 'Private First Class'
    - 'Corporal'
    - 'Specialist'
  -
  # This string will be able to be replaced with one of the random possible_replacements in a player's nickname, by enclosing it with two %s on each side (e.g rank would be %rank%)
    name: 'rankmid'
    possible_replacements:
    - 'Corporal'
    - 'Sergeant'
    - 'Staff Sergeant'
    - 'Gunnery Sergeant'
    - 'Master Sergeant'
  -
  # This string will be able to be replaced with one of the random possible_replacements in a player's nickname, by enclosing it with two %s on each side (e.g rank would be %rank%)
    name: 'rankhigh'
    possible_replacements:
    - 'Second Lieutenant'
    - 'First Lieutenant'
    - 'Captain'
    - 'Major'
    - 'Lieutenant Colonel'
  random_replacements:
  -
  # This string will be able to be replaced with one of the random possible_replacements in a player's nickname, by enclosing it with two %s on each side (e.g rank would be %rank%)
    name: 'firstname_white'
    possible_replacements:
    - 'John'
    - 'Jacob'
    - 'Zachary'
    - 'Jonny'
    - 'Louis'
    - 'Bob'
# Rolename (good for custom RP events) related config.
rolename_config:
# Whether this feature set is enabled
  is_enabled: true
  role_role_names:
    ClassD: Class-D Personnel
    Scientist: Research Personnel
    FacilityGuard: FAC-SEC Personnel
    NtfSpecialist: Mobile Task Force Specialist
    NtfCaptain: Mobile Task Force Captain
    NtfSergeant: Mobile Task Force Sergeant
    NtfPrivate: Mobile Task Force Private
    Tutorial: Unknown Personnel
    ChaosConscript: Chaos Insurgency Conscript
    ChaosMarauder: Chaos Insurgency Marauder
    ChaosRepressor: Chaos Insurgency Repressor
    ChaosRifleman: Chaos Insurgency Rifleman
    Scp079: SCP-079
    Scp096: SCP-096
    Scp939: SCP-939
    Scp049: SCP-049
    Scp173: SCP-173
    Scp0492: SCP-049-2

```

## [Omni-CustomSquads](https://github.com/icedchai/Omni-CustomSquads)
This is another plugin which used to be part of this plugin, but is now separate. I recommend checking it out!