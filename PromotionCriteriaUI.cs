﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DojoStudentManagement
{
    public partial class PromotionCriteriaUI : Form
    {
        public PromotionCriteriaUI(DataTable promotionRequirementsTable)
        {
            InitializeComponent();

            dgvPromotionSettings.DataSource = promotionRequirementsTable;
        }

        private void PromotionSettingsUI_Load(object sender, EventArgs e)
        {

        }
    }
}