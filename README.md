# Xbox Controller Remapper - Usage Instructions

## Installation
### Prerequisites
- .NET 6 or later
- Xbox controller (wired or wireless)

### Setup Instructions
1. Clone the repository:
   ```sh
   git clone https://github.com/julyan97/Xbox-Controller-Remapper.git
   ```
2. Navigate to the project directory:
   ```sh
   cd Xbox-Controller-Remapper
   ```
3. Restore dependencies:
   ```sh
   dotnet restore
   ```
4. Build the project:
   ```sh
   dotnet build
   ```
5. Run the application:
   ```sh
   dotnet run --project ControllerRebinder
   ```

---

## Configuration
### Editing `Configurations.json`
Modify the `Configurations.json` file to adjust controller mappings and settings.

Example `Configurations.json`:
```json
{
  "RefreshRate": 10,
  "Log": true,
  "LeftJoyStick": {
    "On": true,
    "StaticArea": 0.5,
    "ForwardDown": 1.0,
    "LeftRight": 0.8,
    "DeadZone": 1000,
    "MaxValController": 32768,
    "ThreshHoldAreaCal": 5000,
    "Controlls": {
      "Up": "VK_W",
      "Down": "VK_S",
      "Left": "VK_A",
      "Right": "VK_D"
    }
  }
}
```

---

## Running the Application
Start the application with:
```sh
   dotnet run --project ControllerRebinder
```

Once running, the application reads the configuration file and maps controller inputs to keyboard actions.

---

## Troubleshooting
### 1. No Key Presses Detected
- Ensure the Xbox controller is connected.
- Check logs for detected joystick movements.
- Verify `Configurations.json` formatting and validity.

### 2. Application Crashes on Startup
- Confirm `Configurations.json` is correctly formatted.
- Ensure all dependencies are installed.
- Run in debug mode for detailed logs:
  ```sh
  dotnet run --project ControllerRebinder --configuration Debug
  ```

