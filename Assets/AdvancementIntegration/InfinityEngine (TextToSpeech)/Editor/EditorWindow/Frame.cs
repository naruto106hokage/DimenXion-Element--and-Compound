/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                                    *
*************************************************************************************************************************************/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using InfinityEngine;
using System.Reflection;
using InfinityEngine.Extensions;
using System.Collections;
using System.Linq;
using UnityEngine.Events;

namespace InfinityEditor
{

    /// <summary>
    ///   Represents a Simple editor window.
    /// </summary>
    [Serializable]
    public class Frame : EditorWindow
    {

        #region Events

        protected Dictionary<EventType, Action> EventMap { get; set; }


        public Action onGUI;
        public Action onRepaint;
        public Action onClose;

        private Action<Keyboard, Event> KeyUpListener;
        private Action<Keyboard, Event> KeyDownListener;

        private Action<MouseButton, Vector2, Event> MouseUpListener;
        private Action<MouseButton, Vector2, Event> MouseDownListener;
        private Action<MouseButton, Vector2, Event> MouseDragListener;

        private Action<Vector2, Event> MouseScrollListener;

        #endregion Events

        /// <summary>
        /// Creates new window
        /// </summary>
        /// <param name="size">The size of the window</param>
        /// <param name="title">The title of the window</param>
        /// <param name="isResizable">Is the window is resizable ?</param>
        /// <returns></returns>
        public static Frame Create(Vector2 size, GUIContent title, bool isResizable = true)
        {
            var window = CreateInstance<Frame>();
            window.maxSize = size;
            if (!isResizable)
            {
                window.minSize = size;
            }
            window.titleContent = title;
            return window;
        }

      
        /// <summary>
        /// Called when the window is focused.
        /// </summary>
        protected virtual void OnEnable()
        {
            if (EventMap == null)
            {
                EventMap = new Dictionary<EventType, Action>
            {
                { EventType.Repaint, this.OnRepaint },

                { EventType.KeyDown, () => {
                    this.OnKeyDown(new Keyboard(Event.current), Event.current);
                }},

                { EventType.KeyUp, () => {
                    this.OnKeyUp(new Keyboard(Event.current), Event.current);
                }},
                { EventType.MouseDown, () => {
                    this.OnMouseDown((MouseButton)Event.current.button, Event.current.mousePosition, Event.current);
                }},
                { EventType.MouseUp, () => {
                    this.OnMouseUp((MouseButton)Event.current.button, Event.current.mousePosition, Event.current);
                }},
                { EventType.MouseDrag, () => {
                    this.OnMouseDrag((MouseButton)Event.current.button, Event.current.delta, Event.current);
                }},
                { EventType.ScrollWheel, () => {
                    this.OnScrollWheel(Event.current.delta, Event.current);
                }}
            };
            }
        }

        /// <summary>
        /// Called when the window is drawed
        /// </summary>
        protected virtual void OnGUI()
        {
            var controlId = GUIUtility.GetControlID(FocusType.Passive);

            var controlEvent = Event.current.GetTypeForControl(controlId);

            if (EventMap.ContainsKey(controlEvent))
            {
                EventMap[controlEvent].Invoke();
            }
            if (onGUI != null)
            {
                onGUI.Invoke();
            }
        }

        /// <summary>
        /// Called before the window being destroyed
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (onClose != null)
            {
                onClose.Invoke();
            }
        }


        #region Listeners


        protected virtual void OnRepaint()
        {
            if (onRepaint != null)
            {
                onRepaint.Invoke();
            }
        }

        protected virtual void OnKeyDown(Keyboard keyboard, Event e)
        {
            if (KeyDownListener != null)
                KeyDownListener.Invoke(keyboard, e);
        }

        public void AddKeyDownListener(Action<Keyboard, Event> listener)
        {
            this.KeyDownListener += listener;
        }

        protected virtual void OnKeyUp(Keyboard keyboard, Event e)
        {
            if (KeyUpListener != null)
                KeyUpListener.Invoke(keyboard, e);
        }

        public void AddKeyUpListener(Action<Keyboard, Event> listener)
        {
            this.KeyUpListener += listener;
        }

        protected virtual void OnMouseDown(MouseButton button, Vector2 position, Event e)
        {
            if (MouseDownListener != null)
                MouseDownListener.Invoke(button, position, e);
        }

        public void AddMouseDownListener(Action<MouseButton, Vector2, Event> listener)
        {
            this.MouseDownListener += listener;
        }

        protected virtual void OnMouseUp(MouseButton button, Vector2 position, Event e)
        {
            if (MouseUpListener != null)
                MouseUpListener.Invoke(button, position, e);
        }

        public void AddMouseUpListener(Action<MouseButton, Vector2, Event> listener)
        {
            this.MouseUpListener += listener;
        }

        protected virtual void OnMouseDrag(MouseButton button, Vector2 delta, Event e)
        {
            if (MouseDragListener != null)
                MouseDragListener.Invoke(button, delta, e);
        }
        public void AddMouseDragListener(Action<MouseButton, Vector2, Event> listener)
        {
            this.MouseDragListener += listener;
        }

        protected virtual void OnScrollWheel(Vector2 delta, Event e)
        {
            if (MouseScrollListener != null)
                MouseScrollListener.Invoke(delta, e);
        }
        public void AddMouseScrollListener(Action<Vector2, Event> listener)
        {
            this.MouseScrollListener += listener;
        }

        #endregion Listeners
        

    }
}