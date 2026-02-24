using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace An_Early_Warning_System_for_Student
{
    public sealed class MLAnalytics : Form
    {
        private readonly Label _lblModel;
        private readonly Label _lblAccuracy;
        private readonly Label _lblPrecision;
        private readonly Label _lblRecall;
        private readonly Label _lblF1;
        private readonly Label _lblNote;

        public MLAnalytics()
        {
            BackColor = Color.White;

            var title = new Label
            {
                AutoSize = true,
                Font = new Font("Century Gothic", 18F, FontStyle.Bold, GraphicsUnit.Point, 0),
                ForeColor = Color.Black,
                Location = new Point(24, 22),
                Text = "ML Analytics",
            };

            _lblModel = new Label
            {
                AutoSize = true,
                Font = new Font("Century Gothic", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0),
                ForeColor = Color.Black,
                Location = new Point(26, 58),
                Text = "Model: —",
            };

            _lblNote = new Label
            {
                AutoSize = true,
                Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                ForeColor = Color.FromArgb(64, 64, 64),
                Location = new Point(26, 83),
                Text = "Metrics are computed from the training dataset split.",
            };

            var metricsPanel = new Panel
            {
                BackColor = Color.White,
                Location = new Point(24, 118),
                Size = new Size(820, 260),
            };

            var card1 = CreateMetricCard("Accuracy", out _lblAccuracy);
            var card2 = CreateMetricCard("Precision (At Risk)", out _lblPrecision);
            var card3 = CreateMetricCard("Recall (At Risk)", out _lblRecall);
            var card4 = CreateMetricCard("F1 (At Risk)", out _lblF1);

            card1.Location = new Point(0, 0);
            card2.Location = new Point(420, 0);
            card3.Location = new Point(0, 130);
            card4.Location = new Point(420, 130);

            metricsPanel.Controls.Add(card1);
            metricsPanel.Controls.Add(card2);
            metricsPanel.Controls.Add(card3);
            metricsPanel.Controls.Add(card4);

            Controls.Add(title);
            Controls.Add(_lblModel);
            Controls.Add(_lblNote);
            Controls.Add(metricsPanel);

            Load += (_, _) => LoadMetrics();
        }

        private static Panel CreateMetricCard(string title, out Label valueLabel)
        {
            var panel = new Panel
            {
                BackColor = Color.FromArgb(240, 240, 240),
                Size = new Size(400, 120),
            };

            var lblTitle = new Label
            {
                AutoSize = true,
                Font = new Font("Century Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0),
                ForeColor = Color.Black,
                Location = new Point(16, 16),
                Text = title,
            };

            valueLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Century Gothic", 22F, FontStyle.Bold, GraphicsUnit.Point, 0),
                ForeColor = Color.Black,
                Location = new Point(16, 52),
                Text = "—",
            };

            panel.Controls.Add(lblTitle);
            panel.Controls.Add(valueLabel);
            return panel;
        }

        private void LoadMetrics()
        {
            try
            {
                string path = Path.Combine(AppContext.BaseDirectory, "ModelData", "model_metrics.json");
                if (!File.Exists(path))
                {
                    _lblNote.Text = "Metrics file not found.";
                    return;
                }

                string json = File.ReadAllText(path);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                string modelName = root.TryGetProperty("model", out var modelProp)
                    ? modelProp.GetString() ?? "—"
                    : "—";

                _lblModel.Text = $"Model: {modelName}";

                double acc = root.TryGetProperty("accuracy", out var accProp) ? accProp.GetDouble() : 0.0;
                double prec = root.TryGetProperty("precision_pos", out var pProp) ? pProp.GetDouble() : 0.0;
                double rec = root.TryGetProperty("recall_pos", out var rProp) ? rProp.GetDouble() : 0.0;
                double f1 = root.TryGetProperty("f1_pos", out var f1Prop) ? f1Prop.GetDouble() : 0.0;

                _lblAccuracy.Text = $"{acc * 100:0.#}%";
                _lblPrecision.Text = $"{prec * 100:0.#}%";
                _lblRecall.Text = $"{rec * 100:0.#}%";
                _lblF1.Text = $"{f1 * 100:0.#}%";
            }
            catch (Exception)
            {
                _lblNote.Text = "Failed to load metrics.";
            }
        }
    }
}
