using System.Drawing;
using System.Windows.Forms;

namespace Edamam
{
    public class DeleteConfirmationForm : Form
    {
        public DeleteConfirmationForm(string message = "Are you sure you want to delete this meal?")
        {
            // avoid semi-transparent BackColor on controls (WinForms doesn't support alpha)
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(240, 240, 240); // solid light gray outer (no alpha)
            ClientSize = new Size(600, 260);
            ShowInTaskbar = false;

            // inner white card with subtle border to make it distinct
            var card = new Panel
            {
                BackColor = Color.White,
                Size = new Size(480, 160),
                Location = new Point((ClientSize.Width - 480) / 2, (ClientSize.Height - 160) / 2),
            };

            card.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(220, 220, 220), 1);
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            var title = new Label
            {
                Text = "Delete",
                Font = new Font("Segoe UI Variable Text", 12F, FontStyle.Regular),
                ForeColor = Color.FromArgb(33, 33, 33),
                AutoSize = true,
                Location = new Point(16, 12)
            };

            var closeBtn = new Button
            {
                Text = "X",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(120, 120, 120),
                Size = new Size(28, 28),
                Location = new Point(card.Width - 44, 8)
            };
            closeBtn.FlatAppearance.BorderSize = 0;
            closeBtn.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            // message label: fixed size + wrapping so text doesn't get cut
            var msg = new Label
            {
                Text = message,
                Font = new Font("Segoe UI Variable Text", 10F),
                ForeColor = Color.FromArgb(60, 60, 60),
                AutoSize = false,
                Location = new Point(16, 44),
                Size = new Size(card.Width - 32, 60),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Width = 110,
                Height = 36,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(60, 60, 60),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(card.Width - 260, card.Height - 52)
            };
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnCancel.FlatAppearance.BorderSize = 1;

            var btnDelete = new Button
            {
                Text = "Delete",
                DialogResult = DialogResult.OK,
                Width = 110,
                Height = 36,
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(card.Width - 130, card.Height - 52)
            };
            btnDelete.FlatAppearance.BorderSize = 0;

            card.Controls.Add(title);
            card.Controls.Add(closeBtn);
            card.Controls.Add(msg);
            card.Controls.Add(btnCancel);
            card.Controls.Add(btnDelete);

            this.Controls.Add(card);

            this.AcceptButton = btnDelete;
            this.CancelButton = btnCancel;

            // position relative to parent when shown
            this.Shown += (s, e) =>
            {
                if (this.Owner != null)
                {
                    this.StartPosition = FormStartPosition.Manual;
                    var owner = this.Owner;
                    this.Location = new Point(owner.Location.X + (owner.Width - this.Width) / 2, owner.Location.Y + (owner.Height - this.Height) / 2);
                }
            };
        }
    }
}


