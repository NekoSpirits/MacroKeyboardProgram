using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MacroKeyboardProgram
{
    public partial class frmNewMacro : Form
    {
        public string response;
        public frmNewMacro()
        {
            InitializeComponent();
        }

        private void frmNewMacro_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            response = "import";
            Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            response = "new";
            Close();
        }
    }
}
