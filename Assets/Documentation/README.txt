# Unity FPS Project

## Overview

This Unity project demonstrates a basic AI-controlled shooter game. The project includes scripts for player control, enemy AI behavior, particle effects, and UI elements. This project is a place to show some of what I've learned. Things will be added regularly.

## Scripts

### 1. [PlayerController.cs](Assets/Scripts/PlayerController.cs)

#### Description:

The `PlayerController` script handles player movement, shooting bullets, and throwing grenades. It includes features such as sprinting, crouching, shooting hit-scan bullets, and throwing grenades.

### 2. [ParticleController.cs](Assets/Scripts/ParticleController.cs)

#### Description:

The `ParticleController` script is responsible for destroying the game object with a particle system component after the particle effect finishes its duration.

### 3. [Interactable_Chest.cs](Assets/Scripts/Interactable_Chest.cs)

#### Description:

The `Interactable_Chest` script allows the player to interact with a chest by pressing the 'E' key. It opens the chest lid when in range. TODO: Add in lootable items.

### 4. [HPBar.cs](Assets/Scripts/HPBar.cs)

#### Description:

The `HPBar` script updates the health bar UI, revealing a red bar to represent the current health of an entity in the game.

### 5. [Grenade.cs](Assets/Scripts/Grenade.cs)

#### Description:

The `Grenade` script handles the behavior of grenades, including explosion effects, damage calculation, and triggering events when the grenade is destroyed.

### 6. [ChargePrefab.cs](Assets/Scripts/ChargePrefab.cs)

#### Description:

The `ChargePrefab` script manages the UI representation of a charging mechanic, such as the power of a grenade throw.

### 7. [CharacterAnimator.cs](Assets/Scripts/CharacterAnimator.cs)

#### Description:

The `CharacterAnimator` script, attached to characters with animations, synchronizes animation parameters with the character's movement using NavMeshAgent.

### 8. [AIController.cs](Assets/Scripts/AIController.cs)

#### Description:

The `AIController` script controls the behavior of AI enemies. It includes features such as wandering, chasing the player, taking damage, and updating health bar UI.

## How to Use

1. Clone the repository or download the project files.
2. Open the project in Unity.
3. Navigate to the `Scenes` folder and open the main scene.
4. Play the scene to interact with the provided features.

## Dependencies

This project was developed using Unity (version 2022.3.10f1).
Models created in Blender (version 4.0).

This project uses "Gridbox Prototype Materials" (version 1.1.0) by Ciathyza from the Unity Asset store.


## License

This project was made using the Unity Personal license.
