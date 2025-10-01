/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace InfinityEditor
{

    /// <summary>
    /// Represents the informations the system keyboard
    /// </summary>
    public class Keyboard
    {
        public Keyboard()
        {
        }

        /// <summary>
        /// Creates new instance of Keyboard class
        /// </summary>
        /// <param name="evt">The event associated to the keybord</param>
        public Keyboard(Event evt)
        {
            this.Code = evt.keyCode;
            this.IsAlt = evt.alt;
            this.IsCapsLock = evt.capsLock;
            this.IsControl = evt.control;
            this.IsFunctionKey = evt.functionKey;
            this.IsNumeric = evt.numeric;
            this.IsShift = evt.shift;
            this.Modifiers = evt.modifiers;
        }

        /// <summary>
        /// The code of the key pressed.
        /// </summary>
        public KeyCode Code { get; set; }

        /// <summary>
        /// Is the pressed key is alt key
        /// </summary>
        public bool IsAlt { get; set; }

        /// <summary>
        /// Is the pressed key is alt capslock
        /// </summary>
        public bool IsCapsLock { get; set; }

        /// <summary>
        /// Is the pressed key is alt control key
        /// </summary>
        public bool IsControl { get; set; }

        /// <summary>
        /// Is the pressed key is alt function key
        /// </summary>
        public bool IsFunctionKey { get; set; }

        /// <summary>
        /// Is the pressed key is alt numeric key
        /// </summary>
        public bool IsNumeric { get; set; }

        /// <summary>
        /// Is the pressed key is alt shift key
        /// </summary>
        public bool IsShift { get; set; }

        /// <summary>
        /// The type of the modifier key that can be active during keystroke event.
        /// </summary>
        public EventModifiers Modifiers { get; set; }
    }
}