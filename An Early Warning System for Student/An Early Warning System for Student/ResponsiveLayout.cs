using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace An_Early_Warning_System_for_Student
{
    internal static class ResponsiveLayout
    {
        private sealed record FormState(Size BaseClientSize, bool ScaleFonts);

        private sealed record ControlState(Rectangle Bounds, float FontSize, FontStyle FontStyle);

        private static readonly ConditionalWeakTable<Form, FormState> _forms = new();
        private static readonly ConditionalWeakTable<Control, ControlState> _controls = new();

        public static void Attach(Form form, bool scaleFonts = true)
        {
            if (_forms.TryGetValue(form, out _))
                return;

            // Capture baseline from designer (before maximization / docking stretch).
            var baseSize = form.ClientSize;
            if (baseSize.Width <= 0 || baseSize.Height <= 0)
                baseSize = form.Size;

            _forms.Add(form, new FormState(baseSize, scaleFonts));

            CaptureControlTree(form);

            form.Resize += (_, _) => Apply(form);

            // Apply once (important for child forms docked into a container).
            form.Shown += (_, _) => Apply(form);
        }

        private static void CaptureControlTree(Control root)
        {
            CaptureControl(root);

            foreach (Control child in root.Controls)
                CaptureControlTree(child);
        }

        private static void CaptureControl(Control control)
        {
            if (_controls.TryGetValue(control, out _))
                return;

            float fontSize = control.Font?.Size ?? 9f;
            FontStyle fontStyle = control.Font?.Style ?? FontStyle.Regular;
            _controls.Add(control, new ControlState(control.Bounds, fontSize, fontStyle));
        }

        private static void Apply(Form form)
        {
            if (!_forms.TryGetValue(form, out var state))
                return;

            var current = form.ClientSize;
            if (current.Width <= 0 || current.Height <= 0)
                return;

            double scaleX = current.Width / (double)Math.Max(1, state.BaseClientSize.Width);
            double scaleY = current.Height / (double)Math.Max(1, state.BaseClientSize.Height);

            // Avoid doing work for 1:1 sizes.
            if (Math.Abs(scaleX - 1.0) < 0.001 && Math.Abs(scaleY - 1.0) < 0.001)
                return;

            form.SuspendLayout();
            try
            {
                ApplyToTree(form, scaleX, scaleY, state.ScaleFonts);
            }
            finally
            {
                form.ResumeLayout(true);
            }
        }

        private static void ApplyToTree(Control root, double scaleX, double scaleY, bool scaleFonts)
        {
            if (_controls.TryGetValue(root, out var baseline))
            {
                // If control is docked, let docking handle its bounds.
                if (root.Dock == DockStyle.None)
                {
                    root.Bounds = new Rectangle(
                        x: (int)Math.Round(baseline.Bounds.X * scaleX),
                        y: (int)Math.Round(baseline.Bounds.Y * scaleY),
                        width: (int)Math.Round(baseline.Bounds.Width * scaleX),
                        height: (int)Math.Round(baseline.Bounds.Height * scaleY)
                    );
                }

                if (scaleFonts && root.Font != null)
                {
                    float newSize = (float)(baseline.FontSize * Math.Min(scaleX, scaleY));
                    if (newSize < 6f) newSize = 6f;
                    if (newSize > 48f) newSize = 48f;

                    // Only recreate font if size changed meaningfully.
                    if (Math.Abs(root.Font.Size - newSize) > 0.25f)
                        root.Font = new Font(root.Font.FontFamily, newSize, baseline.FontStyle);
                }
            }

            foreach (Control child in root.Controls)
                ApplyToTree(child, scaleX, scaleY, scaleFonts);
        }
    }
}
