# IMPORTANT: plugin is under limited support. message `icedchqi` for more info.

# omni-plugins-pub
Omni Plugins were originally developed for Omni-2 SCP SL roleplay.

It is split into two plugins & one dependency for convenience and expandability.

**Contact icedchqi on Discord for any issues, questions, or concerns.**

## Omni Common Library
Omni-CommonLibrary is a small class library that features a couple of extensions & works as a main hub for generalized things I make for these plugins.

It contains one important class, *OverallRoleType*, which is critical but simple to understand.

The `RoleType` field can be one of three values: `BaseGameRole`, `CrRole` (EXILED Custom Role), or `UcrRole` (UncomplicatedCustomRoles).

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
  role_type: BaseGameRole
```
Another example `OverallRoleType` corresponding to a UCR role with its ID set to 22:
```
  role_id: 22
  role_type: UcrRole
```
If there are any questions, do not be afraid to send me a DM!


## Omni Roleplay Utilities
Omni-Utils is a roleplay utilities plugin, adding a .nick command, the ability to create randomized name systems, a jump-stamina waster, custom termination announcements, a powerful system that can be used to create custom spawn waves/squads, and more. All of these features can be enabled, disabled, and configured to your wishes.

**Dependencies are RUEI and UCR.**
**Please see the README under Omni-Utils to learn more, and shoot me any questions if you need using my discord @icedchqi!**

Config
```
# Indicates plugin enabled or not
is_enabled: true
# Indicates debug mode enabled or not
debug: false
# All squad names must be different. Leave empty if you're using my other custom squad plugin. PLEASE NOTE: THESE SQUADS ARE UNUSABLE IN THEIR CURRENT STATE, AND ARE FOR DEMONSTRATION PURPOSES ONLY!
custom_squads:
  #SEE: CUSTOM_SQUADS
# Amount of stamina to consume when jumping. Set to 0 to disable (especially if other plugin already does this).
stamina_use_on_jump: 30
# Roleplay Height features include randomized height and a player-command to change it
use_roleplay_height: true
height_min: 0.9
height_max: 1.1
# Nickname configs, good for roleplay purposes.
nickname_config:
  #SEE: NICKNAME_CONFIG
# Rolename (good for custom RP events) related config.
rolename_config:
  #SEE: ROLENAME_CONFIG
# Custom Termination Announcement config.
custom_termination_announcement_config:
  #SEE: CUSTOM_TERMINATION_ANNOUNCE_CONFIG
```



## Omni Custom Items
Omni-CustomItems adds the `GogglesItem` class, which can be used to add custom "goggles", of which only one can be worn at a time.

It adds NVGs and SCRAMBLE goggles.

Config:
```
# Is plugin enabled or not?
is_enabled: true
# Is plugin in debug mode?
debug: true
# Custom Item ID prefix
id_prefix: 1100
# Delay before items are registered.
registry_delay: 6
```
