using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzlePirater {
    public partial class BilgeForm : Form {

        
        BilgePirater bilgePirater;
        private bool isActive = false;

        public BilgeForm() {
            InitializeComponent();
            bilgePirater = new BilgePirater();
        }

        private void btnToggleStatus_Click(object sender, EventArgs e) {
            isActive = !isActive;
            if (isActive) {
                btnToggleStatus.Text = "Stop";
                lblStatus.Text = "Activated!";
                lblStatus.ForeColor = Color.FromArgb(0, 200, 0);
                updateImagePreview();
                previewProcessedBoard();
            } else {
                btnToggleStatus.Text = "Start";
                lblStatus.Text = "Disabled";
                lblStatus.ForeColor = Color.FromArgb(200, 0, 0);
                picturePreview.Image = null;
            }
        }

        private void updateImagePreview() {
            picturePreview.Image = bilgePirater.getRaw();
        }

        private void previewProcessedBoard() {
            BilgePiece[] pieces = bilgePirater.processBoard();
            string rawString = string.Format("The array contains {0} pieces, it should contain 72.", pieces.Length);

            for (int i = 0; i < pieces.Length; i++) {
                if (i % 6 == 0) rawString += "\n";
                rawString += pieces[i].pieceName;
                rawString += " ";
            }

            lblRawOutput.Text = rawString;
        }
    }
}
