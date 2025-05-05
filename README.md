# Clock Application - Technical Assignment for Telexistence

## Overview
This is a clock application built with Unity, implementing a Clock, Timer, and Stopwatch using reactive programming (UniRx) and dependency injection (Zenject).
Designed with a clean architecture to allow easy future integration and maintainability.

## Index
- [Architecture](#architecture): Core organization of the project using Domain, Application, and Presentation layers with DI and Reactive Programming.
- [Project Structure Philosophy](#project-structure-philosophy): Modular feature-based approach ensuring separation of concerns and maintainability.
- [Architecture Pattern Used](#architecture-pattern-used): Detailed explanation of MVP pattern implementation and its advantages over MVC.
- [Features](#features): List of main functionalities including Clock, Timer, and Stopwatch capabilities.
- [Future Improvements and Concerns](#future-improvements-and-concerns): Analysis of UI, code refactoring, and VR adaptation considerations.
- [Project Structure](#project-structure): Directory layout and organization of code files.
- [Testing Coverage](#testing-coverage): Comprehensive test metrics and coverage analysis per module.
- [Requirements](#requirements): Required Unity version and dependencies.
- [Setup Instructions](#setup-instructions): Steps to get the project running.
- [Testing](#testing): Instructions for running unit and integration tests.
- [Development Time Log](#development-time-log): Breakdown of time spent on each development phase.

## Architecture
- **Domain Layer:** Models and States
- **Application Layer:** Services handling business logic
- **Presentation Layer:** UI Views and Presenters
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
The application implements the **Model-View-Presenter (MVP)** architectural pattern, adapted for reactive programming using **UniRx**.

### Why MVP instead of MVC?

While MVC is common in Unity, MVP offers **greater decoupling and testability**, especially when building applications with **complex UI logic and data flows**.

- **Presenter** handles all presentation logic. It acts as a mediator between the View and the Model. It listens to input from the View and reacts to changes in the Model, updating the View accordingly.
- **View** remains passive and lightweight. It doesn’t contain logic — it only displays data and forwards user actions to the Presenter.
- **Model** contains the pure data and business states.

This separation allows us to:
- Write **unit tests** for Presenters without relying on UI elements or MonoBehaviours.
- Reuse and swap Views more easily, since the logic is not tied to Unity components.
- Avoid tightly coupled code, which is especially useful when building for different platforms like iOS, VR, etc.

In our case, this structure also fits well with **UniRx**, enabling reactive streams between layers and making the flow of data and events clearer and more maintainable.

#### Difference between Controller and Presenter

In the classic MVC pattern, the **Controller** typically responds to user input and updates both the View and the Model directly. In Unity, this often leads to **tight coupling with MonoBehaviours** and **UI-specific logic** embedded in Controllers, making it harder to test and maintain.

In contrast, the **Presenter in MVP** is fully decoupled from the Unity engine and **does not manipulate Views or Models directly** through Unity components. Instead, it works through interfaces, making it more testable, flexible, and modular.

This design is particularly effective when combined with **UniRx**, as it allows us to handle user interactions and data updates through **observable streams**, keeping the Presenter free from lifecycle dependencies and better aligned with reactive programming principles.

## Features
- Display current time in user's timezone
- Timer functionality with Start, Stop, Reset, and Pause. Plays a sound when finished
- Stopwatch functionality with Start, Stop, Reset, and Lap recording
- Multitasking: All functions can run simultaneously

## Future Improvements and Concerns

### 1. UI Concerns on iOS/iPad Devices

Right now, the canvases in the project use the **“Scale with Screen Size”** mode, which makes the layout flexible and allows it to adapt to different resolutions. This works quite well on PC in both vertical and horizontal modes. However, on iOS and especially on iPad, it’s important to **rethink the responsive design**, as well as **adjust the size and position of UI elements** so they fit well on both portrait and landscape orientations. It would be good to polish the layout with real device testing and make sure all elements scale properly and are comfortable to interact with.

### 2. Code Refactoring and Improvements After Release

The current architecture uses an **MVP structure**, designed to keep logic decoupled by using interfaces. This works well for separation of concerns, but sometimes introduces **many classes and interfaces**, especially in areas like the Timer, which could make UI integration harder.

#### Must-Happen:
- **Review and simplify interfaces**: Some UI-related interfaces (like `ITimerUI`) have very specific contracts. It would be better to move those UI behaviors directly to the views and keep interfaces more flexible for other use cases.
- **Refactor and clean Presenter classes**: Focus on improving method organization and logic distribution to keep the code readable and maintainable.
- **Keep test coverage strong**: Around 95% of the current code is already covered by unit tests. It’s important to **keep testing consistently** with every new feature or change, especially in the core stopwatch logic.

#### Nice-to-Have:
- **Add better feedback**: Improve animations and transitions to give the user a more responsive feel when interacting with the app.
- **Support additional widgets**: For example, allow users to add labels or notes to laps, or track custom values.
- **Improve naming and consistency**: Clean up minor details to make the codebase easier to understand and scale.

### 3. Concerns About Supporting VR

If this clock application is going to be used in a VR app, the main concern is adapting the **2D canvas-based layout to 3D space**. The current responsive design is based on screen resolution, but in VR we need to position UI elements in **world space** and make sure they are **clearly visible at a readable distance**.

Here are a few points to consider:

- **UI layout should be refitted for VR**: Some UI elements might look too big or too small in 3D. We'll need to adjust scale and spacing for comfortable viewing.
- **Interaction must be adapted**: Instead of mouse clicks or finger taps, we need to support **raycasting, hand tracking or controller input**.
- **Avoid motion discomfort**: Keep the UI stable and avoid moving or floating elements too much. Placing it at a fixed distance in front of the user would be ideal.
- **Test readability and depth**: Text must be high-contrast and easy to read without causing eye strain in VR.

This will require rethinking how the app presents information, but the current logic can still be reused with minimal changes.


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
        UI/
    Tests/
        Editor/
            ClockTest/
            StopwatchTest/
            TimerTest/
        Play/
            Features/
                ClockTest/
                StopwatchTest/
                TimerTest/
            UI/

```

## Testing Coverage

### Overview
This project includes comprehensive unit and play mode testing across all core modules. The following coverage data was generated using CodeCoverage on 2025-05-05.

### Overall Metrics
| Metric              | Value     |
| ------------------- | --------- |
| Assemblies analyzed | 11        |
| Classes             | 23        |
| Files              | 23        |
| Coverable lines    | 954       |
| Covered lines      | 906       |
| Line coverage      | **94.9%** |
| Methods            | 258       |
| Covered methods    | 236       |
| Method coverage    | **91.4%** |

### Module Coverage

#### Clock Module
| Component           | Coverage | Status               |
| ------------------- | -------- | -------------------- |
| ClockModel          | 100%     | ██████████████████ |
| ClockPresenter      | 95.2%    | █████████████████  |
| ClockService        | 100%     | ██████████████████ |
| ClockView           | 100%     | ██████████████████ |
| ClockTest           | 100%     | ██████████████████ |

#### Stopwatch Module
| Component              | Coverage | Status               |
| ---------------------- | -------- | -------------------- |
| StopwatchModel         | 100%     | ██████████████████ |
| StopwatchPresenter     | 81.4%    | ████████████▍      |
| StopwatchService       | 90.2%    | ██████████████     |
| StopwatchView          | 88.3%    | █████████████▊     |
| StopwatchTest          | 97.7%    | ███████████████▍   |

#### Timer Module
| Component          | Coverage | Status               |
| ------------------ | -------- | -------------------- |
| TimerModel         | 100%     | ██████████████████ |
| TimerPresenter     | 94.2%    | ████████████████▎  |
| TimerService       | 83.6%    | ███████████▌       |
| TimerView          | 100%     | ██████████████████ |
| TimerTest          | 89.1%    | █████████████▏     |

#### UI Components
| Component             | Coverage | Status               |
| --------------------- | -------- | -------------------- |
| MainViewNavigator     | 100%     | ██████████████████ |
| MainViewNavigatorTest | 100%     | ██████████████████ |

> Note: The project uses Play Mode and Edit Mode tests to ensure robustness of business logic and UI interactions across all modules.

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
| Implement Clock | 1.5 hours |
| Implement Timer | 3 hours |
| Implement Stopwatch | 3 hours |
| Create Tests | 4 hours |
| Refactor and polish | 2.5 hours |
| Documentation | 3 hours |
