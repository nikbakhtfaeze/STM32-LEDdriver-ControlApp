using System.Drawing.Drawing2D;

namespace LEDdriverControlApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // siOPTICA-inspired dark instrument palette.
        private static readonly Color BrandTeal = Color.FromArgb(0x1F, 0xB5, 0xB5);
        private static readonly Color BrandTealHover = Color.FromArgb(0x35, 0xC7, 0xC7);
        private static readonly Color BrandTealPressed = Color.FromArgb(0x17, 0x8F, 0x8F);
        private static readonly Color BrandBackground = Color.FromArgb(0x0F, 0x14, 0x17);
        private static readonly Color BrandHeader = Color.FromArgb(0x13, 0x1A, 0x1E);
        private static readonly Color BrandCard = Color.FromArgb(0x19, 0x22, 0x26);
        private static readonly Color BrandInset = Color.FromArgb(0x12, 0x1B, 0x1F);
        private static readonly Color BrandInputBg = Color.FromArgb(0x22, 0x2D, 0x31);
        private static readonly Color BrandTextPrimary = Color.FromArgb(0xF1, 0xF6, 0xF6);
        private static readonly Color BrandTextSecondary = Color.FromArgb(0xBF, 0xCB, 0xCC);
        private static readonly Color BrandTextMuted = Color.FromArgb(0x7F, 0x96, 0x98);
        private static readonly Color BrandDivider = Color.FromArgb(0x2A, 0x39, 0x3E);
        private static readonly Color BrandSuccess = Color.FromArgb(0x5C, 0xD6, 0x91);
        private static readonly Color BrandToggleOffBg = Color.FromArgb(0x30, 0x3B, 0x3F);
        private static readonly Color BrandOnAccentText = Color.FromArgb(0x0B, 0x16, 0x18);

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private static Image? LoadEmbeddedImage(string fileName)
        {
            var asm = typeof(Form1).Assembly;
            string? resName = Array.Find(asm.GetManifestResourceNames(),
                n => n.EndsWith(fileName, StringComparison.OrdinalIgnoreCase));
            if (resName == null)
                return null;

            using var s = asm.GetManifestResourceStream(resName);
            if (s == null)
                return null;

            using var source = Image.FromStream(s);
            return new Bitmap(source);
        }

        private static Icon? LoadEmbeddedIcon(string fileName)
        {
            var asm = typeof(Form1).Assembly;
            string? resName = Array.Find(asm.GetManifestResourceNames(),
                n => n.EndsWith(fileName, StringComparison.OrdinalIgnoreCase));
            if (resName == null)
                return null;

            using var s = asm.GetManifestResourceStream(resName);
            if (s == null)
                return null;

            using var source = new Icon(s);
            return (Icon)source.Clone();
        }

        private static void ApplyRoundedRegion(Control control, int radius)
        {
            void Apply()
            {
                if (control.Width <= 0 || control.Height <= 0)
                    return;

                var rect = new Rectangle(0, 0, control.Width, control.Height);
                int diameter = Math.Min(radius * 2, Math.Min(rect.Width, rect.Height));
                if (diameter <= 0)
                    return;

                using var path = new GraphicsPath();
                path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
                path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
                path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
                path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
                path.CloseFigure();
                control.Region = new Region(path);
            }

            Apply();
            control.Resize += (_, _) => Apply();
        }

        private static void StylePrimaryButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = BrandTealHover;
            button.FlatAppearance.MouseDownBackColor = BrandTealPressed;
            button.BackColor = BrandTeal;
            button.ForeColor = BrandOnAccentText;
            button.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;
            ApplyRoundedRegion(button, 8);
        }

        private static void StyleSecondaryButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = BrandDivider;
            button.FlatAppearance.MouseOverBackColor = BrandInputBg;
            button.FlatAppearance.MouseDownBackColor = BrandDivider;
            button.BackColor = BrandCard;
            button.ForeColor = BrandTextPrimary;
            button.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;
            ApplyRoundedRegion(button, 8);
        }

        private static void StyleInput(TextBox textBox, bool readOnly = false)
        {
            textBox.BackColor = BrandInputBg;
            textBox.ForeColor = readOnly ? BrandTextSecondary : BrandTextPrimary;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Regular);
            textBox.ReadOnly = readOnly;
        }

        private static void StyleSegmentButton(Button button)
        {
            button.AutoSize = false;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.TextAlign = ContentAlignment.MiddleCenter;
            button.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;
            ApplyRoundedRegion(button, 8);
        }

        // Drives the visible "active" segment of a Computer / External POT
        // pair -- the actual mode state lives in a headless CheckBox
        // (see ConfigureChannelCard), this only controls appearance.
        internal static void SetSegmentActive(Button button, bool active)
        {
            if (active)
            {
                button.BackColor = BrandTeal;
                button.ForeColor = BrandOnAccentText;
                button.FlatAppearance.MouseOverBackColor = BrandTealHover;
            }
            else
            {
                button.BackColor = BrandToggleOffBg;
                button.ForeColor = BrandTextSecondary;
                button.FlatAppearance.MouseOverBackColor = BrandDivider;
            }
        }

        private static Label MakeCaption(string text, int x, int y)
        {
            return new Label
            {
                AutoSize = true,
                Text = text.ToUpperInvariant(),
                Location = new Point(x, y),
                Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold),
                ForeColor = BrandTextMuted,
                BackColor = Color.Transparent
            };
        }

        private static Label MakeChannelTitle(string text, int x, int y)
        {
            return new Label
            {
                AutoSize = true,
                Text = text,
                Location = new Point(x, y),
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = BrandTextPrimary,
                BackColor = Color.Transparent
            };
        }

        private static Panel MakeDivider(int x, int y, int width)
        {
            return new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, 1),
                BackColor = BrandDivider
            };
        }

        private static Panel MakeMetricPanel(int x, int y, int width, int height)
        {
            var panel = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = BrandInset
            };
            ApplyRoundedRegion(panel, 10);
            return panel;
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            cmbPorts = new ComboBox();
            btnRefreshPorts = new Button();
            btnConnect = new Button();
            btnLedToggle = new Button();
            lblConnectionStatus = new Label();
            pnlStatusDot = new Panel();
            pnlStatusGroup = new FlowLayoutPanel();
            pnlHeader = new Panel();
            picLogo = new PictureBox();
            lblAppTitle = new Label();
            lblAppSubtitle = new Label();
            pnlConnection = new CardPanel();
            grpChannel1 = new CardPanel();
            grpChannel2 = new CardPanel();
            lblVout = new Label();
            lblIout = new Label();
            lblVout2 = new Label();
            lblIout2 = new Label();
            trackBarDAC = new TrackBar();
            trackBarDAC2 = new TrackBar();
            lblDACValue = new TextBox();
            lblDACValue2 = new TextBox();
            chkPotMode = new CheckBox();
            chkPotMode2 = new CheckBox();
            btnComputer1 = new Button();
            btnPot1 = new Button();
            lblControlDesc1 = new Label();
            btnComputer2 = new Button();
            btnPot2 = new Button();
            lblControlDesc2 = new Label();
            txtMaxmA1 = new TextBox();
            txtMaxmA2 = new TextBox();
            btnSetMax1 = new Button();
            btnSetMax2 = new Button();
            lblFooter = new Label();
            toolTip = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            pnlHeader.SuspendLayout();
            pnlConnection.SuspendLayout();
            grpChannel1.SuspendLayout();
            grpChannel2.SuspendLayout();
            SuspendLayout();

            // Form
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = BrandBackground;
            ClientSize = new Size(1010, 730);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            ForeColor = BrandTextPrimary;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimumSize = new Size(1028, 777);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "siOPTICA LED Driver Control";
            Icon = LoadEmbeddedIcon("app.ico");
            Load += Form1_Load;
            FormClosed += Form1_FormClosed;

            // Header
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 92;
            pnlHeader.BackColor = BrandHeader;
            pnlHeader.Padding = new Padding(24, 0, 24, 0);

            picLogo.Location = new Point(26, 27);
            picLogo.Size = new Size(190, 35);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.Image = LoadEmbeddedImage("siOPTICA_logo.png");
            picLogo.BackColor = Color.Transparent;
            picLogo.TabStop = false;

            lblAppTitle.AutoSize = true;
            lblAppTitle.Location = new Point(245, 22);
            lblAppTitle.Text = "LED Driver Control";
            lblAppTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblAppTitle.ForeColor = BrandTextPrimary;
            lblAppTitle.BackColor = Color.Transparent;

            lblAppSubtitle.AutoSize = true;
            lblAppSubtitle.Location = new Point(247, 57);
            lblAppSubtitle.Text = "Dual-channel output controller";
            lblAppSubtitle.Font = new Font("Segoe UI", 9F);
            lblAppSubtitle.ForeColor = BrandTextMuted;
            lblAppSubtitle.BackColor = Color.Transparent;

            pnlStatusDot.Size = new Size(10, 10);
            pnlStatusDot.Margin = new Padding(8, 8, 0, 0);
            pnlStatusDot.BackColor = BrandTextMuted;
            ApplyRoundedRegion(pnlStatusDot, 5);

            lblConnectionStatus.AutoSize = true;
            lblConnectionStatus.Margin = new Padding(0, 5, 0, 0);
            lblConnectionStatus.Text = "Disconnected";
            lblConnectionStatus.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblConnectionStatus.ForeColor = BrandTextSecondary;
            lblConnectionStatus.BackColor = Color.Transparent;

            // A fixed-width, right-anchored, RightToLeft-flowing panel keeps
            // "* COM3  .  Connected" fully clear of the window edge no
            // matter how long the text gets (COM12, longer port names,
            // different DPI/font metrics) -- unlike a fixed absolute X
            // position, which clips as soon as the text grows past it.
            pnlStatusGroup.FlowDirection = FlowDirection.RightToLeft;
            pnlStatusGroup.WrapContents = false;
            pnlStatusGroup.AutoSize = false;
            pnlStatusGroup.Size = new Size(260, 26);
            pnlStatusGroup.Location = new Point(1010 - 24 - 260, 27);
            pnlStatusGroup.BackColor = Color.Transparent;
            pnlStatusGroup.Margin = new Padding(0);
            pnlStatusGroup.Padding = new Padding(0);
            pnlStatusGroup.Controls.Add(lblConnectionStatus); // added first = rightmost
            pnlStatusGroup.Controls.Add(pnlStatusDot);        // sits to its left

            pnlHeader.Controls.Add(picLogo);
            pnlHeader.Controls.Add(lblAppTitle);
            pnlHeader.Controls.Add(lblAppSubtitle);
            pnlHeader.Controls.Add(pnlStatusGroup);

            // Connection card
            pnlConnection.Location = new Point(24, 112);
            pnlConnection.Size = new Size(962, 112);
            pnlConnection.BackColor = BrandCard;
            pnlConnection.BorderColor = BrandDivider;
            pnlConnection.BorderThickness = 1;
            pnlConnection.CornerRadius = 14;

            var lblConnectionTitle = MakeChannelTitle("Device Connection", 22, 15);
            var lblPortCaption = MakeCaption("COM Port", 0, 0);
            lblPortCaption.Margin = new Padding(0, 14, 0, 0);

            // A TableLayoutPanel keeps each control in its own fixed-width
            // cell, so the label can never overlap the ComboBox (or the
            // ComboBox the buttons) regardless of font/DPI scaling -- unlike
            // absolute Point positioning, where a wider-than-expected label
            // glyph run silently overlaps whatever comes next.
            var tblConnection = new TableLayoutPanel
            {
                Location = new Point(22, 46),
                Size = new Size(900, 44),
                ColumnCount = 6,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            tblConnection.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));  // COM PORT label
            tblConnection.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 112));  // ComboBox
            tblConnection.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 102));  // Refresh
            tblConnection.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 142));  // Connect/Disconnect
            tblConnection.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F)); // spacer -- pushes LED Toggle to the right edge
            tblConnection.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 158));  // LED Toggle
            tblConnection.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));

            // Compact and fixed-width (no Left|Right stretch) so it stays
            // sized for "COM3".."COM15" instead of ballooning to look like
            // the whole connection row.
            cmbPorts.Size = new Size(100, 31);
            cmbPorts.Margin = new Padding(0, 6, 0, 0);
            cmbPorts.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPorts.FlatStyle = FlatStyle.Flat;
            cmbPorts.BackColor = BrandInputBg;
            cmbPorts.ForeColor = BrandTextPrimary;
            cmbPorts.Font = new Font("Segoe UI", 9.5F);

            btnRefreshPorts.Size = new Size(90, 36);
            btnRefreshPorts.Margin = new Padding(0, 4, 0, 0);
            btnRefreshPorts.Text = "Refresh";
            btnRefreshPorts.Click += btnRefreshPorts_Click;
            StyleSecondaryButton(btnRefreshPorts);

            btnConnect.Size = new Size(130, 36);
            btnConnect.Margin = new Padding(0, 4, 0, 0);
            btnConnect.Text = "Connect";
            btnConnect.Click += btnConnect_Click;
            StylePrimaryButton(btnConnect);

            btnLedToggle.Size = new Size(150, 40);
            btnLedToggle.Margin = new Padding(0, 2, 0, 0);
            btnLedToggle.Text = "LED Toggle";
            btnLedToggle.Enabled = false;
            btnLedToggle.Click += btnLedToggle_Click;
            StyleSecondaryButton(btnLedToggle);
            toolTip.SetToolTip(btnLedToggle, "Toggle the LED output to verify communication with the device.");

            tblConnection.Controls.Add(lblPortCaption, 0, 0);
            tblConnection.Controls.Add(cmbPorts, 1, 0);
            tblConnection.Controls.Add(btnRefreshPorts, 2, 0);
            tblConnection.Controls.Add(btnConnect, 3, 0);
            // Column 4 is the percent-width spacer; LED Toggle lives in 5.
            tblConnection.Controls.Add(btnLedToggle, 5, 0);

            pnlConnection.Controls.Add(lblConnectionTitle);
            pnlConnection.Controls.Add(tblConnection);

            // Channel cards
            ConfigureChannelCard(
                grpChannel1,
                "Output 1",
                new Point(24, 244),
                lblVout,
                lblIout,
                trackBarDAC,
                lblDACValue,
                chkPotMode,
                btnComputer1,
                btnPot1,
                lblControlDesc1,
                1,
                txtMaxmA1,
                btnSetMax1,
                trackBarDAC_Scroll,
                checkBox1_CheckedChanged,
                btnSetMax1_Click);

            ConfigureChannelCard(
                grpChannel2,
                "Output 2",
                new Point(514, 244),
                lblVout2,
                lblIout2,
                trackBarDAC2,
                lblDACValue2,
                chkPotMode2,
                btnComputer2,
                btnPot2,
                lblControlDesc2,
                2,
                txtMaxmA2,
                btnSetMax2,
                trackBarDAC2_Scroll,
                chkPotMode2_CheckedChanged,
                btnSetMax2_Click_1);

            lblFooter.AutoSize = true;
            lblFooter.Location = new Point(25, 691);
            lblFooter.Text = "115200 baud  •  Measurements poll every 250 ms while connected";
            lblFooter.Font = new Font("Segoe UI", 8.5F);
            lblFooter.ForeColor = BrandTextMuted;
            lblFooter.BackColor = Color.Transparent;

            Controls.Add(pnlHeader);
            Controls.Add(pnlConnection);
            Controls.Add(grpChannel1);
            Controls.Add(grpChannel2);
            Controls.Add(lblFooter);

            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlConnection.ResumeLayout(false);
            pnlConnection.PerformLayout();
            grpChannel1.ResumeLayout(false);
            grpChannel1.PerformLayout();
            grpChannel2.ResumeLayout(false);
            grpChannel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private void ConfigureChannelCard(
            CardPanel card,
            string title,
            Point location,
            Label voltageValue,
            Label currentValue,
            TrackBar trackBar,
            TextBox percentValue,
            CheckBox modeState,
            Button computerButton,
            Button potButton,
            Label sourceDescription,
            int potNumber,
            TextBox maxCurrent,
            Button applyButton,
            EventHandler trackHandler,
            EventHandler modeHandler,
            EventHandler applyHandler)
        {
            card.Location = location;
            card.Size = new Size(472, 420);
            card.BackColor = BrandCard;
            card.BorderColor = BrandDivider;
            card.BorderThickness = 1;
            card.CornerRadius = 14;

            var titleLabel = MakeChannelTitle(title, 22, 17);
            var outputBadge = new Label
            {
                AutoSize = false,
                Location = new Point(376, 18),
                Size = new Size(72, 27),
                Text = "OUTPUT",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Semibold", 7.5F, FontStyle.Bold),
                ForeColor = BrandTeal,
                BackColor = BrandInset
            };
            ApplyRoundedRegion(outputBadge, 8);

            var voltagePanel = MakeMetricPanel(22, 60, 204, 74);
            var currentPanel = MakeMetricPanel(244, 60, 204, 74);
            var voltageCaption = MakeCaption("Voltage", 15, 11);
            var currentCaption = MakeCaption("Current", 15, 11);

            voltageValue.AutoSize = true;
            voltageValue.Location = new Point(15, 32);
            voltageValue.Text = "— V";
            voltageValue.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold);
            voltageValue.ForeColor = BrandTextPrimary;
            voltageValue.BackColor = Color.Transparent;

            currentValue.AutoSize = true;
            currentValue.Location = new Point(15, 32);
            currentValue.Text = "— mA";
            currentValue.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold);
            currentValue.ForeColor = BrandTextPrimary;
            currentValue.BackColor = Color.Transparent;

            voltagePanel.Controls.Add(voltageCaption);
            voltagePanel.Controls.Add(voltageValue);
            currentPanel.Controls.Add(currentCaption);
            currentPanel.Controls.Add(currentValue);

            var outputCaption = MakeCaption("PC Output Level", 22, 151);
            trackBar.Location = new Point(18, 174);
            trackBar.Size = new Size(340, 45);
            trackBar.Minimum = 0;
            trackBar.Maximum = 100;
            trackBar.TickFrequency = 10;
            trackBar.SmallChange = 1;
            trackBar.LargeChange = 5;
            trackBar.TickStyle = TickStyle.None;
            trackBar.Scroll += trackHandler;

            percentValue.Location = new Point(372, 177);
            percentValue.Size = new Size(76, 30);
            percentValue.Text = "0%";
            percentValue.TextAlign = HorizontalAlignment.Center;
            StyleInput(percentValue, readOnly: true);

            card.Controls.Add(titleLabel);
            card.Controls.Add(outputBadge);
            card.Controls.Add(voltagePanel);
            card.Controls.Add(currentPanel);
            card.Controls.Add(outputCaption);
            card.Controls.Add(trackBar);
            card.Controls.Add(percentValue);
            card.Controls.Add(MakeDivider(22, 226, 426));

            // modeState is a headless state holder (never added to the
            // visual tree) -- it exists only so the existing MODE1/MODE2
            // CheckedChanged wiring in Form1.cs keeps working unmodified.
            // The Computer / External POT buttons below just flip it.
            modeState.Checked = true; // default: External POT, until the user chooses otherwise
            modeState.CheckedChanged += modeHandler;

            var sourceCaption = MakeCaption("Control Source", 22, 245);

            StyleSegmentButton(computerButton);
            computerButton.Text = "Computer";
            computerButton.Location = new Point(22, 268);
            computerButton.Size = new Size(116, 36);
            computerButton.Click += (s, e) => modeState.Checked = false;

            StyleSegmentButton(potButton);
            potButton.Text = "External POT";
            potButton.Location = new Point(146, 268);
            potButton.Size = new Size(168, 36);
            potButton.Click += (s, e) => modeState.Checked = true;

            sourceDescription.AutoSize = true;
            sourceDescription.Location = new Point(22, 310);
            sourceDescription.Font = new Font("Segoe UI", 8.5F);
            sourceDescription.ForeColor = BrandTextSecondary;
            sourceDescription.BackColor = Color.Transparent;
            sourceDescription.Text = $"Controlled by potentiometer connected to POT{potNumber}";

            var maxCaption = MakeCaption("Maximum Current", 22, 344);
            maxCurrent.Location = new Point(22, 368);
            maxCurrent.Size = new Size(100, 32);
            maxCurrent.TextAlign = HorizontalAlignment.Center;
            StyleInput(maxCurrent);

            var unitLabel = new Label
            {
                AutoSize = true,
                Location = new Point(130, 378),
                Text = "mA",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = BrandTextSecondary,
                BackColor = Color.Transparent
            };

            applyButton.Location = new Point(180, 364);
            applyButton.Size = new Size(140, 40);
            applyButton.Text = "Apply Limit";
            applyButton.Click += applyHandler;
            StylePrimaryButton(applyButton);

            card.Controls.Add(sourceCaption);
            card.Controls.Add(computerButton);
            card.Controls.Add(potButton);
            card.Controls.Add(sourceDescription);
            card.Controls.Add(maxCaption);
            card.Controls.Add(maxCurrent);
            card.Controls.Add(unitLabel);
            card.Controls.Add(applyButton);
        }

        #endregion

        private ComboBox cmbPorts;
        private Button btnRefreshPorts;
        private Button btnConnect;
        private Button btnLedToggle;
        private Label lblConnectionStatus;
        private Panel pnlStatusDot;
        private FlowLayoutPanel pnlStatusGroup;
        private Panel pnlHeader;
        private PictureBox picLogo;
        private Label lblAppTitle;
        private Label lblAppSubtitle;
        private CardPanel pnlConnection;
        private CardPanel grpChannel1;
        private CardPanel grpChannel2;
        private Label lblVout;
        private Label lblIout;
        private Label lblVout2;
        private Label lblIout2;
        private TrackBar trackBarDAC;
        private TrackBar trackBarDAC2;
        private TextBox lblDACValue;
        private TextBox lblDACValue2;
        private CheckBox chkPotMode;
        private CheckBox chkPotMode2;
        private Button btnComputer1;
        private Button btnPot1;
        private Label lblControlDesc1;
        private Button btnComputer2;
        private Button btnPot2;
        private Label lblControlDesc2;
        private TextBox txtMaxmA1;
        private TextBox txtMaxmA2;
        private Button btnSetMax1;
        private Button btnSetMax2;
        private Label lblFooter;
        private ToolTip toolTip;
    }
}
