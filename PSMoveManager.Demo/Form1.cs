using System;
using System.Numerics;
using System.Text.RegularExpressions;
using PSMove;
using PSMove.EventArgs;
using static Math3D.Math3D;

namespace PSMoveManager.Demo
{
    public partial class Form1 : Form
    {
        private readonly int desiredConnectionCount = 5;
        private readonly int frameCut = 4;
        private readonly ToolStripMenuItem[] menuItems;
        private readonly PSMove.PSMoveManager manager = new();
        private readonly Cube cube = new(60, 120, 60);
        private PSMoveController? targetController;
        private int frame = 0;

        public Form1()
        {
            InitializeComponent();

            menuItems = new ToolStripMenuItem[]
            {
                controller1ToolStripMenuItem,
                controller2ToolStripMenuItem,
                controller3ToolStripMenuItem,
                controller4ToolStripMenuItem,
                controller5ToolStripMenuItem
            };

            Load += (sender, e) =>
            {
                SearchControllers();
                Start(menuItems[0], 0);
            };
            FormClosing += (sender, e) => manager.Dispose();
            exitToolStripMenuItem.Click += (sender, e) => Application.Exit();
            detectToolStripMenuItem.Click += (sender, e) => SearchControllers();
            controller1ToolStripMenuItem.Click += (sender, e) => Start(menuItems[0], 0);
            controller2ToolStripMenuItem.Click += (sender, e) => Start(menuItems[1], 1);
            controller3ToolStripMenuItem.Click += (sender, e) => Start(menuItems[2], 2);
            controller4ToolStripMenuItem.Click += (sender, e) => Start(menuItems[3], 3);
            controller5ToolStripMenuItem.Click += (sender, e) => Start(menuItems[4], 4);
            numericUpDown1.ValueChanged += (sender, e) => SetColor();
            numericUpDown2.ValueChanged += (sender, e) => SetColor();
            numericUpDown3.ValueChanged += (sender, e) => SetColor();
            button1.Click += (sender, e) => targetController?.SetLeds(label1.BackColor);
            button2.Click += (sender, e) => targetController?.SetVibration((byte)trackBar1.Value);
            button3.Click += (sender, e) => targetController?.ResetOrientation();
            button4.Click += (sender, e) => textBox1.Clear();

            void Start(ToolStripMenuItem menuItem, int index)
            {
                if (manager.Controllers.Count <= index)
                {
                    return;
                }

                StartController(manager.Controllers[index]);
                ChangeToolStripMenuItemChecked(menuItem);
                ShowStateMessage(string.Empty);
                ShowConnectionMessage(targetController?.ConnectionType, targetController?.IsConnected ?? false, targetController?.IsDataAvailable ?? false);
                ShowConnectionTypeMessage(targetController?.ConnectionType);
                ShowBatteryLevelMessage(targetController?.ConnectionType, targetController?.BatteryLevel);
                Update3d(null);
            }

            void SetColor()
            {
                var red = (int)numericUpDown1.Value;
                var green = (int)numericUpDown2.Value;
                var blue = (int)numericUpDown3.Value;

                label1.BackColor = Color.FromArgb(red, green, blue);
            }
        }

        private void ChangeToolStripMenuItemChecked(ToolStripMenuItem toolStripMenuItem)
        {
            foreach (var item in menuItems)
            {
                item.Checked = false;
            }

            toolStripMenuItem.Checked = true;
        }

        private void SearchControllers()
        {
            manager.Close();
            manager.Open(desiredConnectionCount);

            foreach (var item in menuItems)
            {
                item.Checked = false;
                item.Enabled = false;
            }
            for (int i = 0; i < manager.Controllers.Count; i++)
            {
                if (menuItems.Length > i)
                {
                    menuItems[i].Enabled = true;
                }
            }
        }

        private void StartController(PSMoveController controller)
        {
            if (controller == targetController)
            {
                return;
            }

            if (targetController != null)
            {
                targetController.Elapsed -= PSMoveController_Elapsed;
                targetController.ConnectionChanged -= PSMoveController_ConnectionChanged;
                targetController.ButtonDown -= PSMoveController_ButtonDown;
                targetController.ButtonUp -= PSMoveController_ButtonUp;
                targetController.BatteryLevelChanged -= PSMoveController_BatteryLevelChanged;
                targetController.Stop();
            }
            if (controller != null)
            {
                controller.Elapsed += PSMoveController_Elapsed;
                controller.ConnectionChanged += PSMoveController_ConnectionChanged;
                controller.ButtonDown += PSMoveController_ButtonDown;
                controller.ButtonUp += PSMoveController_ButtonUp;
                controller.BatteryLevelChanged += PSMoveController_BatteryLevelChanged;
                controller.Run();
            }

            targetController = controller;

            void PSMoveController_Elapsed(object? sender, PSMoveStateEventArgs e)
            {
                if (frame++ % frameCut != 0)
                {
                    return;
                }

                frame = 1;

                var message = string.Empty;
                var buttonFlags = Enum.GetNames(typeof(PSMoveButton)).Where(x => string.Compare(x, "trigger", StringComparison.OrdinalIgnoreCase) != 0);
                var buttonDownFlags = e.Buttons.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
                var newLine = Environment.NewLine;

                foreach (var item in buttonFlags)
                {
                    message += $"{ConvertToMark(item)} : {buttonDownFlags.Contains(item)}{newLine}";
                }

                message += $"Trigger : {e.Trigger}{newLine}";
                message += $"Temperature : {e.Temperature}℃{newLine}";

                ShowStateMessage(message);
                Update3d(e.EulerAngles);
            }
            void PSMoveController_ConnectionChanged(object? sender, PSMoveConnectionChangedEventArgs e)
            {
                ShowConnectionMessage(targetController?.ConnectionType, e.IsConnected, e.IsDataAvailable);
                ShowConnectionTypeMessage(e.ConnectionType);
            }
            void PSMoveController_ButtonDown(object? sender, PSMoveButtonEventArgs e)
            {
                ShowMessage(ConvertToMark(e.Buttons.ToString()) + " が " + nameof(PSMoveController.ButtonDown));
            }
            void PSMoveController_ButtonUp(object? sender, PSMoveButtonEventArgs e)
            {
                ShowMessage(ConvertToMark(e.Buttons.ToString()) + " が " + nameof(PSMoveController.ButtonUp));
            }
            void PSMoveController_BatteryLevelChanged(object? sender, PSMoveBatteryLevelChangedEventArgs e)
            {
                ShowBatteryLevelMessage(targetController?.ConnectionType, e.BatteryLevel);
            }
        }

        private void Update3d(Vector3? eulerAngles)
        {
            pictureBox1.Image?.Dispose();

            if (eulerAngles is not Vector3 v)
            {
                pictureBox1.Image = null;

                return;
            }

            cube.InitializeCube();
            cube.RotateX = v.X;
            cube.RotateY = -v.Z;
            cube.RotateZ = -v.Y;
            pictureBox1.Image = cube.DrawCube(new Point(pictureBox1.Width / 2, pictureBox1.Height / 2), Pens.Aquamarine);
        }

        private void ShowMessage(string message)
        {
            textBox1.Invoke(new Action(() => textBox1.AppendText(message + Environment.NewLine)));
        }

        private void ShowStateMessage(string message)
        {
            label6.Invoke(new Action(() => label6.Text = message));
        }

        private void ShowConnectionMessage(PSMoveConnectionType? connectionType, bool isConnected, bool isDataAvailable)
        {
            var message = isConnected ? (isDataAvailable || connectionType == PSMoveConnectionType.USB) ? "コントローラ接続中" : "コントローラ通信切断中" : "コントローラが接続されていません";

            statusStrip1.Invoke(new Action(() => toolStripStatusLabel1.Text = message));
        }

        private void ShowConnectionTypeMessage(PSMoveConnectionType? connectionType)
        {
            var message = (connectionType != null && connectionType != PSMoveConnectionType.Unknown) ? $"接続タイプ : {connectionType}" : string.Empty;

            statusStrip1.Invoke(new Action(() => toolStripStatusLabel2.Text = message));
        }

        private void ShowBatteryLevelMessage(PSMoveConnectionType? connectionType, PSMoveBatteryLevel? batteryLevel)
        {
            var message = connectionType == PSMoveConnectionType.Bluetooth ? $"残量 : {ConvertToPercent(batteryLevel.ToString())}" : string.Empty;

            statusStrip1.Invoke(new Action(() => toolStripStatusLabel3.Text = message));
        }

        private static string ConvertToMark(string source)
        {
            return source
                .Replace("triangle", "△", StringComparison.OrdinalIgnoreCase)
                .Replace("circle", "○", StringComparison.OrdinalIgnoreCase)
                .Replace("cross", "×", StringComparison.OrdinalIgnoreCase)
                .Replace("square", "□", StringComparison.OrdinalIgnoreCase);
        }

        private static string ConvertToPercent(string? source)
        {
            return Regex.Replace(source ?? string.Empty, @"percent(\d+)", "$1％", RegexOptions.IgnoreCase);
        }
    }
}