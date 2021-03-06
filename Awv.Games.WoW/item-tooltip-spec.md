Sources:
- https://www.wowhead.com/
- https://worldofwarcraft.fandom.com/et/wiki/Proc

The below is my compiled order of components that appear in item tooltips.

Input on modification (with sources) would be appreciated.

**three dashses (---) signifies a break between left and right floating text*
<br>
**Also, there is no significance to any of the items linked below. I just provided examples I found.*

Each heading is a different "Section" of the tooltip.

# Header Section
- `Name`
- *multi-equipment only (multiple slots consumed by 1 item)*
    - `Piece Name` (https://www.wowhead.com/item=128872)
- `Special Item Flags` (ex: "Titanforged" / "Warforged")
- `Usage` (ex: "Champion equipment")
- `Item Level`
- `Binds On`
- `Uniqueness`

# Core Section (nullable)
- *equipment only*
    - `Slot` --- `Equipment Type`
        - https://classic.wowhead.com/item=13262
        - https://www.wowhead.com/item=132357
- *weapon only*
    - `Minimum` - `Max` Damage --- `Speed` - https://classic.wowhead.com/item=13262
    - +&nbsp;`Minimum` - `Maximum` `Element` Damage --- `Speed` - https://classic.wowhead.com/item=13262
    - (`DPS` damage per second) - https://classic.wowhead.com/item=13262
- *armor only*
    - `Armor`

# Stats Section (nullable)
- *equipment only*
    - `Stats` - https://www.wowhead.com/item=49623
    - `Artifact Relic Slots` - https://www.wowhead.com/item=128872 **Not yet implemented*
    - `Gem Sockets` - https://www.wowhead.com/item=49623 **Not yet implemented*
    - `Socket Bonus` - https://www.wowhead.com/item=49623 **Not yet implemented*
    - `Durability` - https://www.wowhead.com/item=49623
- Classes: `classes` - https://www.wowhead.com/item=49623 **Not yet implemented*

# Effects Section
- *weapon only*
    - `Chance On Hit` - https://www.wowhead.com/item=18816
    - `Chance On Critial Hit` - https://www.wowhead.com/item=28034
    - `Chance On Spell Hit` - https://www.wowhead.com/item=38071
    - `Chance On Spell Critical Hit` - https://www.wowhead.com/item=28418
- *equipment only*
    - `Equip Effects` - https://classic.wowhead.com/item=19120
- `Use` - https://www.wowhead.com/item=158924

# Footer Section
- `Requires Level` - https://www.wowhead.com/item=18816
- *equipment only*
    - Set Name - https://www.wowhead.com/item=77949 **Not yet implemented*
    - Set Pieces - https://www.wowhead.com/item=77949 **Not yet implemented*
    - Set Effects - https://www.wowhead.com/item=77949 **Not yet implemented*
    - **See also: Azerite Gear - https://www.wowhead.com/item=174119* **Not yet implemented*
- `Duration`
- `Max Stack`
- `Flavor` - https://www.wowhead.com/item=132357
- `Sell Price` - https://www.wowhead.com/item=132357