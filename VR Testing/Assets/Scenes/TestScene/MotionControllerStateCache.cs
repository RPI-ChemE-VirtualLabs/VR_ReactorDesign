using Microsoft.MixedReality.Input;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MotionControllerStateCache : MonoBehaviour {
    private class MotionControllerState {
        public MotionController MotionController { get; private set; } 
        public MotionControllerReading CurrentReading { get; private set; } 

        public MotionControllerState(MotionController mc) {
            this.MotionController = mc;
        }

        public void Update(DateTime when) { 
            this.CurrentReading = this.MotionController.TryGetReadingAtTime(when); 
        } 

    }

    private MotionControllerWatcher _watcher;
    private Dictionary<Handedness, MotionControllerState> _controllers = new Dictionary<Handedness, MotionControllerState>();

    public void Start() { // Starts monitoring controller's connections and disconnections 
        _watcher = new MotionControllerWatcher();
        _watcher.MotionControllerAdded += _watcher_MotionControllerAdded;
        _watcher.MotionControllerRemoved += _watcher_MotionControllerRemoved;
        var nowait = _watcher.StartAsync();
    }

    public void Update() { 
        var now = DateTime.Now; 

        lock (_controllers) { // update the motion controllers
            foreach (var controller in _controllers) { 
                controller.Value.Update(now); 
            } 
        } 
    } 

    public void Stop() { // Stops monitoring controller's connections and disconnections 
        if (_watcher != null) {
            _watcher.MotionControllerAdded -= _watcher_MotionControllerAdded;
            _watcher.MotionControllerRemoved -= _watcher_MotionControllerRemoved;
            _watcher.Stop();
        }
    }

    // called when a motion controller has been removed from the system

    private void _watcher_MotionControllerRemoved(object sender, MotionController e) {
        lock (_controllers) { // Remove a motion controller from the cache 
            _controllers.Remove(e.Handedness);
        }
    }

    // called when a motion controller has been added to the system
    private void _watcher_MotionControllerAdded(object sender, MotionController e) {
        lock (_controllers) { // Remove a motion controller from the cache 
            _controllers[e.Handedness] = new MotionControllerState(e);
        }
    }

    /*
    /// <summary> 
    /// Returns the current value of a controller input such as button or trigger 
    /// </summary> 
    /// <param name="handedness">Handedness of the controller</param> 
    /// <param name="input">Button or Trigger to query</param> 
    /// <returns>float value between 0.0 (not pressed) and 1.0 
    /// (fully pressed)</returns> 
    public float GetValue(Handedness handedness, ControllerInput input) { 
        MotionControllerReading currentReading = null; 

        lock (_controllers) { 
            if (_controllers.TryGetValue(handedness, out MotionControllerState mc)) { 
                currentReading = mc.CurrentReading; 
            } 
        } return (currentReading == null) ? 0.0f : currentReading.GetPressedValue(input); 
    } 

    /// <summary> 
    /// Returns the current value of a controller input such as button or trigger 
    /// </summary> 
    /// <param name="handedness">Handedness of the controller</param> 
    /// <param name="input">Button or Trigger to query</param> 
    /// <returns>float value between 0.0 (not pressed) and 1.0 
    /// (fully pressed)</returns> 
    public float GetValue(UnityEngine.XR.WSA.Input.InteractionSourceHandedness handedness, ControllerInput input) { 
        UnityEngine.XR.WSA.
        return GetValue(Convert(handedness), input); 
    } 

    /// <summary> 
    /// Returns a boolean indicating whether a controller input such as button or trigger is pressed 
    /// </summary> 
    /// <param name="handedness">Handedness of the controller</param> 
    /// <param name="input">Button or Trigger to query</param> 
    /// <returns>true if pressed, false if not pressed</returns> 
    public bool IsPressed(Handedness handedness, ControllerInput input) { 
        return GetValue(handedness, input) >= PressedThreshold; 
    } */
}