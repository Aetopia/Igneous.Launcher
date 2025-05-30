# Igneous Launcher

An experimental dynamic link library injector for Minecraft: Bedrock Edition that can load multiple dynamic link libraries at game startup & runtime.

# Features

- Load dynamic link libraries at game startup & runtime.

- Load multiple dynamic link libraries at once.

# Usage

- Download the latest release from [GitHub Releases](https://github.com/Aetopia/Igneous.Launcher/releases).

- Locate desired modifications & determine when they should be injected:

    |State|Description|
    |-|-|
    |<kbd>🛠️</kbd> Startup|Load specified dynamic link libraries before the game is initialized.|
    |<kbd>🎮</kbd> Runtime|Load specified dynamic link libraries after the game is initialized.|

    - For most modifications loading at <kbd>🎮</kbd> Runtime should suffice.

    - Consider consulting modification developers for more information.

- In the relevant section:

    - Click <kbd>➕</kbd> to add modifications.

    - Click <kbd>➖</kbd> to remove select modifications.