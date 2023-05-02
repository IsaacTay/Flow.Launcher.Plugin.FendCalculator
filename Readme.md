# Flow.Launcher.Plugin.FendCalculator

Integration of [Fend](https://printfn.github.io/fend/) with [Flow launcher](https://github.com/Flow-Launcher/Flow.Launcher).
![](assets/FendCalculator.gif)

## Settings

| Setting      | Type   | Default | Description                                    |
| ------------ | ------ | ------- | ---------------------------------------------- |
| Fend Command | string | fend    | Command to run fend or path to fend executable | 

## Fend installation

This plugin currently does not automatically install fend.

Until this is added, you can install fend through [scoop](https://scoop.sh/#/apps?id=e6f0a9d6e20f46ef6481143944e6bb44fb766fb4), [chocolatey](https://community.chocolatey.org/packages/fend), [winget](https://printfn.github.io/fend/documentation/#installation), the [official msi installer](https://github.com/printfn/fend/releases/latest/download/fend-windows-x64.msi), or  as a [standalone binary](https://github.com/printfn/fend/releases/tag/v1.1.6).

## TODOs

- [ ] Automatic installation of fend if there is none
  - [ ] Auto-detect package managers in the system
- [ ] Better support for multi-line outputs
- [ ] Save/Loading
  - [ ] Ability to save snippets/functions for reuse
  - [ ] Ability to load a file with predefined variables & functions
