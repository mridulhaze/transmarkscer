using System;
using System.Drawing;
using System.Windows.Forms;

namespace NU_Solver
{
    public static class UITheme
    {
        // Lite Green and White-Orange Palette
        public static readonly Color Primary         = Color.FromArgb(16, 149, 93);   // #10955D Lite Green / Emerald
        public static readonly Color PrimaryDark     = Color.FromArgb(10, 110, 68);   // #0A6E44 Deep Green
        public static readonly Color PrimaryHover    = Color.FromArgb(22, 175, 110);  // #16AF6E Green Hover
        public static readonly Color PrimaryLight    = Color.FromArgb(236, 253, 245); // #ECFDF5 Mint Tint
        public static readonly Color Secondary       = Color.FromArgb(249, 115, 22);  // #F97316 Warm Orange
        public static readonly Color SecondaryDark   = Color.FromArgb(217, 83, 7);    // #D95307 Deep Orange
        public static readonly Color SecondaryHover  = Color.FromArgb(251, 146, 60);  // #FB923C Orange Hover
        public static readonly Color SecondaryLight  = Color.FromArgb(255, 247, 237); // #FFF7ED Peach Tint
        public static readonly Color Accent          = Color.FromArgb(245, 158, 11);  // #F59E0B Amber / Orange Gold
        public static readonly Color AppBg           = Color.FromArgb(247, 250, 247); // #F7FAF7 Crisp Natural White-Green
        public static readonly Color SurfaceCard     = Color.FromArgb(255, 255, 255); // #FFFFFF Pure White
        public static readonly Color TextPrimary     = Color.FromArgb(26, 46, 34);    // #1A2E22 Deep Slate Forest
        public static readonly Color TextSecondary   = Color.FromArgb(88, 112, 98);   // #587062 Secondary Forest Slate
        public static readonly Color BorderDivider   = Color.FromArgb(226, 235, 228); // #E2EBE4 Clean Mint Slate Border
        public static readonly Color Success         = Color.FromArgb(16, 149, 93);   // #10955D Success Green
        public static readonly Color Warning         = Color.FromArgb(249, 115, 22);  // #F97316 Warning Orange
        public static readonly Color Error           = Color.FromArgb(220, 38, 38);   // #DC2626 Coral Red

        // Backward compatibility mappings for legacy forms
        public static readonly Color GreenPrimary    = Primary;
        public static readonly Color GreenHover      = PrimaryHover;
        public static readonly Color GreenPressed    = PrimaryDark;
        public static readonly Color GreenHeader     = PrimaryDark;
        public static readonly Color GreenLightTint  = PrimaryLight;
        public static readonly Color LightBg         = AppBg;
        public static readonly Color LightSurface    = SurfaceCard;
        public static readonly Color LightControl    = SurfaceCard;
        public static readonly Color BorderColor     = BorderDivider;
        public static readonly Color TextBlack       = TextPrimary;
        public static readonly Color TextMuted       = TextSecondary;
        public static readonly Color ErrorRed        = Error;

        // Fonts
        public static readonly Font FontMain         = new Font("Segoe UI", 9.5f, FontStyle.Regular);
        public static readonly Font FontBold         = new Font("Segoe UI", 10f, FontStyle.Bold);
        public static readonly Font FontHeader       = new Font("Segoe UI", 11.5f, FontStyle.Bold);
        public static readonly Font FontGrid         = new Font("Consolas", 10.5f, FontStyle.Regular);

        public static Image GetEmbeddedOrFileImage(string fileName)
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string path1 = System.IO.Path.Combine(baseDir, "static", fileName);
                string path2 = System.IO.Path.Combine(baseDir, "..", "..", "static", fileName);
                string path3 = System.IO.Path.Combine(@"E:\projects\NU H-Type Solver_code_5_for_database_solving\static", fileName);

                if (System.IO.File.Exists(path1)) return Image.FromFile(path1);
                if (System.IO.File.Exists(path2)) return Image.FromFile(path2);
                if (System.IO.File.Exists(path3)) return Image.FromFile(path3);

                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                foreach (string name in assembly.GetManifestResourceNames())
                {
                    if (name.EndsWith(fileName, StringComparison.OrdinalIgnoreCase))
                    {
                        using (var stream = assembly.GetManifestResourceStream(name))
                        {
                            if (stream != null)
                            {
                                return Image.FromStream(stream);
                            }
                        }
                    }
                }
            }
            catch { }
            return null;
        }

        public static void ApplyTheme(Form form, string titleText = "")
        {
            if (form == null) return;

            form.BackColor = LightBg;
            form.ForeColor = TextBlack;
            form.Font = FontMain;

            // Add Header Banner
            AddHeaderBanner(form, string.IsNullOrEmpty(titleText) ? form.Text : titleText);

            // Apply recursively to controls
            StyleControls(form.Controls);
        }

        private static void AddHeaderBanner(Form form, string title)
        {
            if (form is login) return;

            foreach (Control c in form.Controls)
            {
                if (c.Name == "pnlHeaderBanner") return;
            }

            Panel headerPanel = new Panel
            {
                Name = "pnlHeaderBanner",
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = GreenHeader,
                Padding = new Padding(12, 0, 12, 0)
            };

            Label lblLogoIcon = new Label
            {
                Text = "❇ NU SOLVER",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(12, 11)
            };

            Label lblSubTitle = new Label
            {
                Text = "| " + title,
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                ForeColor = Color.FromArgb(220, 252, 231),
                AutoSize = true,
                Location = new Point(135, 12)
            };

            headerPanel.Controls.Add(lblLogoIcon);
            headerPanel.Controls.Add(lblSubTitle);

            Panel accentLine = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 3,
                BackColor = GreenHover
            };
            headerPanel.Controls.Add(accentLine);

            form.Controls.Add(headerPanel);
            headerPanel.SendToBack(); // Keep top dock precedence clean so Fill controls fit underneath
        }

        private static int buttonStyleCounter = 0;

        public static void StyleButton(Button btn, ButtonLiteStyle explicitStyle = null)
        {
            if (btn == null) return;

            ButtonLiteStyle style = explicitStyle ?? GetAppropriateLiteStyle(btn, buttonStyleCounter++);

            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = style.NormalBack;
            btn.ForeColor = style.ForeColor;
            btn.Font = FontBold;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = style.BorderColor;
            btn.FlatAppearance.MouseOverBackColor = style.HoverBack;
            btn.FlatAppearance.MouseDownBackColor = style.PressedBack;
            btn.Cursor = Cursors.Hand;

            // Attach interactive hover and micro-press animations
            ButtonAnimator.Attach(btn, style);
        }

        public static ButtonLiteStyle GetAppropriateLiteStyle(Button btn, int indexFallback = 0)
        {
            string text = (btn.Text ?? "").ToUpperInvariant();
            string name = (btn.Name ?? "").ToUpperInvariant();
            string combined = text + " " + name;

            if (combined.Contains("UPLOAD") || combined.Contains("IMPORT") || combined.Contains("BROWSE"))
                return StyleSky;
            if (combined.Contains("REFRESH") || combined.Contains("SYNC"))
                return StyleTeal;
            if (combined.Contains("HEX") || combined.Contains("CONVERT") || combined.Contains("BIN"))
                return StyleAmber;
            if (combined.Contains("EXCEL") || combined.Contains("DOWNLOAD") || combined.Contains("SAVE") || combined.Contains("LOGIN") || combined.Contains("SIGN IN"))
                return StyleEmerald;
            if (combined.Contains("USER") || combined.Contains("ASSIGN") || combined.Contains("AUTH"))
                return StyleIndigo;
            if (combined.Contains("CREATETABLE") || combined.Contains("CREATE TABLES") || combined.Contains("INIT TABLE"))
                return StyleSlate;
            if (combined.Contains("MTEMP"))
                return StyleTeal;
            if (combined.Contains("TABLE") || combined.Contains("DATABASE") || combined.Contains("CREATE") || combined.Contains("SETTINGS") || combined.Contains("CONFIG"))
                return StyleIndigo;
            if (combined.Contains("DELETE") || combined.Contains("REMOVE") || combined.Contains("LOGOUT") || combined.Contains("STOP") || combined.Contains("CANCEL"))
                return StyleRose;
            if (combined.Contains("UNASSIGN") || combined.Contains("PAUSE") || combined.Contains("RESUME") || combined.Contains("DEFAULT") || combined.Contains("TEST"))
                return StyleSlate;

            ButtonLiteStyle[] cycle = new ButtonLiteStyle[] { StyleSky, StyleEmerald, StyleIndigo, StyleTeal, StyleAmber, StyleSlate };
            return cycle[Math.Abs(indexFallback) % cycle.Length];
        }

        private static void StyleControls(Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl.Name == "pnlHeaderBanner") continue;

                Button btn = ctrl as Button;
                TextBox txt = ctrl as TextBox;
                ListBox lst = ctrl as ListBox;
                Label lbl = ctrl as Label;
                DataGridView dgv = ctrl as DataGridView;
                StatusStrip ss = ctrl as StatusStrip;
                Panel pnl = ctrl as Panel;
                GroupBox gb = ctrl as GroupBox;

                if (btn != null)
                {
                    StyleButton(btn);
                }
                else if (txt != null)
                {
                    txt.BackColor = LightControl;
                    txt.ForeColor = TextBlack;
                    txt.BorderStyle = BorderStyle.FixedSingle;
                    txt.Font = new Font("Segoe UI", 10.5f);
                }
                else if (lst != null)
                {
                    lst.BackColor = LightSurface;
                    lst.ForeColor = TextBlack;
                    lst.BorderStyle = BorderStyle.FixedSingle;
                    lst.Font = new Font("Segoe UI", 11f);
                    lst.ItemHeight = 26;
                }
                else if (lbl != null)
                {
                    if (lbl.Parent is Panel && lbl.Parent.Name == "pnlHeaderBanner")
                    {
                        lbl.ForeColor = Color.White;
                    }
                    else
                    {
                        lbl.ForeColor = TextBlack;
                    }
                }
                else if (pnl != null && pnl.Name == "pnlLoginCard")
                {
                    pnl.BackColor = LightSurface;
                    pnl.BorderStyle = BorderStyle.FixedSingle;
                }
                else if (gb != null)
                {
                    gb.ForeColor = TextBlack;
                    gb.Font = FontBold;
                }
                else if (dgv != null)
                {
                    StyleDataGridView(dgv);
                }
                else if (ss != null)
                {
                    ss.BackColor = Color.FromArgb(241, 245, 249);
                    ss.ForeColor = TextBlack;
                }

                if (ctrl.HasChildren && !(ctrl is DataGridView))
                {
                    StyleControls(ctrl.Controls);
                }
            }
        }

        public static void StyleDataGridView(DataGridView dgv)
        {
            if (dgv == null) return;

            dgv.EnableHeadersVisualStyles = false;
            dgv.BackgroundColor = LightBg;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = BorderColor;

            // Column Header Styling (Emerald Green Header with White Text)
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 36;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = GreenHeader;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // Data Cell Styling (Pure White background, Black Text)
            dgv.DefaultCellStyle.BackColor = LightSurface;
            dgv.DefaultCellStyle.ForeColor = TextBlack;
            dgv.DefaultCellStyle.SelectionBackColor = GreenHover;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Font = FontGrid;
            dgv.DefaultCellStyle.Padding = new Padding(4, 2, 4, 2);

            // Alternating Row Styling (Soft Mint-White Tint)
            dgv.AlternatingRowsDefaultCellStyle.BackColor = GreenLightTint;
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = TextBlack;
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = GreenHover;
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;

            // Row Headers Sizing & Colors
            dgv.RowHeadersWidth = 45;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
            dgv.RowHeadersDefaultCellStyle.ForeColor = TextMuted;
            dgv.RowHeadersDefaultCellStyle.SelectionBackColor = GreenHover;
        }

        // ==================== MODERN LITE BUTTON PALETTES ====================
        public static readonly ButtonLiteStyle StyleEmerald = new ButtonLiteStyle
        {
            NormalBack = Color.FromArgb(236, 253, 245),
            HoverBack = Color.FromArgb(209, 250, 229),
            PressedBack = Color.FromArgb(167, 243, 208),
            ForeColor = Color.FromArgb(6, 95, 70),
            BorderColor = Color.FromArgb(110, 231, 183),
            HoverBorderColor = Color.FromArgb(5, 150, 105)
        };

        public static readonly ButtonLiteStyle StyleSky = new ButtonLiteStyle
        {
            NormalBack = Color.FromArgb(239, 246, 255),
            HoverBack = Color.FromArgb(219, 234, 254),
            PressedBack = Color.FromArgb(191, 219, 254),
            ForeColor = Color.FromArgb(29, 78, 216),
            BorderColor = Color.FromArgb(147, 197, 253),
            HoverBorderColor = Color.FromArgb(37, 99, 235)
        };

        public static readonly ButtonLiteStyle StyleIndigo = new ButtonLiteStyle
        {
            NormalBack = Color.FromArgb(245, 243, 255),
            HoverBack = Color.FromArgb(237, 233, 254),
            PressedBack = Color.FromArgb(221, 214, 254),
            ForeColor = Color.FromArgb(109, 40, 217),
            BorderColor = Color.FromArgb(196, 181, 253),
            HoverBorderColor = Color.FromArgb(124, 58, 237)
        };

        public static readonly ButtonLiteStyle StyleTeal = new ButtonLiteStyle
        {
            NormalBack = Color.FromArgb(240, 253, 250),
            HoverBack = Color.FromArgb(204, 251, 241),
            PressedBack = Color.FromArgb(153, 246, 228),
            ForeColor = Color.FromArgb(15, 118, 110),
            BorderColor = Color.FromArgb(94, 234, 212),
            HoverBorderColor = Color.FromArgb(13, 148, 136)
        };

        public static readonly ButtonLiteStyle StyleAmber = new ButtonLiteStyle
        {
            NormalBack = Color.FromArgb(254, 252, 232),
            HoverBack = Color.FromArgb(254, 243, 199),
            PressedBack = Color.FromArgb(253, 230, 138),
            ForeColor = Color.FromArgb(180, 83, 9),
            BorderColor = Color.FromArgb(252, 211, 77),
            HoverBorderColor = Color.FromArgb(217, 119, 6)
        };

        public static readonly ButtonLiteStyle StyleRose = new ButtonLiteStyle
        {
            NormalBack = Color.FromArgb(255, 241, 242),
            HoverBack = Color.FromArgb(255, 228, 230),
            PressedBack = Color.FromArgb(254, 205, 211),
            ForeColor = Color.FromArgb(190, 18, 60),
            BorderColor = Color.FromArgb(253, 164, 175),
            HoverBorderColor = Color.FromArgb(225, 29, 72)
        };

        public static readonly ButtonLiteStyle StyleSlate = new ButtonLiteStyle
        {
            NormalBack = Color.FromArgb(248, 250, 252),
            HoverBack = Color.FromArgb(241, 245, 249),
            PressedBack = Color.FromArgb(226, 232, 240),
            ForeColor = Color.FromArgb(51, 65, 85),
            BorderColor = Color.FromArgb(203, 213, 225),
            HoverBorderColor = Color.FromArgb(100, 116, 139)
        };
    }

    public class ButtonLiteStyle
    {
        public Color NormalBack { get; set; }
        public Color HoverBack { get; set; }
        public Color PressedBack { get; set; }
        public Color ForeColor { get; set; }
        public Color BorderColor { get; set; }
        public Color HoverBorderColor { get; set; }
    }

    public class ButtonAnimator
    {
        private Button button;
        private ButtonLiteStyle style;
        private Timer animTimer;
        private float currentProgress = 0f;
        private float targetProgress = 0f;
        private bool isPressed = false;
        private bool isElevated = false;

        private ButtonAnimator(Button btn, ButtonLiteStyle btnStyle)
        {
            this.button = btn;
            this.style = btnStyle;

            animTimer = new Timer();
            animTimer.Interval = 15;
            animTimer.Tick += AnimTimer_Tick;

            button.MouseEnter += Button_MouseEnter;
            button.MouseLeave += Button_MouseLeave;
            button.MouseDown += Button_MouseDown;
            button.MouseUp += Button_MouseUp;
            button.Disposed += (s, e) => { animTimer.Stop(); animTimer.Dispose(); };
        }

        public static void Attach(Button btn, ButtonLiteStyle btnStyle)
        {
            if (btn == null) return;
            if (btn.Tag is ButtonAnimator)
            {
                ButtonAnimator existing = (ButtonAnimator)btn.Tag;
                existing.style = btnStyle;
                return;
            }
            btn.Tag = new ButtonAnimator(btn, btnStyle);
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            targetProgress = 1f;
            if (!animTimer.Enabled) animTimer.Start();

            if (!isElevated && !isPressed && button.Dock == DockStyle.None && !(button.Parent is TableLayoutPanel))
            {
                button.Location = new Point(button.Location.X, button.Location.Y - 1);
                isElevated = true;
            }
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            targetProgress = 0f;
            if (!animTimer.Enabled) animTimer.Start();

            if (isElevated && button.Dock == DockStyle.None && !(button.Parent is TableLayoutPanel))
            {
                button.Location = new Point(button.Location.X, button.Location.Y + 1);
                isElevated = false;
            }
        }

        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPressed = true;
                button.BackColor = style.PressedBack;
                if (button.Dock == DockStyle.None && !(button.Parent is TableLayoutPanel))
                {
                    button.Location = new Point(button.Location.X, button.Location.Y + 2);
                }
                else
                {
                    button.Padding = new Padding(0, 2, 0, 0);
                }
            }
        }

        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            if (isPressed)
            {
                isPressed = false;
                if (button.Dock == DockStyle.None && !(button.Parent is TableLayoutPanel))
                {
                    button.Location = new Point(button.Location.X, button.Location.Y - 2);
                }
                else
                {
                    button.Padding = new Padding(0, 0, 0, 0);
                }

                if (button.ClientRectangle.Contains(button.PointToClient(Cursor.Position)))
                {
                    button.BackColor = style.HoverBack;
                }
                else
                {
                    button.BackColor = style.NormalBack;
                }
            }
        }

        private void AnimTimer_Tick(object sender, EventArgs e)
        {
            if (isPressed) return;

            float diff = targetProgress - currentProgress;
            if (Math.Abs(diff) < 0.05f)
            {
                currentProgress = targetProgress;
                animTimer.Stop();
            }
            else
            {
                currentProgress += diff * 0.35f;
            }

            button.BackColor = Interpolate(style.NormalBack, style.HoverBack, currentProgress);
            button.FlatAppearance.BorderColor = Interpolate(style.BorderColor, style.HoverBorderColor, currentProgress);
        }

        private static Color Interpolate(Color c1, Color c2, float t)
        {
            if (t <= 0f) return c1;
            if (t >= 1f) return c2;
            int r = (int)(c1.R + (c2.R - c1.R) * t);
            int g = (int)(c1.G + (c2.G - c1.G) * t);
            int b = (int)(c1.B + (c2.B - c1.B) * t);
            return Color.FromArgb(
                Math.Min(255, Math.Max(0, r)),
                Math.Min(255, Math.Max(0, g)),
                Math.Min(255, Math.Max(0, b)));
        }
    }
}
