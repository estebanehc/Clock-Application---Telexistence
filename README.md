# Clock Application - Technical Assignment for Telexistence

## Overview
This is a clock application built with Unity, implementing a Clock, Timer, and Stopwatch using reactive programming (UniRx) and dependency injection (Zenject).
Designed with a clean architecture to allow easy future integration and maintainability.

## Architecture
- **Domain Layer:** Models and States
- **Application Layer:** Services handling business logic
- **Presentation Layer:** UI Controllers and Presenters
- **Dependency Injection:** Zenject
- **Reactive Programming:** UniRx
- **Testing:** NUnit and Unity Test Framework (Edit Mode and Play Mode tests)

## Features
- Display current time in user's timezone
- Timer functionality with Start, Stop, Reset, and Pause. Plays a sound when finished
- Stopwatch functionality with Start, Stop, Reset, and Lap recording
- Multitasking: All functions can run simultaneously

## Future Improvements

### iOS/iPad Support
- UI scaling using Unity's Canvas Scaler with screen size adaptation
- Touch-friendly button sizes
- Lightweight assets for mobile optimization

### VR Support (Optional)
- Consider redesigning UI for 3D interaction (e.g., world-space canvases)
- Gaze or controller-based input adaptations
- Optimized performance for VR rendering

## Project Structure
```
Assets/
    Scripts/
        Core/
        Features/
            Clock/
            Timer/
            Stopwatch/
        Installer/
    Tests/
```

## Requirements
- Unity 2021.3.4f1 LTS
- UniRx
- Zenject

## Setup Instructions
1. Clone the project
2. Open in Unity 2021.3.4f1
3. Open MainScene and click Play

## Testing
1. Open the Unity Test Runner
2. Run Edit Mode and Play Mode tests under the Tests folder

## Development Time Log
| Task | Time |
|------|------|
| Read requirements | 30 minutes |
| Project setup | 1 hour |
| Implement Clock | 2 hours |
| Implement Timer | x |
| Implement Stopwatch | x |
| Create Tests | x |
| Refactor and polish | x |
| Documentation | x |
