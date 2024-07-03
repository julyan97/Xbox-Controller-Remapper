using ControllerRebinder.Common.Enumerations;
using ControllerRebinder.Common.Moddels.Configurations.SubModelsOfConfigurations;
using ControllerRebinder.Core.Caches;
using ControllerRebinder.Core.Events;
using ControllerRebinder.Core.Helpers;
using ControllerRebinder.Core.Services.Imp;
using DXNET.XInput;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WindowsInput;

namespace ControllerRebinder.Core.Services;

public class JoyStickService : IJoyStickService
{
    private readonly Controller _controller;
    private readonly InputSimulator _inputSimulator;
    private readonly JoyStick _joyStick;
    private readonly ILogger _logger;
    private Quadrant _currentQuadrant = Quadrant.TopLeft;
    private double _staticYArea;
    private double _currentXArea;

    private const int ThresholdMultiplier = 100;
    private const int AreaMultiplier = 10_000_000;

    public event JoyStickEventHandler JoyStickMoved;

    public JoyStickService(
        Controller controller,
        InputSimulator inputSimulator,
        JoyStick joyStick,
        ILogger logger)
    {
        _controller = controller;
        _inputSimulator = inputSimulator;
        _joyStick = joyStick;
        _logger = logger;
    }

    public async Task Start()
    {
        var config = ConfigCache.Configurations.LeftJoyStick;
        CircleHelper.FindArea(
            config.ThreshHoldAreaCal,
            config.MaxValController,
            config.MaxValController,
            out double staticYAngle,
            out _staticYArea);

        while (true)
        {
            var state = _controller.GetState();
            short stickX = 0;
            short stickY = 0;

            ChooseJoyStick(state, ref stickX, ref stickY);
            OnJoyStickMoved(stickX, stickY);

            await Task.Delay(ConfigCache.Configurations.RefreshRate).ConfigureAwait(false);
        }
    }

    private void ChooseJoyStick(State state, ref short stickX, ref short stickY)
    {
        switch (_joyStick)
        {
            case JoyStick.Left:
                stickX = state.Gamepad.LeftThumbX;
                stickY = state.Gamepad.LeftThumbY;
                break;
            case JoyStick.Right:
                stickX = state.Gamepad.RightThumbX;
                stickY = state.Gamepad.RightThumbY;
                break;
        }
    }

    protected virtual void OnJoyStickMoved(int stickX, int stickY)
    {
        JoyStickMoved?.Invoke(this, new JoyStickEventArgs(stickX, stickY));
    }
}