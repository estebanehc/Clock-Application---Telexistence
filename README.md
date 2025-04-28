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

## Project Structure Philosophy
The project follows a modular feature-based structure, dividing the core elements into Domain, Application, and Presentation layers for each feature.
This structure ensures:

- **Separation of concerns:** Each feature (Clock, Timer, Stopwatch) is isolated and self-contained
- **Scalability:** New features can be added without interfering with existing modules
- **Testability:** Each module has a dedicated Assembly Definition, enabling precise unit testing
- **Maintainability:** Organized codebase allowing easy understanding and updates by future team members

Each feature is structured as follows:

- **Domain:** Contains pure data models and business states
- **Application:** Implements services and use cases
- **Presentation:** Manages UI elements and presentation logic

This approach prepares the project for integration into larger, production-grade robotic operation systems.

## Architecture Pattern Used
The application implements the Model-View-Presenter (MVP) architectural pattern, adapted for reactive programming using UniRx.

### Why MVP instead of MVC?

- **Presenter** handles the presentation logic, reacting to model changes and updating the view
- **View** remains passive, focusing only on displaying data and receiving user input
- **Model** represents the pure data and states

This separation:

- Enhances testability (presenters can be tested without the actual UI)
- Supports reactive data flows (critical when using UniRx)
- Improves scalability and maintainability
- Facilitates multi-platform adaptations (Windows, iOS/iPad, VR)

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
            Interfaces/
            Common/
        Features/
            Clock/
                Domain/
                Application/
                Presentation/
            Timer/
                Domain/
                Application/
                Presentation/
            Stopwatch/
                Domain/
                Application/
                Presentation/
        Installer/
    Tests/
        Core/
        Features/
            Clock/
            Timer/
            Stopwatch/

```

## Requirements
- Unity 2021.3.4f1 LTS
- UniRx
- Zenject
- TextMeshPro (Recommended for advanced text rendering)

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
