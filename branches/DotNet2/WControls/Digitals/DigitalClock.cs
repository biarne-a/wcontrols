﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace WControls.Digitals
{
    /// <summary>
    /// A digital clock to display times
    /// </summary>
    [ToolboxItem(true)]
    [Description("A digital clock to display times")]
    public partial class DigitalClock : BaseWControl
    {
        private TimeSpan m_value = TimeSpan.Zero;
        /// <summary>
        /// The value of the clock
        /// </summary>
        [Description("The value of the clock")]
        [DefaultValue(typeof(TimeSpan), "00:00:00")]
        [Category("Appearance")]
        public TimeSpan Value
        {
            get { return m_value; }
            set
            {
                if (value < TimeSpan.Zero || value >= TimeSpan.FromHours(100))
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                m_value = value;
                UpdateValue();
            }
        }

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                m_cellHr1.ForeColor = value;
                m_cellHr2.ForeColor = value;
                m_cellMin1.ForeColor = value;
                m_cellMin2.ForeColor = value;
                m_cellSec1.ForeColor = value;
                m_cellSec2.ForeColor = value;
                m_colonLeft.ForeColor = value;
                m_colonRight.ForeColor = value;
                this.Invalidate(true);
            }
        }

        public DigitalClock()
            : base(false)
        {
            InitializeComponent();
            UpdateValue();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            OnPaintGloss(e.Graphics);
        }

        private void UpdateValue()
        {
            string strHr = Value.Hours.ToString("00");
            string strMin = Value.Minutes.ToString("00");
            string strSec = Value.Seconds.ToString("00");

            m_cellHr1.Value = strHr[0];
            m_cellHr2.Value = strHr[1];
            m_cellMin1.Value = strMin[0];
            m_cellMin2.Value = strMin[1];
            m_cellSec1.Value = strSec[0];
            m_cellSec2.Value = strSec[1];
        }
    }
}
