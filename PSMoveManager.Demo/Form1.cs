using System;
using System.Text.RegularExpressions;
using PSMove;
using PSMove.EventArgs;

namespace PSMoveManager.Demo
{
    public partial class Form1 : Form
    {
        private readonly int desiredConnectionCount = 5;
        private readonly int frameCut = 10;
        private readonly ToolStripMenuItem[] menuItems;
        private readonly PSMove.PSMoveManager manager = new();
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
            button3.Click += (sender, e) => textBox1.Clear();

            void Start(ToolStripMenuItem menuItem, int index)
            {
                if (manager.Controllers.Count <= index)
                {
                    return;
                }

                StartController(manager.Controllers[index]);
                ChangeToolStripMenuItemChecked(menuItem);
                ShowConnectionMessage(targetController?.IsConnected ?? false, true);
                ShowConnectionTypeMessage(targetController?.ConnectionType ?? PSMoveConnectionType.Unknown);
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
                var buttonFlags = Enum.GetNames(typeof(PSMoveButton));
                var buttonDownFlags = e.Buttons.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
                var newLine = Environment.NewLine;

                foreach (var item in buttonFlags)
                {
                    message += $"{ConvertToMark(item)} : {buttonDownFlags.Contains(item)}{newLine}";
                }

                message += newLine;
                message += $"TriggerValue : {e.Trigger}{newLine}";
                message += $"Temperature : {e.Temperature}℃{newLine}{newLine}";

                message += $"X : {e.EulerAngles.X:F2}{newLine}";
                message += $"Y : {e.EulerAngles.Y:F2}{newLine}";
                message += $"Z : {e.EulerAngles.Z:F2}{newLine}";

                ShowStateMessage(message);
            }
            void PSMoveController_ConnectionChanged(object? sender, PSMoveConnectionChangedEventArgs e)
            {
                ShowConnectionMessage(e.IsConnected, e.IsDataAvailable);
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
                ShowBatteryLevelMessage(e.BatteryLevel);
            }
        }

        private void ShowMessage(string message)
        {
            textBox1.Invoke(new Action(() => textBox1.AppendText(message + Environment.NewLine)));
        }

        private void ShowStateMessage(string message)
        {
            textBox2.Invoke(new Action(() => textBox2.Text = message));
        }

        private void ShowConnectionMessage(bool isConnected, bool isDataAvailable)
        {
            var message = isConnected ? isDataAvailable ? "コントローラ接続中" : "データが取得できません" : "コントローラが接続されていません";

            statusStrip1.Invoke(new Action(() => toolStripStatusLabel1.Text = message));
        }

        private void ShowConnectionTypeMessage(PSMoveConnectionType connectionType)
        {
            statusStrip1.Invoke(new Action(() => toolStripStatusLabel2.Text = $"接続タイプ : {connectionType}"));
        }

        private void ShowBatteryLevelMessage(PSMoveBatteryLevel batteryLevel)
        {
            statusStrip1.Invoke(new Action(() => toolStripStatusLabel3.Text = $"残量 : {ConvertToPercent(batteryLevel.ToString())}"));
        }

        private static string ConvertToMark(string source)
        {
            return source
                .Replace("triangle", "△", StringComparison.OrdinalIgnoreCase)
                .Replace("circle", "○", StringComparison.OrdinalIgnoreCase)
                .Replace("cross", "×", StringComparison.OrdinalIgnoreCase)
                .Replace("square", "□", StringComparison.OrdinalIgnoreCase);
        }

        private static string ConvertToPercent(string source)
        {
            return Regex.Replace(source, @"percent(\d+)", "$1％", RegexOptions.IgnoreCase);
        }
    }
}