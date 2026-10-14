using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NU_Solver
{
    public static class VectorGraphics
    {
        // Material Design 3 Palette
        public static readonly Color ColorPrimary       = Color.FromArgb(16, 149, 93);   // #10955D Lite Green / Emerald
        public static readonly Color ColorPrimaryDark   = Color.FromArgb(10, 110, 68);   // #0A6E44 Deep Green
        public static readonly Color ColorPrimaryHover  = Color.FromArgb(22, 175, 110);  // #16AF6E Green Hover
        public static readonly Color ColorPrimaryLight  = Color.FromArgb(236, 253, 245); // #ECFDF5 Mint Tint
        public static readonly Color ColorSecondary     = Color.FromArgb(249, 115, 22);  // #F97316 Warm Orange
        public static readonly Color ColorSecondaryDark = Color.FromArgb(217, 83, 7);    // #D95307 Deep Orange
        public static readonly Color ColorSecondaryHover= Color.FromArgb(251, 146, 60);  // #FB923C Orange Hover
        public static readonly Color ColorSecondaryLight= Color.FromArgb(255, 247, 237); // #FFF7ED Peach Tint
        public static readonly Color ColorAccent        = Color.FromArgb(245, 158, 11);  // #F59E0B Amber / Orange Gold
        public static readonly Color ColorAppBg         = Color.FromArgb(247, 250, 247); // #F7FAF7 Crisp Natural White-Green
        public static readonly Color ColorCardSurface   = Color.FromArgb(255, 255, 255); // #FFFFFF Pure White
        public static readonly Color ColorTextPrimary   = Color.FromArgb(26, 46, 34);    // #1A2E22 Deep Slate Forest
        public static readonly Color ColorTextSecondary = Color.FromArgb(88, 112, 98);   // #587062 Secondary Forest Slate
        public static readonly Color ColorBorderDivider = Color.FromArgb(226, 235, 228); // #E2EBE4 Clean Mint Slate Border
        public static readonly Color ColorSuccess       = Color.FromArgb(16, 149, 93);   // #10955D Success Green
        public static readonly Color ColorWarning       = Color.FromArgb(249, 115, 22);  // #F97316 Warning Orange
        public static readonly Color ColorError         = Color.FromArgb(220, 38, 38);   // #DC2626 Coral Red

        // Compatibility Mappings
        public static readonly Color ColorBlueDark    = ColorTextPrimary;
        public static readonly Color ColorBlueHeader  = ColorPrimaryDark;
        public static readonly Color ColorBlueAccent  = ColorPrimary;
        public static readonly Color ColorGreenDark   = ColorSecondaryDark;
        public static readonly Color ColorGreenMain   = ColorPrimary;
        public static readonly Color ColorGreenLight  = ColorSecondary;
        public static readonly Color ColorGreenHover  = ColorSecondaryHover;
        public static readonly Color ColorMagentaDark = ColorSecondaryDark;
        public static readonly Color ColorMagentaMain = ColorSecondary;
        public static readonly Color ColorMagentaLight= ColorSecondaryHover;
        public static readonly Color ColorMagentaHover= ColorSecondaryHover;
        public static readonly Color ColorCyanAccent  = ColorAccent;
        public static readonly Color ColorGold        = Color.FromArgb(245, 158, 11);
        public static readonly Color ColorSurfaceBg   = ColorAppBg;
        public static readonly Color ColorCardBg      = ColorCardSurface;
        public static readonly Color ColorBorder      = ColorBorderDivider;

        /// <summary>
        /// Draws a crisp, multi-color vector academic crest (Shield, Mortarboard, Book, Laurel Wreath)
        /// </summary>
        public static void DrawVectorCrest(Graphics g, Rectangle r)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            float x = r.X;
            float y = r.Y;
            float w = r.Width;
            float h = r.Height;

            float cx = x + w / 2f;
            float cy = y + h / 2f;

            // 1. Outer Glow / Subtle Background
            using (GraphicsPath circle = new GraphicsPath())
            {
                circle.AddEllipse(x + 2, y + 2, w - 4, h - 4);
                using (PathGradientBrush pgb = new PathGradientBrush(circle))
                {
                    pgb.CenterColor = Color.FromArgb(35, ColorGreenLight);
                    pgb.SurroundColors = new Color[] { Color.FromArgb(0, 0, 0, 0) };
                    g.FillPath(pgb, circle);
                }
            }

            // 2. Academic Shield
            float shieldW = w * 0.72f;
            float shieldH = h * 0.78f;
            float sx = cx - shieldW / 2f;
            float sy = y + h * 0.12f;

            using (GraphicsPath shield = new GraphicsPath())
            {
                shield.AddLine(sx, sy, sx + shieldW, sy);
                shield.AddLine(sx + shieldW, sy, sx + shieldW, sy + shieldH * 0.55f);
                shield.AddBezier(
                    sx + shieldW, sy + shieldH * 0.55f,
                    sx + shieldW * 0.9f, sy + shieldH * 0.92f,
                    cx, sy + shieldH,
                    cx, sy + shieldH);
                shield.AddBezier(
                    cx, sy + shieldH,
                    sx + shieldW * 0.1f, sy + shieldH * 0.92f,
                    sx, sy + shieldH * 0.55f,
                    sx, sy + shieldH * 0.55f);
                shield.CloseFigure();

                // Shield fill: Vertical gradient Blue -> Emerald
                using (LinearGradientBrush lgb = new LinearGradientBrush(
                    new PointF(sx, sy), new PointF(sx, sy + shieldH),
                    Color.FromArgb(25, 45, 95), Color.FromArgb(6, 78, 59)))
                {
                    g.FillPath(lgb, shield);
                }

                // Shield border: Gold & Magenta gradient outline
                using (Pen pen = new Pen(ColorGold, Math.Max(2f, w * 0.035f)))
                {
                    pen.LineJoin = LineJoin.Round;
                    g.DrawPath(pen, shield);
                }
            }

            // 3. Laurel Wreath Leaves (Left and Right)
            using (Brush wreathBrush = new SolidBrush(Color.FromArgb(220, ColorGold)))
            {
                for (int i = 0; i < 4; i++)
                {
                    float angle = -35f + i * 22f;
                    float leafR = shieldW * 0.58f;
                    
                    // Left leaf
                    float lx = cx - (float)(Math.Cos(angle * Math.PI / 180.0) * leafR);
                    float ly = cy + (float)(Math.Sin(angle * Math.PI / 180.0) * leafR) * 0.8f;
                    g.FillEllipse(wreathBrush, lx - 3, ly - 2, 7, 5);

                    // Right leaf
                    float rx = cx + (float)(Math.Cos(angle * Math.PI / 180.0) * leafR);
                    float ry = cy + (float)(Math.Sin(angle * Math.PI / 180.0) * leafR) * 0.8f;
                    g.FillEllipse(wreathBrush, rx - 4, ry - 2, 7, 5);
                }
            }

            // 4. Open Book of Knowledge
            float bookW = shieldW * 0.52f;
            float bookH = shieldH * 0.28f;
            float bx = cx - bookW / 2f;
            float by = cy - bookH * 0.2f;

            using (GraphicsPath book = new GraphicsPath())
            {
                // Left page
                book.AddLine(cx, by, bx, by - 3);
                book.AddLine(bx, by - 3, bx, by + bookH);
                book.AddBezier(bx, by + bookH, cx - bookW * 0.25f, by + bookH - 2, cx, by + bookH + 2, cx, by + bookH + 2);
                
                // Right page
                book.AddBezier(cx, by + bookH + 2, cx + bookW * 0.25f, by + bookH - 2, bx + bookW, by + bookH, bx + bookW, by + bookH);
                book.AddLine(bx + bookW, by + bookH, bx + bookW, by - 3);
                book.AddLine(bx + bookW, by - 3, cx, by);
                book.CloseFigure();

                using (SolidBrush bBrush = new SolidBrush(Color.White))
                {
                    g.FillPath(bBrush, book);
                }
                using (Pen bPen = new Pen(Color.FromArgb(180, ColorBlueDark), 1f))
                {
                    g.DrawPath(bPen, book);
                    // Center spine
                    g.DrawLine(bPen, cx, by, cx, by + bookH + 2);
                    // Page text lines
                    g.DrawLine(bPen, bx + 3, by + 4, cx - 3, by + 5);
                    g.DrawLine(bPen, bx + 3, by + 8, cx - 3, by + 9);
                    g.DrawLine(bPen, cx + 3, by + 5, bx + bookW - 3, by + 4);
                    g.DrawLine(bPen, cx + 3, by + 9, bx + bookW - 3, by + 8);
                }
            }

            // 5. Mortarboard / Graduation Cap (Above Book)
            float capW = shieldW * 0.56f;
            float capH = shieldH * 0.22f;
            float capX = cx;
            float capY = by - capH * 0.7f;

            using (GraphicsPath cap = new GraphicsPath())
            {
                // Rhombus diamond cap top
                cap.AddLine(capX, capY - capH * 0.5f, capX + capW * 0.5f, capY);
                cap.AddLine(capX + capW * 0.5f, capY, capX, capY + capH * 0.5f);
                cap.AddLine(capX, capY + capH * 0.5f, capX - capW * 0.5f, capY);
                cap.CloseFigure();

                using (SolidBrush capBrush = new SolidBrush(ColorMagentaMain))
                {
                    g.FillPath(capBrush, cap);
                }
                using (Pen capPen = new Pen(Color.White, 1.2f))
                {
                    g.DrawPath(capPen, cap);
                }

                // Golden tassel
                using (Pen tasselPen = new Pen(ColorGold, 1.4f))
                {
                    g.DrawLine(tasselPen, capX, capY, capX + capW * 0.42f, capY + capH * 0.6f);
                    g.FillEllipse(new SolidBrush(ColorGold), capX + capW * 0.38f, capY + capH * 0.55f, 4, 4);
                }
            }

            // 6. Star of Excellence on Top
            float starSize = w * 0.08f;
            float starY = y + 2;
            using (SolidBrush starBrush = new SolidBrush(ColorGold))
            {
                PointF[] starPts = new PointF[]
                {
                    new PointF(cx, starY),
                    new PointF(cx + starSize * 0.28f, starY + starSize * 0.65f),
                    new PointF(cx + starSize * 0.95f, starY + starSize * 0.65f),
                    new PointF(cx + starSize * 0.4f, starY + starSize * 1.05f),
                    new PointF(cx + starSize * 0.62f, starY + starSize * 1.7f),
                    new PointF(cx, starY + starSize * 1.25f),
                    new PointF(cx - starSize * 0.62f, starY + starSize * 1.7f),
                    new PointF(cx - starSize * 0.4f, starY + starSize * 1.05f),
                    new PointF(cx - starSize * 0.95f, starY + starSize * 0.65f),
                    new PointF(cx - starSize * 0.28f, starY + starSize * 0.65f)
                };
                g.FillPolygon(starBrush, starPts);
            }
        }

        /// <summary>
        /// Paints a clean, modern Material Design header banner (Deep Indigo with official crest & crisp typography)
        /// </summary>
        public static void PaintHeaderBanner(Graphics g, Rectangle r, string title, string subtitle)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // Header Background: Deep Emerald Green #0A6E44
            using (SolidBrush bgBrush = new SolidBrush(ColorPrimaryDark))
            {
                g.FillRectangle(bgBrush, r);
            }

            // Draw official logo if available, otherwise vector crest
            Rectangle logoRect = new Rectangle(14, (r.Height - 40) / 2, 40, 40);
            Image officialLogo = UITheme.GetEmbeddedOrFileImage("logo_NU.png");
            if (officialLogo != null)
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(officialLogo, logoRect);
            }
            else
            {
                DrawVectorCrest(g, logoRect);
            }

            // Text Typography
            using (Font fTitle = new Font("Segoe UI", 11.5f, FontStyle.Bold))
            using (Font fSub = new Font("Segoe UI", 8.5f, FontStyle.Regular))
            using (SolidBrush bWhite = new SolidBrush(Color.White))
            using (SolidBrush bMuted = new SolidBrush(ColorPrimaryLight))
            {
                g.DrawString(title, fTitle, bWhite, 64, (r.Height / 2) - 17);
                g.DrawString(subtitle, fSub, bMuted, 64, (r.Height / 2) + 2);
            }

            // Bottom 2.5px Warm Orange Accent Line
            using (Pen accentPen = new Pen(ColorSecondary, 2.5f))
            {
                g.DrawLine(accentPen, 0, r.Height - 1, r.Width, r.Height - 1);
            }
        }

        /// <summary>
        /// Paints a modern rounded card background with subtle drop shadow
        /// </summary>
        public static void PaintCard(Graphics g, Rectangle r, int radius = 8)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw subtle shadow
            if (r.Width > 4 && r.Height > 4)
            {
                Rectangle shadowRect = new Rectangle(r.X + 1, r.Y + 1, r.Width - 1, r.Height - 1);
                using (GraphicsPath shadowPath = GetRoundedRect(shadowRect, radius))
                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(16, 15, 23, 42)))
                {
                    g.FillPath(shadowBrush, shadowPath);
                }
            }

            Rectangle cardRect = new Rectangle(r.X, r.Y, r.Width - 1, r.Height - 1);
            using (GraphicsPath path = GetRoundedRect(cardRect, radius))
            {
                using (SolidBrush b = new SolidBrush(ColorCardBg))
                {
                    g.FillPath(b, path);
                }
                using (Pen p = new Pen(ColorBorderDivider, 1f))
                {
                    g.DrawPath(p, path);
                }
            }
        }

        public static GraphicsPath GetRoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius <= 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // top left arc  
            path.AddArc(arc, 180, 90);

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Styles a button with a modern solid/subtle gradient fill, rounded corners, and proper disabled/hover states
        /// </summary>
        public static void ApplyModernButton(
            Button btn, 
            Color col1, 
            Color col2, 
            Color colHover, 
            Color textCol,
            int radius = 6)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = col1;
            btn.ForeColor = textCol;
            btn.Cursor = Cursors.Hand;

            btn.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                Rectangle rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
                using (GraphicsPath path = GetRoundedRect(rect, radius))
                {
                    if (!btn.Enabled)
                    {
                        using (SolidBrush disabledBg = new SolidBrush(Color.FromArgb(241, 245, 249)))
                        using (Pen disabledBorder = new Pen(Color.FromArgb(226, 232, 240), 1f))
                        {
                            e.Graphics.FillPath(disabledBg, path);
                            e.Graphics.DrawPath(disabledBorder, path);
                        }

                        TextRenderer.DrawText(
                            e.Graphics, 
                            btn.Text, 
                            btn.Font, 
                            btn.ClientRectangle, 
                            Color.FromArgb(148, 163, 184), 
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
                        return;
                    }

                    bool isHover = btn.ClientRectangle.Contains(btn.PointToClient(Cursor.Position));
                    Color fillCol = isHover ? colHover : col1;

                    using (SolidBrush fillBrush = new SolidBrush(fillCol))
                    {
                        e.Graphics.FillPath(fillBrush, path);
                    }

                    // Subtle top highlight
                    using (Pen p = new Pen(Color.FromArgb(40, 255, 255, 255), 1f))
                    {
                        e.Graphics.DrawPath(p, path);
                    }
                }

                // Draw Text
                TextRenderer.DrawText(
                    e.Graphics, 
                    btn.Text, 
                    btn.Font, 
                    btn.ClientRectangle, 
                    btn.ForeColor, 
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            };
        }

        /// <summary>
        /// Styles an outlined secondary button (white background, crisp colored border and text)
        /// </summary>
        public static void ApplyOutlinedButton(
            Button btn, 
            Color strokeColor, 
            Color textColor, 
            int radius = 6)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.White;
            btn.ForeColor = textColor;
            btn.Cursor = Cursors.Hand;

            btn.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                Rectangle rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
                using (GraphicsPath path = GetRoundedRect(rect, radius))
                {
                    if (!btn.Enabled)
                    {
                        using (SolidBrush disabledBg = new SolidBrush(Color.FromArgb(248, 250, 252)))
                        using (Pen disabledBorder = new Pen(Color.FromArgb(226, 232, 240), 1f))
                        {
                            e.Graphics.FillPath(disabledBg, path);
                            e.Graphics.DrawPath(disabledBorder, path);
                        }

                        TextRenderer.DrawText(
                            e.Graphics, 
                            btn.Text, 
                            btn.Font, 
                            btn.ClientRectangle, 
                            Color.FromArgb(148, 163, 184), 
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
                        return;
                    }

                    bool isHover = btn.ClientRectangle.Contains(btn.PointToClient(Cursor.Position));
                    Color bgCol = isHover ? Color.FromArgb(241, 245, 249) : Color.White;

                    using (SolidBrush fillBrush = new SolidBrush(bgCol))
                    {
                        e.Graphics.FillPath(fillBrush, path);
                    }

                    using (Pen p = new Pen(strokeColor, 1.2f))
                    {
                        e.Graphics.DrawPath(p, path);
                    }
                }

                TextRenderer.DrawText(
                    e.Graphics, 
                    btn.Text, 
                    btn.Font, 
                    btn.ClientRectangle, 
                    btn.ForeColor, 
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            };
        }
    }
}
