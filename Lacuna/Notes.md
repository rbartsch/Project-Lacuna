# Notes

Game description/themes:

- Text-based/adventure spaceship tactics game with dice rolling.

Ship configuration:

- Each ship has shields, a part of it can become under pressure depending on side taking fire, bow, stern, port, and starboard.
  - Shields can be managed by mitigating damage from a side by changing ship/combat orientation.
  - If shields are gone entirely, it can be restored depending on crew skill, ship status, enemy interference, and other factors. How much they are restored also depend on such factors too, with each side being restored uniform or random.
  - Depending on type of shields (i.e high resistance to a damage type/capacity), they draw varying amounts of ship resources like power

- Each ship has armoured plates, bow, stern, port, and starboard.
  - Armoured plates cannot be restored in combat, only at a port/outside of combat.
  - If any armoured plate is entirely destroyed, critical ship systems are exposed, as well as crew lives. Very high chance of critical systems being destroyed.
  - Chance of critical systems being damaged depend on how damaged armoured plates are.
  - Depending on type of armoured plate (i.e high resistance to a damage type/thickness), they can affect mass/maneuverability of a ship.

- Sides of a ship depending on the hull/class/fittings can have higher or lower chance of difficulty to be hit, which is combined with the attacker's sides of higher or lower chance for weapons to hit from their side (i.e hardpoint placement locations)
  - e.g Class A ship's bow hardpoints have a 70% chance to hit + combined weapon hit chance, where the enemy's port side has 60% evasion etc

- Each ship can have weapons on the bow, stern, port, and starboard.
- Tile distance matters in RNG calculation like hit chance etc. Weapons have a range, i.e 1 tile, 2 tile, etc. That means you can have a weapon on port side that has 1 tile range and a weapon on starboard with 2 tile range, that factors into your positioning strategy.
- Each ship has a set of different attributes, maneuverability, stealth, power, signature radius, etc

Other mechanics:

- Four damage types, thermal, explosive, kinetic, and electromagnetic.
  - Different weapons fire different type.
  - Depending on shield and armour plates, they can have different resistances.

- Ship movement based on grid, 4 directions, no diagonals (maybe later?). In each move, you move the direction and then can change rotation? Player and NPC can "pass" a turn move by going into defensive or idle? Add a delay/timer between player turn and npc turn to make it seem more obvious

- Dice rolls to resolve almost everything.
- Rolls still determine things like chance to hit based on target maneuverability, but strategy of ship positioning is entirely dependent on the player.
- Events with a short and long text narrative that can involve a wide range of activities and situations, such as stumbling upon a derelict ship randomly that can branch into salvaging or mishaps, with associated text narrative/adventure. An emphasis on writing strong short stories and short events, as well as connected arcs.
- Interactions with other ships can involve hailing them, diplomacy, trade, surrender terms, etc.
- You can send out probes to scout the grid and other star systems.
- Game has an economy, trade and manufacturing/industry jobs i.e EVE Online-style.
- Star map and local system map, with two screens for each. If you go to a location on  local map, such as a planet, it has the 3x3 grid for that local area, which includes its graphics. When you select a star on star map, you arrive at the star, with its 3x3 grid

Graphics:

- Procedural celestial generation, or make all celestial graphics white and then randomise colours? Or use parts to build up celestial objects like independent rings
- Display arrows next to ship if adjacent grid is valid move position
- Display small banners/images for events/planets selections like PDX GSG style, use minimalism. Make use of procedural generation to generate horizonal views of city skyscrapers etc

Game/Engine Technical:

- Have two screen types, ScreenStartup, ScreenLoadable?
- Have a contentmanager for global and contentmanager for each screen, so we can unload content for a screen if needed.
- Madlib system for description text generation

Other:

- Maybe fleet/squadron-based? Would need to change the grid to be bigger grid
- Star has a name and uses alphabet letter if more than one, planet derives name from star name and uses roman numerals, moon derives from planet name and uses arabic numerals
  - [Star] Aberulug a, [Planet] Aberulug a -> I, [Moon] Aberulug a -> I -> 1