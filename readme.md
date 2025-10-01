# Unity Art

A Unity project featuring creative tools and controllers for procedural art generation, animation, and visual effects.

## 🎥 Demo

Check out the demo videos showcasing the project's capabilities:
[Unity Art Demo Playlist](https://www.youtube.com/playlist?list=PLKz7QOwPaa35nsaio8KjFOKk10Y8UubsK)

## ✨ Features

### 🎛️ Controllers
- **Time Controllers**: Global and local time management for animations
- **BPM Controllers**: Beat-synchronized animations and effects
- **LFO Controllers**: Low Frequency Oscillator controllers for various data types:
  - Float values
  - Vector3 positions/rotations/scales
  - Colors
  - Integer values
- **Cycle Controllers**: Precise cycle-based timing and event triggering

### 🎨 Object Manipulation
- **Clone Pattern**: Create dynamic patterns of objects with:
  - Position, rotation, and scale transformations
  - Color gradients and lists
  - Real-time parameter adjustment
- **Copy and Transform**: Batch operations for duplicating and transforming objects
- **Procedural Generation**: Automated object creation with mathematical precision

### 🎵 Audio-Visual Sync
- **BPM Synchronization**: Link visual effects to musical beats
- **External BPM Sources**: Connect to external timing sources
- **Rhythm-based Animations**: Create visuals that respond to music

### 🔧 Custom Attributes
- **Conditional Fields**: Show/hide inspector fields based on other values
  - [`HideIfEqualAttribute`](Assets/Attributes/HideIfEqualAttribute.cs)
  - [`ShowIfEqualAttribute`](Assets/Attributes/ShowIfEqualAttribute.cs)
- **Value Constraints**: Enforce minimum and maximum values
  - [`MinAttribute`](Assets/Attributes/MinAttribute.cs)
  - [`MaxAttribute`](Assets/Attributes/MaxAttribute.cs)

## 🏗️ Architecture

### Core Components

**Time Management**
- [`ITimeController`](Assets/Scripts/Controllers/ITimeController.cs): Interface for time-based calculations
- [`TimeController`](Assets/Scripts/Controllers/TimeController.cs): Base class for global/local time handling
- [`BPMController`](Assets/Scripts/Controllers/BPMController.cs): BPM-aware time calculations

**LFO System**
- [`LFOController`](Assets/Scripts/Controllers/LFOControllers/LFOController.cs): Base LFO functionality
- Multiple waveform types: Sine, Triangle, Square, Linear, Custom
- Type-specific implementations for different data types

**Animation Utilities**
- [`AnimationCurveUtils`](Assets/Scripts/AnimationCurveUtils.cs): Curve manipulation and normalization
- Custom waveform generation
- Curve joining and processing

## 🚀 Getting Started

### Prerequisites
- Unity 6000.0.24f1 or later
- Input System package (included)

### Setup
1. Clone the repository
2. Open the project in Unity
3. Load the main scene
4. Explore the prefabs and example setups

### Basic Usage

**Creating a Simple LFO Animation:**
```csharp
// Add an LFO controller to animate position
var lfoController = gameObject.AddComponent<LFOFloatController>();
lfoController.waveform = LFOWaveformType.Sine;
lfoController.beatsPerCycle = 4f; // Complete cycle every 4 beats
```

**Setting up Object Cloning:**
```csharp
// Create a pattern of objects
var clonePattern = gameObject.AddComponent<ClonePattern>();
clonePattern.NumClones = 10;
clonePattern.DeltaPosition = new Vector3(2f, 0f, 0f);
clonePattern.colorMode = ColorMode.Gradient;
```

## 🎨 Art Generation Features

### Pattern Creation
- Mathematical transformations
- Color gradient mapping
- Procedural arrangements
- Real-time parameter tweaking

### Animation Systems
- Cycle-based timing
- Phase offset control
- Looping and one-shot modes
- Multiple waveform types

### Visual Effects
- Color interpolation
- Transform animations
- Synchronized movements
- Beat-matching visuals

## 📁 Project Structure

```
Assets/
├── Attributes/           # Custom property attributes
├── Scripts/
│   ├── Controllers/      # Time, BPM, LFO, and Cycle controllers
│   ├── AnimationCurveUtils.cs
│   ├── ClonePattern.cs
│   ├── CopyAndTransform.cs
│   └── UnityEvents.cs
├── Materials/            # Visual materials
├── Prefabs/             # Reusable components
└── TutorialInfo/        # Project documentation
```

## 🤝 Contributing

This project demonstrates various Unity programming patterns and creative coding techniques. Feel free to explore the code and adapt it for your own creative projects.

## 📜 License

This project is provided as-is for educational and creative purposes.

---

*Explore the intersection of code, mathematics, and visual art with Unity Art.*