## Omni Utils detailed overview!

Example custom_squad:
```
# Whether to make a CASSIE announcement
  use_cassie_announcement: true
  # Name used to refer to the squad in commands and logs
  squad_name: 'delta4'
  # Respawn wave this will replace. Use NineTailedFox to get the NATO divisions (Juliet-15), and ChaosInsurgency to not.
  squad_type: NtfWave
  # Announcement CASSIE will say when the custom squad enters
  entrance_announcement: 'MTFUnit nato_d 4 designated Minute Men division %division% hasentered AllRemaining'
  entrance_announcement_subs: 'Mobile Task Force Unit Delta-4 designated ''Minutemen'', division %division% has entered the facility.<split>All remaining personnel are advised to proceed with standard evacuation protocols until an MTF squad reaches your destination.'
  use_team_vehicle: true
  # Role type corresponding to the number in the spawn queue. ONLY PUT NUMBERS 0-9.
  custom_roles:
    c:
      role_id: 12
      role_type: BaseGameRole
    s:
      role_id: 11
      role_type: BaseGameRole
    p:
      role_id: 13
      role_type: BaseGameRole
  # Put a string of numbers corresponding to the custom-role lookup system above.
  spawn_queue: 'csssspsspspsppspppsspspppp'
```

NICKNAME_CONFIG defaults:
```# Whether this feature set is enabled.
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
```

ROLENAME_CONFIG default:
```
# Whether this feature set is enabled
  is_enabled: true
  role_role_names:
    ClassD: Class-D Personnel
    Scientist: Research Personnel
    FacilityGuard: FAC-SEC Personnel
    NtfCaptain: Mobile Task Force %rankhigh%
    NtfSergeant: Mobile Task Force %rankmid%
    NtfPrivate: Mobile Task Force %ranklow%
    Tutorial: Unknown Personnel
    ChaosConscript: Chaos Insurgency Conscript
    ChaosMarauder: Chaos Insurgency Marauder
    ChaosRepressor: Chaos Insurgency Repressor
    ChaosRifleman: Chaos Insurgency Rifleman
```
Rolenames will set the text below a player's name to a custom string. This is unique because the custominfo field goes above the player's name. As such, this feature has low compatibility and I recommend you turn it off if you're dealing with any other plugins that use custominfo for anything.

CUSTOM_TERMINATION_ANNOUNCE_CONFIG default:
```
# Whether this feature set is enabled
  is_enabled: true
  scp_cassie_string:
    ? role_id: 5
      role_type: BaseGameRole
    : words: 'scp 0 4 9'
      translation: 'SCP-049'
    ? role_id: 7
      role_type: BaseGameRole
    : words: 'scp 0 7 9'
      translation: 'SCP-079'
    ? role_id: 9
      role_type: BaseGameRole
    : words: 'scp 0 9 6'
      translation: 'SCP-096'
    ? role_id: 3
      role_type: BaseGameRole
    : words: 'scp 1 0 6'
      translation: 'SCP-106'
    ? role_id: 0
      role_type: BaseGameRole
    : words: 'scp 1 7 3'
      translation: 'SCP-173'
    ? role_id: 16
      role_type: BaseGameRole
    : words: 'scp 9 3 9'
      translation: 'SCP-939'
    ? role_id: 23
      role_type: BaseGameRole
    : words: 'scp 3 1 1 4'
      translation: 'SCP-3114'
  # Use %subject% in the announcements for the termination's name. Key is overallroletype, Value is name of Key in scp_termination_cassie_announcements.
  scp_termination_announcement_index:
    ? role_id: 6
      role_type: BaseGameRole
    : civil_science
    ? role_id: 1
      role_type: BaseGameRole
    : civil_classd
    ? role_id: 14
      role_type: BaseGameRole
    : goi_unknown_military
    ? role_id: 15
      role_type: BaseGameRole
    : mtf_facsec
    ? role_id: 4
      role_type: BaseGameRole
    : mtf_epsilon11
    ? role_id: 11
      role_type: BaseGameRole
    : mtf_epsilon11
    ? role_id: 12
      role_type: BaseGameRole
    : mtf_epsilon11
    ? role_id: 13
      role_type: BaseGameRole
    : mtf_epsilon11
    ? role_id: 8
      role_type: BaseGameRole
    : goi_chaos_insurgency
    ? role_id: 18
      role_type: BaseGameRole
    : goi_chaos_insurgency
    ? role_id: 19
      role_type: BaseGameRole
    : goi_chaos_insurgency
    ? role_id: 20
      role_type: BaseGameRole
    : goi_chaos_insurgency
  scp_termination_cassie_announcements:
    goi_unusual_incidents_unit:
      words: '%subject% terminated by the jam_1_3 un- use jam_1_2 u all pitch_1 In Jam_15_3 Sigma dids .g4 unit'
      translation: '%subject% terminated by the Unusual Incidents Unit'
    goi_chaosinsurgency:
      words: '%subject% terminated by chaosinsurgency'
      translation: '%subject% terminated by Chaos Insurgency.'
    goi_unknown_military:
      words: '%subject% terminated by Unknown Military Personnel'
      translation: '%subject% terminated by unknown military personnel.'
    mtf_epsilon11:
      words: '%subject% terminated by mtfunit epsilon 11 designated ninetailedfox'
      translation: '%subject% terminated by Mobile Task Force Unit Epsilon-11 designated ''Nine-Tailed Fox''.'
    mtf_facsec:
      words: '%subject% terminated by security personnel'
      translation: '%subject% terminated by Security Personnel.'
    civil_science:
      words: '%subject% terminated by science personnel'
      translation: '%subject% terminated by science personnel.'
    civil_classd:
      words: '%subject% terminated by classd personnel'
      translation: '%subject% terminated by Class-D personnel.'
```