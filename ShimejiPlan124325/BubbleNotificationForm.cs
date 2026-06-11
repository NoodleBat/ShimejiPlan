using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ShimejiPlan124325.Services.ShimejiWindowFinder;
using static ShimejiPlan124325.Services.MascotProcessManager;

namespace ShimejiPlan124325
{
    public partial class BubbleNotificationForm : Form
    {
        private readonly string _message;
        private readonly System.Windows.Forms.Timer _closeTimer;
        private const int DisplayDurationMs = 8000; // 8 секунд
        // Координаты, куда позиционировать (вычисляются снаружи)
        public BubbleNotificationForm(string message)
        {
            InitializeComponent();
            _message = message;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.Size = new Size(260, 100);
            this.BackColor = Color.White;  // основной цвет
            this.TransparencyKey = Color.Fuchsia; // фуксия будет прозрачной; сам фон мы будем рисовать белым
            this.DoubleBuffered = true;

            // Таймер автоматического закрытия
            _closeTimer = new System.Windows.Forms.Timer { Interval = DisplayDurationMs };
            _closeTimer.Tick += (s, e) => this.Close();
        }
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x08000000; // WS_EX_NOACTIVATE – не забирает фокус
                cp.ExStyle |= 0x00000008; // WS_EX_TOPMOST – поверх всех окон
                return cp;
            }
        }
        // Установка местоположения относительно окна маскота
        public void PositionNearMascot(RECT mascotRect)
        {
            int bubbleWidth = this.Width;
            int bubbleHeight = this.Height;
            int margin = 10;

            // Определяем экран, на котором находится центр маскота
            Point mascotCenter = new Point(mascotRect.Left + mascotRect.Width / 2,
                                           mascotRect.Top + mascotRect.Height / 2);
            Screen targetScreen = Screen.FromPoint(mascotCenter); // этот метод не возвращает null
            var screenArea = targetScreen.WorkingArea;

            // Приоритет: сверху, если места хватает, иначе снизу
            int targetY = mascotRect.Top - bubbleHeight - margin;
            if (targetY < screenArea.Top)
                targetY = mascotRect.Bottom + margin; // показываем снизу

            int mascotCenterX = mascotRect.Left + mascotRect.Width / 2;
            int targetX = mascotCenterX - bubbleWidth / 2;

            // Корректировка X
            if (targetX < screenArea.Left) targetX = screenArea.Left + margin;
            if (targetX + bubbleWidth > screenArea.Right) targetX = screenArea.Right - bubbleWidth - margin;

            this.Location = new Point(targetX, targetY);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Рисуем облачко: скруглённый прямоугольник с «хвостиком» внизу (по центру)
            Rectangle bubbleRect = new Rectangle(5, 5, this.ClientSize.Width - 10, this.ClientSize.Height - 20);

            // Тень (опционально)
            using (var shadowBrush = new SolidBrush(Color.FromArgb(80, 0, 0, 0)))
            {
                g.FillPath(shadowBrush, GetBubblePath(new Rectangle(bubbleRect.X + 2, bubbleRect.Y + 2, bubbleRect.Width, bubbleRect.Height)));
            }

            // Основное тело
            using (var bubbleBrush = new SolidBrush(Color.White))
            using (var borderPen = new Pen(Color.Gray, 1))
            {
                g.FillPath(bubbleBrush, GetBubblePath(bubbleRect));
                g.DrawPath(borderPen, GetBubblePath(bubbleRect));
            }

            // Текст
            using (var textBrush = new SolidBrush(Color.Black))
            using (var font = new Font("Segoe UI", 9, FontStyle.Regular))
            {
                var textRect = new Rectangle(bubbleRect.X + 10, bubbleRect.Y + 8, bubbleRect.Width - 20, bubbleRect.Height - 16);
                g.DrawString(_message, font, textBrush, textRect);
            }
        }


        // Путь для облачка с хвостиком
        private GraphicsPath GetBubblePath(Rectangle rect)
        {
            GraphicsPath path = new GraphicsPath();
            int radius = 10;
            // Скруглённые углы
            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();

            // Хвостик (треугольник внизу)
            int tailWidth = 12;
            int tailHeight = 10;
            int tailX = rect.X + rect.Width / 2 - tailWidth / 2;
            int tailY = rect.Bottom;
            Point[] tailPoints = new Point[] {
                new Point(tailX, tailY),
                new Point(tailX + tailWidth, tailY),
                new Point(tailX + tailWidth / 2, tailY + tailHeight)
            };
            path.AddPolygon(tailPoints);
            return path;
        }
        // При клике в любом месте – закрыть
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            this.Close();
        }

        // Запуск таймера после показа
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _closeTimer.Start();
        }

        private void BubbleNotificationForm_Load(object sender, EventArgs e)
        {

        }
    }
}
