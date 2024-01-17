<p align="center">
⚠️ The script is still in development!! ⚠️
</p>

# Project C_Zones_Influence
## This FiveM script is written in C# using MONO_V2.

This script creates zones of influence throughout the city, players can score points in the zone to have some benefits from it. <br> 
(The benefits will be added in the future).

### How it works

The influence is presented on a scale from 0 to 1, when any organization have more than 0.8, the zone will be marked as occupied.
Other organizations and the police can reduce the influence points. 
When a player does something <b>actions</b> (Triggers from other scripts) in an occupied zone, members of the organization which the zone belongs to get a notification.
Players, who are members of an organization can check the influence map.

### Default Actions

* Occupation - Time spent in the zone

## NUI Presentation

<p align="center">
  <img src="https://github.com/mikoslaf/c-zones-influence/assets/93710959/721d6ed9-68fc-4b8d-9586-03bd977b2d79"/>
</p>

## Installation

### Requirements
* QBCore
* [QB_Handler](https://github.com/mikoslaf/qb-handler)

### From a Release Build

- Download the [latest version](https://github.com/mikoslaf/c-zones-influence/releases/tag/v1.0)
- Add `c-zones-influence` directory to the `resources` directory on the FiveM server
- Add the following directive to your FiveM ```server.cfg``` file
```
ensure c-zones-influence
```
## Configuration
### NUI
* In the `nui` directory, you can find the `config.json` file.
* In this file, you can add color to any group without configuration, the color is gray.
  
| Name of the group           | Color - RGB  |
| --------------------------- | -------------|
| ballas                      | `[49,2,68]`  |
| vagos                       | `[235,228,5]`|
| police                      | `[0,0,255]`  | 

# ⚠️ Warning! ⚠️

Work on the script is currently on hold because ... <br>
![error img](https://github.com/mikoslaf/c-zones-influence/assets/93710959/cd66ab4c-f353-4aa3-81ac-069a92cfbdfa)
