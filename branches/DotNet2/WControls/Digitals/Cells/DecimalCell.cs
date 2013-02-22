﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WControls.Drawable;

namespace WControls.Digitals.Cells
{
    /// <summary>
    /// A cell to display decimal point, comma, colon and semi-colon
    /// </summary>
    [ToolboxItem(true)]
    [Description("A cell to display decimal point, comma, colon and semi-colon")]
    public partial class DecimalCell : DigitalCell
    {
        //this cell intended to be half the width of a 7-segment cell
        protected const double PADDING_RATIO_Y = .0446d;
        protected const double BARSIZE_RATIO_X = .3276d;
        protected const double BARSIZE_RATIO_Y = .1049d;
        protected const double COMMA_PAD = .02d;

        private Dictionary<char, bool[]> SEGMENT_MAP = new Dictionary<char, bool[]>()
        {
            {'.', new bool[3] { false, true, false } },
            {',', new bool[3] { false, true, true } },
            {':', new bool[3] { true, true, false } },
            {';', new bool[3] { true, true, true } }
        };

        protected DigitalBar m_segmentA = new DigitalBar(SegmentOrientation.Horizontal);
        protected DigitalBar m_segmentB = new DigitalBar(SegmentOrientation.Vertical);
        protected DigitalBar m_segmentC = new DigitalBar(SegmentOrientation.Vertical);

        public DecimalCell()
        {
            Value = '.';

            m_segmentA.Color = this.ForeColor;
            m_segmentB.Color = this.ForeColor;
            m_segmentC.Color = this.ForeColor;

            m_segmentA.OpacityWhenOff = this.OpacityWhenOff;
            m_segmentB.OpacityWhenOff = this.OpacityWhenOff;
            m_segmentC.OpacityWhenOff = this.OpacityWhenOff;

            //never use internal padding for this type of cell
            m_segmentA.Padding = 0;
            m_segmentB.Padding = 0;
            m_segmentC.Padding = 0;

            //corners for the comma
            m_segmentC.Corners = SegmentCorners.TopLeft | SegmentCorners.TopRight | SegmentCorners.BottomRight;

            Segments.Add(m_segmentA);
            Segments.Add(m_segmentB);
            Segments.Add(m_segmentC);

            InitializeComponent();
        }

        protected override void CalculateSegments(RectangleF container)
        {
            float paddingY = (float)(PADDING_RATIO_Y * container.Height);
            float segWidthX = (float)(BARSIZE_RATIO_X * container.Width);
            float segWidthY = (float)(BARSIZE_RATIO_Y * container.Height);
            float commaPad = (float)(COMMA_PAD * container.Height);

            float aYStart = container.Top + paddingY + segWidthY;
            float bYStart = container.Bottom - (paddingY + (segWidthY * 2));
            float startX = container.X + ((container.Width - segWidthX) / 2f);
            float cHeight = (container.Bottom - (bYStart + segWidthY)) - commaPad;

            RectangleF rectA = new RectangleF(startX, aYStart, segWidthX, segWidthY);
            RectangleF rectB = new RectangleF(startX, bYStart, segWidthX, segWidthY);
            RectangleF rectC = new RectangleF(startX, bYStart + segWidthY, segWidthX, cHeight);

            m_segmentA.TipLength = 0;
            m_segmentA.CalculatePaths(rectA);

            m_segmentB.TipLength = 0;
            m_segmentB.CalculatePaths(rectB);

            m_segmentC.TipLength = cHeight / 2f;
            m_segmentC.CalculatePaths(rectC);
        }

        protected override bool CanHandleValue(char value)
        {
            return (value == '.') || (value == ',') ||
                   (value == ':') || (value == ';');
        }

        protected override void UpdateSegmentsOnOff()
        {
            bool[] segs = SEGMENT_MAP[Value];

            m_segmentA.IsOn = segs[0];
            m_segmentB.IsOn = segs[1];
            m_segmentC.IsOn = segs[2];
        }
    }
}
