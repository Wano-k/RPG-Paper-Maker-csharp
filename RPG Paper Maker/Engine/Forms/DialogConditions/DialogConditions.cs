﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPG_Paper_Maker
{
    public partial class DialogConditions : Form
    {


        // -------------------------------------------------------------------
        // Constructors
        // -------------------------------------------------------------------

        public DialogConditions()
        {
            InitializeComponent();
        }

        // -------------------------------------------------------------------
        // EVENTS
        // -------------------------------------------------------------------

        private void ok_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
