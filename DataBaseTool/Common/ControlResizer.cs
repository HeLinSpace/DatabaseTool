using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;

namespace DataBaseTool.Common
{
    class ControlResizeEventArgs : EventArgs
    {
        public double HorizontalChange { get; private set; }
        public double VerticalChange { get; private set; }
        public bool? LeftDirection { get; private set; }
        public bool? TopDirection { get; private set; }

        public override string ToString()
        {
            return String.Format("{0}({1}) {2}({3})", LeftDirection, HorizontalChange, TopDirection, VerticalChange);
        }

        public ControlResizeEventArgs(double hori, double verti, bool? lefdir, bool? topdir)
        {
            HorizontalChange = hori;
            VerticalChange = verti;
            LeftDirection = lefdir;
            TopDirection = topdir;
        }
    }
    class ControlResizer
    {
        FrameworkElement _control;
        bool _pressed;
        Point _prevPoint;
        public Thickness Thickness { get; private set; }
        public double Radius { get; private set; }
        public Cursor DefaultCursor { get; private set; }

        public bool? LeftDirection { get; private set; }
        public bool? TopDirection { get; private set; }

        public ControlResizer(FrameworkElement control, Thickness thickness, double radius, Cursor defCursor)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            Thickness = thickness;
            Radius = radius;
            DefaultCursor = defCursor;
            _control = control;
            _control.MouseEnter += _control_MouseEnter;
            _control.MouseMove += _control_MouseMove;
            _control.MouseDown += _control_MouseDown;
            _control.MouseUp += _control_MouseUp;
        }

        public event EventHandler<ControlResizeEventArgs> Resize;

        protected virtual void OnResize(ControlResizeEventArgs e)
        {
            var handler = this.Resize;
            if (handler != null)
                handler(this, e);
        }

        void _control_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _pressed = false;
            _control.ReleaseMouseCapture();
        }

        void _control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(_control);
            _pressed = true;
            _control.CaptureMouse();
            _prevPoint = _control.PointToScreen(point);
        }

        void _control_MouseMove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(_control);
            if (!_pressed)
            {
                _SetCursor(point);
            }
            else
            {
                double vertiChange, horiChange;
                vertiChange = horiChange = 0;
                var pointScr = _control.PointToScreen(point);
                if (LeftDirection.HasValue)
                {
                    horiChange = pointScr.X - _prevPoint.X;
                    if (LeftDirection.Value)
                        horiChange *= -1;
                }
                if (TopDirection.HasValue)
                {
                    vertiChange = pointScr.Y - _prevPoint.Y;
                    if (TopDirection.Value)
                        vertiChange *= -1;
                }
                OnResize(new ControlResizeEventArgs(horiChange, vertiChange, LeftDirection, TopDirection));
                _prevPoint = pointScr;
            }
        }

        void _control_MouseEnter(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(_control);
            if (!_pressed)
            {
                _SetCursor(point);
            }
        }

        void _SetCursor(Point point)
        {
            var left = point.X;
            var top = point.Y;
            var right = _control.ActualWidth - left;
            var bottom = _control.ActualHeight - top;

            LeftDirection = TopDirection = null;
            if (left < Thickness.Left)
                LeftDirection = true;
            else if (right < Thickness.Right)
                LeftDirection = false;

            if (top < Thickness.Top)
                TopDirection = true;
            else if (bottom < Thickness.Bottom)
                TopDirection = false;


            Cursor cur = null;
            if (LeftDirection.HasValue)
            {
                //如果上下没有进入区域，尝试按照Radius属性再次计算
                if (!TopDirection.HasValue)
                {
                    if (top < Radius)
                        TopDirection = true;
                    else if (bottom < Radius)
                        TopDirection = false;
                }

                if (TopDirection.HasValue)
                {
                    if (LeftDirection.Value == TopDirection.Value)
                        cur = Cursors.SizeNWSE;
                    else
                        cur = Cursors.SizeNESW;
                }
                else
                    cur = Cursors.SizeWE;
            }
            else if (TopDirection.HasValue)
            {
                //这里leftDirection.HasValue必定是false，所以直接计算
                if (left < Radius)
                    LeftDirection = true;
                else if (right < Radius)
                    LeftDirection = false;

                if (LeftDirection.HasValue)
                {
                    if (LeftDirection.Value == TopDirection.Value)
                        cur = Cursors.SizeNWSE;
                    else
                        cur = Cursors.SizeNESW;
                }
                else
                    cur = Cursors.SizeNS;
            }
            if (cur != null)
                _control.Cursor = cur;
            else
                _control.Cursor = DefaultCursor;
        }
    }
}
