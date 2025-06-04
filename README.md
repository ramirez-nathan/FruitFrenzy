# Fruit Frenzy VR

Fruit Frenzy VR is a fast-paced, arcade-style virtual reality game developed in Unity. Built for the Meta Quest 2, players slice flying fruit in an immersive, low-poly environment using hand-tracking powered by Meta’s All-in-One SDK. Inspired by the simplicity and addictiveness of Fruit Ninja, this game was designed and developed in a 2-week sprint for UCR CS135.

## Features

- Hand-tracked fruit slicing using Meta’s SDK
- Low-poly stylized 3D art optimized for VR
- Custom-built particle effects and slicing logic
- In-game scoring system
- Built and deployed on Meta Quest 2

## Visual Effects

Fruit Frenzy VR includes custom-designed particle effects for immersive gameplay feedback:

<p align="center">
  <img src="BombExplosion.gif" alt="Bomb Explosion" width="250"/>
  <img src="CannonSmoke.gif" alt="Cannon Smoke" width="250"/>
  <img src="WindwakerWind.gif" alt="Wind Waker Wind" width="250"/>
</p>

## What is Unity?

[Unity](https://unity.com/) is a cross-platform game engine widely used for developing both 2D and 3D interactive experiences. It offers a powerful real-time editor, a comprehensive suite of tools, and strong support for XR development.

### Why Unity?

- VR-ready: Native support for Meta Quest and other major VR platforms  
- Real-time development: Rapid iteration using Unity Editor and Play Mode  
- Asset flexibility: Easy integration of 3D models, sound, and effects  
- Large community: Extensive documentation, plugins, and support  
- Cross-platform: Build once, deploy to PC, mobile, and VR

## Installation

To open and run the project in Unity:
```
# Clone the repository
git clone https://github.com/ramirez-nathan/FruitFrenzy.git
cd FruitFrenzy
```
1. Open the folder in **Unity 6000.0.47f1** or later.  
2. Make sure the Meta XR All-in-One SDK is installed.  
3. Connect your Meta Quest 2 via USB.  
4. Build and run to deploy the game to the headset.

## Build

Use Unity’s Build Settings to target **Android** (Meta Quest 2), and click **Build and Run**.  
Ensure that Developer Mode is enabled on your headset and all required XR plugins are configured.

## Asset Credits

While all scripts and particle effects were developed in-house, we used the following asset packs:

- `Low Poly Environment Starter Pack` by `MysticForge` – [link](https://assetstore.unity.com/packages/3d/environments/low-poly-environment-starter-pack-228606)
- `Low Poly Samurai Warrior Weapons Pack` by `Poly Ronin` – [link](https://assetstore.unity.com/packages/3d/props/weapons/low-poly-samurai-warrior-weapons-pack-310630)
- `Customizable skybox` by `Key Mouse` – [link](https://assetstore.unity.com/packages/2d/textures-materials/sky/customizable-skybox-174576)
