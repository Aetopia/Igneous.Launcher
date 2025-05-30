# Igneous Launcher

An experimental dynamic link library injector for Minecraft: Bedrock Edition that can load multiple dynamic link libraries at game startup & runtime.

## Features

- Load dynamic link libraries at game startup & runtime.

- Load multiple dynamic link libraries at once.

## [Usage](https://www.youtube.com/watch?v=OfFarr4wVK0)

- Download the latest release from [GitHub Releases](https://github.com/Aetopia/Igneous.Launcher/releases).

- Locate desired modifications & determine when they should be injected:

    |State|Description|
    |-|-|
    |<kbd>🛠️</kbd> Startup|Load specified dynamic link libraries before the game is initialized.|
    |<kbd>🎮</kbd> Runtime|Load specified dynamic link libraries after the game is initialized.|

    - For most modifications loading at <kbd>🎮</kbd> Runtime should suffice.

    - Consider consulting modification developers for more information.

- Once you have determined when to inject the desired modifications, in either <kbd>🛠️</kbd> or <kbd>🎮</kbd> sections:

    - Click <kbd>➕</kbd> to add modifications.

    - Click <kbd>➖</kbd> to remove select modifications.

- Once done, click on <kbd>▶</kbd> to launch the game with the selected modifications.