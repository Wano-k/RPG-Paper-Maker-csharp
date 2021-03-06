﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace RPG_Paper_Maker
{
    class MouseManager
    {
        private Point MousePosition = Point.Zero;
        private bool FirstLeftClick = false;
        private bool FirstRightClick = false;
        private bool FirstWheelClick = false;
        private bool OnLeftClick = false;
        private bool OnRightClick = false;
        private bool OnWheelClick = false;
        private bool WheelUp = false;
        private bool WheelDown = false;


        // -------------------------------------------------------------------
        // InitializeMouse
        // -------------------------------------------------------------------

        public void InitializeMouse()
        {
            FirstLeftClick = false;
            FirstRightClick = false;
            FirstWheelClick = false;
            OnLeftClick = false;
            OnRightClick = false;
            OnWheelClick = false;
            WheelUp = false;
            WheelDown = false;
        }

        // -------------------------------------------------------------------
        // Set mouse status
        // -------------------------------------------------------------------

        public void SetMouseDownStatus(MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    FirstLeftClick = true;
                    OnLeftClick = true;
                    break;
                case MouseButtons.Right:
                    FirstRightClick = true;
                    OnRightClick = true;
                    break;
                case MouseButtons.Middle:
                    FirstWheelClick = true;
                    OnWheelClick = true;
                    break;
            }
        }

        public void SetMouseUpStatus(MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    FirstLeftClick = true;
                    OnLeftClick = false;
                    break;
                case MouseButtons.Right:
                    FirstRightClick = true;
                    OnRightClick = false;
                    break;
                case MouseButtons.Middle:
                    FirstWheelClick = true;
                    OnWheelClick = false;
                    break;
            }
        }

        public void SetWheelStatus(int delta)
        {
            if (delta > 0) WheelUp = true;
            else WheelDown = true;
        }

        // -------------------------------------------------------------------
        // Position
        // -------------------------------------------------------------------

        public Point GetPosition()
        {
            return MousePosition;
        }

        public void SetPosition(int x, int y)
        {
            MousePosition = new Point(x,y);
        }

        // -------------------------------------------------------------------
        // Update
        // -------------------------------------------------------------------

        public void Update()
        {
            FirstLeftClick = false;
            FirstRightClick = false;
            FirstWheelClick = false;
            WheelUp = false;
            WheelDown = false;
        }

        // -------------------------------------------------------------------
        // Is button
        // -------------------------------------------------------------------

        public bool IsButtonDown(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return OnLeftClick && FirstLeftClick;
                case MouseButtons.Right:
                    return OnRightClick && FirstRightClick;
                case MouseButtons.Middle:
                    return OnWheelClick && FirstWheelClick;
            }

            throw new Exception(button.ToString() + " is not managed.");
        }

        public bool IsButtonDownRepeat(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return OnLeftClick;
                case MouseButtons.Right:
                    return OnRightClick;
                case MouseButtons.Middle:
                    return OnWheelClick;
            }

            throw new Exception(button.ToString() + " is not managed.");
        }

        public bool IsButtonUp(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return !OnLeftClick && FirstLeftClick;
                case MouseButtons.Right:
                    return !OnRightClick && FirstRightClick;
                case MouseButtons.Middle:
                    return !OnWheelClick && FirstWheelClick;
            }

            throw new Exception(button.ToString() + " is not managed.");
        }

        public bool IsWheelDown()
        {
            return WheelDown;
        }

        public bool IsWheelUp()
        {
            return WheelUp;
        }
    }
}
