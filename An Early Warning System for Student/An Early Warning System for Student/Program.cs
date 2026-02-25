namespace An_Early_Warning_System_for_Student
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Make the entire desktop app fit different computer screen sizes.
            // This attaches scaling to every opened form and maximizes top-level forms.
            Application.Idle += (_, _) =>
            {
                try
                {
                    foreach (Form form in Application.OpenForms)
                    {
                        string name = form.GetType().Name;
                        bool isLoginOrOtp =
                            name == nameof(teacherAccess) ||
                            name == nameof(adminAccess) ||
                            name == nameof(OTPVerification) ||
                            name == nameof(AdminOTPVerification);

                        // Forms that use fixed, designer-positioned controls.
                        // ResponsiveLayout scaling can cause overlaps on these forms.
                        bool isFixedLayout =
                            isLoginOrOtp ||
                            name == nameof(Registration);

                        bool isShellForm =
                            name == nameof(Form1) ||
                            name == nameof(MainPage) ||
                            name == nameof(AdminDashboard) ||
                            name == nameof(Guidance);

                        // Don't scale login / OTP windows; keep their designed layout.
                        if (!isFixedLayout)
                            ResponsiveLayout.Attach(form);

                        // Only auto-maximize the main desktop shells.
                        // Login / OTP forms should keep their designed size.
                        if (form.TopLevel && form.WindowState == FormWindowState.Normal)
                        {
                            if (!isFixedLayout && isShellForm)
                                form.WindowState = FormWindowState.Maximized;
                        }

                        // If a form uses rounded-corner Region, remove it when maximized to avoid clipped edges.
                        if (form.TopLevel && form.WindowState == FormWindowState.Maximized && form.Region != null)
                            form.Region = null;
                    }
                }
                catch
                {
                    // Ignore UI timing edge cases.
                }
            };

            Application.Run(new Form1());
        }
    }
}